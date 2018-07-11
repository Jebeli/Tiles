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

    public class MultiPartEntityVisual : EntityVisual
    {
        private IDictionary<string, AnimationSet> animationSets;
        private IDictionary<int, IList<string>> layerOrder;
        private IList<Animation> activeAnimations;

        public MultiPartEntityVisual(IDictionary<string, AnimationSet> animationSets, IDictionary<int, IList<string>> layerOrder)
            : base(null)
        {
            this.animationSets = animationSets;
            this.layerOrder = layerOrder;
            activeAnimations = new List<Animation>();
        }

        protected override void AdvanceAnimation()
        {
            foreach (var anim in activeAnimations)
            {
                anim.AdvanceFrame();
            }
        }

        public override void ResetActiveAnimation()
        {
            foreach (var anim in activeAnimations)
            {
                anim.Reset();
            }
        }

        protected override bool SetAnimation(string name, int direction)
        {
            if (activeAnimations == null) return false;
            if (activeAnimations.Count > 0 && activeAnimations[0].HasName(name) && Direction == direction) return true;
            Direction = direction;
            activeAnimations.Clear();
            if (layerOrder.TryGetValue(direction, out var order))
            {
                var sorted = animationSets.OrderBy(it => order.IndexOf(it.Key));
                foreach (var animSet in sorted)
                {
                    var anim = animSet.Value.GetAnimation(name);
                    activeAnimations.Add(anim);
                }
            }
            else
            {
                foreach (var animSet in animationSets)
                {
                    var anim = animSet.Value.GetAnimation(name);
                    activeAnimations.Add(anim);
                }
            }
            return activeAnimations.Count > 0;
        }

        public override void AddRenderables(float mapPosX, float mapPosY, IList<RenderTextureRegion> list)
        {
            int i = 0;
            foreach(var anim in activeAnimations)
            {
                var frame = anim.GetCurrentFrame(Direction);
                if (frame != null)
                {
                    i++;
                    RenderTextureRegion ren = new RenderTextureRegion(0, frame, mapPosX, mapPosY);
                    ren.Prio = i;
                    list.Add(ren);
                }
            }
        }

    }
}
