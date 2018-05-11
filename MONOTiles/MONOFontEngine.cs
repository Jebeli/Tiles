using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;

namespace MONOTiles
{
    public class MONOFontEngine : AbstractFontEngine
    {
        private MONOGame game;
        public MONOFontEngine(MONOGame game)
        {
            this.game = game;            
        }

        protected override Font MakeFont(string name, int size)
        {
            if (name.Contains("Icon"))
            {
                return new MONOFont(name, size, game.iconFont);
            }
            return new MONOFont(name, size, game.smallFont);
        }

        protected override void DestroyFont(Font font)
        {

        }
    }
}
