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
    public class BasicInput : IInput
    {
        private float viewScale = 1.0f;
        private bool handleScale = true;
        public bool HandleScale
        {
            get { return handleScale; }
            set { handleScale = value; }
        }

        public float ViewScale
        {
            get { return viewScale; }
            set { viewScale = value; }
        }

        public event EventHandler<MouseEventArgs> OnMouseWheel;

        public event EventHandler<MouseEventArgs> OnMouseDown;

        public event EventHandler<MouseEventArgs> OnMouseUp;

        public event EventHandler<MouseEventArgs> OnMouseMove;

        public event EventHandler<KeyEventArgs> OnKeyDown;

        public event EventHandler<KeyEventArgs> OnKeyUp;

        public void MouseDown(int screenX, int screenY, MouseButton button)
        {
            OnMouseDown?.Invoke(this, CreateMouseEventArgs(screenX, screenY, button));
        }

        public void MouseMove(int screenX, int screenY, MouseButton button)
        {
            OnMouseMove?.Invoke(this, CreateMouseEventArgs(screenX, screenY, button));
        }

        public void MouseUp(int screenX, int screenY, MouseButton button)
        {
            OnMouseUp?.Invoke(this, CreateMouseEventArgs(screenX, screenY, button));
        }

        public void MouseWheel(int screenX, int screenY, int delta)
        {
            OnMouseWheel?.Invoke(this, CreateMouseEventArgs(screenX, screenY, delta));
        }

        public void KeyDown(Key key)
        {
            OnKeyDown?.Invoke(this, CreateKeyEventArgs(key, (char)0));
        }
        public void KeyUp(Key key)
        {
            OnKeyUp?.Invoke(this, CreateKeyEventArgs(key, (char)0));
        }

        private KeyEventArgs CreateKeyEventArgs(Key data, char code)
        {
            return new KeyEventArgs(data, code);
        }


        private MouseEventArgs CreateMouseEventArgs(int screenX, int screenY, MouseButton button)
        {
            float x = screenX;
            float y = screenY;
            if (handleScale)
            {
                float scale = viewScale;
                x *= scale;
                y *= scale;
            }
            return new MouseEventArgs(x, y, button);
        }

        private MouseEventArgs CreateMouseEventArgs(int screenX, int screenY, int delta)
        {
            float x = screenX;
            float y = screenY;
            if (handleScale)
            {
                float scale = viewScale;
                x *= scale;
                y *= scale;
            }
            return new MouseEventArgs(x, y, MouseButton.None, delta);
        }
    }
}
