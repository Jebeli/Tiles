﻿/*
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

namespace TileEngine
{
    using System;
    using Core;
    using Files;
    using Graphics;
    using Maps;
    using Resources;
    using Screens;

    public class Engine : ITimeInfoProvider
    {
        private int maxFramesPerSecond = 60;
        private IFileResolver fileResolver;
        private IGraphics graphics;
        private ITimeInfoProvider timeProvider;
        private ResourceManager<Texture> textureManager;
        private IScreen currentScreen;
        private MapScreen mapScreen;
        private Map map;
        private Camera camera;

        public Engine(IFileResolver fileResolver, IGraphics graphics)
        {
            this.fileResolver = fileResolver;
            this.graphics = graphics;
            timeProvider = new StopWatchTimeInfoProvider();
            textureManager = new ResourceManager<Texture>();
            currentScreen = new NullScreen(this);
            mapScreen = new MapScreen(this);
            map = MapFactory.MakeNullMap();
            camera = new Camera();
        }
        public IFileResolver FileResolver
        {
            get { return fileResolver; }
        }

        public IGraphics Graphics
        {
            get { return graphics; }
        }

        public ITimeInfoProvider TimeProvider
        {
            get { return timeProvider; }
        }

        public IScreen Screen
        {
            get { return currentScreen; }
        }

        public Map Map
        {
            get { return map; }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public int MaxFramesPerSecond
        {
            get { return maxFramesPerSecond; }
            set { maxFramesPerSecond = value; }
        }

        public double FrameRate
        {
            get { return 1.0 / maxFramesPerSecond; }
        }
        public bool Update()
        {
            return Update(GetUpdateTimeInfo());
        }

        public bool Update(TimeInfo time)
        {
            if (time.ElapsedGameTime.TotalSeconds >= FrameRate)
            {
                currentScreen.Update(time);
                return true;
            }
            return false;
        }

        public void Render()
        {
            Render(GetRenderTimeInfo());
        }

        public void Render(TimeInfo time)
        {
            graphics.BeginFrame();
            graphics.ClearScreen();
            currentScreen.Render(time);
            graphics.EndFrame();
        }

        public TimeInfo GetRenderTimeInfo()
        {
            return timeProvider.GetRenderTimeInfo();
        }

        public TimeInfo GetUpdateTimeInfo()
        {
            return timeProvider.GetUpdateTimeInfo();
        }
        public TimeSpan GetCurrentTime()
        {
            return timeProvider.GetCurrentTime();
        }

        public Texture GetTexture(string textureId)
        {
            Texture tex = null;
            if (textureManager.Exists(textureId))
            {
                tex = textureManager.Get(textureId);
            }
            else
            {
                tex = graphics.GetTexture(textureId, fileResolver);
                if (tex != null) textureManager.Add(tex);
            }
            return tex;
        }

        public void SwitchToMapScreen()
        {
            SetScreen(mapScreen);
        }
        public void SetMap(Map map)
        {
            this.map = map;
        }

        internal void SetScreen(IScreen screen)
        {
            currentScreen.Hide();
            currentScreen = screen;
            currentScreen.Show();
        }


    }
}
