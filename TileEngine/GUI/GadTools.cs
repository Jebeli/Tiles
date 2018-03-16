using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;
using TileEngine.GUI.GadToolsClasses;
using TileEngine.Input;
using TileEngine.Screens;

namespace TileEngine.GUI
{
    public static class GadTools
    {
        private static Engine engine;

        private static IClass buttonclass;
        private static IClass checkboxclass;
        private static IClass cycleclass;
        private static IClass textclass;
        private static IClass stringclass;
        private static IClass scrollerclass;
        private static IClass sliderclass;
        private static IClass arrowclass;
        private static IClass mxclass;
        private static IClass listviewclass;

        public static void Init(Engine engine)
        {
            GadTools.engine = engine;
            InitClasses();
        }

        private static void InitClasses()
        {
            buttonclass = new IClass(typeof(ButtonClass));
            checkboxclass = new IClass(typeof(CheckboxClass));
            cycleclass = new IClass(typeof(CycleClass));
            textclass = new IClass(typeof(TextClass));
            stringclass = new IClass(typeof(StringClass));
            scrollerclass = new IClass(typeof(ScrollerClass));
            sliderclass = new IClass(typeof(SliderClass));
            arrowclass = new IClass(typeof(ArrowClass));
            mxclass = new IClass(typeof(MxClass));
            listviewclass = new IClass(typeof(ListviewClass));
        }

        public static Gadget CreateGadget(GadKind kind, List<Gadget> glist, NewGadget newGadget, params (Tags, object)[] tags)
        {
            Gadget gadget = null;
            var tagList = NewGadgetToTagList(newGadget, kind).JoinTags(tags);
            switch (kind)
            {
                case GadKind.Button:
                    gadget = Intuition.NewObject(buttonclass, tagList) as Gadget;
                    break;
                case GadKind.Checkbox:
                    gadget = Intuition.NewObject(checkboxclass, tagList) as Gadget;
                    break;
                case GadKind.Cycle:
                    gadget = Intuition.NewObject(cycleclass, tagList) as Gadget;
                    break;
                case GadKind.Text:
                    gadget = Intuition.NewObject(textclass, tagList.AddTag((Tags.GTA_GadgetKind, GadKind.Text))) as Gadget;
                    break;
                case GadKind.Number:
                    gadget = Intuition.NewObject(textclass, tagList.AddTag((Tags.GTA_GadgetKind, GadKind.Number))) as Gadget;
                    break;
                case GadKind.String:
                    gadget = Intuition.NewObject(stringclass, tagList.AddTag((Tags.GTA_GadgetKind, GadKind.String))) as Gadget;
                    break;
                case GadKind.Integer:
                    gadget = Intuition.NewObject(stringclass, tagList.AddTag((Tags.GTA_GadgetKind, GadKind.Integer))) as Gadget;
                    break;
                case GadKind.Scroller:
                    gadget = Intuition.NewObject(scrollerclass, tagList) as Gadget;
                    if (gadget != null)
                    {
                        Gadget arrow1 = Intuition.NewObject(arrowclass, GetArrowTagList(new NewGadget(newGadget), gadget, true).JoinTags(tags)) as Gadget;
                        Gadget arrow2 = Intuition.NewObject(arrowclass, GetArrowTagList(new NewGadget(newGadget), gadget, false).JoinTags(tags)) as Gadget;
                        if (arrow1 != null && arrow2 != null)
                        {
                            glist.Add(arrow1);
                            glist.Add(arrow2);
                            gadget.Set((Tags.GTA_ScrollerArrow1, arrow1), (Tags.GTA_ScrollerArrow2, arrow2));
                        }
                    }
                    break;
                case GadKind.Slider:
                    gadget = Intuition.NewObject(sliderclass, tagList) as Gadget;
                    if (gadget != null)
                    {
                        Gadget formatGad = Intuition.NewObject(textclass, GetFormatGadgetTagList(new NewGadget(newGadget)).JoinTags(tags)) as Gadget;
                        if (formatGad != null)
                        {
                            glist.Add(formatGad);
                            gadget.Set((Tags.ICA_TARGET, formatGad));
                        }
                    }
                    break;
                case GadKind.Mx:
                    newGadget.Width = 24;
                    newGadget.Height = 18;
                    var items = tags.GetTagData<IList<object>>(Tags.GTMX_Labels);
                    int spacing = tags.GetIntTag(Tags.GTMX_Spacing);
                    int active = tags.GetIntTag(Tags.GTMX_Active);
                    tagList = NewGadgetToTagList(newGadget, kind).JoinTags(tags);
                    gadget = Intuition.NewObject(mxclass, tagList.AddTag((Tags.GA_Text, items[0].ToString()))) as Gadget;
                    List<Gadget> mxGadgets = new List<Gadget>();
                    mxGadgets.Add(gadget);
                    ((MxClass)gadget).MxGadgets = mxGadgets;
                    if (gadget != null)
                    {
                        gadget.Checked = active == 0;
                        for (int i = 1; i < items.Count; i++)
                        {
                            newGadget.TopEdge += 18;
                            newGadget.TopEdge += spacing;
                            tagList = NewGadgetToTagList(newGadget, kind).JoinTags(tags);
                            Gadget mx = Intuition.NewObject(mxclass, tagList.AddTag((Tags.GA_Text, items[i].ToString()))) as Gadget;
                            mx.Checked = active == i;
                            mxGadgets.Add(mx);
                            glist.Add(mx);
                            ((MxClass)mx).MxGadgets = mxGadgets;
                        }
                    }
                    break;
                case GadKind.ListView:
                    int scrollWidth = tags.GetIntTag(Tags.GTLV_ScrollWidth, 16);
                    newGadget.Width -= scrollWidth;
                    var lvitems = tags.GetTagData<IList<object>>(Tags.GTLV_Labels);
                    int itemHeight = 18;
                    tagList = NewGadgetToTagList(newGadget, kind).JoinTags(tags);
                    gadget = Intuition.NewObject(listviewclass, tagList) as Gadget;
                    Gadget scroller = MakeListViewScrollBar(newGadget.VisualInfo, gadget, glist, scrollWidth, lvitems.Count, itemHeight);
                    ((ListviewClass)gadget).Scroller = scroller;
                    break;
            }
            if (gadget != null) glist.Add(gadget);
            return gadget;
        }

