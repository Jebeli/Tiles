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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;

    public class MapParallax : NamedObject
    {

        private List<ParallaxLayer> layers;

        public MapParallax(string name)
            : base(name)
        {
            layers = new List<ParallaxLayer>();
        }

        public IList<ParallaxLayer> Layers
        {
            get { return layers; }
        }

        public IList<ParallaxLayer> GetMatchingLayers(string mapLayer)
        {
            List<ParallaxLayer> list = new List<ParallaxLayer>();
            foreach (var layer in layers)
            {
                if (string.IsNullOrEmpty(mapLayer))
                {
                    if (string.IsNullOrEmpty(layer.MapLayer))
                    {
                        list.Add(layer);
                    }
                }
                else if (mapLayer.Equals(layer.MapLayer, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(layer);
                }
            }
            return list;
        }

        public void AddLayer(ParallaxLayer layer)
        {
            layers.Add(layer);
        }

        public void Update()
        {
            foreach (var layer in layers)
            {
                layer.Update();
            }
        }
    }
}
