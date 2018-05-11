using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.Nano
{
    public class Theme
    {
        public Theme()
        {
            WindowHeaderHeight = 30;
            StandardFontSize = 16;
            ButtonFontSize = 20;
            ButtonFocused = new Color(64, 64, 64);
            ButtonBotFocused = new Color(48, 48, 48);
            ButtonUnfocused = new Color(74, 74, 74);
            ButtonBotUnfocused = new Color(58, 58, 58);
            ButtonPushed = new Color(41, 41, 41);
            ButtonBotPushed = new Color(29, 29, 29);
            TextColor = new Color(255, 255, 255, 160);
            DisabledTextColor = new Color(128, 128, 128, 80);
            BorderDark = new Color(29, 29, 29);
            BorderLight = new Color(92, 92, 92);
            BorderMedium = new Color(35, 35, 35);
            WindowFillFocused = new Color(45, 45, 45, 230);
            WindowFillUnfocused = new Color(43, 43, 43, 230);
            WindowHeader = ButtonUnfocused;
            WindowHeaderBot = ButtonBotUnfocused;
            WindowTitleFocused = new Color(255, 255, 255, 190);
            WindowTitleUnfocused = new Color(220, 220, 220, 160);
            WindowPopup = new Color(50, 50, 50, 255);

            PopupChevronRightIcon = Icons.ENTYPO_ICON_CHEVRON_RIGHT;
            PopupChevronLeftIcon = Icons.ENTYPO_ICON_CHEVRON_LEFT;
            CheckBoxIcon = Icons.ENTYPO_ICON_CHECK;
            MessageAltButtonIcon = Icons.ENTYPO_ICON_CIRCLE_WITH_CROSS;
            MessagePrimaryButtonIcon = Icons.ENTYPO_ICON_CHECK;
            MessageInformationIcon = Icons.ENTYPO_ICON_INFO_WITH_CIRCLE;
            MessageQuestionIcon = Icons.ENTYPO_ICON_HELP_WITH_CIRCLE;
            MessageWarningIcon = Icons.ENTYPO_ICON_WARNING;
        }
        public int WindowHeaderHeight { get; set; }
        public int StandardFontSize { get; set; }
        public int ButtonFontSize { get; set; }
        public Color TextColor { get; set; }
        public Color DisabledTextColor { get; set; }
        public Color ButtonFocused { get; set; }
        public Color ButtonBotFocused { get; set; }
        public Color ButtonUnfocused { get; set; }
        public Color ButtonBotUnfocused { get; set; }
        public Color ButtonPushed { get; set; }
        public Color ButtonBotPushed { get; set; }
        public Color BorderLight { get; set; }
        public Color BorderDark { get; set; }
        public Color BorderMedium { get; set; }
        public Color WindowFillFocused { get; set; }
        public Color WindowFillUnfocused { get; set; }
        public Color WindowHeader { get; set; }
        public Color WindowHeaderBot { get; set; }
        public Color WindowTitleFocused { get; set; }
        public Color WindowTitleUnfocused { get; set; }
        public Color WindowPopup { get; set; }

        public Icons PopupChevronRightIcon { get; set; }
        public Icons PopupChevronLeftIcon { get; set; }
        public Icons CheckBoxIcon { get; set; }
        public Icons MessageInformationIcon { get; set; }
        public Icons MessageQuestionIcon { get; set; }
        public Icons MessageWarningIcon { get; set; }

        public Icons MessageAltButtonIcon { get; set; }
        public Icons MessagePrimaryButtonIcon { get; set; }


    }
}
