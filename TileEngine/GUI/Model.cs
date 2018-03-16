using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI
{
    public class Model : IC
    {
        public Model() 
        {
        }

        protected override int UpdateTags(GadgetInfo gadgetInfo, UpdateFlags updateFlags, IList<(Tags, object)> attrList)
        {
            if (CheckLoop())
            {
                SetLoop();
                foreach (var mem in Members)
                {
                    mem.Update(gadgetInfo, updateFlags, attrList);
                }
                ClearLoop();
            }
            return base.UpdateTags(gadgetInfo, updateFlags, attrList);
        }
    }
}
