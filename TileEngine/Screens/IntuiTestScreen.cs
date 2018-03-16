using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.GUI;

namespace TileEngine.Screens
{
    public class IntuiTestScreen : AbstractScreen
    {
        private const int GADID_EXIT = 100;
        private const int GADID_REQ = 101;
        private const int GADID_AUTO = 102;
        private const int GADID_DEMO = 103;
        private const int GADID_BOOPSI = 104;

        private Window backWin;
        private Window win1;
        private Window win2;
        private Gadget gadProp1;
        private Gadget gadProp2;
        private Gadget gadStr1;
        private Requester req1;

        private Window gtWin;
        private Gadget gtExit;
        private Gadget gtDisable;
        private Gadget gtCycle;
        private Gadget gtText;
        private Gadget gtNumber;
        private Gadget gtMx;
        private Gadget gtStr;
        private Gadget gtStr2;
        private Gadget gtInt;
        private Gadget gtScr;
        private Gadget gtScr2;
        private Gadget gtSli;
        private Gadget gtList;

        private Window bWin;

        public IntuiTestScreen(Engine engine)
            : base(engine, "IntuiTestScreen")
        {
            MakeWindows();
        }

        private void MakeWindows()
        {
            backWin = Intuition.OpenWindowTags(null,
                (Tags.WA_Width, 800),
                (Tags.WA_Height, 800),
                (Tags.WA_Flags, WindowFlags.WFLG_BACKDROP | WindowFlags.WFLG_BORDERLESS),
                (Tags.WA_Screen, this));

            OpenWinA();

            gadProp1 = Gadget.MakeHorizPropGadget(1000, 10, 0, -40, 20);
            gadProp1.SetPosition(4 + 5, 5 + 24);
            gadProp2 = Gadget.MakeVertPropGadget(100, 10, 0, 50, 50);
            gadProp2.SetPosition(4 + 5, 5 + 24 + 25);
            gadStr1 = Gadget.MakeStringGadget("Hello", 100, 20);
            gadStr1.SetPosition(4 + 5, 5 + 24 + 25 + 50 + 5);

            win2 = Intuition.OpenWindowTags(null,
                (Tags.WA_Left, 120),
                (Tags.WA_Top, 120),
                (Tags.WA_Width, 180),
                (Tags.WA_Height, 200),
                (Tags.WA_MinWidth, 180),
                (Tags.WA_MinHeight, 200),
                (Tags.WA_MaxWidth, 800),
                (Tags.WA_MaxHeight, 800),
                (Tags.WA_Flags, WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM | WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_ACTIVATE | WindowFlags.WFLG_SIZEBRIGHT),
                (Tags.WA_IDCMP, IDCMPFlags.GADGETUP),
                (Tags.WA_Title, "Window B"),
                (Tags.WA_Gadgets, new[] { gadProp1, gadProp2, gadStr1 }),
                (Tags.WA_Screen, this));
        }

