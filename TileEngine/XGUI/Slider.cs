using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.XGUI
{
    public class Slider : PropGadget
    {
        private int level;
        private int min;
        private int max;
        private Label formatLabel;
        private string format;

        public Slider(Widget parent)
            : base(parent)
        {
            min = 1;
            max = 16;
            level = 5;
            format = "{0}";
            ChangeValues();
        }

        public Label FormatLabel
        {
            get { return formatLabel; }
            set {
                if (formatLabel != value)
                {
                    formatLabel = value;
                    UpdateFormat();
                }
            }
        }

        public string Format
        {
            get { return format; }
            set {
                if (format != value)
                {
                    format = value;
                    UpdateFormat();
                }
            }
        }

        public int Level
        {
            get { return level; }
            set
            {
                value = Math.Min(max, value);
                value = Math.Max(min, value);
                if (level != value)
                {
                    level = value;
                    ChangeValues();
                }
            }
        }

        public int Min
        {
            get { return min; }
            set
            {
                if (min != value)
                {
                    min = value;
                    ChangeValues();
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
                    ChangeValues();
                }
            }
        }

        private void UpdateFormat()
        {
            if (formatLabel != null)
            {
                string txt;
                if (!string.IsNullOrEmpty(format))
                {
                    txt = string.Format(format, level);
                }
                else
                {
                    txt = level.ToString();
                }
                formatLabel.Text = txt;
            }
        }

        protected override void UpdateValues()
        {            
            int numLevels = max - min + 1;
            int pot = Flags.HasFlag(PropFlags.FreeHoriz) ? HorizPot : VertPot;
            int lev = FindSliderLevel(numLevels, pot);
            level = lev + min;
            UpdateFormat();
            base.UpdateValues();
        }

        private void ChangeValues()
        {
            int numLevels = max - min + 1;
            int lev = level - min;
            FindSliderValues(numLevels, lev, out int body, out int pot);
            if (Flags.HasFlag(PropFlags.FreeHoriz))
            {
                HorizBody = body;
                HorizPot = pot;
            }
            if (Flags.HasFlag(PropFlags.FreeVert))
            {
                VertBody = body;
                VertPot = pot;
            }
            CalcKnobSize();
            UpdateFormat();
            base.UpdateValues();
        }

        private void FindSliderValues(int numLevels, int level, out int body, out int pot)
        {
            if (numLevels > 0)
            {
                body = MAXBODY / numLevels;
            }
            else
            {
                body = MAXBODY;
            }
            if (numLevels > 1)
            {
                pot = (MAXPOT * level) / (numLevels - 1);
            }
            else
            {
                pot = 0;
            }
        }

        private int FindSliderLevel(int numLevels, int pot)
        {
            if (numLevels > 1)
            {
                return (pot * (numLevels - 1) + MAXPOT / 2) / MAXPOT;
            }
            else
            {
                return 0;
            }
        }

        public Orientation Orientation
        {
            get
            {
                if (Flags.HasFlag(PropFlags.FreeHoriz)) return Orientation.Horizontal;
                return Orientation.Vertical;
            }
            set
            {
                if (value == Orientation.Vertical)
                {
                    Flags &= ~PropFlags.FreeHoriz;
                    Flags |= PropFlags.FreeVert;
                }
                else
                {
                    Flags &= ~PropFlags.FreeVert;
                    Flags |= PropFlags.FreeHoriz;
                }
            }
        }
    }
}
