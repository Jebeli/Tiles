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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.GUI;

    public class TitleScreen : TextTitleScreen
    {
        private WidgetButton newButton;
        private WidgetButton contButton;
        private WidgetButton loadButton;
        private WidgetButton exitButton;
        public TitleScreen(Engine engine)
            : base(engine, "TitleScreen", "TILES")
        {
            newButton = new WidgetButton("New Game");
            contButton = new WidgetButton("Continue Game");
            loadButton = new WidgetButton("Load Game");
            exitButton = new WidgetButton("Quit Game");
            newButton.SetBounds(100, 100, 128, 30);
            contButton.SetBounds(100, 130, 128, 30);
            loadButton.SetBounds(100, 160, 128, 30);
            exitButton.SetBounds(100, 190, 128, 30);
            AddWidget(newButton);
            AddWidget(contButton);
            AddWidget(loadButton);
            AddWidget(exitButton);
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
            int y = engine.Graphics.ViewHeight / 2 + 75;
            int x = engine.Graphics.ViewWidth / 2 - newButton.Width / 2;
            newButton.SetPosition(x, y);
            contButton.SetPosition(x, y + 40);
            loadButton.SetPosition(x, y + 80);
            exitButton.SetPosition(x, y + 120);
        }

        public override void Render(TimeInfo time)
        {
            base.Render(time);
        }

        protected override void OnWidgetClick(Widget widget)
        {
            base.OnWidgetClick(widget);
            if (widget == exitButton)
            {
                engine.SwitchToExitScreen();
            }
            else if (widget == loadButton)
            {
                engine.SwitchToLoadScreen();
            }
            else if (widget == contButton)
            {
                engine.SwitchToLoadScreen();
            }
            else if (widget == newButton)
            {
                engine.SwitchToLoadScreen();
            }
        }
    }
}
