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
            gdiGraphics = new GDIGraphics(ClientSize.Width, ClientSize.Height);
            engine = new Engine(gdiFileResolver, gdiGraphics);
            Application.Idle += HandleApplicationIdle;
        }

        protected override void OnLoad(EventArgs e)
        {
            bg = engine.GetTexture("WP_20160910_011.jpg");
            base.OnLoad(e);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            engine.Render();
            gdiGraphics.RenderTo(e.Graphics, ClientRectangle);
            base.OnPaint(e);
        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (NativeMethods.IsApplicationIdle())
            {
                if (engine.Update())
                {
                    engine.Graphics.SetSize(ClientSize.Width, ClientSize.Height);
                    Invalidate();
                }
            }
        }
    }
}
