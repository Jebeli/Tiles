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
    using TileEngine;
    using TileEngine.Input;
    using TileEngine.Logging;
    using TileEngine.Maps;

    public class Game1 : Game
    {
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

        public Game1()
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
            fileResolver = new MONOFileResolver();
            monoGraphics = new MONOGraphics(this, Window.ClientBounds.Width, Window.ClientBounds.Height);
            engine = new Engine(fileResolver, monoGraphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            engine.SetMap(MapFactory.MakeDummyMap(engine));
            engine.SwitchToMapScreen();

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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            monoGraphics.BeginFrame(spriteBatch);
            engine.Render(gameTime.GetTimeInfo());
            monoGraphics.EndFrame(spriteBatch);
            base.Draw(gameTime);
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
