/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace GDITiles
{
    using System;
    using System.Windows.Forms;
    using TileEngine;
    using TileEngine.Graphics;
    using TileEngine.Logging;

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
            Logger.AddLogger(new ConsoleLogger());
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
