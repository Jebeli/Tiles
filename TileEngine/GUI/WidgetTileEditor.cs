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

namespace TileEngine.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Maps;

    public class WidgetTileEditor : WidgetWindow
    {
        private Map map;
        private int x;
        private int y;
        private Layer layer;
        private Tile tile;
        private WidgetLabel labelLayers;
        private WidgetLabel labelTile;
        private List<WidgetButton> layerButtons;
        private WidgetImage tileImage;
        private WidgetButton buttonPrev;
        private WidgetButton buttonNext;
        private WidgetButton buttonApply;
        private WidgetButton buttonCancel;
        private List<int> originalIds;
        public WidgetTileEditor(Map map, int x, int y)
        {
            this.map = map;
            this.x = x;
            this.y = y;
            Visible = false;

            SetSize(256, 256);

            labelLayers = new WidgetLabel("Layers:");
            labelLayers.SetBounds(0, 0, 64, 30);
            AddWidget(labelLayers);
            labelTile = new WidgetLabel($"Tile ({x}/{y})");
            labelTile.SetBounds(0, 30, 256, 30);
            AddWidget(labelTile);
            tileImage = new WidgetImage();
            tileImage.SetBounds(64, 256 - 128, 256 - 64, 200);
            AddWidget(tileImage);
            buttonPrev = new WidgetButton("<");
            buttonPrev.SetBounds(0, 256 - 30, 64, 30);
            AddWidget(buttonPrev);
            buttonNext = new WidgetButton(">");
            buttonNext.SetBounds(64, 256 - 30, 64, 30);
            AddWidget(buttonNext);
            buttonApply = new WidgetButton("Apply");
            buttonApply.SetBounds(64 * 2, 256 - 30, 64, 30);
            AddWidget(buttonApply);
            buttonCancel = new WidgetButton("Cancel");
            buttonCancel.SetBounds(64 * 3, 256 - 30, 64, 30);
            AddWidget(buttonCancel);

            int index = 0;
            layerButtons = new List<WidgetButton>();
            originalIds = new List<int>();
            foreach (var layer in map.Layers)
            {
                if (layer.Visible)
                {
                    WidgetButton buttonLayer = new WidgetButton(layer.Name);
                    buttonLayer.SetBounds(64 + index * 64, 0, 64, 30);
                    buttonLayer.Tag = layer;
                    layerButtons.Add(buttonLayer);
                    AddWidget(buttonLayer);
                    int id = layer[x, y].TileId;
                    originalIds.Add(id);
                    if (id >= 0)
                    {
                        SwitchLayer(layer);
                    }
                    index++;
                }
            }
        }

        public void Cancel()
        {
            int index = 0;
            foreach (var layer in map.Layers)
            {
                if (layer.Visible)
                {
                    layer[x, y].TileId = originalIds[index];
                    index++;
                }
            }
            map.InvalidateRenderLists();
            Visible = false;

        }

        public bool HandleWidgetClick(Widget widget)
        {
            if (buttonPrev == widget)
            {
                int id = tile.TileId;

                id--;
                while (layer.TileSet.GetTile(id) == null && id >= -1)
                {
                    id--;
                }
                if (id < -1)
                {
                    id = layer.TileSet.TileCount - 1;
                }
                while (layer.TileSet.GetTile(id) == null && id >= -1)
                {
                    id--;
                }
                tile.TileId = id;
                SwitchLayer(layer);
            }
            else if (buttonNext == widget)
            {
                int id = tile.TileId;
                id++;
                while (layer.TileSet.GetTile(id) == null && id < layer.TileSet.TileCount)
                {
                    id++;
                }
                if (id >= layer.TileSet.TileCount)
                {
                    id = 0;
                }
                while (layer.TileSet.GetTile(id) == null && id < layer.TileSet.TileCount)
                {
                    id++;
                }
                tile.TileId = id;
                SwitchLayer(layer);
            }
            else if (buttonCancel == widget)
            {
                Cancel();
            }
            else if (buttonApply == widget)
            {
                Visible = false;
            }
            else
            {
                foreach (WidgetButton button in layerButtons)
                {
                    if (button == widget)
                    {
                        SwitchLayer(button.Tag as Layer);
                    }
                }
            }
            return false;
        }

        private void SwitchLayer(Layer layer)
        {
            if (layer != null)
            {
                this.layer = layer;
                tile = layer[x, y];
                labelTile.Text = $"Tile ({x}/{y}) Layer {layer.Name} Id: {tile.TileId}";
                tileImage.Image = layer.TileSet.GetTile(tile.TileId);
                map.InvalidateRenderLists();
            }
        }



    }
}
