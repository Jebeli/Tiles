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
    using TileEngine.Graphics;
    using TileEngine.Logging;

    public class EntityVisual
    {
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

        public void Update()
        {
            SetAnimation(stance, direction);
            AdvanceAnimation();
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
            return SetAnimation(GetAnimationName(stance), direction);
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

        public virtual void ResetActiveAnimation()
        {
            activeAnimation?.Reset();
        }

        private string GetAnimationName(EntityStance stance)
        {
            switch (stance)
            {
                case EntityStance.Standing:
                    return "stance";
                case EntityStance.Running:
                    return "run";
            }
            return "";
        }
    }
}
