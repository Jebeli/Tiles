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
    using Input;

    public class MapScreen : AbstractScreen
    {
        private MapRenderer renderer;
        private float mouseX;
        private float mouseY;
        private bool mousePanning;
        private bool panning;
        public MapScreen(Engine engine)
            : base(engine, "MapScreen")
        {
            renderer = new MapRenderer(engine);
            mousePanning = true;
        }


        public override void Render(TimeInfo time)
        {
            base.Render(time);
            renderer.RenderMap(engine.Map);
        }

        protected override void OnMouseDown(float x, float y, MouseButton button)
        {
            base.OnMouseDown(x, y, button);
            mouseX = x;
            mouseY = y;
            panning = button == MouseButton.Right;
        }

        protected override void OnMouseUp(float x, float y, MouseButton button)
        {
            base.OnMouseUp(x, y, button);
            mouseX = x;
            mouseY = y;
            if (panning)
            {
                panning = false;
            }
            else
            {
                Click(mouseX, mouseY);
            }
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

        private void Pan(float dx, float dy)
        {
            engine.Camera.Shift(dx, dy);
        }

        private void Hover(float x, float y)
        {
            float mapX;
            float mapY;
            int tileX;
            int tileY;
            engine.Camera.IsoScreenToMap(x, y, out mapX, out mapY);
            engine.Camera.IsoMapToTile(mapX, mapY, out tileX, out tileY);
            engine.Camera.HoverTileX = tileX;
            engine.Camera.HoverTileY = tileY;
        }

        private void Click(float x, float y)
        {
            float mapX;
            float mapY;
            int tileX;
            int tileY;
            engine.Camera.IsoScreenToMap(x, y, out mapX, out mapY);
            engine.Camera.IsoMapToTile(mapX, mapY, out tileX, out tileY);
            engine.Camera.ClickTileX = tileX;
            engine.Camera.ClickTileY = tileY;
        }
    }
}
