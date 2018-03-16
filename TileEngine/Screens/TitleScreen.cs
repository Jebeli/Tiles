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
        private const int GADID_NEW = 100;
        private const int GADID_CONT = 101;
        private const int GADID_LOAD = 102;
        private const int GADID_QUIT = 103;
        private const int GADID_TEST = 104;
        private const int buttonWidth = 128;
        private const int buttonHeight = 30;
        private List<Gadget> glist;
        private Window window;

        public TitleScreen(Engine engine)
            : base(engine, "TitleScreen", "TILES")
        {
            MakeWindow();
        }

        private void MakeWindow()
        {
            int buttonLeft = 100;
            int buttonTop = 100;
            glist = new List<Gadget>();
            var btnImage = Intuition.NewObject(Intuition.FRAMEICLASS,
                (Tags.IA_Width, buttonWidth),
                (Tags.IA_Height, buttonHeight),
                (Tags.IA_EdgesOnly, false),
                (Tags.IA_FrameType, FrameType.Button)
                );
            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, buttonLeft),
                (Tags.GA_Top, buttonTop),
                (Tags.GA_Width, buttonWidth),
                (Tags.GA_Height, buttonHeight),
                (Tags.GA_Text, "New Game"),
                (Tags.GA_ID,GADID_NEW),
                (Tags.GA_Image, btnImage)
                );
            buttonTop += 40;
            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, buttonLeft),
                (Tags.GA_Top, buttonTop),
                (Tags.GA_Width, buttonWidth),
                (Tags.GA_Height, buttonHeight),
                (Tags.GA_Text, "Continue Game"),
                (Tags.GA_ID, GADID_CONT),
                (Tags.GA_Image, btnImage)
                );
            buttonTop += 40;
            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, buttonLeft),
                (Tags.GA_Top, buttonTop),
                (Tags.GA_Width, buttonWidth),
                (Tags.GA_Height, buttonHeight),
                (Tags.GA_Disabled,true),
                (Tags.GA_Text, "Load Game"),
                (Tags.GA_ID, GADID_LOAD),
                (Tags.GA_Image, btnImage)
                );
            buttonTop += 40;
            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, buttonLeft),
                (Tags.GA_Top, buttonTop),
                (Tags.GA_Width, buttonWidth),
                (Tags.GA_Height, buttonHeight),
                (Tags.GA_Text, "Quit Game"),
                (Tags.GA_ID, GADID_QUIT),
                (Tags.GA_Image, btnImage)
                );
            buttonTop += 40;
            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, buttonLeft),
                (Tags.GA_Top, buttonTop),
                (Tags.GA_Width, buttonWidth),
                (Tags.GA_Height, buttonHeight),
                (Tags.GA_Text, "Intui Test"),
                (Tags.GA_ID, GADID_TEST),
                (Tags.GA_Image, btnImage)
                );

            window = Intuition.OpenWindowTags(null,
                (Tags.WA_Left, 0),
                (Tags.WA_Top, 0),
                (Tags.WA_Width, engine.Graphics.ViewWidth),
                (Tags.WA_Height, engine.Graphics.ViewHeight),
                (Tags.WA_Flags, WindowFlags.WFLG_BORDERLESS | WindowFlags.WFLG_BACKDROP),
                (Tags.WA_IDCMP, IDCMPFlags.GADGETUP),
                (Tags.WA_Gadgets, glist),
                (Tags.WA_BackgroundColor, Color.Black),
                (Tags.WA_Screen, this));

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
            int x = engine.Graphics.ViewWidth / 2 - buttonWidth / 2;
            foreach(var gad in glist)
            {
                gad.Set((Tags.GA_Left,x),(Tags.GA_Top,y));
                y += 40;
            }
        }

        public override void Render(TimeInfo time)
        {
            base.Render(time);
        }

        protected override void OnGadgetClick(Gadget gadget)
        {
            base.OnGadgetClick(gadget);
            switch (gadget.GadgetId)
            {
                case GADID_QUIT:
                    engine.SwitchToExitScreen();
                    break;
                case GADID_LOAD:
                    engine.SwitchToLoadScreen();
                    break;
                case GADID_CONT:
                    engine.SwitchToLoadScreen();
                    break;
                case GADID_NEW:
                    engine.SwitchToLoadScreen();
                    break;
                case GADID_TEST:
                    engine.SwitchToTestScreen();
                    break;
            }
        }

    }
}
