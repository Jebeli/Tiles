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

namespace TileEngine.Maps
{
    using System.Collections.Generic;
    using Core;
    public class Map : NamedObject
    {
        private int width;
        private int height;
        private IList<Layer> layers;
        public Map(string name, int width, int height)
            : base(name)
        {
            this.width = width;
            this.height = height;
            layers = new List<Layer>();
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
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
