using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI
{
    public class NewGadget
    {
        public NewGadget()
        {

        }

        public NewGadget(NewGadget other)
        {
            LeftEdge = other.LeftEdge;
            TopEdge = other.TopEdge;
            Width = other.Width;
            Height = other.Height;
            GadgetText = other.GadgetText;
            Flags = other.Flags;
            VisualInfo = other.VisualInfo;
            UserData = other.UserData;
        }
        public int LeftEdge { get; set; }
        public int TopEdge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string GadgetText { get; set; }
        public int GadgetId { get; set; }
        public NewGadgetFlags Flags { get; set; }
        public VisualInfo VisualInfo { get; set; }
        public object UserData { get; set; }
    }
}
