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
    public enum MouseButton
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4
    }
    public interface IInput
    {
        bool HandleScale { get; set; }
        float ViewScale { get; set; }

        event EventHandler<MouseEventArgs> OnMouseWheel;

        event EventHandler<MouseEventArgs> OnMouseDown;

        event EventHandler<MouseEventArgs> OnMouseUp;

        event EventHandler<MouseEventArgs> OnMouseMove;
        void MouseWheel(int screenX, int screenY, int delta);
        void MouseDown(int screenX, int screenY, MouseButton button);
        void MouseUp(int screenX, int screenY, MouseButton button);
        void MouseMove(int screenX, int screenY, MouseButton button);
    }
}
