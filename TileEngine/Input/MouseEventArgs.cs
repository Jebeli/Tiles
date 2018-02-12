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

namespace TileEngine.Input
{
    using System;
    public class MouseEventArgs : EventArgs
    {
        private readonly float x;
        private readonly float y;
        private readonly MouseButton button;
        private readonly int delta;

        public MouseEventArgs(float x, float y, MouseButton button = MouseButton.None, int delta = 0)
        {
            this.x = x;
            this.y = y;
            this.button = button;
            this.delta = delta;
        }
        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }

        public MouseButton Button
        {
            get { return button; }
        }

        public int Delta
        {
            get { return delta; }
        }
    }
}
