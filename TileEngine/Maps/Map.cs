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

namespace TileEngine.Maps
{
    using System;
    using System.Collections.Generic;
    using Core;
    using TileEngine.Graphics;

    public class Map : NamedObject
    {
        private string fileName;
        private int width;
        private int height;
        private IList<Layer> layers;
        private MapOrientation orientation;
        private int tileWidth;
        private int tileHeight;
        private Color backgroundColor;
        private MapParallax parallax;

        public Map(string name, int width, int height, int tileWidth, int tileHeight, MapOrientation orientation = MapOrientation.Isometric)
            : base(name)
        {
            backgroundColor = new Color(0, 0, 0, 255);
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            layers = new List<Layer>();
            this.orientation = orientation;
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public MapParallax Parallax
        {
            get { return parallax; }
            set { parallax = value; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public void InvalidateRenderLists()
        {
            foreach (var layer in layers)
            {
                layer.RenderList = null;
            }
        }

        public MapOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public int TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public void Update(TimeInfo time)
        {
            foreach(var l in layers)
            {
                l.Update(time);
            }
            if (parallax != null)
            {
                //if (time.GetElapsedTimeSince(lastParallaxTime).TotalSeconds > 1)
                //{
                //    lastParallaxTime = time.TotalGameTime;
                    foreach (var p in parallax.Layers)
                    {
                        p.Update();
                    }
                //}
            }
        }

        public Layer AddLayer(string name)
        {
            Layer layer = null;
            if (CheckNameClash(layers, name))
            {
                layer = new Layer(name, this, width, height);
                layers.Add(layer);
            }
            return layer;
        }

        public bool HasLayer(string name)
        {
            return Contains(layers, name);
        }

        public Layer GetLayer(string name)
        {
            return Find(layers, name);
        }

        public Layer GetLayer(int index)
        {
            return layers[index];
        }

        public int LayerCount
        {
            get { return layers.Count; }
        }

        public IEnumerable<Layer> Layers
        {
            get { return layers; }
        }
    }
}
