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
    using TileEngine.Maps;

    public partial class GDIForm : Form
    {
        private const string MAPNAME1 = "maps/frontier_outpost.txt";
        private const string MAPNAME2 = "maps/part2_map.xml";
        private const string MAPNAME3 = "maps/part4_map.xml";
        private const string PLAYERNAME1 = "enemies/lvl10_minotaur.txt";
        private const string PLAYERNAME2 = "npcs/male.txt";

        private GDIFileResolver gdiFileResolver;
        private GDIGraphics gdiGraphics;
        private GDIFontEngine gdiFonts;
        private Engine engine;
        public GDIForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            ClientSize = new System.Drawing.Size(800, 600);
            KeyPreview = true;
            Logger.AddLogger(new ConsoleLogger());
            gdiFileResolver = new GDIFileResolver("Content");
            gdiFonts = new GDIFontEngine();
            gdiGraphics = new GDIGraphics(ClientSize.Width, ClientSize.Height, gdiFonts, new DebugOptions()
            {
                ShowGrid = false,
                ShowHighlight = true,
                ShowTileCounter = false,
                ShowCoordinates = false
            });
            engine = new Engine(gdiFileResolver, gdiGraphics, gdiFonts);
            Application.Idle += HandleApplicationIdle;
        }

        protected override void OnLoad(EventArgs e)
        {
            engine.SetNextMap(MAPNAME1);
            engine.SetNextPlayer("Hugentobler", PLAYERNAME2);
            engine.Start();
            base.OnLoad(e);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            engine.Render();
            gdiGraphics.RenderTo(e.Graphics, ClientRectangle);
            Text = engine.DebugInfoText;
            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            engine.Input.MouseDown(e.X, e.Y, e.Button.GetMouseButton());
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            engine.Input.MouseUp(e.X, e.Y, e.Button.GetMouseButton());
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            engine.Input.MouseMove(e.X, e.Y, e.Button.GetMouseButton());
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            engine.Input.MouseWheel(e.X, e.Y, e.Delta);
            base.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    engine.Camera.CameraY--;
                    break;
                case Keys.Down:
                    engine.Camera.CameraY++;
                    break;
                case Keys.Left:
                    engine.Camera.CameraX--;
                    break;
                case Keys.Right:
                    engine.Camera.CameraX++;
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    engine.Camera.CameraY--;
                    break;
                case Keys.Down:
                    engine.Camera.CameraY++;
                    break;
                case Keys.Left:
                    engine.Camera.CameraX--;
                    break;
                case Keys.Right:
                    engine.Camera.CameraX++;
                    break;
            }
            base.OnKeyUp(e);
        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (NativeMethods.IsApplicationIdle())
            {
                if (engine.Update())
                {
                    engine.SetViewSize(ClientSize.Width, ClientSize.Height);
                    Invalidate();
                }
            }
        }

    }
}
