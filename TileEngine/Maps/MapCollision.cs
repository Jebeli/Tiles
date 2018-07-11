/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace TileEngine.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;

    public enum MovementType
    {
        Normal,
        Flying,
        Intangible
    }

    public class MapCollision
    {
        public const int BLOCKS_NONE = 0;
        public const int BLOCKS_ALL = 1;
        public const int BLOCKS_MOVEMENT = 2;
        public const int BLOCKS_ALL_HIDDEN = 3;
        public const int BLOCKS_MOVEMENT_HIDDEN = 4;
        public const int MAP_ONLY = 5;
        public const int MAP_ONLY_ALT = 6;
        public const int BLOCKS_ENTITIES = 7; // hero or enemies are blocking this tile, so any other entity is blocked
        public const int BLOCKS_ENEMIES = 8;  // an ally is standing on that tile, so the hero could pass if ENABLE_ALLY_COLLISION is false

        const int CHECK_MOVEMENT = 1;
        const int CHECK_SIGHT = 2;

        const float MIN_TILE_GAP = 0.001f;

        private int w;
        private int h;
        private int[,] colMap;
        private bool enableAllyCollision;

        public MapCollision()
            : this(1, 1)
        {
        }

        public MapCollision(int width, int height)
        {
            w = width;
            h = height;
            colMap = new int[w, h];
        }

        public MapCollision(Layer layer)
        {
            w = layer.Width;
            h = layer.Height;
            colMap = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    colMap[x, y] = layer[x, y].TileId;
                }
            }
        }

        public void SetMap(int[,] colMap, int w, int h)
        {
            this.colMap = colMap;
            this.w = w;
            this.h = h;
        }

        public int[,] ColMap
        {
            get { return colMap; }
            set { colMap = value; }
        }

        public int Width
        {
            get { return w; }
        }

        public int Height
        {
            get { return h; }
        }

        public bool Move(ref float x, ref float y, float stepX, float stepY, MovementType movementType, bool hero)
        {
            bool forceSlide = (stepX != 0 && stepY != 0);
            while (stepX != 0 || stepY != 0)
            {
                float step_x = 0;
                if (stepX > 0)
                {
                    step_x = Math.Min((float)Math.Ceiling(x) - x, stepX);
                    if (step_x <= MIN_TILE_GAP) step_x = Math.Min(1.0f, stepX);
                }
                else if (stepX < 0)
                {
                    step_x = Math.Max((float)Math.Floor(x) - x, stepX);
                    if (step_x == 0) step_x = Math.Max(-1.0f, stepX);
                }
                float step_y = 0;
                if (stepY > 0)
                {
                    step_y = Math.Min((float)Math.Ceiling(y) - y, stepY);
                    if (step_y <= MIN_TILE_GAP) step_y = Math.Min(1.0f, stepY);
                }
                else if (stepY < 0)
                {
                    step_y = Math.Max((float)Math.Floor(y) - y, stepY);
                    if (step_y == 0) step_y = Math.Max(-1.0f, stepY);
                }
                stepX -= step_x;
                stepY -= step_y;
                if (!SmallStep(ref x, ref y, step_x, step_y, movementType, hero))
                {
                    if (forceSlide)
                    {
                        if (!SmallStepForcedSlideAlongGrid(ref x, ref y, step_x, step_y, movementType, hero))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!SmallStepForcedSlid(ref x, ref y, step_x, step_y, movementType, hero))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void Block(float mapX, float mapY, bool ally)
        {
            int tileX = (int)mapX;
            int tileY = (int)mapY;
            if (colMap[tileX, tileY] == BLOCKS_NONE)
            {
                if (ally)
                {
                    colMap[tileX, tileY] = BLOCKS_ENEMIES;
                }
                else
                {
                    colMap[tileX, tileY] = BLOCKS_ENTITIES;
                }
            }
        }

        public void Unblock(float mapX, float mapY)
        {
            int tileX = (int)mapX;
            int tileY = (int)mapY;
            if (colMap[tileX, tileY] == BLOCKS_ENTITIES || colMap[tileX, tileY] == BLOCKS_ENEMIES)
            {
                colMap[tileX, tileY] = BLOCKS_NONE;
            }
        }

        public FPoint GetRandomNeighbor(Point target, int range, bool ignoreBlocked = false)
        {

            List<FPoint> validTiles = new List<FPoint>();
            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    FPoint newTarget = new FPoint(target.X + i + 0.5f, target.Y + j + 0.5f);
                    if (IsValidPosition(newTarget.X, newTarget.Y, MovementType.Normal, false) || ignoreBlocked)
                    {
                        validTiles.Add(newTarget);
                    }
                }
            }
            if (validTiles.Count > 0)
            {
                return validTiles[Utils.Rand() % validTiles.Count];
            }
            else
            {
                return new FPoint(target);
            }
        }

        public bool ComputePath(float x1, float y1, float x2, float y2, IList<FPoint> path, MovementType movementType, int limit = 0)
        {
            if (IsOutsideMap(x2, y2)) return false;
            path.Clear();
            if (limit == 0) limit = w * h;
            FPoint startPos = new FPoint(x1, y1);
            FPoint endPos = new FPoint(x2, y2);
            Point start = Utils.MapToCollision(startPos);
            Point end = Utils.MapToCollision(endPos);
            bool targetBlocks = false;
            int targetBlockType = colMap[end.X, end.Y];
            if (targetBlockType == BLOCKS_ENTITIES || targetBlockType == BLOCKS_ENEMIES)
            {
                targetBlocks = true;
                Unblock(end.X, end.Y);
            }
            Point current = start;
            AStarNode node = new AStarNode(start);
            node.ActualCost = 0;
            node.EstimatedCost = Utils.CalcDist(start, end);
            node.Parent = current;
            AStarContainer open = new AStarContainer(w, h, limit);
            AStarCloseContainer close = new AStarCloseContainer(w, h, limit);
            open.Add(node);
            while (!open.IsEmpty && close.Size < limit)
            {
                node = open.GetShortestF();
                current.X = node.X;
                current.Y = node.Y;
                close.Add(node);
                open.Remove(node);
                if (current.X == end.X && current.Y == end.Y) break;
                List<Point> neighbours = node.GetNeighbours(w, h);
                foreach (Point neighbour in neighbours)
                {
                    if (open.Size >= limit) break;
                    if (!IsValidTile(neighbour.X, neighbour.Y, movementType, false)) continue;
                    if (close.Exists(neighbour)) continue;
                    if (!open.Exists(neighbour))
                    {
                        AStarNode newNode = new AStarNode(neighbour);
                        newNode.ActualCost = node.ActualCost + Utils.CalcDist(current, neighbour);
                        newNode.Parent = current;
                        newNode.EstimatedCost = Utils.CalcDist(neighbour, end);
                        open.Add(newNode);
                    }
                    else
                    {
                        AStarNode i = open.Get(neighbour.X, neighbour.Y);
                        if (node.ActualCost + Utils.CalcDist(current, neighbour) < i.ActualCost)
                        {
                            Point pos = new Point(i.X, i.Y);
                            Point parent_pos = new Point(node.X, node.Y);
                            open.UpdateParent(pos, parent_pos, node.ActualCost + Utils.CalcDist(current, neighbour));
                        }
                    }
                }
            }
            if (!(current.X == end.X && current.Y == end.Y))
            {
                node = close.GetShortestH();
                current.X = node.X;
                current.Y = node.Y;
            }
            while (!(current.X == start.X && current.Y == start.Y))
            {
                path.Add(Utils.CollisionToMap(current));
                current = close.Get(current.X, current.Y).Parent;
            }

            if (targetBlocks) Block(end.X, end.Y, targetBlockType == BLOCKS_ENEMIES);
            //if (path.Count > 0 && !IsValidTile(end.X, end.Y, movementType))
            //{
            //    path.RemoveAt(0);
            //}
            return path.Count > 0;
        }

        public bool LineOfMovement(float x1, float y1, float x2, float y2, MovementType movementType)
        {
            if (IsOutsideMap(x2, y2)) return false;
            if (movementType == MovementType.Intangible) return true;
            int tileX = (int)x2;
            int tileY = (int)y2;
            bool targetBlocks = false;
            int targetBlocksType = colMap[tileX, tileY];
            if (targetBlocksType == BLOCKS_ENTITIES || targetBlocksType == BLOCKS_ENEMIES)
            {
                targetBlocks = true;
                Unblock(x2, y2);
            }
            bool hasMovement = LineCheck(x1, y1, x2, y2, CHECK_MOVEMENT, movementType);
            if (targetBlocks) Block(x2, y2, targetBlocksType == BLOCKS_ENEMIES);
            return hasMovement;
        }

        private bool LineCheck(float x1, float y1, float x2, float y2, int checkType, MovementType movementType)
        {
            float x = x1;
            float y = y1;
            float dx = Math.Abs(x2 - x1);
            float dy = Math.Abs(y2 - y1);
            float stepX = 0;
            float stepY = 0;
            int steps = (int)Math.Max(dx, dy);
            if (dx > dy)
            {
                stepX = 1;
                stepY = dy / dx;
            }
            else
            {
                stepY = 1;
                stepX = dx / dy;
            }
            if (x1 > x2) stepX = -stepX;
            if (y1 > y2) stepY = -stepY;
            if (checkType == CHECK_SIGHT)
            {
                for (int i = 0; i < steps; i++)
                {
                    x += stepX;
                    y += stepY;
                    if (IsWall(x, y)) return false;
                }
            }
            else if (checkType == CHECK_MOVEMENT)
            {
                for (int i = 0; i < steps; i++)
                {
                    x += stepX;
                    y += stepY;
                    if (!IsValidPosition(x, y, movementType, false)) return false;
                }
            }
            return true;
        }

        public bool IsValidPosition(float x, float y, MovementType movementType, bool hero, bool entity = true)
        {
            if (x < 0 || y < 0) return false;
            return IsValidTile((int)x, (int)y, movementType, hero, entity);
        }

        public bool IsValidTile(int tileX, int tileY, MovementType movementType, bool hero, bool entity = true)
        {
            if (IsOutsideMap(tileX, tileY)) return false;
            if (entity)
            {
                if (hero)
                {
                    if ((colMap[tileX, tileY] == BLOCKS_ENEMIES) && !enableAllyCollision) return true;
                }
                else if (colMap[tileX, tileY] == BLOCKS_ENEMIES) return false;
                if (colMap[tileX, tileY] == BLOCKS_ENTITIES) return false;
            }
            if (movementType == MovementType.Intangible) return true;
            if (movementType == MovementType.Flying)
            {
                return (!(colMap[tileX, tileY] == BLOCKS_ALL || colMap[tileX, tileY] == BLOCKS_ALL_HIDDEN));
            }
            if (colMap[tileX, tileY] == MAP_ONLY || colMap[tileX, tileY] == MAP_ONLY_ALT)
                return true;
            return (colMap[tileX, tileY] == BLOCKS_NONE);
        }

        public bool IsOutsideMap(int tileX, int tileY)
        {
            return (tileX < 0 || tileY < 0 || tileX >= w || tileY >= h);
        }

        public bool IsOutsideMap(float tileX, float tileY)
        {
            return IsOutsideMap((int)tileX, (int)tileY);
        }

        public bool IsEmpty(float x, float y)
        {
            int tileX = (int)x;
            int tileY = (int)y;
            if (IsOutsideMap(tileX, tileY)) return false;
            return (colMap[tileX, tileY] == BLOCKS_NONE || colMap[tileX, tileY] == MAP_ONLY || colMap[tileX, tileY] == MAP_ONLY_ALT);
        }

        public bool IsWall(float x, float y)
        {
            int tileX = (int)x;
            int tileY = (int)y;
            if (IsOutsideMap(tileX, tileY)) return true;
            return (colMap[tileX, tileY] == BLOCKS_ALL || colMap[tileX, tileY] == BLOCKS_ALL_HIDDEN);
        }

        private bool SmallStep(ref float x, ref float y, float stepX, float stepY, MovementType movementType, bool hero)
        {
            if (IsValidPosition(x + stepX, y + stepY, movementType, hero))
            {
                x += stepX;
                y += stepY;
                return true;
            }
            return false;
        }

        private bool SmallStepForcedSlideAlongGrid(ref float x, ref float y, float stepX, float stepY, MovementType movementType, bool hero)
        {
            if (IsValidPosition(x + stepX, y, movementType, hero))
            {
                if (stepX == 0) return true;
                x += stepX;
            }
            else if (IsValidPosition(x, y + stepY, movementType, hero))
            {
                if (stepY == 0) return true;
                y += stepY;
            }
            else
            {
                return false;
            }
            return true;
        }

        private static int sgn(float f)
        {
            if (f > 0) return 1;
            else if (f < 0) return -1;
            else return 0;
        }

        private bool SmallStepForcedSlid(ref float x, ref float y, float stepX, float stepY, MovementType movementType, bool hero)
        {
            const float epsilon = 0.01f;
            if (stepX != 0)
            {
                float dy = y - (float)Math.Floor(y);

                if (IsValidTile((int)x, (int)y + 1, movementType, hero)
                        && IsValidTile((int)x + sgn(stepX), (int)y + 1, movementType, hero)
                        && dy > 0.5)
                {
                    y += Math.Min(1 - dy + epsilon, (float)(Math.Abs(stepX)));
                }
                else if (IsValidTile((int)x, (int)y - 1, movementType, hero)
                         && IsValidTile((int)x + sgn(stepX), (int)y - 1, movementType, hero)
                         && dy < 0.5)
                {
                    y -= Math.Min(dy + epsilon, (float)(Math.Abs(stepX)));
                }
                else
                {
                    return false;
                }
            }
            else if (stepY != 0)
            {
                float dx = x - (float)Math.Floor(x);

                if (IsValidTile((int)x + 1, (int)y, movementType, hero)
                        && IsValidTile((int)x + 1, (int)y + sgn(stepY), movementType, hero)
                        && dx > 0.5)
                {
                    x += Math.Min(1 - dx + epsilon, (float)(Math.Abs(stepY)));
                }
                else if (IsValidTile((int)x - 1, (int)y, movementType, hero)
                         && IsValidTile((int)x - 1, (int)y + sgn(stepY), movementType, hero)
                         && dx < 0.5)
                {
                    x -= Math.Min(dx + epsilon, (float)(Math.Abs(stepY)));
                }
                else
                {
                    return false;
                }
            }
            else
            {
            }
            return true;
        }
    }
}
