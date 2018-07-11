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
    using Core;
    using TileEngine.Entities;
    using TileEngine.Events;
    using TileEngine.Graphics;


    public enum BlockType
    {
        None,
        All,
        Movement,
        AllHidden,
        MovementHidden,
        MapOnly,
        MapOnlyAlt,
        Entities,
        Enemies
    }

    public class Map : NamedObject
    {
        private string fileName;
        private int width;
        private int height;
        private IList<Layer> layers;
        private IList<ParallaxLayer> parallaxLayers;
        private EventLayer eventLayer;
        private MapCollision collision;
        private MapOrientation orientation;
        private int tileWidth;
        private int tileHeight;
        private int startX;
        private int startY;
        private Color backgroundColor;
        private List<Event> loadEvents;
        private List<EntityLoadInfo> loadNPCs;
        private List<EnemyGroup> loadEnemyGroups;

        public Map(string name, int width, int height, int tileWidth, int tileHeight, MapOrientation orientation = MapOrientation.Isometric)
            : base(name)
        {
            backgroundColor = new Color(0, 0, 0, 255);
            parallaxLayers = new List<ParallaxLayer>();
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            startX = -1;
            startY = -1;
            layers = new List<Layer>();
            this.orientation = orientation;
            eventLayer = new EventLayer(width, height);
            loadEvents = new List<Event>();
            loadNPCs = new List<EntityLoadInfo>();
            loadEnemyGroups = new List<EnemyGroup>();
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

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public int StartX
        {
            get { return startX; }
            set { startX = value; }
        }

        public int StartY
        {
            get { return startY; }
            set { startY = value; }
        }

        public void AddLoadEvent(Event evt)
        {
            loadEvents.Add(evt);
        }

        public IEnumerable<Event> LoadEvents
        {
            get { return loadEvents; }
        }

        public void ClearLoadEvents()
        {
            loadEvents.Clear();
        }

        public void AddLoadNPC(EntityLoadInfo npc)
        {
            loadNPCs.Add(npc);
        }

        public IEnumerable<EntityLoadInfo> LoadNPCs
        {
            get { return loadNPCs; }
        }

        public void ClearLoadNPCs()
        {
            loadNPCs.Clear();
        }

        public void AddLoadEnemyGroup(EnemyGroup enemy)
        {
            loadEnemyGroups.Add(enemy);
        }

        public IEnumerable<EnemyGroup> LoadEnemyGroups
        {
            get { return loadEnemyGroups; }
        }

        public void ClearEnemyGroups()
        {
            loadEnemyGroups.Clear();
        }

        public void AddMapParallax(MapParallax parallax)
        {
            parallaxLayers.Clear();
            foreach (var layer in layers)
            {
                layer.ParallaxLayers.Clear();
            }
            if (parallax != null)
            {
                foreach (var pl in parallax.Layers)
                {
                    var layer = GetLayer(pl.MapLayer);
                    if (layer != null)
                    {
                        layer.ParallaxLayers.Add(pl);
                    }
                    else
                    {
                        parallaxLayers.Add(pl);
                    }
                }
            }
        }

        public IList<ParallaxLayer> GetAllParallaxLayers()
        {
            List<ParallaxLayer> list = new List<ParallaxLayer>();
            list.AddRange(parallaxLayers);
            foreach (var l in layers)
            {
                list.AddRange(l.ParallaxLayers);
            }
            return list;
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

        public EventLayer EventLayer
        {
            get { return eventLayer; }
        }

        public MapCollision Collision
        {
            get { return collision; }
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
            foreach (var pl in parallaxLayers)
            {
                pl.Update();
            }
            foreach (var l in layers)
            {
                l.Update(time);
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

        public Layer GetBottomLayer()
        {
            Layer layer = null;
            foreach (Layer l in layers)
            {
                if (l.Visible)
                {
                    layer = l;
                    break;
                }
            }
            return layer;
        }

        public Layer GetTopLayer()
        {
            Layer layer = null;
            foreach (Layer l in layers)
            {
                if (l.Visible)
                {
                    layer = l;
                }
            }
            return layer;
        }

        public int LayerCount
        {
            get { return layers.Count; }
        }

        public IEnumerable<Layer> Layers
        {
            get { return layers; }
        }

        public IEnumerable<ParallaxLayer> ParallaxLayers
        {
            get { return parallaxLayers; }
        }

        public IList<Event> GetEventsAt(int x, int y)
        {
            if ((x >= 0) && (y >= 0) && (x < width) && (y < height))
            {
                return eventLayer[x, y].Events;
            }
            return new Event[0];
        }

        public void InitCollision()
        {
            Layer collisionLayer = Find(layers, "collision");
            if (collisionLayer != null)
            {
                collision = new MapCollision(collisionLayer);
            }
            else
            {
                collision = new MapCollision(width, height);
            }
        }

        public void DoMapMod(MapMod mod)
        {
            var layer = GetLayer(mod.Layer);
            if (layer != null)
            {
                layer[mod.MapX, mod.MapY].TileId = mod.Value;
            }
            if (mod.Layer.Equals("collision"))
            {
                collision.ColMap[mod.MapX, mod.MapY] = mod.Value;
            }
        }
    }
}
