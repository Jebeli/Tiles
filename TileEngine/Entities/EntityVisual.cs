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
    using TileEngine.Logging;

    public class EntityVisual
    {
        private static EntityVisual nullInstance = new NullEntityVisual();
        private Animation activeAnimation;
        private AnimationSet animationSet;
        private int direction;
        private EntityStance stance;

        public EntityVisual(AnimationSet animationSet)
        {
            direction = 0;
            stance = EntityStance.Standing;
            this.animationSet = animationSet;
            SetAnimation(stance, direction);
        }

        public static EntityVisual Empty
        {
            get { return nullInstance; }
        }

        public int Direction
        {
            get { return direction; }
            set
            {
                if (direction != value)
                {
                    direction = value;
                    Logger.Detail("EntityVisual", $"Changing Direction To {direction}");
                }
            }
        }

        public EntityStance Stance
        {
            get { return stance; }
            set
            {
                if (stance != value)
                {
                    stance = value;
                    Logger.Detail("EntityVisual", $"Changing Stance To {stance}");
                }
            }
        }

        public bool AnimationFinished
        {
            get { return HasFinished(); }
        }

        public string AnimationName
        {
            get { return GetAnimationName(); }
        }

        public void Update()
        {
            SetAnimation(stance, direction);
            AdvanceAnimation();
        }

        public bool Init(EntityStance stance, int direction)
        {
            this.stance = stance;
            this.direction = direction;
            return SetAnimation(stance, direction);
        }

        public Rect GetFrameRect(int x, int y)
        {
            Rect rect = new Rect();
            List<RenderTextureRegion> r = new List<RenderTextureRegion>();
            AddRenderables(0, 0, r);
            for (int i = 0; i < r.Count; i++)
            {
                if (i == 0)
                {
                    rect = r[i].GetDestRect(x, y);
                }
                else
                {
                    rect.Union(r[i].GetDestRect(x, y));
                }
            }
            return rect;
        }

        public virtual void AddRenderables(float mapPosX, float mapPosY, IList<RenderTextureRegion> list)
        {
            if (activeAnimation != null)
            {
                var frame = activeAnimation.GetCurrentFrame(direction);
                if (frame != null)
                {
                    RenderTextureRegion ren = new RenderTextureRegion(0, frame, mapPosX, mapPosY);
                    list.Add(ren);
                }
            }
        }

        private bool SetAnimation(EntityStance stance, int direction)
        {
            bool ok = SetAnimation(GetAnimationName(stance), direction);
            this.stance = GetEntityStance(AnimationName);
            return ok;
        }

        protected virtual bool SetAnimation(string name, int direction)
        {
            if (activeAnimation != null && activeAnimation.HasName(name) && this.direction == direction) return true;
            this.direction = direction;
            if (animationSet != null)
            {
                activeAnimation = animationSet.GetAnimation(name);
                ResetActiveAnimation();
            }
            return activeAnimation != null;
        }

        protected virtual void AdvanceAnimation()
        {
            activeAnimation?.AdvanceFrame();
        }

        protected virtual bool HasFinished()
        {
            if (activeAnimation != null)
            {
                return activeAnimation.IsFinished;
            }
            else
            {
                return true;
            }
        }

        protected virtual string GetAnimationName()
        {
            if (activeAnimation != null)
            {
                return activeAnimation.Name;
            }
            return "stance";
        }

        public virtual void ResetActiveAnimation()
        {
            activeAnimation?.Reset();
        }

        private EntityStance GetEntityStance(string name)
        {
            switch (name)
            {
                case "run":
                    return EntityStance.Running;
                case "spawn":
                    return EntityStance.Spawning;
                case "swing":
                    return EntityStance.Attacking;
                case "hit":
                    return EntityStance.BeingHit;
                case "block":
                    return EntityStance.Blocking;
                case "shoot":
                    return EntityStance.Shooting;
                case "die":
                    return EntityStance.Dying;
                case "critdie":
                    return EntityStance.Dying;
                case "cast":
                    return EntityStance.Casting;
            }
            return EntityStance.Standing;
        }

        private string GetAnimationName(EntityStance stance)
        {
            switch (stance)
            {
                case EntityStance.Standing:
                    return "stance";
                case EntityStance.Running:
                    return "run";
                case EntityStance.Spawning:
                    return "spawn";
            }
            return "";
        }
    }
}
