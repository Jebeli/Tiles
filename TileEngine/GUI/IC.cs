using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI
{
    public class IC : Root
    {
        public IC() 
        {
        }

        protected override int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update)
        {
            return 0;
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            return 0;
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            return returnValue;
        }
    }
}
