using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public enum Tags
    {
        None,
        More,
        Skip,

        ICA_MAP,
        ICA_TARGET,

        IDCMP_Update,

        GA_Left,
        GA_RelRight,
        GA_Top,
        GA_RelBottom,
        GA_Width,
        GA_RelWidth,
        GA_Height,
        GA_RelHeight,
        GA_WidthWeight,
        GA_HeightWeight,
        GA_Text,
        GA_Image,
        GA_Disabled,
        GA_GZZGadget,
        GA_ID,
        GA_UserData,
        GA_Selected,
        GA_EndGadget,
        GA_Immediate,
        GA_RelVerify,
        GA_FollowMouse,
        GA_RightBorder,
        GA_LeftBorder,
        GA_TopBorder,
        GA_BottomBorder,
        GA_ToggleSelect,
        GA_SysGadget,
        GA_SysGType,
        GA_TabCycle,
        GA_Icon,
        GA_HintInfo,

        IA_Top,
        IA_Left,
        IA_Width,
        IA_Height,
        IA_SupportsDisable,
        IA_Recessed,
        IA_EdgesOnly,
        IA_FrameType,
        IA_Font,

        BEVEL_Style,
        BEVEL_Label,
        BEVEL_LabelPlace,

        STRINGA_MaxChars,
        STRINGA_Buffer,
        STRINGA_BufferPos,
        STRINGA_DispPos,
        STRINGA_Justification,
        STRINGA_LongVal,
        STRINGA_TextVal,
        STRINGA_Placeholder,

        PGA_Freedom,
        PGA_Borderless,
        PGA_Total,
        PGA_Visible,
        PGA_Top,

        SLIDER_Level,
        SLIDER_Min,
        SLIDER_Max,
        SLIDER_Orientation,
        SLIDER_LevelFormat,
        SLIDER_LevelLabel,

        SCROLLER_Orientation,
        SCROLLER_Total,
        SCROLLER_Visible,
        SCROLLER_Top,

        CHOOSER_Title,
        CHOOSER_PopUp,
        CHOOSER_DropDown,
        CHOOSER_Hidden,
        CHOOSER_Labels,
        CHOOSER_LabelArray,
        CHOOSER_MaxLabels,
        CHOOSER_Selected,
        CHOOSER_Width,

        CLICKTAB_Labels,
        CLICKTAB_Current,
        CLICKTAB_PageGroup,
        CLICKTAB_PageGroupBorder,

        PAGE_Add,
        PAGE_Remove,
        PAGE_Current,

        LISTBROWSER_Labels,
        LISTBROWSER_VPropTop,

        WA_Left,
        WA_Top,
        WA_Width,
        WA_Height,
        WA_Flags,
        WA_Gadgets,
        WA_Gadget,
        WA_Checkmark,
        WA_Title,
        WA_ScreenTitle,
        WA_MinWidth,
        WA_MinHeight,
        WA_MaxWidth,
        WA_MaxHeight,
        WA_InnerWidth,
        WA_InnerHeight,
        WA_Zoom,
        WA_SizeGadget,
        WA_DragBar,
        WA_DepthGadget,
        WA_CloseGadget,
        WA_Backdrop,
        WA_ReportMouse,
        WA_NoCareRefresh,
        WA_Borderless,
        WA_Activate,
        WA_RMBTrap,
        WA_SimpleRefresh,
        WA_SmartRefresh,
        WA_SizeBRight,
        WA_SizeBBottom,
        WA_AutoAdjust,
        WA_GimmeZeroZero,
        WA_MenuHelp,
        WA_NewLookMenus,
        WA_AmigaKey,
        WA_NotifyDepth,
        WA_Pointer,
        WA_BusyPointer,
        WA_PointerDelay,

        LAYOUT_Orientation,
        LAYOUT_FixedHoriz,
        LAYOUT_FixedVert,
        LAYOUT_HorizAlignment,
        LAYOUT_VertAlignment,
        LAYOUT_ShrinkWrap,
        LAYOUT_EvenSize,
        LAYOUT_SpaceInner,
        LAYOUT_SpaceOuter,
        LAYOUT_InnerSpacing,
        LAYOUT_TopSpacing,
        LAYOUT_BottomSpacing,
        LAYOUT_LeftSpacing,
        LAYOUT_RightSpacing,
        LAYOUT_Frame,
        LAYOUT_Label,
        LAYOUT_LabelImage,
        LAYOUT_LabelPlace,
        LAYOUT_RemoveChild,
        LAYOUT_AddChild,
        LAYOUT_AddChildren,
        LAYOUT_AddImage,
        LAYOUT_ModifyChild,
        LAYOUT_RelVerify,
        LAYOUT_RelCode,
        LAYOUT_TabVerify,
        LAYOUT_RelAddress,
        LAYOUT_HelpHit,
        LAYOUT_Parent,
        LAYOUT_DeferLayout,
        LAYOUT_RequestLayout,
        LAYOUT_RequestRefresh,
        LAYOUT_LabelColumn,
        LAYOUT_LabelWidth,
        LAYOUT_AlignLabels,
        LAYOUT_Inversed
    }

    public static class TagItems
    {
        private static readonly (Tags, object)[] empty = new(Tags, object)[0];
        public static (Tags, object)[] Empty
        {
            get { return empty; }
        }

        public static T GetTagData<T>(this (Tags, object) tag)
        {
            return GetTagData(tag, default(T));
        }
        public static T GetTagData<T>(this (Tags, object) tag, T def)
        {
            if (tag.Item2 != null && tag.Item2 is T)
            {
                return (T)tag.Item2;
            }
            return def;
        }

        public static IList<(Tags, object)> MapTags(this IList<(Tags, object)> tags, IList<(Tags, Tags)> attrMap)
        {
            if ((attrMap == null) || attrMap.Count == 0) return tags;
            var mapped = new List<(Tags, object)>();
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    var tag = tags[i];
                    var map = attrMap.FirstOrDefault(x => x.Item1.Equals(tag.Item1));
                    if (map.Item1 != Tags.None)
                    {
                        mapped.Add((map.Item2, tag.Item2));
                    }
                    else
                    {
                        mapped.Add(tag);
                    }
                }
            }
            return mapped;
        }


    }
}
