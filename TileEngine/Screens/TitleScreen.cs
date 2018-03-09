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
    using Graphics;
    using GUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;

    public class TitleScreen : TextTitleScreen
    {
        private Window window;

        private Gadget newButton;
        private Gadget contButton;
        private Gadget loadButton;
        private Gadget exitButton;
        private Gadget testButton;
        public TitleScreen(Engine engine)
            : base(engine, "TitleScreen", "TILES")
        {
            MakeWindow();
        }

        private void MakeWindow()
        {
            newButton = Gadget.MakeBoolGadget("New Game", 128, 30);
            contButton = Gadget.MakeBoolGadget("Continue Game", 128, 30);
            loadButton = Gadget.MakeBoolGadget("Load Game", 128, 30);
            exitButton = Gadget.MakeBoolGadget("Quit Game", 128, 30);
            testButton = Gadget.MakeBoolGadget("Intui Test", 128, 30);
            newButton.SetPosition(100, 100);
            contButton.SetPosition(100, 130);
            loadButton.SetPosition(100, 160);
            exitButton.SetPosition(100, 190);
            testButton.SetPosition(100, 220);

            window = Intuition.OpenWindowTags(null,
                Tag(WATags.WA_Left, 0),
                Tag(WATags.WA_Top, 0),
                Tag(WATags.WA_Width, engine.Graphics.ViewWidth),
                Tag(WATags.WA_Height, engine.Graphics.ViewHeight),
                Tag(WATags.WA_Flags, WindowFlags.WFLG_BORDERLESS | WindowFlags.WFLG_BACKDROP),
                Tag(WATags.WA_IDCMP, IDCMPFlags.GADGETUP),
                Tag(WATags.WA_Gadgets, new[] { newButton, contButton, loadButton, exitButton, testButton }),
                Tag(WATags.WA_BackgroundColor, Color.Black),
                Tag(WATags.WA_Screen, this));

            Intuition.OffGadget(loadButton, window);
        }

        private void CloseWindow()
        {
            Intuition.CloseWindow(window);
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
            window.SetWindowBox(0, engine.Graphics.ViewHeight / 2 + 75, engine.Graphics.ViewWidth, engine.Graphics.ViewHeight / 2 - 75);
            int y = 0;
            int x = engine.Graphics.ViewWidth / 2 - newButton.Width / 2;
            newButton.SetPosition(x, y);
            contButton.SetPosition(x, y + 40);
            loadButton.SetPosition(x, y + 80);
            exitButton.SetPosition(x, y + 120);
            testButton.SetPosition(x, y + 160);
        }

        public override void Render(TimeInfo time)
        {
            base.Render(time);
        }

        protected override void OnGadgetClick(Gadget gadget)
        {
            base.OnGadgetClick(gadget);
            if (gadget == exitButton)
            {
                engine.SwitchToExitScreen();
            }
            else if (gadget == loadButton)
            {
                engine.SwitchToLoadScreen();
            }
            else if (gadget == contButton)
            {
                engine.SwitchToLoadScreen();
            }
            else if (gadget == newButton)
            {
                engine.SwitchToLoadScreen();
            }
            else if (gadget == testButton)
            {
                engine.SwitchToTestScreen();
            }
        }

    }
}
