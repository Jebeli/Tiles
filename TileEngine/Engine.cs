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
    using TileEngine.Events;
    using System.Text;
    using TileEngine.Entities;
    using TileEngine.Logging;
    using TileEngine.Audio;

    public class Engine : ITimeInfoProvider
    {
        private int maxFramesPerSecond = 60;
        private float interactRange = 2.5f;
        private IFileResolver fileResolver;
        private IGraphics graphics;
        private ITimeInfoProvider timeProvider;
        private IInput input;
        private IFontEngine fonts;
        private ISounds sounds;
        private ResourceManager<Texture> textureManager;
        private ResourceManager<TileSet> tileSetManager;
        private ResourceManager<Music> musicManager;
        private ResourceManager<Sound> soundManager;
        private EventManager eventManager;
        private EntityManager entityManager;
        private EnemyManager enemyManager;
        private IScreen currentScreen;
        private SplashScreen splashScreen;
        private TitleScreen titleScreen;
        private LoadScreen loadScreen;
        private MapScreen mapScreen;
        private ExitScreen exitScreen;
        private TestScreen testScreen;
        private MapLoadInfo nextMap;
        private EntityLoadInfo nextPlayer;
        private List<string> nextEnemyTemplates;
        private Map map;
        private Camera camera;
        private FrameCounter frameCounter;
        private List<ILoader> loaders;
        private List<ISaver> savers;
        private bool paused;
        private string playerName;
        private string playerId;
        private Entity player;
        private bool guiUsesMouse;

        public Engine(IFileResolver fileResolver, IGraphics graphics, IFontEngine fonts, ISounds sounds = null)
        {
            this.fileResolver = fileResolver;
            this.graphics = graphics;
            this.fonts = fonts;
            this.sounds = sounds;
            if (sounds == null) { this.sounds = new NoSounds(); }
            timeProvider = new StopWatchTimeInfoProvider();
            input = new BasicInput();
            textureManager = new ResourceManager<Texture>();
            tileSetManager = new ResourceManager<TileSet>();
            musicManager = new ResourceManager<Music>();
            soundManager = new ResourceManager<Sound>();
            eventManager = new EventManager(this);
            entityManager = new EntityManager(this);
            enemyManager = new EnemyManager(this);
            currentScreen = new NullScreen(this);
            mapScreen = new MapScreen(this);
            loadScreen = new LoadScreen(this);
            splashScreen = new SplashScreen(this);
            titleScreen = new TitleScreen(this);
            exitScreen = new ExitScreen(this);
            testScreen = new TestScreen(this);
            map = MapFactory.MakeNullMap(this);
            camera = new Camera(this, map);
            frameCounter = new FrameCounter();
            loaders = new List<ILoader>();
            loaders.Add(new XmlLoader(this));
            loaders.Add(new IniLoader(this));
            savers = new List<ISaver>();
            savers.Add(new XmlSaver(this));
            nextEnemyTemplates = new List<string>();
        }

        public event EventHandler<MapEventArgs> MapLoaded;
        public event EventHandler<EntityEventArgs> PlayerLoaded;
        public event EventHandler<EventEventArgs> EventExecuting;
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

        public ISounds Sounds
        {
            get { return sounds; }
        }

        public IScreen Screen
        {
            get { return currentScreen; }
        }

        public Map Map
        {
            get { return map; }
        }

        public MapCollision Collision
        {
            get { return map?.Collision; }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public EventManager EventManager
        {
            get { return eventManager; }
        }

        public EntityManager EntityManager
        {
            get { return entityManager; }
        }

        public EnemyManager EnemyManager
        {
            get { return enemyManager; }
        }

        public MapScreen MapScreen
        {
            get { return mapScreen; }
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

        public float InteractRange
        {
            get { return interactRange; }
        }

        public bool GUIUseseMouse
        {
            get { return guiUsesMouse; }
            set { guiUsesMouse = value; }
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
                StringBuilder sb = new StringBuilder();
                sb.Append("FPS: ");
                sb.Append(FPS);
                sb.Append(" ");
                sb.Append(map.Name);
                sb.Append(" (");
                sb.Append(map.Width);
                sb.Append("/");
                sb.Append(map.Height);
                sb.Append(") POS: (");
                sb.Append(camera.HoverTileX);
                sb.Append("/");
                sb.Append(camera.HoverTileY);
                sb.Append(") CAM: (");
                sb.Append(camera.CameraX.ToString("F"));
                sb.Append("/");
                sb.Append(camera.CameraY.ToString("F"));
                sb.Append(") SIZE: (");
                sb.Append(graphics.ViewWidth);
                sb.Append("/");
                sb.Append(graphics.ViewHeight);
                sb.Append(") ZOOM: ");
                sb.Append(graphics.ViewScale);
                var events = map.GetEventsAt(camera.HoverTileX, camera.HoverTileY);
                foreach (var evt in events)
                {
                    sb.Append(" ");
                    sb.Append(evt.ToString());
                }
                return sb.ToString();

                //return $"FPS: {FPS} MAP: {map.Name}({map.Width}/{map.Height}) POS: ({camera.HoverTileX}/{camera.HoverTileY}) CAM: ({camera.CameraX}/{camera.CameraY}) SIZE: {graphics.ViewWidth}/{graphics.ViewHeight} ZOOM: {graphics.ViewScale}";
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

        public Music GetMusic(string musicId)
        {
            Music mus = null;
            if (musicManager.Exists(musicId))
            {
                mus = musicManager.Get(musicId);
            }
            else
            {
                mus = sounds.GetMusic(musicId, fileResolver);
                if (mus != null) musicManager.Add(mus);
            }
            return mus;
        }

        public Sound GetSound(string soundId)
        {
            Sound snd = null;
            if (soundManager.Exists(soundId))
            {
                snd = soundManager.Get(soundId);
            }
            else
            {
                snd = sounds.GetSound(soundId, fileResolver);
                if (snd != null) soundManager.Add(snd);
            }
            return snd;
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
            loadScreen.PlayerLoadInfo = nextPlayer;
            SetScreen(loadScreen);
        }

        public void SwitchToTestScreen()
        {
            SetScreen(testScreen);
        }

        public void SetNextMap(string name, int posX = -1, int posY = -1)
        {
            nextMap = new MapLoadInfo(name, posX, posY);
        }

        public void SetNextPlayer(string pname, string name, int posX = -1, int posY = -1)
        {
            playerName = pname;
            playerId = name;
            nextPlayer = new EntityLoadInfo(playerName, playerId, posX, posY);
        }

        public void ResetNextPlayer()
        {
            SetNextPlayer(playerName, playerId, -1, -1);
        }

        public void SetMap(Map map, int posX = -1, int posY = -1)
        {
            InitEnemyManager();
            entityManager.Clear();
            eventManager.Clear();
            enemyManager.Clear();
            sounds.Reset();

            this.map = map;
            this.map.InitCollision();
            camera = new Camera(this, map, posX, posY);
            TransferEvents(map);
            TransferNPCs(map);
            TransferEnemyGroups(map);
            PlayMusic(map.MusicName);
        }

        public Map LoadMap(string mapId, int posX = -1, int posY = -1)
        {
            Map map = null;
            foreach (ILoader loader in loaders)
            {
                if (loader.DetectFileTpye(mapId) == FileType.Map)
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
                if (loader.DetectFileTpye(tileSetId) == FileType.TileSet)
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
                if (loader.DetectFileTpye(parallaxId) == FileType.Parallax)
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

        public AnimationSet LoadAnimationSet(string animId)
        {
            AnimationSet animationSet = null;
            foreach (ILoader loader in loaders)
            {
                if (loader.DetectFileTpye(animId) == FileType.AnimationSet)
                {
                    animationSet = loader.LoadAnimation(animId);
                    if (animationSet != null)
                    {
                        break;
                    }
                }
            }
            return animationSet;
        }

        public Entity LoadEntity(string fileId)
        {
            Entity entity = null;
            foreach (ILoader loader in loaders)
            {
                if (loader.DetectFileTpye(fileId) == FileType.Entity)
                {
                    entity = loader.LoadEntity(fileId);
                    if (entity != null)
                    {
                        break;
                    }
                }
            }
            return entity;
        }

        public Entity LoadNPC(string fileId, int posX, int posY)
        {
            Entity npc = LoadEntity(fileId);
            if (npc != null)
            {
                npc.Type = EntityType.NPC;
                npc.MapPosX = posX + 0.5f;
                npc.MapPosY = posY + 0.5f;
            }
            return npc;
        }

        public Entity LoadEnemy(string fileId)
        {
            Entity enemy = LoadEntity(fileId);
            if (enemy != null)
            {
                enemy.Type = EntityType.Enemy;
            }
            return enemy;
        }

        public Entity LoadPlayer(string fileId, int posX = -1, int posY = -1)
        {
            player = LoadEntity(fileId);
            if (player != null)
            {
                player.Type = EntityType.Player;
                if (posX >= 0 && posY >= 0)
                {
                    player.MapPosX = posX + 0.5f;
                    player.MapPosY = posY + 0.5f;
                }
                else
                {
                    player.MapPosX = map.StartX + 0.5f;
                    player.MapPosY = map.StartY + 0.5f;
                }
                player.TrackWithCamera = true;
                player.MoveWithMouse = true;
                player.TriggersEvents = true;
                entityManager.AddEntity(player);
                OnPlayerLoaded(player);
            }
            return player;
        }

        public void PlayMusic(string name)
        {
            Music music = GetMusic(name);
            sounds.PlayMusic(music);
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

        public bool CanExecuteEvent(Event evt)
        {
            return OnEventExecuting(evt);
        }

        public void ExecuteEventComponent(Event evt, EventComponent ec)
        {
            switch (ec.Type)
            {
                case EventComponentType.InterMap:
                    if (TravelTo(ec.StringParam, ec.MapX, ec.MapY))
                    {
                        Logger.Info("Engine", $"Travelled to {ec.StringParam}");
                    }
                    break;
                case EventComponentType.IntraMap:
                    if (TeleportTo(ec.MapX, ec.MapY))
                    {

                    }
                    break;
                case EventComponentType.MapMod:
                    foreach (var mod in ec.MapMods)
                    {
                        map.DoMapMod(mod);
                    }
                    break;
                case EventComponentType.Spawn:
                    foreach (var spawn in ec.MapSpawns)
                    {
                        enemyManager.SpawnMapSpawn(spawn);
                    }
                    break;
                case EventComponentType.Music:
                    PlayMusic(ec.StringParam);
                    break;
                case EventComponentType.SoundFX:
                    FPoint pos = new FPoint();
                    bool loop = false;
                    if (ec.MapX != -1 && ec.MapY != -1)
                    {
                        if (ec.MapX != 0 && ec.MapY != 0)
                        {
                            pos.X = ec.MapX + 0.5f;
                            pos.Y = ec.MapY + 0.5f;
                        }
                    }
                    else if (evt.PosX != 0 && evt.PosY != 0)
                    {
                        pos.X = evt.PosX + 0.5f;
                        pos.Y = evt.PosY + 0.5f;
                    }
                    if (evt.Type == EventType.Load)
                    {
                        loop = true;
                    }
                    Sound sound = GetSound(ec.StringParam);
                    if (sound != null)
                    {
                        sounds.PlaySound(sound, pos, loop);
                    }
                    break;
            }
        }

        public bool TravelTo(string mapName, int posX, int posY)
        {
            SetNextMap(mapName, posX, posY);
            SetNextPlayer(playerName, playerId, posX, posY);
            SwitchToLoadScreen();
            return true;
        }

        public bool TeleportTo(int posX, int posY)
        {
            map.Collision.Unblock(player.MapPosX, player.MapPosY);
            player.MapPosX = posX + 0.5f;
            player.MapPosY = posY + 0.5f;
            map.Collision.Block(player.MapPosX, player.MapPosY, false);
            return true;
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

        private void OnPlayerLoaded(Entity player)
        {
            if (player != null)
                PlayerLoaded?.Invoke(this, new EntityEventArgs(player));
        }

        private bool OnEventExecuting(Event evt)
        {
            var args = new EventEventArgs(evt);
            EventExecuting?.Invoke(null, args);
            return !args.Cancel;
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

        public void InitEnemyManager()
        {
            if (!enemyManager.Initialized)
            {
                var enemyDir = fileResolver.ResolveDir("enemies");
                if (!string.IsNullOrEmpty(enemyDir))
                {
                    var fileList = fileResolver.GetFiles(enemyDir);
                    List<string> shortNames = new List<string>();
                    foreach (string s in fileList)
                    {
                        shortNames.Add(s.Substring(s.IndexOf("enemies")));
                    }
                    shortNames.Sort();
                    AddEnemyTemplates(shortNames);
                    TransferEnemyTemplates();
                }
            }
        }

        public void TransferEnemyTemplates()
        {
            Logger.Info("Enemy", "Adding Enemy Templates");
            //Load();
            enemyManager.ClearAll();
            enemyManager.AddEnemyTemplates(nextEnemyTemplates);
            nextEnemyTemplates.Clear();
            //Run();
        }

        public void AddEnemyTemplates(IEnumerable<string> files)
        {
            nextEnemyTemplates.AddRange(files);
        }

        private void TransferEvents(Map map)
        {
            foreach (var info in map.LoadEvents)
            {
                eventManager.AddEvent(info);
            }
            map.ClearLoadEvents();
        }

        private void TransferNPCs(Map map)
        {
            foreach (var info in map.LoadNPCs)
            {
                Entity npc = LoadNPC(info.EntityId, info.PosX, info.PosY);
                if (npc != null)
                {
                    entityManager.AddEntity(npc);
                    CreateNPCEvent(npc);
                }
            }
            map.ClearLoadNPCs();
        }

        private void TransferEnemyGroups(Map map)
        {
            foreach (var eg in map.LoadEnemyGroups)
            {
                enemyManager.AddEnemyGroup(eg);
            }
            enemyManager.SpwanEnemies();
            map.ClearEnemyGroups();
        }

        private void CreateNPCEvent(Entity npc)
        {
            Event evt = new Event(this, EventType.Trigger, npc.Name);
            evt.PosX = (int)npc.MapPosX;
            evt.PosY = (int)npc.MapPosY;
            evt.Width = 1;
            evt.Height = 1;
            evt.HotSpotFromLocation();
            evt.AddComponent(EventComponentType.Tooltip, npc.Name);
            var npcHS = evt.AddComponent(EventComponentType.NPCHotspot);
            Rect hs = npc.GetFrameRect();
            npcHS.MapX = hs.X;
            npcHS.MapY = hs.Y;
            npcHS.MapWidth = hs.Width;
            npcHS.MapHeight = hs.Height;
            eventManager.AddEvent(evt);
        }
    }
}
