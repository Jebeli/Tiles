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

namespace TileEngine
{
    using System;
    using Core;
    using Files;
    using Graphics;
    using Maps;
    using Resources;
    using Screens;
    using Input;
    using Loaders;
    using System.Collections.Generic;
    using Savers;
    using TileEngine.Fonts;

    public class Engine : ITimeInfoProvider
    {
        private int maxFramesPerSecond = 60;
        private IFileResolver fileResolver;
        private IGraphics graphics;
        private ITimeInfoProvider timeProvider;
        private IInput input;
        private IFontEngine fonts;
        private ResourceManager<Texture> textureManager;
        private ResourceManager<TileSet> tileSetManager;
        private IScreen currentScreen;
        private SplashScreen splashScreen;
        private TitleScreen titleScreen;
        private LoadScreen loadScreen;
        private MapScreen mapScreen;
        private ExitScreen exitScreen;
        private TestScreen testScreen;
        private MapLoadInfo nextMap;
        private Map map;
        private Camera camera;
        private FrameCounter frameCounter;
        private List<ILoader> loaders;
        private List<ISaver> savers;
        private bool paused;

        public Engine(IFileResolver fileResolver, IGraphics graphics, IFontEngine fonts)
        {
            this.fileResolver = fileResolver;
            this.graphics = graphics;
            this.fonts = fonts;
            timeProvider = new StopWatchTimeInfoProvider();
            input = new BasicInput();
            textureManager = new ResourceManager<Texture>();
            tileSetManager = new ResourceManager<TileSet>();
            currentScreen = new NullScreen(this);
            mapScreen = new MapScreen(this);
            loadScreen = new LoadScreen(this);
            splashScreen = new SplashScreen(this);
            titleScreen = new TitleScreen(this);
            exitScreen = new ExitScreen(this);
            testScreen = new TestScreen(this);
            map = MapFactory.MakeNullMap(this);
            camera = new Camera(map);
            frameCounter = new FrameCounter();
            loaders = new List<ILoader>();
            loaders.Add(new XmlLoader(this));
            loaders.Add(new IniLoader(this));
            savers = new List<ISaver>();
            savers.Add(new XmlSaver(this));
        }

        public event EventHandler<MapEventArgs> MapLoaded;
        public event EventHandler<ScreenEventArgs> ScreenHidden;
        public event EventHandler<ScreenEventArgs> ScreenShown;
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

        public IInput Input
        {
            get { return input; }
        }

