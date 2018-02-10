using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileEngine;

namespace GDITiles
{
    public partial class GDIForm : Form
    {
        private GDIFileResolver gdiFileResolver;
        private GDIGraphics gdiGraphics;
        private Engine engine;
        public GDIForm()
        {
            InitializeComponent();
            gdiFileResolver = new GDIFileResolver();
            gdiGraphics = new GDIGraphics();
            engine = new Engine(gdiFileResolver, gdiGraphics);
        }


    }
}
