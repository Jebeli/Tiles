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

namespace TileEngine.Screens
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core;
    using Graphics;
    using Input;
    using TileEngine.Maps;
    using YGUI;

    public class MapScreen : AbstractScreen
    {
        private const int GADID_EXIT = 1001;
        private const int GADID_LOAD = 1002;
        private const int GADID_SAVE = 1003;

        private MapRenderer renderer;
        private float mouseX;
        private float mouseY;
        private bool mousePanning;
        private bool panning;
        private bool hasPanned;
        private TileEditor editor;
        private FileGadget fileDialog;

        public MapScreen(Engine engine)
            : base(engine, "MapScreen")
        {
            renderer = new MapRenderer(engine);
            mousePanning = true;
        }

        protected override void InitGUI(Screen screen)
        {
            var window = new Window(screen, "", Orientation.Horizontal)
            {
                LeftEdge = 10,
                TopEdge = 10,
                Borderless = false,
                ThinBorder = true
            };
            new ButtonGadget(window, "Exit")
            {
                GadgetUpEvent = (o, i) => { engine.SwitchToTitleScreen(); }
            };
            new ButtonGadget(window, "Load")
            {
                GadgetUpEvent = (o, i) =>
                {
                    fileDialog?.Cancel();
                    string dir = Path.GetDirectoryName(engine.Map.FileName);
                    fileDialog = FileGadget.ShowSelectFile(engine.FileResolver, screen, dir, LoadFileEvent);
                }
            };
            new ButtonGadget(window, "Save")
            {
                GadgetDownEvent = (o, i) =>
                {
                    engine.SaveMap(engine.Map);
                    foreach (var layer in engine.Map.Layers)
                    {
                        engine.SaveTileSet(layer.TileSet);
                    }
                }
            };
        }

        private void LoadFileEvent(object sender, EventArgs args)
        {
            var fg = (FileGadget)sender;
            string map = fg.SelectedFile.Path;
            engine.SetNextMap(map, -1, -1);
            engine.SwitchToLoadScreen();
        }

        public override void Render(TimeInfo time)
        {
            renderer.RenderMap(engine.Map);
            base.Render(time);
        }

        protected override bool OnMouseDown(float x, float y, MouseButton button)
        {
            if (!base.OnMouseDown(x, y, button))
            {
                mouseX = x;
                mouseY = y;
                hasPanned = false;
                panning = button == MouseButton.Right;
            }
            return true;
        }

        protected override bool OnMouseUp(float x, float y, MouseButton button)
        {
            if (!base.OnMouseUp(x, y, button))
            {
                mouseX = x;
                mouseY = y;
                if (panning)
                {
                    panning = false;
                }
                MapClick(mouseX, mouseY, button);
            }
            return true;
        }

        protected override bool OnMouseMove(float x, float y, MouseButton button)
        {
            bool ret = base.OnMouseMove(x, y, button);
            float oldMouseX = mouseX;
            float oldMouseY = mouseY;
            mouseX = x;
            mouseY = y;
            float dX = oldMouseX - mouseX;
            float dY = oldMouseY - mouseY;
            if (mousePanning && panning && (dX != 0 || dY != 0))
            {
                Pan(dX, dY);
            }
            else
            {
                MapHover(mouseX, mouseY);
            }
            return ret;
        }
     
        private void Pan(float dx, float dy)
        {
            engine.Camera.Shift(dx, dy);
            hasPanned = true;
        }

        private void MapHover(float x, float y)
        {
            float mapX;
            float mapY;
            int tileX;
            int tileY;
            engine.Camera.ScreenToMap(x, y, out mapX, out mapY);
            engine.Camera.MapToTile(mapX, mapY, out tileX, out tileY);
            engine.Camera.HoverTileX = tileX;
            engine.Camera.HoverTileY = tileY;
        }

        private void MapClick(float x, float y, MouseButton button)
        {
            float mapX;
            float mapY;
            int tileX;
            int tileY;
            engine.Camera.ScreenToMap(x, y, out mapX, out mapY);
            engine.Camera.MapToTile(mapX, mapY, out tileX, out tileY);
            engine.Camera.ClickTileX = tileX;
            engine.Camera.ClickTileY = tileY;
            if (button == MouseButton.Right && !hasPanned)
            {
                ShowEditor(tileX, tileY);
            }
        }

        private void HideEditor()
        {
            if (editor != null)
            {
                editor.Cancel();
                editor = null;
            }
        }

        private void ShowEditor(int x, int y)
        {
            if (editor == null)
            {
                MakeEditor(x, y);
                if (editor != null)
                {
                    engine.Camera.MapToScreen(x, y, out int sX, out int sY);
                    sX += engine.Camera.TileWidth;
                    sY += engine.Camera.TileHeight;
                    editor.LeftEdge = sX;
                    editor.TopEdge = sY;
                }
            }
            if (editor != null)
            {
                editor.SetTile(engine.Map, x, y);
                screen.ShowWindow(editor);
                screen.WindowToFront(editor);
                screen.ActivateWindow(editor);
            }
        }

        private void MakeEditor(int x, int y)
        {
            HideEditor();
            editor = new TileEditor(this, screen, engine.Map, x, y)
            {
                CloseGadget = true,
                DepthGadget = true,
                WindowCloseEvent = (o, i) => { HideEditor(); }
            };
        }

        private class TileEditor : Window
        {
            private MapScreen mapScreen;
            private Map map;
            private Layer layer;
            private Tile tile;
            private int mapX;
            private int mapY;
            private LabelGadget labelInfo;
            private BoxGadget layerBox;
            private ImageGadget image;
            private TableGadget table;
            private List<int> backupIds;

            public TileEditor(MapScreen mapScreen, Screen screen, Map map, int mapX, int mapY)
                : base(screen, "Tile Editor")
            {
                this.mapScreen = mapScreen;
                Init();
                SetTile(map, mapX, mapY);
            }

            private void RememberTile()
            {
                List<int> ids = new List<int>();
                int idx = 0;
                foreach (Layer layer in map.Layers)
                {
                    int tileId = layer[mapX, mapY].TileId;
                    ids.Add(tileId);
                    idx++;
                }
                backupIds = ids;
            }

            private void RevertTile()
            {
                if (backupIds != null)
                {
                    int idx = 0;
                    foreach (Layer layer in map.Layers)
                    {
                        int tileId = backupIds[idx];
                        layer[mapX, mapY].TileId = tileId;
                        idx++;
                    }
                }
                map.InvalidateRenderLists();
                backupIds = null;
            }

            public void SetTile(Map map, int x, int y)
            {
                this.map = map;
                if (x >= map.Width) x = map.Width - 1;
                if (y >= map.Height) y = map.Height - 1;
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                mapX = x;
                mapY = y;
                Refresh();
                SetTile(null);
                foreach (Layer layer in map.Layers.Reverse<Layer>())
                {

                    if (layer.Visible && layer[x, y].TileId >= 0)
                    {
                        SetLayer(layer);
                        break;
                    }
                }
                RememberTile();
            }

            public void SetLayer(Layer layer)
            {
                this.layer = layer;
                foreach (var but in layerBox.Children)
                {
                    but.Selected = layer == but.Tag;
                }
                table.ClearRows();
                if (layer != null)
                {
                    table.BeginUpdate();
                    foreach (var tileId in layer.TileSet.Tiles)
                    {
                        string ic = "";
                        string id = "" + tileId;
                        string name = layer.TileSet.GetTileName(tileId);
                        var row = table.AddRow(ic, id, name);
                        row.Cells[0].Image = layer.TileSet.GetTile(tileId);
                        row.Id = tileId;
                    }
                    table.EndUpdate();
                    SetTile(layer[mapX, mapY]);
                }
                Invalidate();
            }

            public void SetTile(Tile tile)
            {
                this.tile = tile;
                if (tile != null)
                {
                    TextureRegion tex = layer.TileSet.GetTile(tile.TileId);
                    image.Image = tex;
                    table.SelectRow(table.FindRow(tile.TileId));
                }
                else
                {
                    image.Image = null;
                    table.SelectRow(null);
                }
            }

            private void ChangeTile()
            {
                if (layer != null)
                {
                    int tileId = table.SelectedRowId;
                    layer[mapX, mapY].TileId = tileId;
                    SetTile(layer[mapX, mapY]);
                    map.InvalidateRenderLists();
                }
            }

            private void Refresh()
            {
                labelInfo.Label = $"Tile {mapX}/{mapY}";
                layerBox.Clear();
                foreach (var layer in map.Layers)
                {
                    var layerGad = new ButtonGadget(layerBox, layer.Name)
                    {
                        Sticky = true,
                        Tag = layer,
                        GadgetUpEvent = (o, i) => { SetLayer(layer); }
                    };
                }
            }

            private void Init()
            {
                labelInfo = new LabelGadget(this, $"Tile {mapX}/{mapY}");
                layerBox = new BoxGadget(this, Orientation.Horizontal);
                var tileBox = new BoxGadget(this, Orientation.Horizontal);
                image = new ImageGadget(tileBox)
                {
                    FixedHeight = 200,
                    FixedWidth = 200
                };
                table = new TableGadget(tileBox)
                {
                    RowHeight = 64,
                    EvenColumns = false,
                    SelectedCellChangedEvent = (o, i) =>
                    {
                        ChangeTile();
                    }
                };
                var colImg = table.AddColumn("Img", 64);
                var colIdx = table.AddColumn("Id", 64);
                var colName = table.AddColumn("Name", 200);

                var butBox = new BoxGadget(this, Orientation.Horizontal, Alignment.Fill, 10, 10);
                new ButtonGadget(butBox, "Ok")
                {
                    GadgetUpEvent = (o, i) =>
                    {
                        Apply();
                        mapScreen.HideEditor();
                    }
                };
                new ButtonGadget(butBox, "Cancel")
                {
                    GadgetUpEvent = (o, i) =>
                    {
                        mapScreen.HideEditor();
                    }
                };
            }

            public void Apply()
            {
                backupIds = null;
            }

            public void Cancel()
            {
                RevertTile();
                CloseWindow();
            }
        }
    }
}
