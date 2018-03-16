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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class KeyEventArgs : EventArgs
    {
        private readonly Key keyCode;
        private readonly Key keyData;
        private readonly char code;
        private bool handled;

        public KeyEventArgs(Key keyData, char code)
        {
            if (keyData == Key.None)
            {
                this.keyData = CharToKeyCode(code);
                keyCode = (Key)((int)this.keyData & (int)Key.KeyCode);
                this.code = code;
            }
            else
            {
                this.keyData = keyData;
                keyCode = (Key)((int)this.keyData & (int)Key.KeyCode);
                this.code = KeyCodeToChar(this.keyData);
            }
        }

        public Key KeyData { get { return keyData; } }
        public Key KeyCode { get { return keyCode; } }
        public char Code { get { return code; } }
        public bool Handled { get { return handled; } set { handled = value; } }

        public Key CharToKeyCode(char c)
        {
            Key res = Key.None;
            bool upper = char.IsUpper(c);
            bool lower = !upper;
            if (lower)
            {
                c = char.ToUpperInvariant(c);
            }
            res = (Key)c;
            if (upper)
            {
                res = (Key)((int)c | (int)Key.Shift);
            }
            return res;
        }
        public char KeyCodeToChar(Key keys)
        {
            char c = (char)0;
            Key modifier = keys & Key.Modifiers;
            Key code = keys & Key.KeyCode;
            if (code != Key.None)
            {
                switch (code)
                {
                    case Key.A:
                    case Key.B:
                    case Key.C:
                    case Key.D:
                    case Key.E:
                    case Key.F:
                    case Key.G:
                    case Key.H:
                    case Key.I:
                    case Key.J:
                    case Key.K:
                    case Key.L:
                    case Key.M:
                    case Key.N:
                    case Key.O:
                    case Key.P:
                    case Key.Q:
                    case Key.R:
                    case Key.S:
                    case Key.T:
                    case Key.U:
                    case Key.V:
                    case Key.W:
                    case Key.X:
                    case Key.Y:
                    case Key.Z:
                        c = (char)code;
                        if ((modifier & Key.Shift) == Key.Shift)
                        {
                            c = char.ToUpper(c);
                        }
                        else
                        {
                            c = char.ToLower(c);
                        }
                        break;
                    case Key.Escape:
                    case Key.Space:
                        c = (char)code;
                        break;
                    case Key.D0:
                        c = '0';
                        break;
                    case Key.D1:
                        c = '1';
                        break;
                    case Key.D2:
                        c = '2';
                        break;
                    case Key.D3:
                        c = '3';
                        break;
                    case Key.D4:
                        c = '4';
                        break;
                    case Key.D5:
                        c = '5';
                        break;
                    case Key.D6:
                        c = '6';
                        break;
                    case Key.D7:
                        c = '7';
                        break;
                    case Key.D8:
                        c = '8';
                        break;
                    case Key.D9:
                        c = '9';
                        break;
                    case Key.OemMinus:
                        c = '-';
                        break;
                    case Key.Oemplus:
                        c = '+';
                        break;
                }
            }
            return c;
        }
    }
}
