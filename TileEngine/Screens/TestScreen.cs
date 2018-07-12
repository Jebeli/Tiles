namespace TileEngine.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Graphics;
    using YGUI;

    public class TestScreen : AbstractScreen
    {

        private const int GADID_EXIT = 200;
        private const int GADID_BUTTONS = 201;
        private const int GADID_PROPS = 202;
        private const int GADID_BASIC = 203;

        private Window buttonWindow;
        private Window basicWindow;
        private Window propWindow;
        private ProgressbarGadget progBar;
        private ProgressbarGadget progBar2;
        public TestScreen(Engine engine)
            : base(engine, "TestScreen")
        {
        }

        protected override void InitGUI(Screen screen)
        {
            var window = new Window(screen, "Com Win", Orientation.Horizontal)
            {
                LeftEdge = 10,
                TopEdge = 10,
                CloseGadget = true,
                DepthGadget = true,
                WindowCloseEvent = (o, i) => { engine.SwitchToTitleScreen(); }
            };
            new ButtonGadget(window, "Exit")
            {
                GadgetUpEvent = (o, i) => { engine.SwitchToTitleScreen(); }
            };
            new ButtonGadget(window, "Buttons")
            {
                GadgetUpEvent = (o, i) => { ButtonDemo(); }
            };
            new ButtonGadget(window, "Props")
            {
                GadgetUpEvent = (o, i) => { PropDemo(); }
            };
            new ButtonGadget(window, "Basic")
            {
                GadgetUpEvent = (o, i) => { BasicDemo(); }
            };
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
            if (propWindow != null && progBar != null)
            {
                int t = (int)time.TotalGameTime.TotalMilliseconds % 5000;
                progBar.Value = t / 50;
                int t2 = (int)time.TotalGameTime.TotalMilliseconds % 50000;
                progBar2.Value = t2 / 50;
            }
        }

        private void ButtonDemo()
        {
            if (buttonWindow == null)
            {
                buttonWindow = new Window(screen, "Button Demo")
                {
                    LeftEdge = 30,
                    TopEdge = 30,
                    CloseGadget = true,
                    DepthGadget = true,
                    SizeGadget = true,
                    SizeBRight = true,
                    WindowCloseEvent = (o, i) => { screen.CloseWindow(buttonWindow); buttonWindow = null; }                    
                };
                new LabelGadget(buttonWindow, "Button Gadgets")
                {
                    Frame = true
                };
                new ButtonGadget(buttonWindow, "Normal")
                {
                    TooltipText = "Tooltip",
                    TooltipPos = new Point(2, 2),
                    ShowTooltip = true
                };
                new ButtonGadget(buttonWindow, "Toggle")
                {
                    Sticky = true
                };
                var box = new BoxGadget(buttonWindow, Orientation.Horizontal)
                {

                };
                new ButtonGadget(box, "A");
                new ButtonGadget(box, "B");
                new ButtonGadget(box, "C");
                new ButtonGadget(box, "D");
                var box2 = new BoxGadget(buttonWindow, Orientation.Horizontal)
                {

                };
                new ButtonGadget(box2, Icons.ENTYPO_ICON_500PX);
                new ButtonGadget(box2, Icons.ENTYPO_ICON_AIR);
                new ButtonGadget(box2, Icons.ENTYPO_ICON_BOOK);
                new ButtonGadget(box2, Icons.ENTYPO_ICON_CLAPPERBOARD);
                new SeparatorGadget(box2);
                new ChooserGadget(box2)
                {
                    Items = new string[] { "Item1", "Item2", "Item3", "Item4" }
                };

                var box3 = new BoxGadget(buttonWindow, Orientation.Horizontal, Alignment.Fill)
                {

                };
                new CheckBoxGadget(box3, "Checkbox 1");
                new CheckBoxGadget(box3, "Checkbox 2");
                new StrGadget(buttonWindow, "")
                {
                    Buffer = "Hello Buffer"
                };
                new NumericalGadget(buttonWindow)
                {

                };
            }
            screen.ShowWindow(buttonWindow);
            screen.WindowToFront(buttonWindow);
            screen.ActivateWindow(buttonWindow);
        }

        private void PropDemo()
        {
            if (propWindow == null)
            {
                propWindow = new Window(screen, "Prop Demo")
                {
                    LeftEdge = 30,
                    TopEdge = 30,
                    CloseGadget = true,
                    DepthGadget = true,
                    SizeGadget = true,
                    WindowCloseEvent = (o, i) => { screen.CloseWindow(propWindow); propWindow = null; }
                };
                new LabelGadget(propWindow, "Proportional Gadgets")
                {
                    Frame = true
                };
                new PropGadget(propWindow)
                {
                    HorizBody = 0x1111
                };
                new PropGadget(propWindow)
                {
                    HorizBody = 0x5555
                };
                new PropGadget(propWindow)
                {
                    FreeHoriz = true,
                    FreeVert = true,
                    HorizBody = 0x1111,
                    VertBody = 0x1111
                };
                new ScrollbarGadget(propWindow)
                {
                    Max = 100,
                    VisibleAmount = 10,
                    Increment = 10
                };
                new ScrollbarGadget(propWindow)
                {
                    Max = 100,
                    VisibleAmount = 10,
                    Increment = 10,
                    ArrowPlace = ArrowPlace.LeftTop
                };
                new ScrollbarGadget(propWindow)
                {
                    Max = 100,
                    VisibleAmount = 10,
                    Increment = 10,
                    ArrowPlace = ArrowPlace.Split
                };
                new ScrollbarGadget(propWindow)
                {
                    Max = 100,
                    VisibleAmount = 10,
                    Increment = 10,
                    ArrowPlace = ArrowPlace.None
                };
                progBar = new ProgressbarGadget(propWindow)
                {
                    Max = 100,
                    Value = 50,
                    ShowPercent = true
                };
                progBar2 = new ProgressbarGadget(propWindow)
                {
                    Max = 1000,
                    Value = 50,
                    ShowPart = true
                };
                var sl = new LabelGadget(propWindow, "Level");
                new SliderGadget(propWindow, Orientation.Horizontal)
                {
                    Min = 1,
                    Max = 16,
                    Value = 3,
                    LabelFormat = "Level {0}",
                    LabelGadget = sl
                };

            }
            screen.ShowWindow(propWindow);
            screen.WindowToFront(propWindow);
            screen.ActivateWindow(propWindow);
        }

        private void BasicDemo()
        {
            if (basicWindow == null)
            {
                basicWindow = new Window(screen, "Baisc Demo")
                {
                    LeftEdge = 30,
                    TopEdge = 30,
                    CloseGadget = true,
                    DepthGadget = true,
                    SizeGadget = true,
                    WindowCloseEvent = (o, i) => { screen.CloseWindow(basicWindow); basicWindow = null; }
                };
                var tabs = new TabPanelGadget(basicWindow);
                var tab1 = tabs.AddTab("Tab 1", Orientation.Vertical);
                new LabelGadget(tab1, "First Tab");
                var boxL = new BoxGadget(tab1, Orientation.Horizontal);
                new LabelGadget(boxL, "Selection:");
                var cg = new ChooserGadget(boxL)
                {
                    Items = new string[] { "Rows", "Cells", "Cols" }
                };

                var table1 = new TableGadget(tab1);

                cg.SelectedIndexChangedEvent = (o, i) =>
                {
                    table1.SelectMode = (TableSelectMode)cg.SelectedIndex;
                };

                var col1 = table1.AddColumn("Name", 100);
                var col2 = table1.AddColumn("Date", 100);
                var col3 = table1.AddColumn("Col 3", 100);
                for (int i = 0; i < 700; i++)
                {
                    var row = table1.AddRow("" + i, "" + (i * i), "" + (i + i));
                }
                table1.AddRow("Last", "The very last row!", "Ultimo");
                var tab2 = tabs.AddTab("Tab 2", Orientation.Vertical);
                new LabelGadget(tab2, "Second Tab");
                var hbox = new BoxGadget(tab2, Orientation.Horizontal);
                new LabelGadget(hbox, "Lab");
                var table2 = new TableGadget(hbox)
                {
                    ShowHeader = false
                };
                Icons icon = Icons.ENTYPO_ICON_500PX;
                for (int i = 0; i < 700; i++)
                {
                    string lab = icon.ToString();
                    var row = table2.AddRow(lab.Replace("ENTYPO_ICON_", ""));
                    row.Cells[0].Icon = icon;
                    icon++;
                    if (icon > Icons.ENTYPO_ICON_YOUTUBE_WITH_CIRCLE)
                    {
                        break;
                    }
                }
                var tab3 = tabs.AddTab("Tab 3", Orientation.Vertical);
                new LabelGadget(tab3, "Third Tab");
                tabs.SelectedIndex = 2;
            }
            screen.ShowWindow(basicWindow);
            screen.WindowToFront(basicWindow);
            screen.ActivateWindow(basicWindow);
        }

        //private void ButtonDemo()
        //{
        //    if (buttonWindow == null)
        //    {
        //        List<Gadget> glist = new List<Gadget>();
        //        glist.Add(new ButtonGadget(
        //            (Tags.GA_Text, "Normal"),
        //            (Tags.GA_HeightWeight, 0)
        //        ));
        //        glist.Add(new ButtonGadget(
        //            (Tags.GA_Text, "Toggle"),
        //            (Tags.GA_ToggleSelect, true),
        //            (Tags.GA_HeightWeight, 0)
        //        ));
        //        List<Gadget> sublist = new List<Gadget>();
        //        sublist.Add(new ButtonGadget(
        //            (Tags.GA_Text, "A")
        //            ));
        //        sublist.Add(new ButtonGadget(
        //            (Tags.GA_Text, "B")
        //            ));
        //        sublist.Add(new ButtonGadget(
        //            (Tags.GA_Text, "C")
        //            ));
        //        sublist.Add(new ButtonGadget(
        //            (Tags.GA_Text, "D")
        //            ));
        //        var subGl = new LayoutGadget(
        //            (Tags.LAYOUT_Orientation, Orientation.Horizontal),
        //            (Tags.GA_HeightWeight, 0),
        //            (Tags.LAYOUT_SpaceOuter, false),
        //            (Tags.LAYOUT_AddChildren, sublist)
        //            );

        //        glist.Add(subGl);

        //        var cbGl = new LayoutGadget(
        //            (Tags.LAYOUT_Orientation, Orientation.Horizontal),
        //            (Tags.GA_HeightWeight, 0),
        //            (Tags.LAYOUT_SpaceOuter, false),
        //            (Tags.LAYOUT_AddChild, new CheckBoxGadget(
        //                (Tags.GA_Text, "Check 1")
        //                )),
        //            (Tags.LAYOUT_AddChild, new CheckBoxGadget(
        //                (Tags.GA_Text, "Check 2")
        //                ))

        //            );
        //        glist.Add(cbGl);

        //        var tbGl = new LayoutGadget(
        //            (Tags.LAYOUT_Orientation, Orientation.Horizontal),
        //            (Tags.GA_HeightWeight, 0),
        //            (Tags.LAYOUT_SpaceOuter, false),
        //            (Tags.LAYOUT_AddChild, new ToolButton(
        //                (Tags.GA_Icon, Icons.ENTYPO_ICON_500PX),
        //                (Tags.GA_WidthWeight, 0)
        //                )),
        //            (Tags.LAYOUT_AddChild, new ToolButton(
        //                (Tags.GA_Icon, Icons.ENTYPO_ICON_ADDRESS),
        //                (Tags.GA_WidthWeight, 0)
        //                )),
        //            (Tags.LAYOUT_AddChild, new ToolButton(
        //                (Tags.GA_Icon, Icons.ENTYPO_ICON_AIR),
        //                (Tags.GA_WidthWeight, 0)
        //                )),
        //            (Tags.LAYOUT_AddChild, new ToolButton(
        //                (Tags.GA_Icon, Icons.ENTYPO_ICON_BOX),
        //                (Tags.GA_WidthWeight, 0)
        //                )),
        //            (Tags.LAYOUT_AddChild, new ChooserGadget(
        //                (Tags.CHOOSER_PopUp, true),
        //                (Tags.CHOOSER_Selected, 2),
        //                (Tags.CHOOSER_Labels, new[] { "Something", "Something else", "More", "Different" }),
        //                (Tags.CHOOSER_Width, 120)
        //                )),
        //            (Tags.LAYOUT_AddChild, new ChooserGadget(
        //                (Tags.CHOOSER_DropDown, true),
        //                (Tags.CHOOSER_Title, "Drink"),
        //                (Tags.CHOOSER_Labels, new[] { "Coffee", "Tea", "Beer", "Schnaps" })
        //                ))
        //            );
        //        glist.Add(tbGl);

        //        var gl = new LayoutGadget(
        //            (Tags.GA_Top, 10),
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 200),
        //            (Tags.LAYOUT_Orientation, Orientation.Vertical),
        //            (Tags.LAYOUT_Label, "Buttons"),
        //            (Tags.LAYOUT_LabelPlace, TextPlace.TopLeft),
        //            (Tags.LAYOUT_AddChildren, glist)
        //            ); ;

        //        List<Gadget> glist2 = new List<Gadget>();
        //        glist2.Add(new LabelGadget(
        //            (Tags.GA_Text, "String Gadgets")
        //            ));
        //        glist2.Add(new StrGadget(
        //            (Tags.GA_HeightWeight, 0),
        //            (Tags.GA_Text, "Name"),
        //            (Tags.STRINGA_TextVal, "Jebeli")
        //            ));
        //        glist2.Add(new StrGadget(
        //            (Tags.GA_HeightWeight, 0),
        //            (Tags.GA_Text, "Number"),
        //            (Tags.STRINGA_LongVal, 12345)
        //            ));
        //        glist2.Add(new StrGadget(
        //            (Tags.GA_HeightWeight, 0),
        //            (Tags.GA_Text, "Label"),
        //            (Tags.STRINGA_Placeholder, "Placeholder")
        //            ));
        //        var gl2 = new LayoutGadget(
        //            (Tags.GA_Top, 220),
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 160),
        //            (Tags.LAYOUT_Orientation, Orientation.Vertical),
        //            (Tags.LAYOUT_Label, "Strings"),
        //            (Tags.LAYOUT_LabelPlace, TextPlace.TopLeft),
        //            (Tags.LAYOUT_AddChildren, glist2)
        //            ); ;


        //        buttonWindow = screen.OpenWindow(
        //        (Tags.WA_Left, 30),
        //        (Tags.WA_Top, 30),
        //        (Tags.WA_Width, 400),
        //        (Tags.WA_Height, 400),
        //        (Tags.WA_Title, "Button Demo"),
        //        (Tags.WA_CloseGadget, true),
        //        (Tags.WA_DragBar, true),
        //        (Tags.WA_DepthGadget, true),
        //        (Tags.WA_SizeGadget, true),
        //        (Tags.WA_SizeBRight, true),
        //        (Tags.WA_Activate, true),
        //        (Tags.WA_Gadgets, new Gadget[] { gl, gl2 }));
        //    }
        //    else
        //    {
        //        screen.MoveWindowToFront(buttonWindow);
        //        screen.ActivateWindow(buttonWindow);
        //    }
        //}

        //private void PropDemo()
        //{
        //    if (propWindow == null)
        //    {
        //        List<Gadget> glist = new List<Gadget>();

        //        glist.Add(new PropGadget(
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_Top, 10),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 20),
        //            (Tags.PGA_Freedom, PropFlags.FreeHoriz),
        //            (Tags.PGA_Total, 100),
        //            (Tags.PGA_Visible, 25),
        //            (Tags.PGA_Top, 25)
        //            ));

        //        glist.Add(new PropGadget(
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_Top, 40),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 20),
        //            (Tags.PGA_Freedom, PropFlags.FreeHoriz),
        //            (Tags.PGA_Total, 1000),
        //            (Tags.PGA_Visible, 25)
        //            ));

        //        var lab = new LabelGadget(
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_Top, 70),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 20)
        //            );
        //        glist.Add(lab);

        //        glist.Add(new SliderGadget(
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_Top, 100),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 20),
        //            (Tags.SLIDER_Orientation, Orientation.Horizontal),
        //            (Tags.SLIDER_Min, 1),
        //            (Tags.SLIDER_Max, 15),
        //            (Tags.SLIDER_Level, 3),
        //            (Tags.SLIDER_LevelLabel, lab),
        //            (Tags.SLIDER_LevelFormat, "Level {0}")
        //            ));

        //        glist.Add(new ScrollerGadget(
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_Top, 130),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_Height, 20),
        //            (Tags.SCROLLER_Orientation, Orientation.Horizontal),
        //            (Tags.SCROLLER_Total, 100),
        //            (Tags.SCROLLER_Visible, 50)

        //            ));

        //        propWindow = screen.OpenWindow(
        //        (Tags.WA_Left, 30),
        //        (Tags.WA_Top, 30),
        //        (Tags.WA_Width, 300),
        //        (Tags.WA_Height, 200),
        //        (Tags.WA_Title, "Prop Demo"),
        //        (Tags.WA_CloseGadget, true),
        //        (Tags.WA_DragBar, true),
        //        (Tags.WA_DepthGadget, true),
        //        (Tags.WA_SizeGadget, true),
        //        (Tags.WA_SizeBRight, true),
        //        (Tags.WA_Activate, true),
        //        (Tags.WA_Gadgets, glist)
        //        );
        //    }
        //    else
        //    {
        //        screen.MoveWindowToFront(propWindow);
        //        screen.ActivateWindow(propWindow);
        //    }
        //}

        //private void BasicDemo()
        //{
        //    if (basicWindow == null)
        //    {
        //        List<string> list = new List<string>();
        //        list.Add("One");
        //        list.Add("Two");
        //        list.Add("Three");
        //        list.Add("Four");
        //        list.Add("Five");
        //        list.Add("Six");
        //        list.Add("Seven");
        //        list.Add("Eight");
        //        list.Add("Nine");
        //        list.Add("Ten");
        //        list.Add("Eleven");
        //        list.Add("Twelve");
        //        list.Add("Thirteen");
        //        list.Add("Fourteen");
        //        list.Add("Fifteen");
        //        list.Add("Sixteen");
        //        list.Add("Seventeen");
        //        list.Add("Eighteen");
        //        list.Add("Nineteen");
        //        list.Add("Twenty");
        //        var page = new PageGadget(
        //            (Tags.PAGE_Add, new LayoutGadget(
        //                (Tags.LAYOUT_Orientation, Orientation.Vertical),
        //                //(Tags.LAYOUT_Label, "Buttons"),
        //                //(Tags.LAYOUT_LabelPlace, TextPlace.TopLeft),
        //                (Tags.LAYOUT_AddChild, new ButtonGadget(
        //                    (Tags.GA_Text, "Normal"),
        //                    (Tags.GA_HeightWeight, 0)
        //                )),
        //                (Tags.LAYOUT_AddChild, new ButtonGadget(
        //                    (Tags.GA_Text, "Toggle"),
        //                    (Tags.GA_ToggleSelect, true),
        //                    (Tags.GA_HeightWeight, 0)
        //                ))
        //                )),
        //            (Tags.PAGE_Add, new LayoutGadget(
        //            (Tags.LAYOUT_Label, "Props"),
        //            (Tags.LAYOUT_LabelPlace, TextPlace.TopLeft)

        //                )),
        //            (Tags.PAGE_Add, new LayoutGadget(
        //            (Tags.LAYOUT_Label, "Strings"),
        //            (Tags.LAYOUT_LabelPlace, TextPlace.TopLeft)

        //                )),
        //            (Tags.PAGE_Add, new LayoutGadget(
        //            (Tags.LAYOUT_Orientation, Orientation.Vertical),
        //            //(Tags.LAYOUT_Label, "Others"),
        //            //(Tags.LAYOUT_LabelPlace, TextPlace.TopLeft),
        //            (Tags.LAYOUT_AddChild,new ListBrowserGadget(
        //                (Tags.LISTBROWSER_Labels,list)
        //                ))
        //                ))
        //            );
        //        var gl = new LayoutGadget(
        //            (Tags.GA_Top, 10),
        //            (Tags.GA_Left, 10),
        //            (Tags.GA_RelWidth, -20),
        //            (Tags.GA_RelHeight, -20),
        //            (Tags.LAYOUT_Orientation, Orientation.Vertical),
        //            (Tags.LAYOUT_Frame, false),
        //            (Tags.LAYOUT_SpaceOuter, false),
        //            (Tags.LAYOUT_SpaceInner, false),
        //            //(Tags.LAYOUT_Label, "Basic"),
        //            //(Tags.LAYOUT_LabelPlace, TextPlace.TopLeft),
        //            (Tags.LAYOUT_AddChild, new ClickTabGadget(
        //            (Tags.CLICKTAB_Labels, new string[] { "Buttons", "Props", "Strings", "Others" }),
        //            (Tags.CLICKTAB_PageGroup, page)
        //            ))
        //            );

        //        basicWindow = screen.OpenWindow(
        //        (Tags.WA_Left, 30),
        //        (Tags.WA_Top, 30),
        //        (Tags.WA_Width, 400),
        //        (Tags.WA_Height, 400),
        //        (Tags.WA_Title, "Basic Demo"),
        //        (Tags.WA_CloseGadget, true),
        //        (Tags.WA_DragBar, true),
        //        (Tags.WA_DepthGadget, true),
        //        (Tags.WA_SizeGadget, true),
        //        (Tags.WA_SizeBRight, true),
        //        (Tags.WA_Activate, true),
        //        (Tags.WA_Gadgets, new Gadget[] { gl }));

        //    }
        //    else
        //    {
        //        screen.MoveWindowToFront(basicWindow);
        //        screen.ActivateWindow(basicWindow);
        //    }
        //}

        //private void ButtonDemo()
        //{
        //    if (buttonWindow == null)
        //    {
        //        buttonWindow = new Window(screen)
        //        {
        //            Title = "Button Demo",
        //            Position = new Vector2(100, 100),
        //            Layout = new GroupLayout(),
        //            CloseGadget = true,
        //            CloseEvent = (o, i) => { buttonWindow.CloseWindow(); buttonWindow = null; }
        //        };
        //        new Label(buttonWindow, "Push buttons");
        //        new Button(buttonWindow, "Normal");
        //        var iconButton = new Button(buttonWindow, "Icon", Icons.ENTYPO_ICON_500PX);
        //        new Label(buttonWindow, "Toggle buttons");
        //        new Button(buttonWindow, "Toggle me")
        //        {
        //            Flags = ButtonFlags.ToggleButton
        //        };
        //        new Label(buttonWindow, "Radio buttons");
        //        new Button(buttonWindow, "Left")
        //        {
        //            Flags = ButtonFlags.RadioButton,
        //            Checked = true,
        //            ClickEvent = (o, i) => { iconButton.IconPlacement = IconPlacement.Left; PerformLayout(); }
        //        };
        //        new Button(buttonWindow, "Top")
        //        {
        //            Flags = ButtonFlags.RadioButton,
        //            ClickEvent = (o, i) => { iconButton.IconPlacement = IconPlacement.Top; PerformLayout(); }
        //        };
        //        new Button(buttonWindow, "Right")
        //        {
        //            Flags = ButtonFlags.RadioButton,
        //            ClickEvent = (o, i) => { iconButton.IconPlacement = IconPlacement.Right; PerformLayout(); }
        //        };
        //        new Button(buttonWindow, "Bottom")
        //        {
        //            Flags = ButtonFlags.RadioButton,
        //            ClickEvent = (o, i) => { iconButton.IconPlacement = IconPlacement.Bottom; PerformLayout(); }
        //        };
        //        new Label(buttonWindow, "A tool palette");
        //        var box = new Widget(buttonWindow)
        //        {
        //            Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 6)
        //        };
        //        new ToolButton(box, Icons.ENTYPO_ICON_CLOUD);
        //        new ToolButton(box, Icons.ENTYPO_ICON_CONTROLLER_FAST_FORWARD);
        //        new ToolButton(box, Icons.ENTYPO_ICON_COMPASS);
        //        new ToolButton(box, Icons.ENTYPO_ICON_INSTALL);
        //        new Label(buttonWindow, "Popup buttons");
        //        var but6 = new PopupButton(buttonWindow, "Popup", Icons.ENTYPO_ICON_FLASH)
        //        {

        //        };
        //        Popup popup = but6.Popup;
        //        popup.Layout = new GroupLayout();
        //        new Label(popup, "Arbitrary widgets can be placed here");
        //        new CheckBox(popup, "A check box");
        //        var popupBtn = new PopupButton(popup, "Recursive popup", Icons.ENTYPO_ICON_FLASH);
        //        Popup popupRight = popupBtn.Popup;
        //        popupRight.Layout = new GroupLayout();
        //        new CheckBox(popupRight, "Another check box");
        //        popupBtn = new PopupButton(popup, "Recursive popup", Icons.ENTYPO_ICON_FLASH);
        //        popupBtn.Side = PopupSide.Left;
        //        Popup popupLeft = popupBtn.Popup;
        //        popupLeft.Layout = new GroupLayout();
        //        new CheckBox(popupLeft, "Another check box");
        //    }
        //    buttonWindow.RequestFocus();
        //}

        //private void BasicDemo()
        //{
        //    if (basicWindow == null)
        //    {
        //        basicWindow = new Window(screen)
        //        {
        //            Title = "Basic Widgets",
        //            Position = new Vector2(300, 50),
        //            Layout = new GroupLayout(),
        //            CloseGadget = true,
        //            CloseEvent = (o, i) => { basicWindow.CloseWindow(); basicWindow = null; }
        //        };
        //        new Label(basicWindow, "Message dialog");
        //        var tools = new Widget(basicWindow);
        //        tools.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 6);
        //        var b = new Button(tools, "Info");
        //        b.Click += (o, i) =>
        //        {
        //            var dlg = new MessageBox(screen, MessageType.Information, "Information", "This is an information message");
        //        };
        //        b = new Button(tools, "Warn");
        //        b.Click += (o, i) =>
        //        {
        //            var dlg = new MessageBox(screen, MessageType.Warning, "Warning", "This is a warning message");
        //        };
        //        b = new Button(tools, "Ask");
        //        b.Click += (o, i) =>
        //        {
        //            var dlg = new MessageBox(screen, MessageType.Question, "Question", "This is a  question message", "Yes", "No", true);
        //        };

        //        new Label(basicWindow, "Image panel & scroll panel");
        //        PopupButton imagePanelBtn = new PopupButton(basicWindow, "Image Panel");
        //        imagePanelBtn.Icon = Icons.ENTYPO_ICON_FOLDER;
        //        var popup = imagePanelBtn.Popup;
        //        VScrollPanel vscroll = new VScrollPanel(popup);
        //        ImagePanel imagePanel = new ImagePanel(vscroll);

        //        var ts = engine.GetTileSet("tilesetdefs/tileset_grassland.xml");
        //        List<TextureRegion> texs = new List<TextureRegion>();
        //        foreach (var i in ts.Tiles)
        //        {
        //            texs.Add(ts.GetTile(i));
        //        }
        //        imagePanel.Images = texs;
        //        popup.FixedSize = new Vector2(245 + 4 + 11, 150 + 64 + 4);

        //        new Label(basicWindow, "Check box");
        //        new CheckBox(basicWindow, "Flag 1")
        //        {
        //            Checked = true
        //        };
        //        new CheckBox(basicWindow, "Flag 2");

        //        new Label(basicWindow, "Slider and text box");

        //        var panel = new Widget(basicWindow);
        //        panel.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 20);
        //        Slider slider = new Slider(panel);
        //        slider.Max = 100;
        //        slider.Min = 1;
        //        slider.Level = 50;
        //        slider.FixedWidth = 80;

        //        StrGadget str = new StrGadget(panel, "50");
        //        str.Click += (o, i) =>
        //        {
        //            slider.Level = str.LongInt;
        //        };

        //        slider.PropChanged += (o, i) =>
        //        {
        //            str.Buffer = "" + slider.Level;
        //        };

        //    }
        //    basicWindow.RequestFocus();
        //}

        //private void PropDemo()
        //{
        //    if (propWindow == null)
        //    {
        //        propWindow = new Window(screen)
        //        {
        //            Title = "Prop Gadgets",
        //            Position = new Vector2(300, 100),
        //            Layout = new BoxLayout(Orientation.Horizontal, Alignment.Minimum),
        //            CloseGadget = true,
        //            CloseEvent = (o, i) => { propWindow.CloseWindow(); propWindow = null; }
        //        };
        //        var leftP = new Widget(propWindow)
        //        {
        //            Layout = new GroupLayout()
        //        };
        //        var rightP = new Widget(propWindow)
        //        {
        //            Layout = new GroupLayout()
        //        };

        //        new Label(leftP, "Props");
        //        new PropGadget(leftP)
        //        {
        //            HorizBody = 0x2222,
        //            HorizPot = 0x1111
        //        };
        //        new PropGadget(leftP)
        //        {
        //            Flags = PropFlags.FreeVert | PropFlags.AutoKnob,
        //            VertBody = 0x2222,
        //            VertPot = 0x1111,
        //            FixedWidth = 20
        //        };
        //        new Label(leftP, "Both Axis Free");
        //        new PropGadget(leftP)
        //        {
        //            Flags = PropFlags.FreeVert | PropFlags.FreeHoriz | PropFlags.AutoKnob,
        //            VertBody = 0x2222,
        //            VertPot = 0x1111,
        //            HorizBody = 0x2222,
        //            HorizPot = 0x1111
        //        };
        //        new Label(leftP, "Slider");
        //        var panel = new Widget(leftP)
        //        {
        //            Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 10, 10)
        //        };
        //        var slider = new Slider(panel)
        //        {
        //            Format = "Level {0}"
        //        };
        //        slider.FormatLabel = new Label(panel, "Level 1");

        //        new Label(leftP, "Scroller");
        //        new Scroller(leftP);

        //        new Label(rightP, "Vert Scroller");
        //        new Scroller(rightP)
        //        {
        //            Orientation = Orientation.Vertical
        //        };
        //    }
        //    propWindow.RequestFocus();
        //}

        //private void ButtonDemo()
        //{
        //    if (window2 == null)
        //    {
        //        window2 = new Window(nanoScreen, "Button Demo")
        //        {
        //            Layout = new GroupLayout(),
        //            Position = new Vector2(100, 100),
        //            Flags = WindowFlags.CloseButton
        //        };
        //        window2.CloseClick += (o, i) =>
        //        {
        //            window2.Close();
        //            window2 = null;
        //        };

        //        new Label(window2, "Push buttons");
        //        var but1 = new Button(window2, "Plain button");
        //        var but2 = new Button(window2, "Styled")
        //        {
        //            BackgroundColor = Color.Blue
        //        };
        //        new Label(window2, "Toggle buttons");
        //        var but3 = new Button(window2, "Toggle me")
        //        {
        //            Flags = ButtonFlags.ToggleButton
        //        };
        //        new Label(window2, "Radio buttons");
        //        var but4 = new Button(window2, "Radio button 1")
        //        {
        //            Flags = ButtonFlags.RadioButton
        //        };
        //        var but5 = new Button(window2, "Radio button 2")
        //        {
        //            Flags = ButtonFlags.RadioButton
        //        };
        //        new Label(window2, "A tool palette");
        //        var box = new Widget(window2)
        //        {
        //            Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 6)
        //        };
        //        new ToolButton(box, Icons.ENTYPO_ICON_CLOUD);
        //        new ToolButton(box, Icons.ENTYPO_ICON_CONTROLLER_FAST_FORWARD);
        //        new ToolButton(box, Icons.ENTYPO_ICON_COMPASS);
        //        new ToolButton(box, Icons.ENTYPO_ICON_INSTALL);
        //        new Label(window2, "Popup buttons");
        //        var but6 = new PopupButton(window2, "Popup", Icons.ENTYPO_ICON_FLASH)
        //        {

        //        };
        //        Popup popup = but6.Popup;
        //        popup.Layout = new GroupLayout();
        //        new Label(popup, "Arbitrary widgets can be placed here");
        //        new CheckBox(popup, "A check box");
        //        var popupBtn = new PopupButton(popup, "Recursive popup", Icons.ENTYPO_ICON_FLASH);
        //        Popup popupRight = popupBtn.Popup;
        //        popupRight.Layout = new GroupLayout();
        //        new CheckBox(popupRight, "Another check box");
        //        // popup left
        //        popupBtn = new PopupButton(popup, "Recursive popup", Icons.ENTYPO_ICON_FLASH);
        //        popupBtn.Side = PopupSide.Left;
        //        Popup popupLeft = popupBtn.Popup;
        //        popupLeft.Layout = new GroupLayout();
        //        new CheckBox(popupLeft, "Another check box");
        //    }
        //    else
        //    {
        //        window2.MoveToFront();
        //    }
        //}

        //private void BasicDemo()
        //{
        //    if (window3 == null)
        //    {
        //        window3 = new Window(nanoScreen, "Basic Widgets")
        //        {
        //            Layout = new GroupLayout(),
        //            Position = new Vector2(200, 100),
        //            Flags = WindowFlags.CloseButton
        //        };

        //        new Label(window3, "Message dialog");
        //        var tools = new Widget(window3);
        //        tools.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 6);
        //        var b = new Button(tools, "Info");
        //        b.Click += (o, i) =>
        //        {
        //            var dlg = new MessageBox(nanoScreen, MessageType.Information, "Title", "This is an information message");
        //        };
        //        b = new Button(tools, "Warn");
        //        b.Click += (o, i) =>
        //        {
        //            var dlg = new MessageBox(nanoScreen, MessageType.Warning, "Title", "This is a warning message");
        //        };
        //        b = new Button(tools, "Ask");
        //        b.Click += (o, i) =>
        //        {
        //            var dlg = new MessageBox(nanoScreen, MessageType.Question, "Title", "This is a  question message", "Yes", "No", true);
        //        };
        //        new Label(window3, "Image panel & scroll panel");
        //        PopupButton imagePanelBtn = new PopupButton(window3, "Image Panel");
        //        imagePanelBtn.Icon = Icons.ENTYPO_ICON_FOLDER;
        //        var popup = imagePanelBtn.Popup;
        //        VScrollPanel vscroll = new VScrollPanel(popup);
        //        ImagePanel imagePanel = new ImagePanel(vscroll);

        //        var ts = engine.GetTileSet("tilesetdefs/tileset_grassland.xml");
        //        List<TextureRegion> texs = new List<TextureRegion>();
        //        foreach (var i in ts.Tiles)
        //        {
        //            texs.Add(ts.GetTile(i));
        //        }
        //        imagePanel.Images = texs;
        //        popup.FixedSize = new Vector2(245, 150);

        //        new Label(window3, "Combo box");
        //        new ComboBox(window3, new[] { "Combo box item 1", "Combo box item 2", "Combo box item 3" });

        //        new Label(window3, "Check box");
        //        CheckBox cb = new CheckBox(window3, "Flag 1");
        //        cb.Checked = true;
        //        cb = new CheckBox(window3, "Flag 2");
        //        new Label(window3, "Progress Bar");
        //        progBar = new ProgressBar(window3)
        //        {
        //            Value = 0.5f
        //        };

        //        new Label(window3, "Slider and text box");

        //        var panel = new Widget(window3);
        //        panel.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 20);
        //        Slider slider = new Slider(panel);
        //        slider.Value = 0.5f;
        //        slider.FixedWidth = 80;
        //        slider.HighlightedRange = (0.0f, 0.5f);
        //        window3.CloseClick += (o, i) =>
        //        {
        //            window3.Close();
        //            window3 = null;
        //        };


        //    }
        //    else
        //    {
        //        window3.MoveToFront();
        //    }
        //}

    }
}