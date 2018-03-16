using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;
using TileEngine.Screens;

namespace TileEngine.GUI
{
    public class GadgetInfo
    {
        public IScreen Screen { get; set; }
        public Window Window { get; set; }
        public Requester Requester { get; set; }
        public IGraphics RastPort { get; set; }
        public IBox Domain { get; set; }
        public int DetailPen { get; set; }
        public int BlockPen { get; set; }
        public DrawInfo DrawInfo { get; set; }
    }
}
