using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.Entities
{
    public class NullEntityVisual : EntityVisual
    {
        public NullEntityVisual()
            : base(null)
        {

        }

        public override void AddRenderables(float mapPosX, float mapPosY, IList<RenderTextureRegion> list)
        {
            
        }

        protected override void AdvanceAnimation()
        {
            
        }

        protected override string GetAnimationName()
        {
            return "stance";
        }

        protected override bool HasFinished()
        {
            return true;
        }

        public override void ResetActiveAnimation()
        {
            
        }

        protected override bool SetAnimation(string name, int direction)
        {
            return true;
        }

    }
}