        public IFontEngine Fonts
        {
            get { return fonts; }
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

        public bool IsRunning
        {
            get { return !paused; }
        }

        public bool IsPaused
        {
            get { return paused; }
        }

        public int FPS
        {
            get { return frameCounter.FramesPerSecond; }
        }

        public string DebugInfoText
        {
            get
            {
                return $"FPS: {FPS} MAP: {map.Name}({map.Width}/{map.Height}) POS: ({camera.HoverTileX}/{camera.HoverTileY}) CAM: ({camera.CameraX}/{camera.CameraY}) SIZE: {graphics.ViewWidth}/{graphics.ViewHeight} ZOOM: {graphics.ViewScale}";
            }
        }
        public bool Update()
        {
            return Update(GetUpdateTimeInfo());
        }

        public bool Update(TimeInfo time)
        {
            if (time.ElapsedGameTime.TotalSeconds >= FrameRate)
            {
                map.Update(time);
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
            frameCounter.FrameRendering(time);
            graphics.BeginFrame();
            graphics.ClearScreen(currentScreen.BackgroundColor);
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

        public TileSet GetTileSet(string tilesetId)
        {
            TileSet tileSet = null;
            if (tileSetManager.Exists(tilesetId))
            {
                tileSet = tileSetManager.Get(tilesetId);
            }
            else
            {
                tileSet = LoadTileSet(tilesetId);
                if (tileSet != null)
                {
                    tileSetManager.Add(tileSet);
                }
                else
                {
                    Texture tex = GetTexture(tilesetId);
                    if (tex != null)
                    {
                        tileSet = new TileSet(tilesetId, tex);
                        tileSetManager.Add(tileSet);
                    }
                }
            }
            return tileSet;
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

        public void Start()
        {
            string defName = fileResolver.Resolve(@"fonts/Roboto-Regular.ttf");
            string icoName = fileResolver.Resolve(@"fonts/entypo.ttf");
            string tpzName = fileResolver.Resolve(@"fonts/Topaz.ttf");
            fonts.Init(defName, 12, tpzName, 12, icoName, 16);
            SwitchToSplashScreen();
        }

        public void Exit()
        {
            graphics.ExitRequested();
        }

        public void SwitchToMapScreen()
        {
            SetScreen(mapScreen);
        }

        public void SwitchToSplashScreen()
        {
            SetScreen(splashScreen);
        }

        public void SwitchToExitScreen()
        {
            SetScreen(exitScreen);
        }

        public void SwitchToTitleScreen()
        {
            SetScreen(titleScreen);
        }

        public void SwitchToLoadScreen()
        {
            loadScreen.MapLoadInfo = nextMap;
            SetScreen(loadScreen);
        }

        public void SwitchToTestScreen()
        {
            SetScreen(testScreen);
        }

        public void SetNextMap(string name, int posX, int posY)
        {
            nextMap = new MapLoadInfo(name, posX, posY);
        }
        public void SetMap(Map map, int posX = -1, int posY = -1)
        {
            this.map = map;
            camera = new Camera(map, posX, posY);
        }

        public Map LoadMap(string mapId, int posX = -1, int posY = -1)
        {
            Map map = null;
            foreach (ILoader loader in loaders)
            {
                if (loader.CanLoad(mapId))
                {
                    map = loader.LoadMap(mapId);
                    if (map != null)
                    {
                        map.FileName = fileResolver.Resolve(mapId);
                        if (!map.HasLayer("buildings"))
                        {
                            Layer layer = map.AddLayer("buildings");
                            layer.TileSet = MapFactory.MakeMediTileSet(this);
                        }
                        SetMap(map, posX, posY);
                        OnMapLoaded(map);
                        break;
                    }
                }
            }
            return map;
        }

        public TileSet LoadTileSet(string tileSetId)
        {
            TileSet tileSet = null;
            foreach (ILoader loader in loaders)
            {
                if (loader.CanLoad(tileSetId))
                {
                    tileSet = loader.LoadTileSet(tileSetId);
                    if (tileSet != null)
                    {
                        break;
                    }
                }
            }
            return tileSet;
        }

        public MapParallax LoadParallax(string parallaxId)
        {
            MapParallax parallax = null;
            foreach (ILoader loader in loaders)
            {
                if (loader.CanLoad(parallaxId))
                {
                    parallax = loader.LoadParallax(parallaxId);
                    if (parallax != null)
                    {
                        break;
                    }
                }
            }
            return parallax;
        }

        public void SaveTileSet(TileSet tileSet, string fileId = null)
        {
            if (fileId == null) fileId = tileSet.Name;
            fileId = fileId.Replace(".txt", ".xml");
            foreach (ISaver saver in savers)
            {
                saver.Save(tileSet, fileId);
                break;
            }
        }

        public void SaveMap(Map map, string fileId = null)
        {
            if (fileId == null) fileId = map.Name;
            fileId = fileId.Replace(".txt", ".xml");
            foreach (ISaver saver in savers)
            {
                saver.Save(map, fileId);
                break;
            }
        }

        public void SetViewSize(int width, int height)
        {
            bool sizeChanged = (graphics.Width != width || graphics.Height != height);
            graphics.SetSize(width, height);
            camera.ViewWidth = graphics.ViewWidth;
            camera.ViewHeight = graphics.ViewHeight;
            map.InvalidateRenderLists();
            if (sizeChanged && currentScreen != null)
            {
                currentScreen.SizeChanged(graphics.ViewWidth, graphics.ViewHeight);
                currentScreen.PerformLayout();
            }
        }

        public void SetViewScale(float scale)
        {
            graphics.SetScale(scale);
            camera.ViewWidth = graphics.ViewWidth;
            camera.ViewHeight = graphics.ViewHeight;
            input.ViewScale = scale;
            map.InvalidateRenderLists();
            if (currentScreen != null)
            {
                currentScreen.SizeChanged(graphics.ViewWidth, graphics.ViewHeight);
                currentScreen.PerformLayout();
            }
        }

        internal void SetScreen(IScreen screen)
        {
            currentScreen.Hide();
            OnScreenHidden(currentScreen);
            currentScreen = screen;
            currentScreen.Show();
            OnScreenShown(currentScreen);
        }

        private void OnMapLoaded(Map map)
        {
            if (map != null)
                MapLoaded?.Invoke(this, new MapEventArgs(map));
        }

        private void OnScreenHidden(IScreen screen)
        {
            if (screen != null)
                ScreenHidden?.Invoke(null, new ScreenEventArgs(screen));
        }
        private void OnScreenShown(IScreen screen)
        {
            if (screen != null)
                ScreenShown?.Invoke(null, new ScreenEventArgs(screen));
        }
    }
}
