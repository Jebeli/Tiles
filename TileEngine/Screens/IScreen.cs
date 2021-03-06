﻿/*
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
    using System.Collections.Generic;
    using TileEngine.Fonts;
    using TileEngine.Graphics;

    public interface IScreen
    {
        Font Font { get; }
        Color BackgroundColor { get; }
        string Name { get; }
        void Show();
        void Hide();
        void Update(TimeInfo time);
        void Render(TimeInfo time);
        void SizeChanged(int width, int height);
        void PerformLayout();
    }
}
