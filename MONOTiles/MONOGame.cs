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

namespace MONOTiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using MonoGame.Extended.BitmapFonts;
    using System.Collections.Generic;
    using TileEngine;
    using TileEngine.Graphics;
    using TileEngine.Input;
    using TileEngine.Logging;
    using TileEngine.Maps;

    public class MONOGame : Game
    {
        private const string MAPNAME1 = "maps/frontier_outpost.xml";
        private const string MAPNAME2 = "maps/part2_map.xml";
        private const string MAPNAME3 = "maps/part4_map.xml";
        private const string MAPNAME4 = "maps/frontier_outpost.txt";
        private const string MAPNAME5 = "maps/ancient_temple.txt";
        private GraphicsDeviceManager graphics;
        private ExtendedSpriteBatch spriteBatch;
        private Engine engine;
        private MONOGraphics monoGraphics;
        private MONOFileResolver fileResolver;
        private int mouseX;
        private int mouseY;
        private int mouseDelta;
        private bool leftMouseDown;
        private bool rightMouseDown;
        private List<Keys> downKeys = new List<Keys>();

        internal BitmapFont smallFont;
        internal ExtendedSpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public MONOGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            Logger.AddLogger(new ConsoleLogger());
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            Window.AllowAltF4 = false;
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            fileResolver = new MONOFileResolver("Content");
            monoGraphics = new MONOGraphics(this, Window.ClientBounds.Width, Window.ClientBounds.Height, new DebugOptions()
            {
                ShowGrid = false,
                ShowHighlight = true,
                ShowTileCounter = false,
                ShowCoordinates = false
            });
            engine = new Engine(fileResolver, monoGraphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            smallFont = Content.Load<BitmapFont>("fonts/Small");
            engine.SetNextMap(MAPNAME1, 25, 25);
            engine.Start();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (engine.Update(gameTime.GetTimeInfo()))
            {
                engine.SetViewSize(Window.ClientBounds.Width, Window.ClientBounds.Height);
                Window.Title = engine.DebugInfoText;
            }
            HandleMouse(Mouse.GetState());
            HandleKeys(Keyboard.GetState());
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            monoGraphics.BeginFrame(spriteBatch);
            engine.Render(gameTime.GetTimeInfo());
            monoGraphics.EndFrame(spriteBatch);
            base.Draw(gameTime);
        }

        private void HandleKeys(KeyboardState ks)
        {
            List<Keys> pressedKeys = new List<Keys>(ks.GetPressedKeys());
            List<Keys> copyDownKeys = new List<Keys>(downKeys);
            foreach (Keys k in copyDownKeys)
            {
                if (!pressedKeys.Contains(k))
                {
                    downKeys.Remove(k);
                    engine.Input.KeyUp((Key)k);
                }
            }
            foreach (Keys k in pressedKeys)
            {
                if (!downKeys.Contains(k))
                {
                    downKeys.Add(k);
                    engine.Input.KeyDown((Key)k);
                }
            }
        }
        private void HandleMouse(MouseState ms)
        {
            if (mouseX != ms.X || mouseY != ms.Y)
            {
                mouseX = ms.X;
                mouseY = ms.Y;
                engine.Input.MouseMove(ms.X, ms.Y, MouseButton.None);
            }
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (!leftMouseDown)
                {
                    leftMouseDown = true;
                    engine.Input.MouseDown(ms.X, ms.Y, MouseButton.Left);
                }
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                if (leftMouseDown)
                {
                    leftMouseDown = false;
                    engine.Input.MouseUp(ms.X, ms.Y, MouseButton.Left);
                }
            }
            if (ms.RightButton == ButtonState.Pressed)
            {
                if (!rightMouseDown)
                {
                    rightMouseDown = true;
                    engine.Input.MouseDown(ms.X, ms.Y, MouseButton.Right);
                }

            }
            else if (ms.RightButton == ButtonState.Released)
            {
                if (rightMouseDown)
                {
                    rightMouseDown = false;
                    engine.Input.MouseUp(ms.X, ms.Y, MouseButton.Right);
                }
            }
            if (mouseDelta != ms.ScrollWheelValue)
            {
                int dx = ms.ScrollWheelValue - mouseDelta;
                mouseDelta = ms.ScrollWheelValue;
                engine.Input.MouseWheel(ms.X, ms.Y, dx);
            }
        }
    }
}
