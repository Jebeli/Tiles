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
    using TileEngine.Logging;
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
        private float mapDestX;
        private float mapDestY;
        private float mapTargetX;
        private float mapTargetY;
        private bool visible;
        private bool dead;
        private EntityVisual visual;
        private string animationName;
        private Dictionary<string, string> animationSetNames;
        private Dictionary<int, IList<string>> layerOrder;
        private int direction;
        private float speed;
        private MovementType movementType;
        private bool flying;
        private bool intangible;
        private bool facing;
        private float meleeRange;
        private bool trackWithCamera;
        private bool moveWithMouse;
        private bool moveWithPath;
        private bool allowedToMove;
        private bool triggersEvents;
        private IList<string> categories;
        private string rarity;
        private int level;
        private bool wander;
        private Rect wanderArea;
        private List<FPoint> wayPoints;
        private List<FPoint> path;
        private bool collided;
        private bool inCombat;
        private int turnDelay;
        private int turnTicks;
        private int wayPointPause;
        private int wayPointPauseTicks;

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
            meleeRange = 1.0f;
            mapDestX = -1;
            mapDestY = -1;
            movementType = MovementType.Normal;
            flying = false;
            intangible = false;
            allowedToMove = true;
            moveWithPath = true;
            facing = true;
            categories = new List<string>();
            rarity = "common";
            level = 1;
            wayPoints = new List<FPoint>();
            path = new List<FPoint>();
            wayPointPause = engine.MaxFramesPerSecond;
            turnDelay = engine.MaxFramesPerSecond;
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
            meleeRange = other.meleeRange;
            movementType = other.movementType;
            flying = other.flying;
            intangible = other.intangible;
            animationName = other.animationName;
            allowedToMove = other.allowedToMove;
            facing = other.facing;
            moveWithPath = other.moveWithPath;
            categories = new List<string>(other.categories);
            rarity = other.rarity;
            level = other.level;
            wander = other.wander;
            wayPointPause = other.wayPointPause;
            turnDelay = other.turnDelay;
            wayPoints = new List<FPoint>(other.wayPoints);
            wanderArea = other.wanderArea;
            path = new List<FPoint>();
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

        public bool MoveWithPath
        {
            get { return moveWithPath; }
            set { moveWithPath = value; }
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

        public bool Flying
        {
            get { return flying; }
            set
            {

                flying = value;
                AdjustMovementType();
            }
        }

        public bool Intangible
        {
            get { return intangible; }
            set
            {
                intangible = value;
                AdjustMovementType();
            }
        }

        public bool Facing
        {
            get { return facing; }
            set { facing = value; }
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

        public float MapDestX
        {
            get { return mapDestX; }
            set { mapDestX = value; }
        }

        public float MapDestY
        {
            get { return mapDestY; }
            set { mapDestY = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float MeleeRange
        {
            get { return meleeRange; }
            set { meleeRange = value; }
        }

        public MovementType MovementType
        {
            get { return movementType; }
            set { movementType = value; }
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
            set
            {
                speed = value;
                allowedToMove = speed > 0.0f;
            }
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

        public int TurnDelay
        {
            get { return turnDelay; }
            set { turnDelay = value; }
        }

        public int TurnTicks
        {
            get { return turnTicks; }
            set { turnTicks = value; }
        }

        public int WayPointPause
        {
            get { return wayPointPause; }
            set { wayPointPause = value; }
        }

        public int WayPointPauseTicks
        {
            get { return wayPointPauseTicks; }
            set { wayPointPauseTicks = value; }
        }

        public void SetWanderArea(int radius)
        {
            wanderArea.X = (int)(Math.Floor(mapPosX)) - radius;
            wanderArea.Y = (int)(Math.Floor(mapPosY)) - radius;
            wanderArea.Width = 2 * radius + 1;
            wanderArea.Height = 2 * radius + 1;
        }

        public float TargetDist
        {
            get
            {
                if (mapTargetX >= 0 && mapTargetY >= 0)
                    return Utils.CalcDist(mapPosX, mapPosY, mapTargetX, mapTargetY);
                else
                    return 0;
            }
        }

        public bool ShouldMove
        {
            get
            {
                if (inCombat)
                {
                    return TargetDist > MeleeRange;
                }
                else
                {
                    float td = TargetDist;
                    float rs = RealSpeed;
                    if (collided) return true;
                    if (td > rs) return true;
                    if (td > 0.05f) return true;

                    return false;
                    //return collided || (TargetDist > 0.1f) || (TargetDist > RealSpeed);
                }
            }
        }

        public IList<FPoint> Path
        {
            get { return path; }
        }

        public bool InCombat
        {
            get { return inCombat; }
            set { inCombat = value; }
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

        public Rect GetFrameRect(int x = 0, int y = 0)
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
            collided = false;
            path.Clear();
            mapDestX = -1;
            mapDestY = -1;
            mapTargetX = -1;
            mapTargetY = -1;
        }

        public void Run()
        {
            Stance = EntityStance.Running;
        }

        public void Update(TimeInfo time)
        {
            UpdateTimer();
            if (triggersEvents)
            {
                engine.EventManager.CheckHotSpots(mapPosX, mapPosY);
            }
            if (moveWithMouse)
            {
                if (moveWithPath)
                {
                    UpdatePathMovement();
                }
                else
                {
                    UpdateMouseMovement();
                }
            }
            else
            {
                UpdateWanderMovement();
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
                collided = false;
                mapPosX = x;
                mapPosY = y;
            }
            return fullMove;
        }

        private void UpdatePathMovement()
        {
            if (allowedToMove)
            {
                if (engine.Camera.MapClicked)
                {
                    engine.Camera.MapClickDone = true;
                    mapDestX = engine.Camera.ClickTileX + 0.5f;
                    mapDestY = engine.Camera.ClickTileY + 0.5f;
                    UpdatePath();
                }
                FollowPath();
            }
        }

        private void FindTarget()
        {
            if (wander && wayPoints.Count == 0)
            {
                FPoint waypoint = GetWanderPoint();
                wayPoints.Add(waypoint);
                wayPointPauseTicks = wayPointPause;
            }
            if (!inCombat && wayPoints.Count > 0)
            {
                FPoint waypoint = wayPoints[0];
                mapDestX = waypoint.X;
                mapDestY = waypoint.Y;
            }
        }

        private void UpdateWanderMovement()
        {
            if (allowedToMove)
            {
                FindTarget();
                UpdateMove();
            }
        }

        private void UpdateMove()
        {
            if (!inCombat && (wayPoints.Count == 0 || wayPointPauseTicks > 0))
            {
                if (Stance == EntityStance.Running)
                {
                    Stance = EntityStance.Standing;
                }
                return;
            }
            float rs = RealSpeed;
            if (rs == 0) return;
            float rt = turnDelay;
            int maxTurnTicks = (int)(1.0f / rs);
            if (rt > maxTurnTicks) { rt = maxTurnTicks; }
            engine.Collision.Unblock(mapPosX, mapPosY);
            if (facing)
            {
                turnTicks++;
                if (turnTicks > turnDelay)
                {
                    UpdatePath();
                    if (path.Count > 0)
                    {
                        mapTargetX = path[path.Count - 1].X;
                        mapTargetY = path[path.Count - 1].Y;
                    }
                    Direction = Utils.CalcDirection(mapPosX, mapPosY, mapTargetX, mapTargetY);
                    turnTicks = 0;
                }
                if (Stance == EntityStance.Standing)
                {
                    CheckMoveStateStance();
                }
                else if (Stance == EntityStance.Running)
                {
                    CheckMoveStateMove();
                }
                if (!inCombat && wayPoints.Count > 0)
                {
                    FPoint waypoint = wayPoints[0];
                    FPoint savedPos = new FPoint(mapPosX, mapPosY);
                    float waypointDist = Utils.CalcDist(waypoint, savedPos);
                    Move();
                    float newDist = Utils.CalcDist(waypoint, new FPoint(mapPosX, mapPosY));
                    mapPosX = savedPos.X;
                    mapPosY = savedPos.Y;
                    if (waypointDist <= rs || (waypointDist <= 0.5f && newDist > waypointDist))
                    {
                        mapPosX = waypoint.X;
                        mapPosY = waypoint.Y;
                        turnTicks = turnDelay;
                        wayPoints.RemoveAt(0);
                        if (wander)
                        {
                            waypoint = GetWanderPoint();
                        }
                        wayPoints.Add(waypoint);
                        wayPointPauseTicks = wayPointPause;
                    }
                }
            }
            engine.Collision.Block(mapPosX, mapPosY, false);
        }

        private FPoint GetWanderPoint()
        {
            FPoint waypoint = new FPoint();
            waypoint.X = (wanderArea.X + Utils.Rand() % wanderArea.Width) + 0.5f;
            waypoint.Y = (wanderArea.Y + Utils.Rand() % wanderArea.Height) + 0.5f;
            var coll = engine.Collision;
            if (coll != null && coll.IsValidPosition(waypoint.X, waypoint.Y, movementType, type == EntityType.Player) &&
                coll.LineOfMovement(mapPosX, mapPosY, waypoint.X, waypoint.Y, movementType))
            {
                return waypoint;
            }
            else
            {
                return new FPoint(mapPosX, mapPosY);
            }
        }

        private void NextWayPoint()
        {
            if (wayPointPauseTicks > 0) wayPointPauseTicks--;
            if (wayPoints.Count > 0 && (wayPointPauseTicks == 0 || collided))
            {
                FPoint waypoint = wayPoints[0];
                wayPoints.RemoveAt(0);
                if (wander)
                {
                    waypoint = GetWanderPoint();
                }
                wayPoints.Add(waypoint);
                wayPointPauseTicks = wayPointPause;
            }
        }

        private void FollowPath()
        {
            if (path.Count > 0)
            {
                engine.Collision.Unblock(mapPosX, mapPosY);

                mapTargetX = path[path.Count - 1].X;
                mapTargetY = path[path.Count - 1].Y;
                Direction = Utils.CalcDirection(mapPosX, mapPosY, mapTargetX, mapTargetY);
                if (Stance == EntityStance.Standing)
                {
                    CheckMoveStateStance();
                }
                else if (Stance == EntityStance.Running)
                {
                    CheckMoveStateMove();
                }
                if (path.Count > 0 && !ShouldMove)
                {
                    Logger.Detail("Entity", $"{this} path popping {path[path.Count - 1]}");
                    path.RemoveAt(path.Count - 1);
                }
                engine.Collision.Block(mapPosX, mapPosY, false);
            }
            else
            {
                Stop();
            }
        }


        private void CheckMoveStateMove()
        {
            if (!ShouldMove)
            {

                Logger.Detail("Entity", $"{this} should not move: Stopping TargetDist: {TargetDist} RealSpeed: {RealSpeed}");
                Stop();
            }
            else if (!Move())
            {
                int prevDirection = Direction;
                Direction = FaceNextBest(mapTargetX, mapTargetY);
                if (!Move())
                {
                    Logger.Detail("Entity", $"{this} collided while moving");
                    collided = true;
                    Direction = prevDirection;
                }
            }
        }

        private void CheckMoveStateStance()
        {
            if (ShouldMove)
            {
                if (Move())
                {
                    Stance = EntityStance.Running;
                }
                else
                {
                    int prevDirection = Direction;
                    Direction = FaceNextBest(mapTargetX, mapTargetY);
                    if (Move())
                    {
                        Stance = EntityStance.Running;
                    }
                    else
                    {
                        Logger.Detail("Entity", $"{this} collided while starting to move");
                        collided = true;
                        Direction = prevDirection;
                    }
                }
            }
        }


        private void UpdatePath()
        {
            if (HasValidDest)
            {
                float dist = Utils.CalcDist(mapPosX, mapPosY, mapDestX, mapDestY);
                if (dist > 0.25f)
                {
                    engine.Collision.ComputePath(mapPosX, mapPosY, mapDestX, mapDestY, path, movementType);
                }
                else
                {
                    path.Clear();
                }
            }
        }

        private void UpdateTimer()
        {
            if (wayPointPauseTicks > 0) wayPointPauseTicks--;            
        }

        public bool HasValidDest
        {
            get
            {
                return !engine.Collision.IsOutsideMap(mapDestX, mapDestY);
            }
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

        private void AdjustMovementType()
        {
            if (intangible) movementType = MovementType.Intangible;
            else if (flying) movementType = MovementType.Flying;
            else movementType = MovementType.Normal;
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

        public int FaceNextBest(float mapx, float mapy)
        {
            float dx = Math.Abs(mapx - mapPosX);
            float dy = Math.Abs(mapy - mapPosY);
            switch (direction)
            {
                case 0:
                    if (dy > dx) return 7;
                    else return 1;
                case 1:
                    if (mapy > mapPosY) return 0;
                    else return 2;
                case 2:
                    if (dx > dy) return 1;
                    else return 3;
                case 3:
                    if (mapx < mapPosX) return 2;
                    else return 4;
                case 4:
                    if (dy > dx) return 3;
                    else return 5;
                case 5:
                    if (mapy < mapPosY) return 4;
                    else return 6;
                case 6:
                    if (dx > dy) return 5;
                    else return 7;
                case 7:
                    if (mapx > mapPosX) return 6;
                    else return 0;
            }
            return 0;
        }
    }
}
