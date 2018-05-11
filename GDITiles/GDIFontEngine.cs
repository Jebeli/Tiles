using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;

namespace GDITiles
{
    public class GDIFontEngine : AbstractFontEngine
    {
        private System.Drawing.Text.PrivateFontCollection pfc;

        public GDIFontEngine()
        {
            pfc = new System.Drawing.Text.PrivateFontCollection();
        }

        private System.Drawing.FontFamily GetFontFamily(string name)
        {
            foreach (var ff in pfc.Families)
            {
                if (name.Contains(ff.Name))
                {
                    return ff;
                }
            }
            return null;
        }
        protected override Font MakeFont(string name, int size)
        {
            var ff = GetFontFamily(name);
            if (ff == null)
            {
                pfc.AddFontFile(name);
                ff = GetFontFamily(name);
                if (ff == null && pfc.Families.Length > 0)
                {
                    ff = pfc.Families[0];
                }
            }
            if (ff != null)
            {
                System.Drawing.Font font = new System.Drawing.Font(ff, size, System.Drawing.GraphicsUnit.Pixel);
                return new GDIFont(name, size, font);
            }
            return null;
        }

        protected override void DestroyFont(Font font)
        {
            var fnt = font.GetFont();
            fnt.Dispose();
        }
    }
}
