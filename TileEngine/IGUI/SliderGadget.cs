using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class SliderGadget : PropGadget
    {
        private int min;
        private int max;
        private int level;
        private string levelFormat;
        private Gadget levelLabel;

        public SliderGadget() : this(TagItems.Empty)
        {
        }

        public SliderGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            total = 1;
            top = 0;
            visible = 1;
            overlap = 0;
            New(tags);
            SetValues();
        }

        public int Min
        {
            get { return min; }
            set
            {
                if (min != value)
                {
                    min = value;
                    SetValues();
                }
            }
        }

        public int Max
        {
            get { return max; }
            set
            {
                if (max != value)
                {
                    max = value;
                    SetValues();
                }
            }
        }

        public int Level
        {
            get { return level; }
            set
            {
                if (level != value)
                {
                    level = value;
                    SetValues();
                }
            }
        }

        public string LevelFormat
        {
            get { return levelFormat; }
            set { levelFormat = value; }
        }

        private void SetValues()
        {
            int num = Max - Min + 1;
            Set((Tags.PGA_Total, num),
                (Tags.PGA_Visible, 1),
                (Tags.PGA_Top, (Level - Min)));
            ChangeValues();
        }

        protected override void UpdateValues()
        {
            base.UpdateValues();
            level = top + min;
            if (levelLabel != null)
            {
                string levelText = string.Format(levelFormat, level);
                levelLabel.Set((Tags.GA_Text, levelText));
            }
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.SLIDER_Min:
                    Min = tag.GetTagData(0);
                    return 1;
                case Tags.SLIDER_Max:
                    Max = tag.GetTagData(0);
                    return 1;
                case Tags.SLIDER_Level:
                    Level = tag.GetTagData(0);
                    return 1;
                case Tags.SLIDER_Orientation:
                    Orientation = tag.GetTagData(Orientation.Vertical);
                    return 1;
                case Tags.SLIDER_LevelFormat:
                    levelFormat = tag.GetTagData("{0}");
                    return 1;
                case Tags.SLIDER_LevelLabel:
                    levelLabel = tag.GetTagData<Gadget>();
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }
    }
}
