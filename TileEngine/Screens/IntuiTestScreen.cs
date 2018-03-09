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
        private Window backWin;
        private Window win1;
        private Window win2;
        private Gadget gad1;
        private Gadget gad2;
        private Gadget gad3;
        private Gadget gadProp1;
        private Gadget gadProp2;
        private Gadget gadStr1;
        private Requester req1;

        public IntuiTestScreen(Engine engine)
            : base(engine, "IntuiTestScreen")
        {
            MakeWindows();
        }

        private void MakeWindows()
        {
            backWin = Intuition.OpenWindowTags(null,
                Tag(WATags.WA_Width, 800),
                Tag(WATags.WA_Height, 800),
                Tag(WATags.WA_Flags, WindowFlags.WFLG_BACKDROP | WindowFlags.WFLG_BORDERLESS),
                Tag(WATags.WA_Screen, this));

            gad1 = Gadget.MakeBoolGadget("Exit", 64, 24);
            gad1.SetPosition(4 + 5, 5 + 24);

            gad2 = Gadget.MakeBoolGadget("Req", 64, 24);
            gad2.SetPosition(4 + 5 + 64 + 4, 5 + 24);

            gad3 = Gadget.MakeBoolGadget("Auto", 64, 24);
            gad3.SetPosition(4 + 5 + 64 * 2 + 4, 5 + 24);

            win1 = Intuition.OpenWindowTags(null,
                Tag(WATags.WA_Left, 20),
                Tag(WATags.WA_Top, 80),
                Tag(WATags.WA_Width, 220),
                Tag(WATags.WA_Height, 120),
                Tag(WATags.WA_MinWidth, 180),
                Tag(WATags.WA_MinHeight, 120),
                Tag(WATags.WA_MaxWidth, 800),
                Tag(WATags.WA_MaxHeight, 800),
                Tag(WATags.WA_Flags, WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM | WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_ACTIVATE | WindowFlags.WFLG_SIZEBBOTTOM),
                Tag(WATags.WA_IDCMP, IDCMPFlags.GADGETUP),
                Tag(WATags.WA_Title, "Window A"),
                Tag(WATags.WA_Gadgets, new[] { gad1, gad2, gad3 }),
                Tag(WATags.WA_Screen, this));

            gadProp1 = Gadget.MakePropGadget(0, 0x1000, -20, 20);
            gadProp1.SetPosition(4 + 5, 5 + 24);

            gadProp2 = Gadget.MakePropGadget(0x2222, 0x2222, 50, 50);
            gadProp2.SetPosition(4 + 5, 5 + 24 + 25);

            gadStr1 = Gadget.MakeStringGadget("Hello", 100, 20);
            gadStr1.SetPosition(4 + 5, 5 + 24 + 25 + 50 + 5);

            win2 = Intuition.OpenWindowTags(null,
                Tag(WATags.WA_Left, 120),
                Tag(WATags.WA_Top, 120),
                Tag(WATags.WA_Width, 180),
                Tag(WATags.WA_Height, 200),
                Tag(WATags.WA_MinWidth, 180),
                Tag(WATags.WA_MinHeight, 200),
                Tag(WATags.WA_MaxWidth, 800),
                Tag(WATags.WA_MaxHeight, 800),
                Tag(WATags.WA_Flags, WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM | WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_ACTIVATE | WindowFlags.WFLG_SIZEBBOTTOM),
                Tag(WATags.WA_IDCMP, IDCMPFlags.GADGETUP),
                Tag(WATags.WA_Title, "Window B"),
                Tag(WATags.WA_Gadgets, new[] { gadProp1, gadProp2, gadStr1 }),
                Tag(WATags.WA_Screen, this));

        }

        private void CloseWindows()
        {
            Intuition.CloseWindow(win1);
            Intuition.CloseWindow(win2);
        }

        protected override void OnGadgetClick(Gadget gadget)
        {
            base.OnGadgetClick(gadget);
            if (gadget == gad1)
            {
                engine.SwitchToTitleScreen();
            }
            else if (gadget == gad2)
            {
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
            }
            else if (gadget == gad3)
            {
                Intuition.AutoRequestPositionMode = AutoRequestPositionMode.CenterWindow;
                EasyStruct easy = new EasyStruct()
                {
                    Title = "Question",
                    TextFormat = "Are you sure?\nAre you really,\nreally sure?",
                    GadgetFormat = "Yes|No|Maybe"
                };
                Intuition.EasyRequest(win1, easy);

                //IntuiText body = new IntuiText("Are you sure?");
                //Intuition.AutoRequest(win1, body, "Yes", "No", IDCMPFlags.GADGETUP, IDCMPFlags.GADGETUP, 200, 100);
            }
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
            backWin.SetWindowBox(0, 0, engine.Graphics.ViewWidth, engine.Graphics.ViewHeight);
        }
    }
}