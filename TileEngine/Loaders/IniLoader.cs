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

namespace TileEngine.Loaders
{
    using Graphics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Entities;
    using TileEngine.Events;
    using TileEngine.Maps;

    public class IniLoader : AbstractLoader
    {
        public IniLoader(Engine engine)
            : base(engine, ".txt", ".ini")
        {
        }

        public override FileType DetectFileTpye(string fileId)
        {
            FileType type = FileType.None;
            if (FitsExtension(fileId))
            {
                Stream stream = engine.FileResolver.OpenFile(fileId);
                if (stream != null)
                {
                    IniFile ini = new IniFile(stream);
                    int width = ini.ReadInt("header", "width");
                    int height = ini.ReadInt("header", "height");
                    string title = ini.ReadString("header", "title");
                    string tileSetId = ini.ReadString("header", "tileset");
                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(tileSetId) && width > 0 && height > 0)
                    {
                        type = FileType.Map;
                    }
                    else
                    {
                        string img = ini.ReadString("", "img");
                        string tile = ini.ReadString("", "tile");
                        if (!string.IsNullOrEmpty(img) && !string.IsNullOrEmpty(tile))
                        {
                            type = FileType.TileSet;
                        }
                        else
                        {
                            string pName = ini.ReadString("layer", "image");
                            if (!string.IsNullOrEmpty(pName))
                            {
                                type = FileType.Parallax;
                            }
                            else
                            {
                                string aname = ini.ReadString("", "name");
                                string anims = ini.ReadString("", "animations");
                                if (!string.IsNullOrEmpty(aname) && !string.IsNullOrEmpty(anims))
                                {
                                    type = FileType.Entity;
                                }
                                else
                                {
                                    string gfx = ini.ReadString("", "gfx");
                                    if (!string.IsNullOrEmpty(aname) && !string.IsNullOrEmpty(gfx))
                                    {
                                        type = FileType.Entity;
                                    }
                                    else
                                    {
                                        string level = ini.ReadString("", "level");
                                        if (!string.IsNullOrEmpty(aname) && !string.IsNullOrEmpty(level))
                                        {
                                            type = FileType.Entity;
                                        }
                                        else
                                        {
                                            string aImg = ini.ReadString("", "image");
                                            if (!string.IsNullOrEmpty(aImg))
                                            {
                                                foreach (var sec in ini.Sections)
                                                {
                                                    string frames = sec.ReadString("frames");
                                                    if (!string.IsNullOrEmpty(frames))
                                                    {
                                                        type = FileType.AnimationSet;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    stream.Dispose();
                }
            }
            return type;
        }

        public override Map LoadMap(string fileId)
        {
            Map map = null;
            if (FitsExtension(fileId))
            {
                Stream stream = engine.FileResolver.OpenFile(fileId);
                if (stream != null)
                {
                    IniFile ini = GetIni(stream);
                    map = InternalLoadMap(ini, fileId);
                    stream.Dispose();
                }
            }
            return map;
        }

        public override TileSet LoadTileSet(string fileId)
        {
            TileSet tileSet = null;
            if (FitsExtension(fileId))
            {
                Stream stream = engine.FileResolver.OpenFile(fileId);
                if (stream != null)
                {
                    IniFile ini = GetIni(stream);
                    tileSet = InternalLoadTileSet(ini, fileId);
                    stream.Dispose();
                }
            }
            return tileSet;
        }

        public override MapParallax LoadParallax(string fileId)
        {
            MapParallax parallax = null;
            if (FitsExtension(fileId))
            {
                Stream stream = engine.FileResolver.OpenFile(fileId);
                if (stream != null)
                {
                    IniFile ini = GetIni(stream);
                    parallax = InternalLoadParallax(ini, fileId);
                    stream.Dispose();
                }
            }
            return parallax;
        }

        public override AnimationSet LoadAnimation(string fileId)
        {
            AnimationSet animationSet = null;
            if (FitsExtension(fileId))
            {
                Stream stream = engine.FileResolver.OpenFile(fileId);
                if (stream != null)
                {
                    IniFile ini = GetIni(stream);
                    animationSet = InternalLoadAnimationSet(ini, fileId);
                    stream.Dispose();
                }
            }
            return animationSet;
        }

        public override Entity LoadEntity(string fileId)
        {
            Entity entity = null;
            if (FitsExtension(fileId))
            {
                Stream stream = engine.FileResolver.OpenFile(fileId);
                if (stream != null)
                {
                    IniFile ini = GetIni(stream);
                    entity = InternalLoadEntity(ini, fileId);
                    stream.Dispose();
                }
            }
            return entity;
        }

        private Map InternalLoadMap(IniFile ini, string fileId)
        {
            Map map = null;
            int width = ini.ReadInt("header", "width");
            int height = ini.ReadInt("header", "height");
            int tileWidth = ini.ReadInt("header", "tilewidth");
            int tileHeight = ini.ReadInt("header", "tileheight");
            string orientation = ini.ReadString("header", "orientation");
            string title = ini.ReadString("header", "title");
            string tileSetId = ini.ReadString("header", "tileset");
            int[] startPos = ini.ReadString("header", "hero_pos").ToIntValues();
            TileSet tileSet = engine.LoadTileSet(tileSetId);
            if (tileSet != null)
            {
                MapOrientation mapOrientation;
                if (Enum.TryParse(orientation, true, out mapOrientation))
                {
                    map = new Map(title, width, height, tileWidth, tileHeight, mapOrientation);
                    string data = null;
                    int[] values = null;
                    string[] sValues = null;
                    if (startPos.Length >= 2)
                    {
                        map.StartX = startPos[0];
                        map.StartY = startPos[1];
                    }
                    foreach (var sec in ini.Sections)
                    {
                        switch (sec.Name)
                        {
                            case "layer":
                                string layerType = sec.ReadString("type");
                                StringBuilder sb = new StringBuilder();
                                foreach (var k in sec.KeyList)
                                {
                                    if (k.Ident.Equals(""))
                                    {
                                        sb.Append(k.Value);
                                    }
                                }
                                data = sb.ToString();
                                values = data.ToIntValues();
                                Layer layer = map.AddLayer(layerType);
                                layer.TileSet = tileSet;
                                if (layerType.Equals("collision", StringComparison.OrdinalIgnoreCase))
                                {
                                    Texture colTex = engine.Graphics.CreateTexture("Collision", tileWidth, tileHeight);
                                    layer.TileSet = TileSet.GetCollisionTileSet(colTex);
                                    layer.Visible = false;
                                }
                                else if (layerType.Equals("object", StringComparison.OrdinalIgnoreCase))
                                {
                                    layer.ObjectLayer = true;
                                }
                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i] >= 0)
                                    {
                                        layer[i].TileId = values[i];
                                    }
                                }
                                break;
                            case "event":
                                string evtType = sec.ReadString("type");
                                EventType evtActivate = sec.ReadString("activate").ToEventType();
                                string evtLocation = sec.ReadString("location");
                                values = evtLocation.ToIntValues();
                                if (values.Length >= 4 && evtType.Equals("event") && evtActivate != EventType.None)
                                {
                                    Event evt = new Event(engine, evtActivate, evtType);
                                    evt.PosX = values[0];
                                    evt.PosY = values[1];
                                    evt.Width = values[2];
                                    evt.Height = values[3];
                                    foreach (var k in sec.KeyList)
                                    {
                                        switch (k.Ident)
                                        {
                                            case "hotspot":
                                                if (k.Value.Equals("location", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    evt.HotSpotFromLocation();
                                                    evt.HotSpot = true;
                                                }
                                                else
                                                {
                                                    values = k.Value.ToIntValues();
                                                    if (values.Length >= 4)
                                                    {
                                                        evt.HotPosX = values[0];
                                                        evt.HotPosY = values[1];
                                                        evt.HotWidth = values[2];
                                                        evt.HotHeight = values[3];
                                                        evt.HotSpot = true;
                                                    }
                                                }
                                                break;
                                            case "mapmod":
                                                var mmEc = evt.AddComponent(EventComponentType.MapMod, 0);
                                                mmEc.MapMods = ParseMapMods(k.Value);
                                                break;
                                            case "spawn":
                                                var spEc = evt.AddComponent(EventComponentType.Spawn, 0);
                                                spEc.MapSpawns = ParseMapSpawns(k.Value);
                                                break;
                                            case "msg":
                                                evt.AddComponent(EventComponentType.Msg, k.Value);
                                                break;
                                            case "repeat":
                                                if (k.Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    evt.Repeat = false;
                                                }
                                                break;
                                            case "shakycam":
                                                evt.AddComponent(EventComponentType.ShakyCam, k.Value.ToDuration());
                                                break;
                                            case "intermap":
                                                sValues = k.Value.ToStrValues();
                                                if (sValues.Length >= 3)
                                                {
                                                    var imEc = evt.AddComponent(EventComponentType.InterMap, sValues[0]);
                                                    imEc.MapX = sValues[1].ToIntValue();
                                                    imEc.MapY = sValues[2].ToIntValue();
                                                }
                                                break;
                                            case "intramap":
                                                values = k.Value.ToIntValues();
                                                if (values.Length >= 2)
                                                {
                                                    var imEc = evt.AddComponent(EventComponentType.IntraMap, 0);
                                                    imEc.MapX = values[0];
                                                    imEc.MapY = values[1];
                                                }
                                                break;
                                            case "tooltip":
                                                evt.AddComponent(EventComponentType.Tooltip, k.Value);
                                                break;
                                            case "soundfx":
                                                sValues = k.Value.ToStrValues();
                                                if (sValues.Length >= 1)
                                                {
                                                    var fxEc = evt.AddComponent(EventComponentType.SoundFX, sValues[0]);
                                                    if (sValues.Length >= 3)
                                                    {
                                                        fxEc.MapX = sValues[0].ToIntValue();
                                                        fxEc.MapY = sValues[1].ToIntValue();
                                                    }
                                                    else
                                                    {
                                                        fxEc.MapX = (int)evt.CenterX;
                                                        fxEc.MapY = (int)evt.CenterY;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    map.AddLoadEvent(evt);
                                }
                                break;
                            case "npc":
                                EntityLoadInfo npc = new EntityLoadInfo();
                                string npcFilename = sec.ReadString("filename");
                                if (!string.IsNullOrEmpty(npcFilename))
                                {
                                    npc.EntityId = npcFilename;
                                    string npcLocation = sec.ReadString("location");
                                    values = npcLocation.ToIntValues();
                                    if (values.Length >= 2)
                                    {
                                        npc.PosX = values[0];
                                        npc.PosY = values[1];

                                        map.AddLoadNPC(npc);
                                    }
                                }
                                break;
                            case "enemy":
                                string enemyName = sec.ReadString("type");
                                if (!string.IsNullOrEmpty(enemyName))
                                {
                                    EnemyGroup enemy = new EnemyGroup(enemyName);
                                    foreach (var k in sec.KeyList)
                                    {
                                        switch (k.Ident)
                                        {
                                            case "category":
                                                enemy.Category = k.Value;
                                                break;
                                            case "chance":
                                                enemy.Chance = k.Value.ToIntValue();
                                                break;
                                            case "location":
                                                values = k.Value.ToIntValues();
                                                if (values.Length >= 2)
                                                {
                                                    enemy.PosX = values[0];
                                                    enemy.PosY = values[1];
                                                }
                                                if (values.Length >= 4)
                                                {
                                                    enemy.Width = values[2];
                                                    enemy.Height = values[3];
                                                }
                                                break;
                                            case "level":
                                                values = k.Value.ToIntValues();
                                                if (values.Length >= 2)
                                                {
                                                    enemy.MinLevel = values[0];
                                                    enemy.MaxLevel = Math.Max(enemy.MinLevel, values[1]);
                                                }
                                                else if (values.Length >= 1)
                                                {
                                                    enemy.MinLevel = values[0];
                                                    enemy.MaxLevel = values[0];
                                                }
                                                break;
                                            case "number":
                                                values = k.Value.ToIntValues();
                                                if (values.Length >= 2)
                                                {
                                                    enemy.MinNumber = values[0];
                                                    enemy.MaxNumber = Math.Max(enemy.MinNumber, values[1]);
                                                }
                                                else if (values.Length >= 1)
                                                {
                                                    enemy.MinNumber = values[0];
                                                    enemy.MaxNumber = values[0];
                                                }
                                                break;
                                        }
                                    }
                                    map.AddLoadEnemyGroup(enemy);
                                }
                                break;
                        }
                    }
                    map.BackgroundColor = ini.ReadString("header", "background_color").ToColor();
                    map.AddMapParallax(engine.LoadParallax(ini.ReadString("header", "parallax_layers")));
                }
            }
            return map;
        }

        private TileSet InternalLoadTileSet(IniFile ini, string fileId)
        {
            TileSet ts = null;
            string img = ini.ReadString("", "img");
            if (!string.IsNullOrEmpty(img))
            {
                Texture tex = engine.GetTexture(img);
                if (tex != null)
                {
                    ts = new TileSet(fileId, tex);
                    var sec = ini.Sections.FirstOrDefault(sn => sn.Name.Equals(""));
                    int[] values = null;
                    if (sec != null)
                    {
                        foreach (var k in sec.KeyList)
                        {
                            switch (k.Ident)
                            {
                                case "tile":
                                    values = k.Value.ToIntValues();
                                    if (values.Length >= 6)
                                    {
                                        int index = values[0];
                                        int clipX = values[1];
                                        int clipY = values[2];
                                        int clipW = values[3];
                                        int clipH = values[4];
                                        int offsetX = values[5];
                                        int offsetY = values[6];

                                        //int offsetX = 32 - values[5];
                                        //int offsetY = 16 - values[6];

                                        ts.AddTile(index, clipX, clipY, clipW, clipH, offsetX, offsetY);
                                    }
                                    break;
                                case "animation":
                                    var sValues = k.Value.ToStrValues(';');
                                    var tileId = sValues[0].ToIntValue();
                                    for (int i = 1; i < sValues.Length; i++)
                                    {
                                        var subValue = sValues[i];
                                        if (subValue.EndsWith("ms")) { subValue = subValue.Replace("ms", ""); }
                                        values = subValue.ToIntValues();
                                        if (values.Length >= 3)
                                        {
                                            ts.AddAnim(tileId, values[0], values[1], values[2]);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return ts;
        }

        private MapParallax InternalLoadParallax(IniFile ini, string filename)
        {
            MapParallax parallax = new MapParallax();
            ParallaxLayer layer = null;
            foreach (var sec in ini.Sections)
            {
                if (sec.Name.Equals("layer"))
                {

                    string img = sec.ReadString("image");
                    Texture tx = engine.GetTexture(img);
                    if (tx != null)
                    {
                        layer = new ParallaxLayer(tx);
                        layer.MapLayer = sec.ReadString("map_layer");
                        layer.Speed = (float)sec.ReadDouble("speed");
                        string[] sValues = sec.ReadString("fixed_speed").ToStrValues();
                        if (sValues.Length >= 2)
                        {
                            layer.FixedSpeedX = sValues[0].ToFloatValue();
                            layer.FixedSpeedY = sValues[1].ToFloatValue();
                        }
                        parallax.AddLayer(layer);

                    }

                }
            }
            return parallax;
        }

        private AnimationSet InternalLoadAnimationSet(IniFile ini, string fileId)
        {
            AnimationSet animationSet = null;
            string img = ini.ReadString("", "image");
            if (!string.IsNullOrEmpty(img))
            {
                Texture tex = engine.GetTexture(img);
                if (tex != null)
                {
                    animationSet = new AnimationSet(fileId, tex);
                    int renderSizeX = 0;
                    int renderSizeY = 0;
                    int renderOffsetX = 0;
                    int renderOffsetY = 0;
                    int[] renderSize = ini.ReadString("", "render_size").ToIntValues();
                    if (renderSize.Length >= 2)
                    {
                        renderSizeX = renderSize[0];
                        renderSizeY = renderSize[1];
                    }
                    int[] offsetSize = ini.ReadString("", "render_offset").ToIntValues();
                    if (offsetSize.Length >= 2)
                    {
                        renderOffsetX = offsetSize[0];
                        renderOffsetY = offsetSize[1];
                    }
                    foreach (var sec in ini.Sections)
                    {
                        if (!string.IsNullOrEmpty(sec.Name))
                        {
                            AnimationType type = sec.ReadString("type").ToAnimationType();
                            Animation anim = animationSet.AddAnimation(sec.Name, type);
                            int frames = sec.ReadInt("frames");
                            int duration = sec.ReadString("duration").ToDuration(engine.MaxFramesPerSecond);
                            int position = sec.ReadInt("position");
                            bool firstFrame = true;
                            bool noFrame = true;
                            foreach (var key in sec.KeyList)
                            {
                                if (key.Ident.Equals("frame"))
                                {
                                    if (firstFrame)
                                    {
                                        anim.Setup(frames, duration);
                                        firstFrame = false;
                                    }
                                    noFrame = false;
                                    int[] values = key.Value.ToIntValues();
                                    int index = values[0];
                                    int direction = values[1];
                                    int x = values[2];
                                    int y = values[3];
                                    int w = values[4];
                                    int h = values[5];
                                    int ox = values[6];
                                    int oy = values[7];
                                    anim.AddFrame(index, direction, x, y, w, h, ox, oy);
                                }
                            }
                            if (noFrame && renderSizeX > 0 && renderSizeY > 0)
                            {
                                anim.Setup(frames, duration);
                                anim.SetUncompressed(renderSizeX, renderSizeY, renderOffsetX, renderOffsetY, position, frames, duration);
                            }
                        }
                    }
                }
            }
            return animationSet;
        }

        private Entity InternalLoadEntity(IniFile ini, string fileId)
        {
            Entity ent = null;
            string name = ini.ReadString("", "name");
            if (!string.IsNullOrEmpty(name))
            {
                ent = new Entity(engine, name);
                ent.Categories = ini.ReadString("", "categories").ToStrValues();
                ent.Rarity = ini.ReadString("", "rarity", "common");
                ent.Level = ini.ReadInt("", "level", 1);
                ent.Speed = (float)(ini.ReadDouble("", "speed") / engine.MaxFramesPerSecond);
                string animId = ini.ReadString("", "animations");
                if (string.IsNullOrEmpty(animId)) animId = ini.ReadString("", "gfx");

                if (!string.IsNullOrEmpty(animId))
                {
                    ent.AnimationName = animId;
                }
                else
                {
                    animId = ini.ReadString("", "gfxpart");
                    if (!string.IsNullOrEmpty(animId))
                    {
                        var sec = ini.Sections.FirstOrDefault(sn => sn.Name.Equals(""));
                        if (sec != null)
                        {
                            foreach (var k in sec.KeyList)
                            {
                                if (k.Ident.Equals("gfxpart"))
                                {
                                    var sValues = k.Value.ToStrValues();
                                    string part = sValues[0];
                                    string partAnimId = sValues[1];
                                    if (!string.IsNullOrEmpty(partAnimId))
                                    {
                                        ent.AddAnimationSetName(part, partAnimId);
                                    }
                                }
                                else if (k.Ident.Equals("layer"))
                                {
                                    var sValues = k.Value.ToStrValues();
                                    int layer = sValues[0].ToIntValue();
                                    var order = sValues.ToList();
                                    order.RemoveAt(0);
                                    ent.SetLayerOrder(layer, order);
                                }
                            }
                        }
                    }
                }
            }
            return ent;
        }

        private IniFile GetIni(Stream stream)
        {
            IniFile ini = new IniFile(stream);
            CheckInclude(ini);
            return ini;
        }

        private void CheckInclude(IniFile ini)
        {
            if (!string.IsNullOrEmpty(ini.Include))
            {
                string includeId = engine.FileResolver.Resolve(ini.Include);
                if (includeId != null)
                {
                    using (Stream stream = engine.FileResolver.OpenFile(includeId))
                    {
                        IniFile incIni = new IniFile(stream);
                        CheckInclude(incIni);
                        ini.AddIni(incIni);
                    }
                }
            }
        }

        private static List<MapMod> ParseMapMods(string value)
        {
            var list = new List<MapMod>();
            foreach (string s in value.Split(';'))
            {
                var mod = ParseMapMod(s);
                if (mod != null)
                {
                    list.Add(mod);
                }
            }
            return list;
        }

        private static MapMod ParseMapMod(string value)
        {
            string[] values = value.ToStrValues();
            if (values.Length >= 4)
            {
                MapMod mod = new MapMod();
                mod.Layer = values[0];
                mod.MapX = values[1].ToIntValue();
                mod.MapY = values[2].ToIntValue();
                mod.Value = values[3].ToIntValue();
                return mod;
            }
            return null;
        }

        private static List<MapSpawn> ParseMapSpawns(string value)
        {
            var list = new List<MapSpawn>();
            foreach (string s in value.Split(';'))
            {
                var spawn = ParseMapSpawn(s);
                if (spawn != null)
                {
                    list.Add(spawn);
                }
            }
            return list;
        }

        private static MapSpawn ParseMapSpawn(string value)
        {
            string[] values = value.ToStrValues();
            if (values.Length >= 3)
            {
                MapSpawn spawn = new MapSpawn();
                spawn.Type = values[0];
                spawn.MapX = values[1].ToIntValue();
                spawn.MapY = values[2].ToIntValue();
                return spawn;
            }
            return null;
        }
    }
}
