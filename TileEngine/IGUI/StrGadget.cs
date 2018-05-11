using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.IGUI
{
    public class StrGadget : Gadget
    {
        private string buffer;
        private string placeholder;
        public StrGadget()
            : this(TagItems.Empty)
        {
        }

        public StrGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.StrGadget;
            PreferredWidth = 100;
            PreferredHeight = 32;
            New(tags);
        }

        public string Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; }
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.STRINGA_Buffer:
                    buffer = tag.GetTagData("");
                    return 1;
                case Tags.STRINGA_TextVal:
                    buffer = tag.GetTagData("");
                    return 1;
                case Tags.STRINGA_LongVal:
                    buffer = tag.GetTagData(0).ToString();
                    return 1;
                case Tags.STRINGA_Placeholder:
                    placeholder = tag.GetTagData("");
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }

    }
}
