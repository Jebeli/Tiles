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
            TileSet tileSet = engine.LoadTileSet(tileSetId);
            if (tileSet != null)
            {
                MapOrientation mapOrientation;
                if (Enum.TryParse(orientation, true, out mapOrientation))
                {
                    map = new Map(fileId, width, height, tileWidth, tileHeight, mapOrientation);
                    string data = null;
                    int[] values = null;
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
                                    layer.Visible = false;
                                }
                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i] > 0)
                                    {
                                        layer[i].TileId = values[i];
                                    }
                                }
                                break;
                        }
                    }
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
                                        int offsetX = 32 - values[5];
                                        int offsetY = 16 - values[6];

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
    }
}
