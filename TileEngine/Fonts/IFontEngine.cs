using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Fonts
{
    public interface IFontEngine
    {
        Font DefaultFont { get; }
        Font IconFont { get; }
        Font TopazFont { get; }
        Font OpenFont(string name, int size);
        void CloseFont(Font font);
        void Init(string defaultFontName, int defaultFontSize, string topazFontName, int topazFontSize, string iconFontName, int iconFontSize);

    }
}
