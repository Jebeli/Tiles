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
using TileEngine.Graphics;

namespace GDITiles
{
    public partial class GDIForm : Form
    {
        private GDIFileResolver gdiFileResolver;
        private GDIGraphics gdiGraphics;
        private Engine engine;
        private Texture bg;
        public GDIForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            gdiFileResolver = new GDIFileResolver("Content/images");
            gdiGraphics = new GDIGraphics();
            engine = new Engine(gdiFileResolver, gdiGraphics);
        }

        protected override void OnLoad(EventArgs e)
        {
            bg = engine.GetTexture("WP_20160910_011.jpg");
            base.OnLoad(e);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(bg.GetBitmap(), 0, 0);
            base.OnPaint(e);
        }


    }
}
