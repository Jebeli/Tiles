﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class CheckBoxGadget : Gadget
    {
        public CheckBoxGadget()
            : this(TagItems.Empty)
        {
        }

        public CheckBoxGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            Activation |= ActivationFlags.ToggleSelect;
            TextPlace = TextPlace.RightCenter;
            New(tags);
        }


    }
}
