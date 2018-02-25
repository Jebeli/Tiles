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
    using Core;
    using Graphics;
    using GUI;
    using Input;

    public class MapScreen : AbstractScreen
    {
        private MapRenderer renderer;
        private float mouseX;
        private float mouseY;
        private bool mousePanning;
        private bool panning;
        private bool hasPanned;
        private WidgetTileEditor editor;
        WidgetWindow window1;
        WidgetButton button1;
        WidgetButton button2;
        WidgetButton button3;
        public MapScreen(Engine engine)
            : base(engine, "MapScreen")
        {
            renderer = new MapRenderer(engine);
            mousePanning = true;

            window1 = new WidgetWindow();
            window1.SetBounds(10, 10, 3 * 64 + 10, 32 + 10);
            button1 = new WidgetButton("Exit");
            button1.SetBounds(5, 5, 64, 32);
            button2 = new WidgetButton("Load");
            button2.SetBounds(5 + 64, 5, 64, 32);
            button3 = new WidgetButton("Save");
            button3.SetBounds(5 + 64 * 2, 5, 64, 32);
            window1.AddWidget(button1);
            window1.AddWidget(button2);
            window1.AddWidget(button3);
            AddWidget(window1);
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
        }


        public override void Render(TimeInfo time)
        {
            renderer.RenderMap(engine.Map);
            base.Render(time);
        }

        protected override bool OnMouseDown(float x, float y, MouseButton button)
        {
            if (base.OnMouseDown(x, y, button))
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
            if (base.OnMouseUp(x, y, button))
            {
                mouseX = x;
                mouseY = y;
                if (panning)
                {
                    panning = false;
                }
                Click(mouseX, mouseY, button);
            }
            return true;
        }

        protected override void OnMouseMove(float x, float y, MouseButton button)
        {
            base.OnMouseMove(x, y, button);
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
                Hover(mouseX, mouseY);
            }
        }

        protected override void OnWidgetClick(Widget widget)
        {
            base.OnWidgetClick(widget);
            if (button1 == widget)
            {
                engine.Exit();
            }
            else if (button2 == widget)
            {

            }
            else if (button3 == widget)
            {
                engine.SaveMap(engine.Map);
                foreach(var layer in engine.Map.Layers)
                {
                    engine.SaveTileSet(layer.TileSet);
                }
            }
            else if (editor != null)
            {
                editor.HandleWidgetClick(widget);
            }
        }

        private void Pan(float dx, float dy)
        {
            engine.Camera.Shift(dx, dy);
            hasPanned = true;
        }

        private void Hover(float x, float y)
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

        private void Click(float x, float y, MouseButton button)
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
                MakeEditor(tileX, tileY);
            }
            else
            {
                HideEditor();
            }
        }

        private void HideEditor()
        {
            if (editor != null)
            {
                editor.Cancel();
                RemoveWidget(editor);
            }
        }
        private void MakeEditor(int x, int y)
        {
            HideEditor();
            editor = new WidgetTileEditor(engine.Map, x, y);
            int sX;
            int sY;
            engine.Camera.MapToScreen(x, y, out sX, out sY);
            sX += engine.Camera.TileWidth;
            sY += engine.Camera.TileHeight;
            if (sX + editor.Width > engine.Camera.ViewWidth) { sX = engine.Camera.ViewWidth - editor.Width; }
            if (sY + editor.Height > engine.Camera.ViewHeight) { sY = engine.Camera.ViewHeight - editor.Height; }
            editor.SetPosition(sX, sY);
            AddWidget(editor);
            editor.Visible = true;
        }
    }
}
