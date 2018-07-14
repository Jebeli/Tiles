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

namespace TileEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;
    using TileEngine.Input;
    using TileEngine.Maps;

    public enum EntityStance
    {
        Standing,
        Running,
        Spawning,
        Attacking,
        Shooting,
        Casting,
        Dying,
        BeingHit,
        Blocking
    }

    public enum EntityType
    {
        Player,
        Enemy,
        NPC
    }

    public class Entity : NamedObject
    {

        private static readonly float M_SQRT2 = (float)Math.Sqrt(2.0);
        private static readonly float M_SQRT2INV = 1.0f / M_SQRT2;
        private static readonly int[] directionDeltaX = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private static readonly int[] directionDeltaY = { 1, 0, -1, -1, -1, 0, 1, 1 };
        private static readonly float[] speedMultiplyer = { M_SQRT2INV, 1.0f, M_SQRT2INV, 1.0f, M_SQRT2INV, 1.0f, M_SQRT2INV, 1.0f };

        private Engine engine;
        private EntityType type;
        private float mapPosX;
        private float mapPosY;
        private bool visible;
        private bool dead;
        private EntityVisual visual;
        private string animationName;
        private Dictionary<string, string> animationSetNames;
        private Dictionary<int, IList<string>> layerOrder;
        private int direction;
        private float speed;
        private MovementType movementType;
        private bool trackWithCamera;
        private bool moveWithMouse;
        private bool allowedToMove;
        private bool triggersEvents;
        private IList<string> categories;
        private string rarity;
        private int level;
        private bool wander;
        private Rect wanderArea;
        private List<FPoint> wayPoints;


        private float effectSpeed = 100.0f;

        public Entity(Engine engine, string name)
            : base(name)
        {
            this.engine = engine;
            visual = EntityVisual.Empty;
            animationSetNames = new Dictionary<string, string>();
            layerOrder = new Dictionary<int, IList<string>>();
            visible = true;
            dead = false;
            direction = 0;
            speed = 0.1f;
            movementType = MovementType.Normal;
            allowedToMove = true;
            categories = new List<string>();
            rarity = "common";
            level = 1;
            wayPoints = new List<FPoint>();
        }

        public Entity(Entity other)
            : base(other)
        {
            engine = other.engine;
            type = other.type;
            visual = EntityVisual.Empty;
            animationSetNames = new Dictionary<string, string>(other.animationSetNames);
            layerOrder = new Dictionary<int, IList<string>>(other.layerOrder);
            visible = other.visible;
            direction = other.direction;
            speed = other.speed;
            movementType = other.movementType;
            animationName = other.animationName;
            allowedToMove = other.allowedToMove;
            categories = new List<string>(other.categories);
            rarity = other.rarity;
            level = other.level;
            wander = other.wander;
            wayPoints = new List<FPoint>(other.wayPoints);
            wanderArea = other.wanderArea;
        }

        public EntityType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool TrackWithCamera
        {
            get { return trackWithCamera; }
            set { trackWithCamera = value; }
        }

        public bool MoveWithMouse
        {
            get { return moveWithMouse; }
            set { moveWithMouse = value; }
        }

        public bool TriggersEvents
        {
            get { return triggersEvents; }
            set { triggersEvents = value; }
        }

        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        public float MapPosX
        {
            get { return mapPosX; }
            set { mapPosX = value; }
        }

        public float MapPosY
        {
            get { return mapPosY; }
            set { mapPosY = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public EntityStance Stance
        {
            get
            {
                return visual.Stance;
            }
            set
            {
                visual.Stance = value;
            }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float RealSpeed
        {
            get { return speed * speedMultiplyer[direction] * effectSpeed / 100.0f; }
        }

        public IList<string> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        public string Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public bool Wander
        {
            get { return wander; }
            set { wander = value; }
        }

        public Rect WanderArea
        {
            get { return wanderArea; }
            set { wanderArea = value; }
        }

        public IList<FPoint> WayPoints
        {
            get { return wayPoints; }
            set
            {
                wayPoints.Clear();
                if (value != null)
                {
                    wayPoints.AddRange(value);
                }
            }
        }

        public void SetWanderArea(int radius)
        {
            wanderArea.X = (int)(Math.Floor(mapPosX)) - radius;
            wanderArea.Y = (int)(Math.Floor(mapPosY)) - radius;
            wanderArea.Width = 2 * radius + 1;
            wanderArea.Height = 2 * radius + 1;
        }

        public string AnimationName
        {
            get { return animationName; }
            set { animationName = value; }
        }

        public void AddAnimationSetName(string id, string name)
        {
            animationSetNames[id] = name;
        }

        public void SetLayerOrder(int layer, IList<string> order)
        {
            layerOrder[layer] = order;
        }

        public Rect GetFrameRect(int x=0, int y=0)
        {
            return visual.GetFrameRect(x, y);
        }

        public void CreateVisual()
        {
            if (animationSetNames.Count > 1)
            {
                var animationSets = new Dictionary<string, AnimationSet>();
                foreach (var asn in animationSetNames)
                {
                    var animSet = engine.LoadAnimationSet(asn.Value);
                    if (animSet != null)
                    {
                        animationSets[asn.Key] = animSet;
                    }
                }
                CreateVisual(animationSets);
            }
            else if (animationSetNames.Count == 1)
            {
                animationName = animationSetNames.Values.First();
                var animationSet = engine.LoadAnimationSet(animationName);
                CreateVisual(animationSet);
            }
            else if (!string.IsNullOrEmpty(animationName))
            {
                var animationSet = engine.LoadAnimationSet(animationName);
                CreateVisual(animationSet);
            }
            else
            {
                visual = EntityVisual.Empty;
            }
        }

        public void CreateVisual(AnimationSet animationSet)
        {
            visual = new EntityVisual(animationSet);
        }

        public void CreateVisual(IDictionary<string, AnimationSet> animationSets)
        {
            visual = new MultiPartEntityVisual(animationSets, layerOrder);
        }

        public bool InitAnimation(EntityStance stance, int direction)
        {
            this.direction = direction;
            return visual.Init(stance, direction);
        }

        public void Stop()
        {
            Stance = EntityStance.Standing;
        }

        public void Run()
        {
            Stance = EntityStance.Running;
        }

        public void Update(TimeInfo time)
        {
            if (moveWithMouse)
            {
                UpdateMouseMovement();
            }
            if (trackWithCamera)
            {
                engine.Camera.SetMapPosition(mapPosX, mapPosY);
            }
            if (ShouldRevertToStanding())
            {
                Stance = EntityStance.Standing;
            }
            visual.Direction = direction;
            visual.Update();
            if (triggersEvents)
            {
                engine.EventManager.CheckHotSpots(mapPosX, mapPosY);
                engine.EventManager.CheckEvents(mapPosX, mapPosY);
            }
        }

        public void AddRenderables(IList<RenderTextureRegion> list)
        {
            visual.AddRenderables(mapPosX, mapPosY, list);
        }

        public bool Move()
        {
            float moveSpeed = RealSpeed;
            float dx = moveSpeed * directionDeltaX[direction];
            float dy = moveSpeed * directionDeltaY[direction];
            float x = mapPosX;
            float y = mapPosY;
            bool fullMove = engine.Collision.Move(ref x, ref y, dx, dy, movementType, true);
            if (fullMove)
            {
                mapPosX = x;
                mapPosY = y;
            }
            return fullMove;
        }

        private void UpdateMouseMovement()
        {
            if (allowedToMove)
            {
                engine.Collision.Unblock(mapPosX, mapPosY);
                switch (Stance)
                {
                    case EntityStance.Standing:
                        if (PressingMove())
                        {
                            SetDirectionFromMouse();
                            if (Move())
                            {
                                Run();
                            }
                        }
                        break;
                    case EntityStance.Running:
                        SetDirectionFromMouse();
                        if (!PressingMove())
                        {
                            Stop();
                        }
                        else if (!Move())
                        {
                            Stop();
                        }
                        break;
                }
                engine.Collision.Block(mapPosX, mapPosY, false);
            }
        }

        private void SetDirectionFromMouse()
        {

            engine.Camera.ScreenToMap(engine.Input.ScaledMouseX, engine.Input.ScaledMouseY, mapPosX, mapPosY, out float mapX, out float mapY);
            direction = Utils.CalcDirection(mapPosX, mapPosY, mapX, mapY);
        }

        private bool PressingMove()
        {
            if (!engine.GUIUseseMouse && engine.Input.IsDown(MouseButton.Left))
            {
                return true;
            }
            return false;
        }


        private bool ShouldRevertToStanding()
        {
            if (dead) return false;
            switch (visual.Stance)
            {
                case EntityStance.Standing:
                case EntityStance.Running:
                case EntityStance.Dying:
                    return false;
            }
            return visual.AnimationFinished;
        }
    }
}
