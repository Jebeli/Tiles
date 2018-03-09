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

        private Window win1;
        private Gadget button1;
        private Gadget button2;
        private Gadget button3;

        public MapScreen(Engine engine)
            : base(engine, "MapScreen")
        {
            renderer = new MapRenderer(engine);
            mousePanning = true;
            MakeWindows();
        }

        private void MakeWindows()
        {
            button1 = Gadget.MakeBoolGadget("Exit", 64, 32);
            button1.SetPosition(5, 5);
            button2 = Gadget.MakeBoolGadget("Load", 64, 32);
            button2.SetPosition(5 + 64, 5);
            button3 = Gadget.MakeBoolGadget("Save", 64, 32);
            button3.SetPosition(5 + 64 * 2, 5);
            win1 = Intuition.OpenWindowTags(null,
                Tag(WATags.WA_Left, 10),
                Tag(WATags.WA_Top, 10),
                Tag(WATags.WA_Width, 3 * 64 + 10),
                Tag(WATags.WA_Height, 32 + 10),
                Tag(WATags.WA_Flags, WindowFlags.WFLG_BORDERLESS),
                Tag(WATags.WA_IDCMP, IDCMPFlags.GADGETUP),
                Tag(WATags.WA_Gadgets, new[] { button1, button2, button3 }),
                Tag(WATags.WA_Opacity, 170),
                Tag(WATags.WA_Screen, this));
        }

        private void CloseWindows()
        {
            Intuition.CloseWindow(win1);
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

        //protected override void OnWidgetClick(Widget widget)
        //{
        //    base.OnWidgetClick(widget);
        //    if (button1 == widget)
        //    {
        //        engine.SwitchToTitleScreen();
        //        //engine.Exit();
        //    }
        //    else if (button2 == widget)
        //    {

        //    }
        //    else if (button3 == widget)
        //    {
        //        engine.SaveMap(engine.Map);
        //        foreach (var layer in engine.Map.Layers)
        //        {
        //            engine.SaveTileSet(layer.TileSet);
        //        }
        //    }
        //    else if (editor != null)
        //    {
        //        editor.HandleWidgetClick(widget);
        //    }
        //}

        protected override void OnGadgetClick(Gadget gadget)
        {
            base.OnGadgetClick(gadget);
            if (button1 == gadget)
            {
                Intuition.AutoRequestPositionMode = AutoRequestPositionMode.CenterScreen;
                IntuiText body = new IntuiText("Return to the");
                body.NextText = new IntuiText("Title Screen?");
                Intuition.AutoRequest(win1, body, "Yes", "No", IDCMPFlags.GADGETUP, IDCMPFlags.GADGETUP, 200, 100);
                //
            }
            else if (button2 == gadget)
            {

            }
            else if (button3 == gadget)
            {
                engine.SaveMap(engine.Map);
                foreach (var layer in engine.Map.Layers)
                {
                    engine.SaveTileSet(layer.TileSet);
                }
            }
        }

        protected override void OnAutoRequest(int gadNum)
        {
            base.OnAutoRequest(gadNum);
            if (gadNum == 1)
            {
                engine.SwitchToTitleScreen();
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
            //if (editor != null)
            //{
            //    editor.Cancel();
            //    RemoveWidget(editor);
            //}
        }
        private void MakeEditor(int x, int y)
        {
            HideEditor();
            //editor = new WidgetTileEditor(engine.Map, x, y);
            //int sX;
            //int sY;
            //engine.Camera.MapToScreen(x, y, out sX, out sY);
            //sX += engine.Camera.TileWidth;
            //sY += engine.Camera.TileHeight;
            //if (sX + editor.Width > engine.Camera.ViewWidth) { sX = engine.Camera.ViewWidth - editor.Width; }
            //if (sY + editor.Height > engine.Camera.ViewHeight) { sY = engine.Camera.ViewHeight - editor.Height; }
            //editor.SetPosition(sX, sY);
            //AddWidget(editor);
            //editor.Visible = true;
        }
    }
}
