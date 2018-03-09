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

    public class IntuiMessage : EventArgs
    {
        private readonly IDCMPFlags message;
        private readonly Gadget gadget;
        private readonly int code;

        public IntuiMessage(IDCMPFlags msg, Gadget gad, int code)
        {
            message = msg;
            gadget = gad;
            this.code = code;
        }

        public IDCMPFlags Message
        {
            get { return message; }
        }

        public Gadget Gadget
        {
            get { return gadget; }
        }

        public int Code
        {
            get { return code; }
        }
    }
}
