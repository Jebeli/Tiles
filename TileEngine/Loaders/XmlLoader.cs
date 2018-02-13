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
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlLoader : AbstractLoader
    {
        public XmlLoader(Engine engine)
            : base(engine)
        {
        }

        public override FileType DetectFileTpye(string fileId)
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
                            stream.Dispose();
                            return FileType.Map;
                        }
                    }
                    else if (root.Name.LocalName.Equals("tileset"))
                    {
                        string tsName = root.Attribute("name").Value;
                        if (!string.IsNullOrEmpty(tsName))
                        {
                            stream.Dispose();
                            return FileType.TileSet;
                        }
                    }
                }
                stream.Dispose();
            }
            return FileType.None;
        }

        public override Map LoadMap(string fileId)
        {
            Map map = null;
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
                            stream.Dispose();
                        }
                    }
                }
                stream.Dispose();
            }
            return map;
        }

        public override TileSet LoadTileSet(string fileId)
        {
            TileSet tileSet = null;
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
                            stream.Dispose();
                        }
                    }
                }
                stream.Dispose();
            }
            return tileSet;
        }
        private Map LoadMap(XElement root, string fileId)
        {
            Map map = null;
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
                Texture tex = engine.Graphics.GetTexture(image, engine.FileResolver);
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
                            if (x != null && y != null && width != null && height != null && offsetX != null && offsetY != null)
                            {
                                ts.AddTile((int)id, (int)x, (int)y, (int)width, (int)height, (int)offsetX, (int)offsetY);
                            }
                        }
                    }
                }
            }
            return ts;
        }

    }
}
