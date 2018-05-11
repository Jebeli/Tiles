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

namespace TileEngine.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public struct Color : IEquatable<Color>
    {

        private byte r;
        private byte g;
        private byte b;
        private byte a;
        public Color(byte r, byte g, byte b, byte alpha = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = alpha;
        }
        public void Set(byte r, byte g, byte b, byte alpha = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = alpha;
        }

        public byte R
        {
            get { return r; }
            set { r = value; }
        }

        public byte G
        {
            get { return g; }
            set { g = value; }
        }
        public byte B
        {
            get { return b; }
            set { b = value; }
        }
        public byte Alpha
        {
            get { return a; }
            set { a = value; }
        }

        public bool IsEmpty
        {
            get { return a == 0 && r == 0 && g == 0 && b == 0; }
        }

        public bool Equals(Color other)
        {
            return r == other.r && g == other.g && b == other.b && a == other.a;
        }

        public static Color Empty
        {
            get { return new Color(0, 0, 0, 0); }
        }
        public static Color White
        {
            get { return new Color(255, 255, 255); }
        }
        public static Color Black
        {
            get { return new Color(0, 0, 0); }
        }
        public static Color Red
        {
            get { return new Color(255, 0, 0); }
        }
        public static Color Green
        {
            get { return new Color(0, 255, 0); }
        }
        public static Color Blue
        {
            get { return new Color(0, 0, 255); }
        }
        public static Color DimGray
        {
            get { return new Color(105, 105, 105); }
        }
        public static Color DarkGray
        {
            get { return new Color(50, 50, 50); }
        }

        public static Color Gray
        {
            get { return new Color(190, 190, 190); }
        }
        public static Color LightGray
        {
            get { return new Color(211, 211, 211); }
        }

        public static Color BrightGray
        {
            get { return new Color(232, 232, 232); }
        }

        public static Color WhiteSmoke
        {
            get { return new Color(245, 245, 245); }
        }

        public static Color LawnGreen
        {
            get { return new Color(124, 252, 0); }
        }

        public static Color LightBlue
        {
            get { return new Color(191, 239, 255); }
        }

        public static Color OrangeRed
        {
            get { return new Color(255, 69, 0); }
        }
    }
}
