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
    using Core;
    using Graphics;
    using Maps;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlLoader : AbstractLoader
    {
        public XmlLoader(Engine engine)
            : base(engine, ".xml")
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
                    XDocument xdoc = XDocument.Load(stream);
                    if (xdoc != null)
                    {
                        var root = xdoc.Root;
                        if (root.Name.LocalName.Equals("map"))
                        {
                            string mapName = root.Attribute("name").Value;
                            if (!string.IsNullOrEmpty(mapName))
                            {
                                type = FileType.Map;
                            }
                        }
                        else if (root.Name.LocalName.Equals("tileset"))
                        {
                            string tsName = root.Attribute("name").Value;
                            if (!string.IsNullOrEmpty(tsName))
                            {
                                type = FileType.TileSet;
                            }
                            else if (root.Name.LocalName.Equals("tileset"))
                            {
                                string pName = root.Attribute("name").Value;
                                if (!string.IsNullOrEmpty(pName))
                                {
                                    type = FileType.Parallax;
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
                    XDocument xdoc = XDocument.Load(stream);
                    if (xdoc != null)
                    {
                        var root = xdoc.Root;
                        if (root.Name.LocalName.Equals("map"))
                        {
                            string mapName = root.Attribute("name").Value;
                            if (!string.IsNullOrEmpty(mapName))
                            {
                                map = LoadMap(root, fileId);
                            }
                        }
                    }
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
                    XDocument xdoc = XDocument.Load(stream);
                    if (xdoc != null)
                    {
                        var root = xdoc.Root;
                        if (root.Name.LocalName.Equals("tileset"))
                        {
                            string tsName = root.Attribute("name").Value;
                            if (!string.IsNullOrEmpty(tsName))
                            {
                                tileSet = LoadTileSet(root, fileId);
                            }
                        }
                    }
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
                    XDocument xdoc = XDocument.Load(stream);
                    if (xdoc != null)
                    {
                        var root = xdoc.Root;
                        if (root.Name.LocalName.Equals("parallax"))
                        {
                            string tsName = root.Attribute("name").Value;
                            if (!string.IsNullOrEmpty(tsName))
                            {
                                parallax = LoadParallax(root, fileId);
                            }
                        }
                    }
                    stream.Dispose();
                }
            }
            return parallax;
        }

        private Map LoadMap(XElement root, string fileId)
        {
            Map map = null;
            string name = (string)root.Attribute("name");
            string title = (string)root.Attribute("title");
            string orientation = (string)root.Attribute("orientation");
            int? width = (int?)root.Attribute("width");
            int? height = (int?)root.Attribute("height");
            int? tileWidth = (int?)root.Attribute("tileWidth");
            int? tileHeight = (int?)root.Attribute("tileHeight");
            if (name != null && orientation != null && width != null && height != null && tileWidth != null && tileHeight != null)
            {
                MapOrientation mapOrientation;
                if (Enum.TryParse(orientation, true, out mapOrientation))
                {
                    map = new Map(title, (int)width, (int)height, (int)tileWidth, (int)tileHeight, mapOrientation);
                    foreach (var node in from item in root.Descendants("layer") select item)
                    {
                        string layerName = (string)node.Attribute("name");
                        int? layerWidth = (int?)node.Attribute("width");
                        int? layerHeight = (int?)node.Attribute("height");
                        bool? visible = (bool?)node.Attribute("visible");
                        string tileSetName = (string)node.Attribute("tileset");
                        var data = node.Descendants().FirstOrDefault();
                        if (layerName != null && layerWidth != null && layerHeight != null && data != null && tileSetName != null)
                        {
                            TileSet tileSet = engine.GetTileSet(tileSetName);
                            string encoding = (string)data.Attribute("encoding");
                            if (encoding != null && tileSet != null)
                            {
                                Layer layer = map.AddLayer(layerName);
                                if (visible != null && visible == false) { layer.Visible = false; }
                                layer.TileSet = tileSet;
                                if (encoding.Equals("csv", StringComparison.OrdinalIgnoreCase))
                                {
                                    layer.SetCSV(data.Value);
                                }
                            }
                        }
                    }
                }
            }
            return map;
        }

        private TileSet LoadTileSet(XElement root, string fileId)
        {
            TileSet ts = null;
            string name = (string)root.Attribute("name");
            int? tileWidth = (int?)root.Attribute("tileWidth");
            int? tileHeight = (int?)root.Attribute("tileHeight");
            string image = (string)root.Attribute("image");
            if (name != null && tileWidth != null && tileHeight != null && image != null)
            {
                Texture tex = engine.GetTexture(image);
                if (tex != null)
                {
                    ts = new TileSet(name, tex);
                    ts.TileWidth = (int)tileWidth;
                    ts.TileHeight = (int)tileHeight;
                    foreach (var node in from item in root.Descendants("tile") select item)
                    {
                        int? id = (int?)node.Attribute("id");
                        var img = node.Descendants().FirstOrDefault();
                        if (id != null && img != null)
                        {
                            int? x = (int?)img.Attribute("x");
                            int? y = (int?)img.Attribute("y");
                            int? width = (int?)img.Attribute("width");
                            int? height = (int?)img.Attribute("height");
                            int? offsetX = (int?)img.Attribute("offsetX");
                            int? offsetY = (int?)img.Attribute("offsetY");
                            string tileName = (string)img.Attribute("name");
                            if (x != null && y != null && width != null && height != null && offsetX != null && offsetY != null)
                            {
                                ts.AddTile((int)id, (int)x, (int)y, (int)width, (int)height, (int)offsetX, (int)offsetY, tileName);
                                foreach (var frameNode in from item in node.Descendants("frame") select item)
                                {
                                    x = (int?)frameNode.Attribute("x");
                                    y = (int?)frameNode.Attribute("y");
                                    int? duration = (int?)frameNode.Attribute("duration");
                                    if (duration != null)
                                    {
                                        ts.AddAnim((int)id, (int)x, (int)y, (int)duration);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return ts;
        }

        private MapParallax LoadParallax(XElement root, string fileId)
        {
            MapParallax parallax = null;
            return parallax;
        }
    }
}