        private static IList<(Tags, object)> GetArrowTagList(NewGadget ng, Gadget scroller, bool first)
        {
            var tags = new List<(Tags, object)>();
            tags.Add((Tags.GTA_ArrowScroller, scroller));
            bool vert = (scroller.PropInfo.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT;
            SysImageType arrowType;
            if (vert)
            {
                if (first)
                {
                    arrowType = SysImageType.Down;
                    ng.TopEdge = ng.TopEdge + ng.Height - 20;
                    ng.Height = 20;
                    scroller.Height -= 20;
                }
                else
                {
                    arrowType = SysImageType.Up;
                    ng.TopEdge = ng.TopEdge + ng.Height - 40;
                    ng.Height = 20;
                    scroller.Height -= 20;
                }
            }
            else
            {
                if (first)
                {
                    arrowType = SysImageType.Right;
                    ng.LeftEdge = ng.LeftEdge + ng.Width - 20;
                    ng.Width = 20;
                    scroller.Width -= 20;
                }
                else
                {
                    arrowType = SysImageType.Left;
                    ng.LeftEdge = ng.LeftEdge + ng.Width - 40;
                    ng.Width = 20;
                    scroller.Width -= 20;
                }
            }
            tags.Add((Tags.GTA_Arrow_Type, arrowType));
            tags.AddRange(NewGadgetToTagList(ng, GadKind.Scroller));
            return tags;
        }

        private static IList<(Tags, object)> GetFormatGadgetTagList(NewGadget ng)
        {
            var tags = new List<(Tags, object)>();
            ng.TopEdge -= 20;
            tags.Add((Tags.GTA_GadgetKind, GadKind.Integer));
            tags.AddRange(NewGadgetToTagList(ng, GadKind.Integer));
            return tags;
        }

        private static IList<(Tags, object)> NewGadgetToTagList(NewGadget ng, GadKind kind)
        {
            var tags = new List<(Tags, object)>();
            if (ng.LeftEdge >= 0)
            {
                tags.Add((Tags.GA_Left, ng.LeftEdge));
            }
            else
            {
                tags.Add((Tags.GA_RelRight, ng.LeftEdge));
            }
            if (ng.TopEdge >= 0)
            {
                tags.Add((Tags.GA_Top, ng.TopEdge));
            }
            else
            {
                tags.Add((Tags.GA_RelBottom, ng.TopEdge));
            }
            if (ng.Width > 0)
            {
                tags.Add((Tags.GA_Width, ng.Width));
            }
            else
            {
                tags.Add((Tags.GA_RelWidth, ng.Width));
            }
            if (ng.Height > 0)
            {
                tags.Add((Tags.GA_Height, ng.Height));
            }
            else
            {
                tags.Add((Tags.GA_RelHeight, ng.Height));
            }
            tags.Add((Tags.GA_Text, ng.GadgetText));
            tags.Add((Tags.GA_ID, ng.GadgetId));
            tags.Add((Tags.GA_UserData, ng.UserData));
            tags.Add((Tags.GA_LabelPlace, NewGadgetFlagsToLabelPlace(ng.Flags, kind)));
            tags.Add((Tags.GT_VisualInfo, ng.VisualInfo));
            tags.Add((Tags.GT_Flags, ng.Flags));
            return tags;
        }

        private static LabelPlace NewGadgetFlagsToLabelPlace(NewGadgetFlags flags, GadKind kind)
        {
            switch (flags & NewGadgetFlags.PlaceText)
            {
                case NewGadgetFlags.PlaceTextAbove:
                    return LabelPlace.PlaceTextAbove;
                case NewGadgetFlags.PlaceTextBelow:
                    return LabelPlace.PlaceTextBelow;
                case NewGadgetFlags.PlaceTextLeft:
                    return LabelPlace.PlaceTextLeft;
                case NewGadgetFlags.PlaceTextRight:
                    return LabelPlace.PlaceTextRight;
                case NewGadgetFlags.PlaceTextIn:
                    return LabelPlace.PlaceTextIn;
                default:
                    switch (kind)
                    {
                        case GadKind.String:
                        case GadKind.Integer:
                        case GadKind.Slider:
                        case GadKind.Cycle:
                            return LabelPlace.PlaceTextLeft;
                        case GadKind.Checkbox:
                        case GadKind.Mx:
                            return LabelPlace.PlaceTextRight;
                        default:
                            return LabelPlace.PlaceTextIn;
                    }
            }
        }

        private static Gadget MakeListViewScrollBar(VisualInfo info, Gadget gadget, List<Gadget> glist, int width, int total, int itemHeight)
        {
            int visible = gadget.Height / itemHeight;
            Gadget sc = CreateGadget(GadKind.Scroller, glist, new NewGadget()
            {
                LeftEdge = gadget.LeftEdge + gadget.Width,
                TopEdge = gadget.TopEdge,
                Width = width,
                Height = gadget.Height,
                VisualInfo = info
            },
            (Tags.GTSC_Arrows, 20),
            (Tags.PGA_Freedom, PropFlags.FREEVERT),
            (Tags.GTSC_Total, total),
            (Tags.GTSC_Visible, visible),
            (Tags.ICA_TARGET, gadget)
            );
            return sc;
        }

        public static VisualInfo GetVisualInfo(IScreen screen)
        {
            VisualInfo info = new VisualInfo();
            info.Screen = screen;
            return info;
        }

        public static void FreeVisualInfo(VisualInfo info)
        {

        }

    }
}