        private void OpenWinA()
        {
            List<Gadget> glist = new List<Gadget>();
            //var btnImage = Intuition.NewObject(Intuition.FRAMEICLASS,
            //    (Tags.IA_Width, 64),
            //    (Tags.IA_Height, 24),
            //    (Tags.IA_EdgesOnly, false),
            //    (Tags.IA_FrameType, FrameType.Button)
            //    );

            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, 10),
                (Tags.GA_Top, 30),
                (Tags.GA_Width, 64),
                (Tags.GA_Height, 24),
                (Tags.GA_Text, "Exit"),
                (Tags.GA_ID, GADID_EXIT)
                );

            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, 10 + 74),
                (Tags.GA_Top, 30),
                (Tags.GA_Width, 64),
                (Tags.GA_Height, 24),
                (Tags.GA_Text, "Req..."),
                (Tags.GA_ID, GADID_REQ)
                );

            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, 10 + 74 * 2),
                (Tags.GA_Top, 30),
                (Tags.GA_Width, 64),
                (Tags.GA_Height, 24),
                (Tags.GA_Text, "Auto..."),
                (Tags.GA_ID, GADID_AUTO)
                );

            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, 10),
                (Tags.GA_Top, 60),
                (Tags.GA_Width, 64),
                (Tags.GA_Height, 24),
                (Tags.GA_Text, "Demo..."),
                (Tags.GA_ID, GADID_DEMO)
                );

            Intuition.NewObject(Intuition.FRBUTTONCLASS,
                (Tags.GA_List, glist),
                (Tags.GA_Left, 10 + 74),
                (Tags.GA_Top, 60),
                (Tags.GA_Width, 64),
                (Tags.GA_Height, 24),
                (Tags.GA_Text, "BOOPSI..."),
                (Tags.GA_ID, GADID_BOOPSI)
                );

            win1 = Intuition.OpenWindowTags(null,
                (Tags.WA_Left, 20),
                (Tags.WA_Top, 80),
                (Tags.WA_Width, 240),
                (Tags.WA_Height, 120),
                (Tags.WA_MinWidth, 240),
                (Tags.WA_MinHeight, 120),
                (Tags.WA_MaxWidth, 800),
                (Tags.WA_MaxHeight, 800),
                (Tags.WA_Flags, WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM | WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_ACTIVATE | WindowFlags.WFLG_SIZEBBOTTOM),
                (Tags.WA_IDCMP, IDCMPFlags.GADGETUP),
                (Tags.WA_Title, "Window A"),
                (Tags.WA_Gadgets, glist),
                (Tags.WA_Screen, this));

        }

        private void OpenGTDemo()
        {
            int gadid = 200;
            if (gtWin == null)
            {
                List<Gadget> glist = new List<Gadget>();
                VisualInfo vi = GadTools.GetVisualInfo(this);
                gtExit = GadTools.CreateGadget(GadKind.Button, glist, new NewGadget()
                {
                    LeftEdge = 20,
                    TopEdge = 40,
                    Width = 64,
                    Height = 24,
                    GadgetId = gadid++,
                    GadgetText = "Exit",
                    VisualInfo = vi
                },
                (Tags.GA_Disabled, true)
                );
                gtDisable = GadTools.CreateGadget(GadKind.Checkbox, glist, new NewGadget()
                {
                    LeftEdge = 20 + 64 + 10,
                    TopEdge = 42,
                    Width = 20,
                    Height = 20,
                    GadgetId = gadid++,
                    GadgetText = "Disable",
                    VisualInfo = vi
                },
                (Tags.GTCB_Checked, true)
                );
                gtCycle = GadTools.CreateGadget(GadKind.Cycle, glist, new NewGadget()
                {
                    LeftEdge = 20,
                    TopEdge = 42 * 2,
                    Width = 100,
                    Height = 24,
                    GadgetId = gadid++,
                    Flags = NewGadgetFlags.PlaceTextAbove,
                    GadgetText = "Action",
                    VisualInfo = vi
                },
                (Tags.GTCY_Labels, new object[] { "Continue", "Repeat", "Abort" }),
                (Tags.GTCY_Active, 0)
                );
                gtText = GadTools.CreateGadget(GadKind.Text, glist, new NewGadget()
                {
                    LeftEdge = 20,
                    TopEdge = 42 * 3,
                    Width = 200,
                    Height = 24,
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.GTTX_Text, "Information: Hello"),
                (Tags.GTTX_Border, true)
                );
                gtNumber = GadTools.CreateGadget(GadKind.Number, glist, new NewGadget()
                {
                    LeftEdge = 20,
                    TopEdge = 42 * 4,
                    Width = 200,
                    Height = 24,
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.GTNM_Number, 102345),
                (Tags.GTNM_Border, true)
                );
                gtMx = GadTools.CreateGadget(GadKind.Mx, glist, new NewGadget()
                {
                    LeftEdge = 300,
                    TopEdge = 42,
                    Width = 100,
                    Height = 24,
                    Flags = NewGadgetFlags.PlaceTextLeft,
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.GTMX_Labels, new object[] { "Picture", "Pattern", "Border" }),
                (Tags.GTMX_Active, 1),
                (Tags.GTMX_Spacing, 2)
                );
                gtStr = GadTools.CreateGadget(GadKind.String, glist, new NewGadget()
                {
                    LeftEdge = 300,
                    TopEdge = 42 * 3,
                    Width = 100,
                    Height = 22,
                    GadgetId = gadid++,
                    GadgetText = "Name:",
                    VisualInfo = vi
                },
                (Tags.GTST_String, "Jebeli")
                );
                gtStr2 = GadTools.CreateGadget(GadKind.String, glist, new NewGadget()
                {
                    LeftEdge = 300,
                    TopEdge = 42 * 4,
                    Width = 100,
                    Height = 22,
                    GadgetId = gadid++,
                    GadgetText = "Occupation:",
                    VisualInfo = vi
                },
                (Tags.GTST_String, "None")
                );
                gtInt = GadTools.CreateGadget(GadKind.Integer, glist, new NewGadget()
                {
                    LeftEdge = 300,
                    TopEdge = 42 * 5,
                    Width = 100,
                    Height = 22,
                    GadgetId = gadid++,
                    GadgetText = "Age:",
                    VisualInfo = vi
                },
                (Tags.GTIN_Number, 45)
                );
                gtScr = GadTools.CreateGadget(GadKind.Scroller, glist, new NewGadget()
                {
                    LeftEdge = -15,
                    TopEdge = 22,
                    Width = 14,
                    Height = -(42),
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.PGA_Freedom, PropFlags.FREEVERT),
                (Tags.GTSC_Total, 1000),
                (Tags.GTSC_Visible, 122),
                (Tags.GTSC_Arrows, 20),
                (Tags.GA_RightBorder, true)
                );

                gtScr2 = GadTools.CreateGadget(GadKind.Scroller, glist, new NewGadget()
                {
                    LeftEdge = 4,
                    TopEdge = -15,
                    Width = -(24),
                    Height = 14,
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.PGA_Freedom, PropFlags.FREEHORIZ),
                (Tags.GTSC_Total, 100),
                (Tags.GTSC_Visible, 20),
                (Tags.GTSC_Arrows, 20),
                (Tags.GA_BottomBorder, true)
                );

                gtSli = GadTools.CreateGadget(GadKind.Slider, glist, new NewGadget()
                {
                    LeftEdge = 40,
                    TopEdge = 300 - 40,
                    Width = 500 - 80,
                    Height = 20,
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.PGA_Freedom, PropFlags.FREEHORIZ),
                (Tags.GTSL_Min, 1),
                (Tags.GTSL_Max, 16),
                (Tags.GTSL_Level, 3),
                (Tags.GTSL_LevelFormat, "Level {0}"),
                (Tags.GTSL_LevelPlace, NewGadgetFlags.PlaceTextAbove)
                );

                List<object> list = new List<object>();
                foreach (var gad in glist)
                {
                    list.Add(gad);
                }
                var showSelected = GadTools.CreateGadget(GadKind.String, glist, new NewGadget()
                {
                    LeftEdge = 100,
                    TopEdge = 400,
                    Width = 500 - 80 - 60,
                    Height = 20,
                    GadgetText = "Selected:",
                    GadgetId = gadid++,
                    VisualInfo = vi
                });
                gtList = GadTools.CreateGadget(GadKind.ListView, glist, new NewGadget()
                {
                    LeftEdge = 40,
                    TopEdge = 300,
                    Width = 500 - 80,
                    Height = 100,
                    GadgetId = gadid++,
                    VisualInfo = vi
                },
                (Tags.GTLV_Labels, list),
                (Tags.GTLV_ShowSelected, showSelected)
                );


                gtWin = Intuition.OpenWindowTags(null,
                    (Tags.WA_Left, 200),
                    (Tags.WA_Top, 100),
                    (Tags.WA_Width, 500),
                    (Tags.WA_Height, 500),
                    (Tags.WA_MinWidth, 180),
                    (Tags.WA_MinHeight, 200),
                    (Tags.WA_MaxWidth, 800),
                    (Tags.WA_MaxHeight, 800),
                    (Tags.WA_Flags, WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM | WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_ACTIVATE | WindowFlags.WFLG_SIZEBBOTTOM | WindowFlags.WFLG_SIZEBRIGHT),
                    (Tags.WA_IDCMP, IDCMPFlags.GADGETUP | IDCMPFlags.GADGETDOWN | IDCMPFlags.CLOSEWINDOW | IDCMPFlags.MOUSEBUTTONS | IDCMPFlags.INTUITICKS),
                    (Tags.WA_Title, "GTDemo"),
                    (Tags.WA_Gadgets, glist),
                    (Tags.WA_Screen, this),
                    (Tags.WA_BackgroundColor, Color.Gray));
            }
            else
            {
                Intuition.WindowToFront(gtWin);
                Intuition.ActivateWindow(gtWin);
            }
        }

        private void OpenBOOPSIDemo()
        {
            if (bWin == null)
            {
                var prop2int = new(Tags, Tags)[] { (Tags.PGA_Top, Tags.STRINGA_LongVal) };
                var int2prop = new(Tags, Tags)[] { (Tags.STRINGA_LongVal, Tags.PGA_Top) };

                List<Gadget> glist = new List<Gadget>();
                var btnImage = Intuition.NewObject(Intuition.FRAMEICLASS,
                    (Tags.IA_Width, 64),
                    (Tags.IA_Height, 24),
                    (Tags.IA_EdgesOnly, false),
                    (Tags.IA_FrameType, FrameType.Button)
                    );

                var group = (GroupGadget)Intuition.NewObject(Intuition.GROUPGCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Left, 20),
                    (Tags.GA_Top, 40)
                    );


                var exitButton = Intuition.NewObject(Intuition.FRBUTTONCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Group, group),
                    (Tags.GA_Left, 0),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 64),
                    (Tags.GA_Height, 24),
                    (Tags.GA_Text, "Exit"),
                    (Tags.GA_Image, btnImage)
                    );

                var loadButton = Intuition.NewObject(Intuition.FRBUTTONCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Group, group),
                    (Tags.GA_Left, 64 + 10),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 64),
                    (Tags.GA_Height, 24),
                    (Tags.GA_Text, "Load"),
                    (Tags.GA_Image, btnImage)
                    );

                var saveButton = Intuition.NewObject(Intuition.FRBUTTONCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Group, group),
                    (Tags.GA_Left, 64 + 10 + 64 + 10),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 64),
                    (Tags.GA_Height, 24),
                    (Tags.GA_Text, "Save"),
                    (Tags.GA_Image, btnImage)
                    );

                var propGad = Intuition.NewObject(Intuition.PROPGCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Left, 20),
                    (Tags.GA_Top, 80),
                    (Tags.GA_Width, 200),
                    (Tags.GA_Height, 20),
                    (Tags.PGA_Top, 0),
                    (Tags.PGA_Total, 100),
                    (Tags.PGA_Visible, 20),
                    (Tags.PGA_Freedom, PropFlags.FREEHORIZ),
                    (Tags.ICA_MAP, prop2int)
                    );

                var intGad = Intuition.NewObject(Intuition.STRGCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Left, 20),
                    (Tags.GA_Top, 120),
                    (Tags.GA_Width, 200),
                    (Tags.GA_Height, 20),
                    (Tags.STRINGA_LongVal, 0),
                    (Tags.ICA_MAP, int2prop),
                    (Tags.ICA_TARGET, propGad)
                    );

                propGad.Set((Tags.ICA_TARGET, intGad));

                Intuition.NewObject(Intuition.STRGCLASS,
                    (Tags.GA_List, glist),
                    (Tags.GA_Left, 20),
                    (Tags.GA_Top, 160),
                    (Tags.GA_Width, 200),
                    (Tags.GA_Height, 20),
                    (Tags.STRINGA_TextVal, "Hello BOOPSI Gadget")
                );

                var groupImage = Intuition.NewObject(Intuition.FRAMEICLASS,
                    (Tags.IA_Width, group.Width + 8),
                    (Tags.IA_Height, group.Height + 8),
                    (Tags.IA_EdgesOnly, true),
                    (Tags.IA_FrameType, FrameType.Ridge),
                    (Tags.IA_Recessed, true),
                    (Tags.IA_ReadOnly, true)
                );
                group.Set((Tags.GA_Image, groupImage));

                bWin = Intuition.OpenWindowTags(null,
                    (Tags.WA_Left, 200),
                    (Tags.WA_Top, 200),
                    (Tags.WA_Width, 500),
                    (Tags.WA_Height, 500),
                    (Tags.WA_MinWidth, 180),
                    (Tags.WA_MinHeight, 200),
                    (Tags.WA_MaxWidth, 800),
                    (Tags.WA_MaxHeight, 800),
                    (Tags.WA_Flags, WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM | WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_ACTIVATE | WindowFlags.WFLG_SIZEBBOTTOM),
                    (Tags.WA_IDCMP, IDCMPFlags.GADGETUP | IDCMPFlags.GADGETDOWN | IDCMPFlags.CLOSEWINDOW | IDCMPFlags.MOUSEBUTTONS | IDCMPFlags.INTUITICKS),
                    (Tags.WA_Title, "BOOPSI Demo"),
                    (Tags.WA_Gadgets, glist),
                    (Tags.WA_Screen, this),
                    (Tags.WA_BackgroundColor, Color.Gray)
                    );
            }
            else
            {
                Intuition.WindowToFront(bWin);
                Intuition.ActivateWindow(bWin);
            }
        }

        private void CloseGTDemo()
        {
            Intuition.CloseWindow(gtWin);
            gtWin = null;
        }
        private void CloseBOOPSIDemo()
        {
            Intuition.CloseWindow(bWin);
            bWin = null;
        }

        private void CloseWindows()
        {
            Intuition.CloseWindow(win1);
            Intuition.CloseWindow(win2);
            Intuition.CloseWindow(gtWin);
        }

        protected override void OnGadgetClick(Gadget gadget)
        {
            base.OnGadgetClick(gadget);
            switch (gadget.GadgetId)
            {
                case GADID_EXIT:
                    engine.SwitchToTitleScreen();
                    break;
                case GADID_REQ:
                    Gadget gad1 = Gadget.MakeBoolGadget("Ok", 64, 24);
                    gad1.Activation |= GadgetActivation.ENDGADGET;
                    gad1.SetPosition(4 + 5, 55);
                    Gadget gad2 = Gadget.MakeBoolGadget("Cancel", 64, 24, GadgetFlags.GRELRIGHT);
                    gad2.Activation |= GadgetActivation.ENDGADGET;
                    gad2.SetPosition(-(4 + 5 + 64), 55);
                    req1 = new Requester();
                    req1.LeftEdge = 20;
                    req1.TopEdge = 20;
                    req1.Width = 200;
                    req1.Height = 100;
                    req1.ReqText = new IntuiText("Hello Requester!");
                    req1.ReqText.LeftEdge = 100;
                    req1.ReqText.TopEdge = 40;
                    req1.ReqBorder = new Border(200, 100);
                    req1.BackFill = Color.Gray;
                    req1.ReqGadgets = new[] { gad1, gad2 };
                    Intuition.Request(req1, win1);
                    break;
                case GADID_AUTO:
                    Intuition.AutoRequestPositionMode = AutoRequestPositionMode.CenterWindow;
                    EasyStruct easy = new EasyStruct()
                    {
                        Title = "Question",
                        TextFormat = "Are you sure?\nAre you really,\nreally sure?",
                        GadgetFormat = "Yes|Maybe|No"
                    };
                    Intuition.EasyRequest(win1, easy);
                    break;
                case GADID_DEMO:
                    OpenGTDemo();
                    break;
                case GADID_BOOPSI:
                    OpenBOOPSIDemo();
                    break;

                default:
                    if (gadget == gtDisable)
                    {
                        if (gadget.Checked)
                        {
                            Intuition.OffGadget(gtExit, gtWin);
                        }
                        else
                        {
                            Intuition.OnGadget(gtExit, gtWin);
                        }
                    }
                    else if (gadget == gtExit)
                    {
                        CloseGTDemo();
                    }
                    break;
            }
        }

        protected override void OnCloseWindow(Window window)
        {
            base.OnCloseWindow(window);
            if (window == gtWin)
            {
                CloseGTDemo();
            }
            else if (window == bWin)
            {
                CloseBOOPSIDemo();
            }
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
            Intuition.MoveActiveWindowToFront = true;
            backWin.SetWindowBox(0, 0, engine.Graphics.ViewWidth, engine.Graphics.ViewHeight);
        }
    }
}