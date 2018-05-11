using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.XGUI
{
    public class IndexEventArgs:EventArgs
    {
        private readonly int index;

        public IndexEventArgs(int index)
        {
            this.index = index;
        }

        public int Index
        {
            get { return index; }
        }
    }
}
