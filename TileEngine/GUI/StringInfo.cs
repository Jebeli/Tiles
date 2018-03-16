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

namespace TileEngine.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StringInfo
    {
        private int bufferPos;
        public StringInfo(string buffer = "")
        {
            Buffer = buffer ?? "";
            UndoBuffer = "";
            MaxChars = short.MaxValue;
            BufferPos = buffer.Length;
        }

        public string Buffer { get; set; }
        public string UndoBuffer { get; set; }
        public int BufferPos
        {
            get { return bufferPos; }
            set
            {
                if (value < 0) value = 0;
                if (value >= NumChars) value = NumChars;
                bufferPos = value;
            }
        }
        public int MaxChars { get; set; }
        public int DispPos { get; set; }
        public int UndoPos { get; set; }
        public int NumChars
        {
            get { return Buffer.Length; }
        }

        public int DispCount { get; set; }
        public int CLeft { get; set; }
        public int CTop { get; set; }
        public int LongInt { get; set; }
    }
}
