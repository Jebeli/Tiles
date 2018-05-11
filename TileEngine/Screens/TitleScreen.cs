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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using YGUI;

    public class TitleScreen : TextTitleScreen
    {
        private const int GADID_NEW = 200;
        private const int GADID_CONT = 201;
        private const int GADID_LOAD = 202;
        private const int GADID_QUIT = 203;
        private const int GADID_TEST = 204;
        private Window window;
        public TitleScreen(Engine engine)
            : base(engine, "TitleScreen", "TILES")
        {
        }

        protected override void InitGUI(Screen screen)
        {
            window = new Window(screen)
            {
                LeftEdge = (800 - 128) / 2,
                TopEdge = (600 - 180) / 2 + 200,
                Width = 128,
                Height = 200,
                ThinBorder = true
            };
            new ButtonGadget(window, "New Game")
            {
                GadgetUpEvent = (i, o) => { engine.SwitchToLoadScreen(); }
            };
            new ButtonGadget(window, "Continue Game")
            {
                Width = 120,
                GadgetUpEvent = (i, o) => { engine.SwitchToLoadScreen(); }
            };
            new ButtonGadget(window, "Load Game")
            {
                Enabled = false,
                GadgetUpEvent = (i, o) => { engine.SwitchToLoadScreen(); }
            };
            new ButtonGadget(window, "Quit Game")
            {
                GadgetUpEvent = (i, o) => { engine.SwitchToExitScreen(); }
            };
            new ButtonGadget(window, "Test Screen")
            {
                GadgetUpEvent = (i, o) => { engine.SwitchToTestScreen(); }
            };           
        }

        protected override void OnScreenSizeChanged(int width, int height)
        {
            window.LeftEdge = width / 2 - window.Width / 2;
            window.TopEdge = height / 2 - window.Height / 2 + 160;
        }

    }
}
