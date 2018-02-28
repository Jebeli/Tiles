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

namespace TileEngine.Savers
{
    using Maps;
    using System.Xml;
    using System.Xml.Linq;
    using System;

    public class XmlSaver : AbstractSaver
    {
        public XmlSaver(Engine engine)
            : base(engine)
        {
        }

        public override void Save(Map map, string fileId)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(GetOutputStream(fileId), settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("map");
                writer.WriteAttributeString("name", AdjustName(map.Name));
                writer.WriteAttributeString("orientation", map.Orientation.ToString().ToLowerInvariant());
                writer.WriteAttributeString("width", map.Width.ToString());
                writer.WriteAttributeString("height", map.Height.ToString());
                writer.WriteAttributeString("tileWidth", map.TileWidth.ToString());
                writer.WriteAttributeString("tileHeight", map.TileHeight.ToString());
                foreach (Layer layer in map.Layers)
                {
                    writer.WriteStartElement("layer");
                    writer.WriteAttributeString("name", layer.Name);
                    writer.WriteAttributeString("width", layer.Width.ToString());
                    writer.WriteAttributeString("height", layer.Height.ToString());
                    writer.WriteAttributeString("tileset", AdjustName(layer.TileSet.Name));
                    writer.WriteAttributeString("visible", layer.Visible.ToString().ToLowerInvariant());
                    writer.WriteStartElement("data");
                    writer.WriteAttributeString("encoding", "csv");
                    writer.WriteString(layer.GetCSV());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public override void Save(TileSet tileSet, string fileId)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(GetOutputStream(fileId), settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("tileset");
                writer.WriteAttributeString("name", AdjustName(tileSet.Name));
                writer.WriteAttributeString("tileWidth", tileSet.TileWidth.ToString());
                writer.WriteAttributeString("tileHeight", tileSet.TileHeight.ToString());
                writer.WriteAttributeString("image", tileSet.Texture.Name.ToString());
                foreach (int tileId in tileSet.Tiles)
                {
                    writer.WriteStartElement("tile");
                    writer.WriteAttributeString("id", tileId.ToString());
                    var region = tileSet.GetTile(tileId);
                    var name = tileSet.GetTileName(tileId);
                    writer.WriteStartElement("region");
                    writer.WriteAttributeString("x", region.X.ToString());
                    writer.WriteAttributeString("y", region.Y.ToString());
                    writer.WriteAttributeString("width", region.Width.ToString());
                    writer.WriteAttributeString("height", region.Height.ToString());
                    writer.WriteAttributeString("offsetX", region.OffsetX.ToString());
                    writer.WriteAttributeString("offsetY", region.OffsetY.ToString());
                    if (!string.IsNullOrEmpty(name))
                    {
                        writer.WriteAttributeString("name", name);
                    }
                    writer.WriteEndElement();
                    var anim = tileSet.GetTileAnim(tileId);
                    if (anim != null)
                    {
                        for (int i = 0; i < anim.FrameCount; i++)
                        {
                            region = anim.Textures[i];
                            writer.WriteStartElement("frame");
                            writer.WriteAttributeString("x", region.X.ToString());
                            writer.WriteAttributeString("y", region.Y.ToString());
                            writer.WriteAttributeString("duration", anim.FrameDurations[i].ToString());
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        protected override string AdjustName(string fileId)
        {
            return fileId.Replace(".txt", ".xml");
        }
    }
}
