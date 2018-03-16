/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace TileEngine.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;
    using TileEngine.Input;
    using TileEngine.Logging;
    using TileEngine.Screens;

    public static class Intuition
    {

        public const string ROOTCLASS = "rootclass";
        public const string IMAGECLASS = "imageclass";
        public const string FRAMEICLASS = "frameiclass";
        public const string SYSICLASS = "sysiclass";
        public const string FILLRECTCLASS = "fillrectclass";
        public const string ITEXTICLASS = "itexticlass";
        public const string GADGETCLASS = "gadgetclass";
        public const string PROPGCLASS = "propgclass";
        public const string STRGCLASS = "strgclass";
        public const string BUTTONGCLASS = "buttongclass";
        public const string FRBUTTONCLASS = "frbuttonclass";
        public const string GROUPGCLASS = "groupgclass";
        public const string ICCLASS = "icclass";
        public const string MODELCLASS = "modelclass";

        private static Engine engine;
        private static IScreen Screen { get { return engine.Screen; } }
        private static Window activeWindow;
        private static Window hoverWindow;

        private static Gadget activeGadget;
        private static Gadget selectedGadget;
        private static Gadget hoverGadget;

        private static int mouseStartX;
        private static int mouseStartY;
        private static int propStartX;
        private static int propStartY;
        private static Color colorText = Color.Black;
        private static Color colorActive = new Color(62, 92, 154);
        private static Color colorInactive = Color.Gray;
        private static Color colorDarkEdge = new Color(64, 64, 64);
        private static Color colorLightEdge = Color.White;
        private static Color colorMidEdge = new Color(128, 128, 128);
        private static Color colorWindow = new Color(128, 128, 128);
        private static TimeSpan tickDuration = TimeSpan.FromMilliseconds(100);
        private static TimeSpan lastTick;
        private static AutoRequestPositionMode autoRequestPositionMode;
        private static bool moveActiveWindowToFront;
        private static List<IClass> classes = new List<IClass>();

        public static event EventHandler<IntuiMessage> Message;

        private static void OnMessage(Gadget gadget, IDCMPFlags msg, int code = 0)
        {
            OnMessage(gadget?.Window, new IntuiMessage(msg, gadget, code));
        }

        private static void OnMessage(Window window, IDCMPFlags msg, int code = 0)
        {
            if ((window != null) && (window.IDCMPFlags & msg) == msg)
            {
                OnMessage(window, new IntuiMessage(msg, null, code));
            }
        }
        private static void OnMessage(Window window, IntuiMessage msg)
        {
            if ((window != null) && (window.IDCMPFlags & msg.Message) == msg.Message)
            {
                Message?.Invoke(window, msg);

            }
        }

        public static AutoRequestPositionMode AutoRequestPositionMode
        {
            get { return autoRequestPositionMode; }
            set { autoRequestPositionMode = value; }
        }

        public static bool MoveActiveWindowToFront
        {
            get { return moveActiveWindowToFront; }
            set { moveActiveWindowToFront = value; }
        }

        public static Color ColorText
        {
            get { return colorText; }
            set { colorText = value; }
        }

        public static Color ColorWindow
        {
            get { return colorWindow; }
            set { colorWindow = value; }
        }

        public static Color ColorDarkEdge
        {
            get { return colorDarkEdge; }
            set { colorDarkEdge = value; }
        }

        public static Color ColorLightEdge
        {
            get { return colorLightEdge; }
            set { colorLightEdge = value; }
        }

        public static Color ColorMidEdge
        {
            get { return colorMidEdge; }
            set { colorMidEdge = value; }
        }

        public static void Init(Engine engine)
        {
            Intuition.engine = engine;
            InitClasses();
        }

        public static Window OpenWindowTags(NewWindow newWindow, params (Tags, object)[] tags)
        {
            if (newWindow == null) newWindow = new NewWindow();
            int opacity = 255;
            int hoverOpacity = 255;
            Color bgColor = colorWindow;
            Color fgColor = colorText;
            if (tags != null)
            {
                newWindow.Flags |= (tags.GetTagData(Tags.WA_Flags, WindowFlags.None) & ~WindowFlags.WFLG_PRIVATEFLAGS);//  ((WindowFlags)tags.GetTagData(Tags.WA_Flags) & ~WindowFlags.WFLG_PRIVATEFLAGS);
                foreach (var tag in tags)
                {
                    switch (tag.Item1)
                    {
                        case Tags.WA_Left:
                            newWindow.LeftEdge = (int)tag.Item2;
                            break;
                        case Tags.WA_Top:
                            newWindow.TopEdge = (int)tag.Item2;
                            break;
                        case Tags.WA_Width:
                            newWindow.Width = (int)tag.Item2;
                            break;
                        case Tags.WA_Height:
                            newWindow.Height = (int)tag.Item2;
                            break;
                        case Tags.WA_IDCMP:
                            newWindow.IDCMPFlags = (IDCMPFlags)tag.Item2;
                            break;
                        case Tags.WA_MinWidth:
                            newWindow.MinWidth = (int)tag.Item2;
                            break;
                        case Tags.WA_MinHeight:
                            newWindow.MinHeight = (int)tag.Item2;
                            break;
                        case Tags.WA_MaxWidth:
                            newWindow.MaxWidth = (int)tag.Item2;
                            break;
                        case Tags.WA_MaxHeight:
                            newWindow.MaxHeight = (int)tag.Item2;
                            break;
                        case Tags.WA_Gadgets:
                            newWindow.Gadgets = (IList<Gadget>)tag.Item2;
                            break;
                        case Tags.WA_Title:
                            newWindow.Title = (string)tag.Item2;
                            break;
                        case Tags.WA_Screen:
                            newWindow.Screen = (IScreen)tag.Item2;
                            break;
                        case Tags.WA_SizeGadget:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_SIZEGADGET);
                            break;
                        case Tags.WA_DragBar:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_DRAGBAR);
                            break;
                        case Tags.WA_DepthGadget:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_DEPTHGADGET);
                            break;
                        case Tags.WA_CloseGadget:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_CLOSEGADGET);
                            break;
                        case Tags.WA_Backdrop:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_BACKDROP);
                            break;
                        case Tags.WA_ReportMouse:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_REPORTMOUSE);
                            break;
                        case Tags.WA_NoCareRefresh:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_NOCAREREFRESH);
                            break;
                        case Tags.WA_Borderless:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_BORDERLESS);
                            break;
                        case Tags.WA_Activate:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_ACTIVATE);
                            break;
                        case Tags.WA_RMBTrap:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_RMBTRAP);
                            break;
                        case Tags.WA_WBenchWindow:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_WBENCHWINDOW);
                            break;
                        case Tags.WA_SizeBRight:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_SIZEBRIGHT);
                            break;
                        case Tags.WA_SizeBBottom:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_SIZEBBOTTOM);
                            break;
                        case Tags.WA_GimmeZeroZero:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_GIMMEZEROZERO);
                            break;
                        case Tags.WA_NewLookMenus:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_NEWLOOKMENUS);
                            break;
                        case Tags.WA_ToolBox:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_TOOLBOX);
                            break;
                        case Tags.WA_Opacity:
                            opacity = (int)tag.Item2;
                            break;
                        case Tags.WA_HoverOpacity:
                            hoverOpacity = (int)tag.Item2;
                            break;
                        case Tags.WA_BackgroundColor:
                            bgColor = (Color)tag.Item2;
                            break;
                        case Tags.WA_ForegroundColor:
                            fgColor = (Color)tag.Item2;
                            break;
                        case Tags.WA_Zoom:

                            break;
                        case Tags.WA_SimpleRefresh:
                            if ((bool)tag.Item2)
                            {
                                newWindow.Flags &= ~WindowFlags.WFLG_REFRESHBITS;
                                newWindow.Flags |= WindowFlags.WFLG_SIMPLE_REFRESH;
                            }
                            break;
                        case Tags.WA_SmartRefresh:
                            if ((bool)tag.Item2)
                            {
                                newWindow.Flags &= ~WindowFlags.WFLG_REFRESHBITS;
                            }
                            break;
                    }
                }
            }
            if ((newWindow.Flags & WindowFlags.WFLG_SIZEGADGET) == WindowFlags.WFLG_SIZEGADGET)
            {
                if ((newWindow.Flags & (WindowFlags.WFLG_SIZEBBOTTOM | WindowFlags.WFLG_SIZEBRIGHT)) == WindowFlags.None)
                {
                    newWindow.Flags |= WindowFlags.WFLG_SIZEBRIGHT;
                }
                newWindow.Flags |= WindowFlags.WFLG_HASZOOM;
            }
            else
            {
                newWindow.Flags &= ~(WindowFlags.WFLG_SIZEBBOTTOM | WindowFlags.WFLG_SIZEBRIGHT);
            }
            if ((newWindow.Flags & WindowFlags.WFLG_BORDERLESS) == WindowFlags.WFLG_BORDERLESS)
            {
                newWindow.Flags &= ~(WindowFlags.WFLG_SIZEGADGET | WindowFlags.WFLG_SIZEBBOTTOM | WindowFlags.WFLG_SIZEBRIGHT);
            }
            if (newWindow.Screen == null) newWindow.Screen = engine.Screen;
            Window window = new Window(engine, newWindow);
            window.IDCMPFlags |= IDCMPFlags.AUTOREQUEST;
            window.Opacity = opacity;
            window.HoverOpacity = hoverOpacity;
            window.BgColor = bgColor;
            window.FgColor = fgColor;
            AddSysGadgets(window);
            AddGList(window, newWindow.Gadgets, -1, -1);
            window.Screen.AddWindow(window);
            if (window.HasFlag(WindowFlags.WFLG_ACTIVATE))
            {
                ActivateWindow(window);
            }
            Logger.Info("Intuition", $"OpenWindow: {window}");
            return window;
        }
        public static Window OpenWindow(NewWindow newWindow)
        {
            return OpenWindowTags(newWindow, null);
        }

        public static void CloseWindow(Window window)
        {
            Logger.Info("Intuition", $"CloseWindow: {window}");
            window.Close();
        }

        public static void Update(TimeInfo time)
        {
            CheckIntuiTick(time);
        }

        public static void Render(IGraphics graphics, TimeInfo time)
        {
            if (Screen != null)
            {
                foreach (Window window in Screen.Windows)
                {
                    if (!window.Valid)
                    {
                        RefreshWindowFrame(window);
                    }
                    RenderWindowBitmap(window, graphics);
                }
            }
        }

        private static void InitClasses()
        {
            classes = new List<IClass>();
            classes.Add(new IClass(BUTTONGCLASS, typeof(ButtonGadget)));
            classes.Add(new IClass(FRBUTTONCLASS, typeof(FrameButtonGadget)));
            classes.Add(new IClass(PROPGCLASS, typeof(PropGadget)));
            classes.Add(new IClass(STRGCLASS, typeof(StringGadget)));
            classes.Add(new IClass(GROUPGCLASS, typeof(GroupGadget)));
            classes.Add(new IClass(FRAMEICLASS, typeof(FrameImage)));
            classes.Add(new IClass(SYSICLASS, typeof(SysImage)));
            classes.Add(new IClass(MODELCLASS, typeof(Model)));
        }

        private static IClass FindClass(string classId)
        {
            return classes.FirstOrDefault(c => c.Name.Equals(classId));
        }

        public static Root NewObject(string classId, IList<(Tags, object)> tags)
        {
            return NewObject(null, classId, tags);
        }
        public static Root NewObject(string classId, params (Tags, object)[] tags)
        {
            return NewObject(null, classId, (IList<(Tags, object)>)tags);
        }
        public static Root NewObject(IClass @class, params (Tags, object)[] tags)
        {
            return NewObject(@class, null, (IList<(Tags, object)>)tags);
        }
        public static Root NewObject(IClass @class, IList<(Tags, object)> tags)
        {
            return NewObject(@class, null, tags);
        }
        public static Root NewObject(IClass @class, string classId, params (Tags, object)[] tags)
        {
            return NewObject(@class, classId, (IList<(Tags, object)>)tags);
        }
        public static Root NewObject(IClass @class, string classId, IList<(Tags, object)> tags)
        {
            if (@class == null) @class = FindClass(classId);
            if (@class != null)
            {
                return @class.Create(tags);
            }
            return null;
        }

        private static void CheckIntuiTick(TimeInfo time)
        {
            var timeDiff = time.TotalGameTime - lastTick;
            if (timeDiff > tickDuration)
            {
                lastTick = time.TotalGameTime;
                if (activeGadget != null)
                {
                    InputEvent ie = new InputEvent()
                    {
                        InputClass = InputClass.TIMER,
                        Key = Key.None,
                        MouseButton = MouseButton.None,
                        X = -1,
                        Y = -1
                    };
                    int termination = 0;
                    HandleActiveGadgetInput(ie, ref termination);
                    OnMessage(activeGadget, IDCMPFlags.INTUITICKS);
                }
                else if (activeWindow != null)
                {
                    OnMessage(activeWindow, IDCMPFlags.INTUITICKS);
                }
            }
        }

        private static void RenderWindowBitmap(Window window, IGraphics graphics)
        {
            graphics.Render(window.Bitmap, window.LeftEdge, window.TopEdge, window.RenderTransparency);
        }

        public static bool Request(Requester req, Window window)
        {
            if (window.Request(req))
            {
                OnMessage(window, IDCMPFlags.REQSET);
                return true;
            }
            return false;
        }

        private static void InternalEndRequest(Requester req, Window window, Gadget gadget)
        {
            if (window != null)
            {
                if (window.EndRequest(req))
                {
                    OnMessage(window, IDCMPFlags.REQCLEAR);
                }
            }
            if ((req.Flags & ReqFlags.SYSREQUEST) == ReqFlags.SYSREQUEST)
            {
                FreeSysRequest(window);
                if (gadget != null)
                {
                    OnMessage(window, IDCMPFlags.AUTOREQUEST, gadget.GadgetId);
                }
            }
        }

        public static void EndRequest(Requester req, Window window)
        {
            InternalEndRequest(req, window, null);
        }

        public static void OnGadget(Gadget gadget, Window window)
        {
            gadget.Flags &= ~GadgetFlags.GADGDISABLED;
            window.Invalidate();
        }
        public static void OffGadget(Gadget gadget, Window window)
        {
            gadget.Flags |= GadgetFlags.GADGDISABLED;
            window.Invalidate();
        }

        public static void ModifyIDCMP(Window window, IDCMPFlags flags)
        {
            window.IDCMPFlags = flags;
            window.Invalidate();
        }

        public static bool ActivateGadget(Gadget gadget, Window window)
        {
            if (activeWindow == window)
            {
                if (activeWindow.Gadgets.Contains(gadget))
                {
                    SetActiveGadget(gadget);
                    return true;
                }
            }
            return false;
        }

        public static void ActivateWindow(Window window)
        {
            if (activeWindow != window)
            {
                if (activeWindow != null)
                {
                    activeWindow.Flags &= ~WindowFlags.WFLG_WINDOWACTIVE;
                    activeWindow.Invalidate();
                    OnMessage(activeWindow, IDCMPFlags.INACTIVEWINDOW);
                }
                activeWindow = window;
                if (activeWindow != null)
                {
                    activeWindow.Flags |= WindowFlags.WFLG_WINDOWACTIVE;
                    activeWindow.Screen.ActiveWindow = activeWindow;
                    if (moveActiveWindowToFront && ((activeWindow.Flags & WindowFlags.WFLG_BACKDROP) != WindowFlags.WFLG_BACKDROP))
                    {
                        WindowToFront(activeWindow);
                    }
                    activeWindow.Invalidate();
                    OnMessage(activeWindow, IDCMPFlags.ACTIVEWINDOW);
                }
            }
        }

        public static int AddGadget(Window window, Gadget gadget, int position)
        {
            return window.AddGadget(gadget, position);
        }

        public static int AddGList(Window window, IEnumerable<Gadget> gadgets, int position, int numgad)
        {
            return window.AddGList(gadgets, position, numgad);
        }

        public static int RemoveGadget(Window window, Gadget gadget)
        {
            return window.RemoveGadget(gadget);
        }

        public static int RemoveGList(Window window, IEnumerable<Gadget> gadgets, int numgad)
        {
            return window.RemoveGList(gadgets, numgad);
        }

        public static void ReportMouse(Window window, bool report)
        {
            if (report)
            {
                window.Flags |= WindowFlags.WFLG_REPORTMOUSE;
            }
            else
            {
                window.Flags &= ~WindowFlags.WFLG_REPORTMOUSE;
            }
        }

        public static void MoveWindow(Window window, int deltaX, int deltaY)
        {
            window.LeftEdge += deltaX;
            window.TopEdge += deltaY;
        }

        public static void SizeWindow(Window window, int deltaX, int deltaY)
        {
            if (deltaX != 0 || deltaY != 0)
            {
                window.Width += deltaX;
                window.Height += deltaY;
                OnMessage(window, IDCMPFlags.NEWSIZE);
            }
        }

        public static void ChangeWindowBox(Window window, int left, int top, int width, int height)
        {
            window.SetWindowBox(left, top, width, height);
        }

        public static void WindowLimits(Window window, int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            window.SetLimits(minWidth, minHeight, maxWidth, maxHeight);
        }

        public static void WindowToBack(Window window)
        {
            window.Screen.WindowToBack(window);
        }

        public static void WindowToFront(Window window)
        {
            window.Screen.WindowToFront(window);
        }

        public static void ZipWindow(Window window)
        {
            window.Zip();
        }

        public static bool AutoRequest(Window window, IntuiText bodyText, string posText, string negText, IDCMPFlags posFlags, IDCMPFlags negFlags, int width, int height)
        {
            IDCMPFlags flags = IDCMPFlags.GADGETUP;
            flags |= posFlags;
            flags |= negFlags;
            Window reqWin = BuildSysRequest(window, bodyText, posText, negText, flags, width, height);
            return reqWin != null;
        }

        public static Window BuildSysRequest(Window window, IntuiText bodyText, string posText, string negText, IDCMPFlags flags, int width, int height)
        {
            IScreen screen = window != null ? window.Screen : Screen;
            List<Gadget> gadgets = new List<Gadget>();
            int gadWidth = width / 4;
            int gadHeight = height / 4;
            var btnImage = NewObject(Intuition.FRAMEICLASS,
                (Tags.IA_Width, gadWidth),
                (Tags.IA_Height, gadHeight),
                (Tags.IA_EdgesOnly, false),
                (Tags.IA_FrameType, FrameType.Button)
            );
            NewObject(FRBUTTONCLASS,
                (Tags.GA_List, gadgets),
                (Tags.GA_Left, 5),
                (Tags.GA_Top, 2 * height / 3),
                (Tags.GA_Width, gadWidth),
                (Tags.GA_Height, gadHeight),
                (Tags.GA_EndGadget, true),
                (Tags.GA_ToggleSelect, true),
                (Tags.GA_ID, 1),
                (Tags.GA_Text, posText),
                (Tags.GA_Image, btnImage)
                );
            NewObject(FRBUTTONCLASS,
                (Tags.GA_List, gadgets),
                (Tags.GA_RelRight, -(5 + width / 4)),
                (Tags.GA_Top, 2 * height / 3),
                (Tags.GA_Width, gadWidth),
                (Tags.GA_Height, gadHeight),
                (Tags.GA_EndGadget, true),
                (Tags.GA_ToggleSelect, true),
                (Tags.GA_ID, 0),
                (Tags.GA_Text, negText),
                (Tags.GA_Image, btnImage)
                );
            string title = window.Title;
            if (string.IsNullOrEmpty(title)) title = "System Request";
            int x = 0;
            int y = 0;
            switch (autoRequestPositionMode)
            {
                case AutoRequestPositionMode.CenterScreen:
                    x = engine.Graphics.ViewWidth / 2 - (width + 2 * 4) / 2;
                    y = engine.Graphics.ViewHeight / 2 - (height + 2 * 4 + 16) / 2;
                    break;
                case AutoRequestPositionMode.CenterWindow:
                    x = window.LeftEdge + window.Width / 2 - (width + 2 * 4) / 2;
                    y = window.TopEdge + window.Height / 2 - (height + 2 * 4 + 16) / 2;
                    break;
            }
            bodyText.LeftEdge = width / 2;
            IntuiText txt = bodyText.NextText;
            int countTexts = 1;
            while (txt != null)
            {
                countTexts++;
                txt.LeftEdge = width / 2;
                txt = txt.NextText;
            }
            int txtHeight = height / (3 * countTexts);
            bodyText.TopEdge = txtHeight;
            int txtTop = txtHeight;
            txt = bodyText.NextText;
            while (txt != null)
            {
                txtTop += txtHeight;
                txt.TopEdge = txtTop;
                txt = txt.NextText;
            }
            Requester req = new Requester();
            req.Width = width;
            req.Height = height;
            req.ReqText = bodyText;
            req.LeftEdge = 4;
            req.TopEdge = 20;
            req.BackFill = Color.Gray;
            req.Flags |= ReqFlags.SYSREQUEST;
            req.ReqBorder = new Border(width, height);
            req.ReqGadgets = gadgets;
            Window reqWin = OpenWindowTags(null,
                (Tags.WA_Left, x),
                (Tags.WA_Top, y),
                (Tags.WA_Width, width + 2 * 4),
                (Tags.WA_Height, height + 2 * 4 + 16),
                (Tags.WA_Flags, WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_ACTIVATE),
                (Tags.WA_IDCMP, IDCMPFlags.GADGETUP),
                (Tags.WA_Title, title),
                (Tags.WA_Screen, screen)
                );
            Request(req, reqWin);
            return reqWin;
        }

        public static void FreeSysRequest(Window window)
        {
            Requester req = window.Requesters.FirstOrDefault();
            while (req != null)
            {
                EndRequest(req, window);
                req = window.Requesters.FirstOrDefault();
            }
            CloseWindow(window);
        }

        public static bool EasyRequest(Window window, EasyStruct easy, params object[] args)
        {
            Window reqWin = BuildEasyRequest(window, easy, args);
            return reqWin != null;
        }

        public static Window BuildEasyRequest(Window window, EasyStruct easy, params object[] args)
        {
            IScreen screen = window != null ? window.Screen : Screen;
            string bodyText = args.Length > 0 ? string.Format(easy.TextFormat, args[0]) : easy.TextFormat;
            IntuiText body = BuildMultiLineText(bodyText);
            List<Gadget> gadgets = new List<Gadget>();
            var formats = easy.GadgetFormat.Split('|');
            int numGads = formats.Length;
            int gadWidth = 64;
            int gadHeight = 24;
            int count = 0;
            int width = 0;
            int posX = 4;
            int height = 120;
            var btnImage = NewObject(Intuition.FRAMEICLASS,
                (Tags.IA_Width, gadWidth),
                (Tags.IA_Height, gadHeight),
                (Tags.IA_EdgesOnly, false),
                (Tags.IA_FrameType, FrameType.Button)
            );

            foreach (var fs in formats)
            {
                object arg = null;
                if (args.Length > count) arg = args[1 + count];
                string gadText = string.Format(fs, arg);
                NewObject(FRBUTTONCLASS,
                    (Tags.GA_List, gadgets),
                    (Tags.GA_Left, posX),
                    (Tags.GA_Top, height - gadHeight - 8),
                    (Tags.GA_Width, gadWidth),
                    (Tags.GA_Height, gadHeight),
                    (Tags.GA_EndGadget, true),
                    (Tags.GA_ToggleSelect, true),
                    (Tags.GA_ID, count < numGads - 1 ? count + 1 : 0),
                    (Tags.GA_Text, gadText),
                    (Tags.GA_Image, btnImage)
                    );
                posX += (gadWidth + 4);
                width = posX;
                count++;
            }
            string title = easy.Title;
            int x = 0;
            int y = 0;
            switch (autoRequestPositionMode)
            {
                case AutoRequestPositionMode.CenterScreen:
                    x = engine.Graphics.ViewWidth / 2 - (width + 2 * 4) / 2;
                    y = engine.Graphics.ViewHeight / 2 - (height + 2 * 4 + 16) / 2;
                    break;
                case AutoRequestPositionMode.CenterWindow:
                    x = window.LeftEdge + window.Width / 2 - (width + 2 * 4) / 2;
                    y = window.TopEdge + window.Height / 2 - (height + 2 * 4 + 16) / 2;
                    break;
            }
            body.LeftEdge = width / 2;
            IntuiText txt = body.NextText;
            int countTexts = 1;
            while (txt != null)
            {
                countTexts++;
                txt.LeftEdge = width / 2;
                txt = txt.NextText;
            }
            int txtHeight = (height - (gadHeight + 8)) / (countTexts + 1);
            body.TopEdge = txtHeight;
            int txtTop = txtHeight;
            txt = body.NextText;
            while (txt != null)
            {
                txtTop += txtHeight;
                txt.TopEdge = txtTop;
                txt = txt.NextText;
            }
            Requester req = new Requester();
            req.Width = width;
            req.Height = height;
            req.ReqText = body;
            req.LeftEdge = 4;
            req.TopEdge = 20;
            req.BackFill = Color.Gray;
            req.Flags |= ReqFlags.SYSREQUEST;
            req.ReqBorder = new Border(width, height);
            req.ReqGadgets = gadgets;
            Window reqWin = OpenWindowTags(null,
                (Tags.WA_Left, x),
                (Tags.WA_Top, y),
                (Tags.WA_Width, width + 2 * 4),
                (Tags.WA_Height, height + 2 * 4 + 16),
                (Tags.WA_Flags, WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_ACTIVATE),
                (Tags.WA_IDCMP, IDCMPFlags.GADGETUP),
                (Tags.WA_Title, title),
                (Tags.WA_Screen, screen)
                );
            Request(req, reqWin);
            return reqWin;
        }

        public static void FreeEasyRequest(Window window)
        {
            Requester req = window.Requesters.FirstOrDefault();
            while (req != null)
            {
                EndRequest(req, window);
                req = window.Requesters.FirstOrDefault();
            }
            CloseWindow(window);
        }

        public static void RefreshGadgets(IEnumerable<Gadget> gadgets, Window window, Requester requester)
        {
            foreach (var gad in gadgets)
            {
                RenderGadget(window, requester, gad, engine.Graphics);
            }
        }

        public static void RefreshGList(IEnumerable<Gadget> gadgets, Window window, Requester requester, int numgad)
        {
            int count = 0;
            foreach (var gad in gadgets)
            {
                if ((numgad >= 0) && (count >= numgad)) break;
                RenderGadget(window, requester, gad, engine.Graphics);
                count++;
            }
        }

        public static void RefreshWindowFrame(Window window)
        {
            RenderWindow(window, engine.Graphics);
        }

        public static void NewModifyProp(Gadget gadget, Window window, Requester req, PropFlags flags, int horizPot, int vertPot, int horizBody, int vertBody, int numGads)
        {
            PropInfo pi = gadget?.PropInfo;
            if (pi != null)
            {
                pi.Flags = flags;
                pi.HorizPot = horizPot;
                pi.HorizBody = horizBody;
                pi.VertPot = vertPot;
                pi.VertBody = vertBody;
            }
        }

        public static void KeyDown(Key key, char code)
        {
            HandleInput(new InputEvent()
            {
                InputClass = InputClass.KEYDOWN,
                Key = key
            });
        }

        public static void KeyUp(Key key, char code)
        {
            HandleInput(new InputEvent()
            {
                InputClass = InputClass.KEYUP,
                Key = key
            });
        }

        private static bool ValidCode(Gadget gadget, char code)
        {
            if ((gadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.STRGADGET)
            {
                bool intGad = ((gadget.Activation & GadgetActivation.LONGINT) == GadgetActivation.LONGINT);
                int pos = gadget.StringInfo.BufferPos;
                if (intGad)
                {
                    if (char.IsDigit(code)) return true;
                    if (pos == 0)
                    {
                        if ((code == '+') || (code == '-')) return true;
                    }
                }
                else
                {
                    if (code >= ' ')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static IntuiText BuildMultiLineText(string txt)
        {
            IntuiText topText = new IntuiText(txt);
            IntuiText text = topText;
            var lines = txt.Split(new char[] { '\n', '\r' });
            if (lines.Length > 1)
            {
                text.IText = lines[0];
                for (int i = 1; i < lines.Length; i++)
                {
                    text.NextText = new IntuiText(lines[i]);
                    text = text.NextText;
                }
            }
            return topText;
        }

        private static void HandleMouseDown(Gadget gadget, int x, int y)
        {
            mouseStartX = x;
            mouseStartY = y;
            SetHoverGadget(gadget);
            SetSelectedGadget(gadget);
            SetActiveGadget(gadget);
            if (gadget != null)
            {
                Window window = gadget.Window;
                if ((gadget.Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED)
                {
                    return;
                }
                if ((gadget.GadgetType & GadgetType.GTYPEMASK) == GadgetType.PROPGADGET)
                {
                    PropInfo pi = gadget.PropInfo;
                    if (pi != null)
                    {
                        int mx = x - window.LeftEdge;
                        int my = y - window.TopEdge;
                        int dx = pi.HorizPot;
                        int dy = pi.VertPot;
                        IBox knob = GetKnobDimensions(gadget);
                        if (knob.ContainsPoint(mx, my))
                        {
                            propStartX = mx - knob.LeftEdge;
                            propStartY = my - knob.TopEdge;
                            pi.Flags |= PropFlags.KNOBHIT;
                        }
                        else
                        {
                            pi.Flags &= ~PropFlags.KNOBHIT;
                            if (((pi.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ))
                            {
                                if (mx < knob.LeftEdge)
                                {
                                    if (dx > pi.HPotRes)
                                    {
                                        dx -= pi.HPotRes;
                                    }
                                    else
                                    {
                                        dx = 0;
                                    }
                                }
                                else if (mx > knob.RightEdge)
                                {
                                    if (dx < PropInfo.MAXPOT - pi.HPotRes)
                                    {
                                        dx += pi.HPotRes;
                                    }
                                    else
                                    {
                                        dx = PropInfo.MAXPOT;
                                    }
                                }
                            }
                            if (((pi.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT))
                            {
                                if (my < knob.TopEdge)
                                {
                                    if (dy > pi.VPotRes)
                                    {
                                        dy -= pi.VPotRes;
                                    }
                                    else
                                    {
                                        dy = 0;
                                    }
                                }
                                else if (my > knob.BottomEdge)
                                {
                                    if (dy < PropInfo.MAXPOT - pi.VPotRes)
                                    {
                                        dy += pi.VPotRes;
                                    }
                                    else
                                    {
                                        dy = PropInfo.MAXPOT;
                                    }
                                }
                            }
                            NewModifyProp(gadget, window, gadget.Requester, pi.Flags, dx, dy, pi.HorizBody, pi.VertBody, 1);
                        }
                    }
                }

                if ((gadget.Activation & GadgetActivation.GADGIMMEDIATE) == GadgetActivation.GADGIMMEDIATE)
                {
                    OnMessage(gadget, IDCMPFlags.GADGETDOWN);
                }

            }

        }

        private static void HandleMouseUp(Gadget gadget, InputEvent ie, int x, int y)
        {
            int termination = 0;
            mouseStartX = x;
            mouseStartY = y;
            SetHoverGadget(gadget);
            SetSelectedGadget(null);
            if (gadget != null)
            {
                Window window = gadget.Window;
                Requester req = gadget.Requester;
                if (window != null)
                {
                    if ((gadget.Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED)
                    {
                        return;
                    }
                    switch (gadget.GadgetType & GadgetType.SYSTYPEMASK)
                    {
                        case GadgetType.WDEPTH:
                            if (window.IsTopMostWindow)
                            {
                                WindowToBack(activeWindow);
                            }
                            else
                            {
                                WindowToFront(activeWindow);
                            }
                            break;
                        case GadgetType.WZOOM:
                            window.Zip();
                            break;
                        case GadgetType.CLOSE:
                            OnMessage(activeGadget, IDCMPFlags.CLOSEWINDOW);
                            break;
                    }
                    switch (gadget.GadgetType & GadgetType.GTYPEMASK)
                    {
                        case GadgetType.PROPGADGET:
                            PropInfo pi = gadget.PropInfo;
                            if (pi != null)
                            {
                                pi.Flags &= ~PropFlags.KNOBHIT;
                            }
                            break;
                    }
                    if ((gadget.Activation & GadgetActivation.RELVERIFY) == GadgetActivation.RELVERIFY)
                    {
                        if (gadget == activeGadget)
                        {
                            var tie = new InputEvent(ie);
                            tie.InputClass = InputClass.GADGETUP;
                            HandleActiveGadgetInput(tie, ref termination);
                            OnMessage(gadget, IDCMPFlags.GADGETUP);
                        }
                    }
                    if ((gadget.Activation & GadgetActivation.ENDGADGET) == GadgetActivation.ENDGADGET)
                    {
                        InternalEndRequest(req, window, gadget);
                    }
                }
            }
        }

        private static void HandleMouseMove(Gadget gadget, int x, int y)
        {
            SetHoverGadget(gadget);
            int deltaX = x - mouseStartX;
            int deltaY = y - mouseStartY;
            mouseStartX = x;
            mouseStartY = y;
            if (selectedGadget != null)
            {
                if ((selectedGadget.Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED)
                {
                    return;
                }
                Window window = selectedGadget.Window;
                Requester req = selectedGadget.Requester;
                if (window != null)
                {
                    switch (selectedGadget.GadgetType & GadgetType.SYSTYPEMASK)
                    {
                        case GadgetType.SIZING:
                            SizeWindow(window, deltaX, deltaY);
                            break;
                        case GadgetType.WDRAGGING:
                            MoveWindow(window, deltaX, deltaY);
                            break;
                    }
                    switch (selectedGadget.GadgetType & GadgetType.GTYPEMASK)
                    {
                        case GadgetType.PROPGADGET:
                            PropInfo pi = selectedGadget.PropInfo;
                            if (pi != null & (pi.Flags & PropFlags.KNOBHIT) == PropFlags.KNOBHIT)
                            {
                                int mx = x - window.LeftEdge - pi.LeftBorder;
                                int my = y - window.TopEdge - pi.TopBorder;
                                int dx = mx - propStartX;
                                int dy = my - propStartY;
                                IBox knob = GetKnobDimensions(selectedGadget);

                                if (((pi.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ) && (pi.CWidth != knob.Width))
                                {
                                    dx = (dx * PropInfo.MAXPOT) / (pi.CWidth - knob.Width);
                                    if (dx < 0) dx = 0;
                                    if (dx > PropInfo.MAXPOT) dx = PropInfo.MAXPOT;
                                }
                                if (((pi.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT) && (pi.CHeight != knob.Height))
                                {
                                    dy = (dy * PropInfo.MAXPOT) / (pi.CHeight - knob.Height);
                                    if (dy < 0) dx = 0;
                                    if (dy > PropInfo.MAXPOT) dx = PropInfo.MAXPOT;
                                }
                                if ((((pi.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ) && (dx != pi.HorizPot)) ||
                                    (((pi.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT) && (dy != pi.VertPot)))
                                {
                                    NewModifyProp(selectedGadget, window, req, pi.Flags, dx, dy, pi.HorizBody, pi.VertBody, 1);

                                }
                            }
                            break;
                    }
                }
            }
        }
        private static GadgetActive HandleGadgetInput(InputEvent ie, Gadget gadget, ref int termination)
        {
            GadgetActive ga = GadgetActive.MeActive;
            if (gadget != null)
            {
                Window win = gadget.Window;
                Requester req = gadget.Requester;
                GadgetInfo gi = SetupGInfo(win, req, gadget, null);
                int mx = ie.X - win.LeftEdge;
                int my = ie.Y - win.TopEdge;
                ga = gadget.HandleInput(gi, ie, ref termination, mx, my);
                if (ga == GadgetActive.MeActive)
                {
                    win.Invalidate();
                }
            }
            return ga;
        }

        private static GadgetActive HandleActiveGadgetInput(InputEvent ie, ref int termination)
        {
            return HandleGadgetInput(ie, activeGadget, ref termination);
        }

        private static GadgetActive HandleHoverGadgetInput(InputEvent ie, ref int termination)
        {
            return HandleGadgetInput(ie, hoverGadget, ref termination);
        }


        private static void HandleInput(InputEvent ie)
        {
            if (FindInput(ie, out IScreen scr, out Window win, out Requester req, out Gadget gad))
            {
                SetHoverWindow(win);
                int termination = 0;
                GadgetInfo gi = SetupGInfo(win, req, gad, null);
                GadgetActive ga = GadgetActive.MeActive;
                switch (ie.InputClass)
                {
                    case InputClass.MOUSEDOWN:
                        ActivateWindow(win);
                        HandleMouseDown(gad, ie.X, ie.Y);
                        ga = HandleActiveGadgetInput(ie, ref termination);
                        break;
                    case InputClass.MOUSEUP:
                        HandleMouseUp(gad, ie, ie.X, ie.Y);
                        ga = HandleActiveGadgetInput(ie, ref termination);
                        break;
                    case InputClass.MOUSEMOVE:
                        HandleMouseMove(gad, ie.X, ie.Y);
                        ga = HandleHoverGadgetInput(ie, ref termination);
                        break;
                    case InputClass.KEYDOWN:
                        ga = HandleActiveGadgetInput(ie, ref termination);
                        break;
                    case InputClass.KEYUP:
                        ga = HandleActiveGadgetInput(ie, ref termination);
                        break;
                    case InputClass.TIMER:
                        ga = HandleActiveGadgetInput(ie, ref termination);
                        break;
                }
            }
        }

        public static void DrawImageState(IGraphics rPort, Image image, int x, int y, int width, int height, ImageState state, DrawInfo drawInfo = null)
        {
            image.DrawFrame(rPort, x, y, width, height, state, drawInfo);
        }

        public static void DrawImageState(IGraphics rPort, Image image, int x, int y, ImageState state, DrawInfo drawInfo = null)
        {
            image.Draw(rPort, x, y, state, drawInfo);
        }

        public static void MouseMove(int x, int y, MouseButton button)
        {
            HandleInput(new InputEvent()
            {
                InputClass = InputClass.MOUSEMOVE,
                X = x,
                Y = y,
                MouseButton = button
            });
        }
        public static void MouseDown(int x, int y, MouseButton button)
        {
            HandleInput(new InputEvent()
            {
                InputClass = InputClass.MOUSEDOWN,
                X = x,
                Y = y,
                MouseButton = button
            });
        }
        public static void MouseUp(int x, int y, MouseButton button)
        {
            HandleInput(new InputEvent()
            {
                InputClass = InputClass.MOUSEUP,
                X = x,
                Y = y,
                MouseButton = button
            });
        }

        internal static void DrawIntuiText(IGraphics graphics, IntuiText text, int x, int y)
        {
            while (text != null)
            {
                graphics.RenderText(text.IText, x + text.LeftEdge, y + text.TopEdge, text.FrontPen, text.HorizontalTextAlign, text.VerticalTextAlign);
                text = text.NextText;
            }
        }

        internal static void DrawBorder(IGraphics graphics, Border border, int x, int y)
        {
            while (border != null)
            {
                int lx = x + border.LeftEdge + border.XY[0];
                int ly = y + border.TopEdge + border.XY[1];
                for (int i = 2; i < border.XY.Count; i += 2)
                {
                    int nx = x + border.LeftEdge + border.XY[i];
                    int ny = y + border.TopEdge + border.XY[i + 1];
                    graphics.DrawLine(lx, ly, nx, ny, border.FrontPen);
                    lx = nx;
                    ly = ny;
                }
                border = border.NextBorder;
            }
        }

        private static void DrawImage(IGraphics graphics, Image image, int x, int y)
        {
            while (image != null)
            {
                image.ImageData.Draw(graphics, x + image.LeftEdge, y + image.TopEdge, image.Width, image.Height);
                image = image.NextImage;
            }
        }

        private static void SetActiveGadget(Gadget gadget)
        {
            if (activeGadget != gadget)
            {
                if (activeGadget != null)
                {
                    activeGadget.Activation &= ~GadgetActivation.ACTIVEGADGET;
                }
                activeGadget = null;
                if (gadget != null && !gadget.Disabled)
                {
                    activeGadget = gadget;
                    activeGadget.Activation |= GadgetActivation.ACTIVEGADGET;
                    ActivateWindow(activeGadget.Window);
                }
            }
        }

        private static void SetSelectedGadget(Gadget gadget)
        {
            if (selectedGadget != gadget)
            {
                if (selectedGadget != null)
                {
                    selectedGadget.Selected = false;
                }
                selectedGadget = null;
                if (gadget != null && !gadget.Disabled)
                {
                    selectedGadget = gadget;
                    selectedGadget.Selected = true;
                }
            }
        }

        private static void SetHoverGadget(Gadget gadget)
        {
            if (hoverGadget != gadget)
            {
                if (hoverGadget != null)
                {
                    hoverGadget.Flags &= ~GadgetFlags.HOVER;
                    hoverGadget.Window.Invalidate();
                }
                hoverGadget = gadget;
                if (hoverGadget != null)
                {
                    hoverGadget.Flags |= GadgetFlags.HOVER;
                    hoverGadget.Window.Invalidate();
                    Logger.Info("Intuition", $"Hover Gadget: \"{hoverGadget}\"");
                }
            }
        }

        private static void SetHoverWindow(Window window)
        {
            if (hoverWindow != window)
            {
                if (hoverWindow != null)
                {
                    hoverWindow.MoreFlags &= ~MoreWindowFlags.WFLG_HOVER;
                    hoverWindow.Invalidate();
                }
                hoverWindow = window;
                if (hoverWindow != null)
                {
                    hoverWindow.MoreFlags |= MoreWindowFlags.WFLG_HOVER;
                    hoverWindow.Invalidate();
                    Logger.Info("Intuition", $"Hover Window: \"{hoverWindow}\"");
                }
            }
        }

        private static void AddSysGadgets(Window window)
        {
            int titleLeft = 0;
            int titleRight = 0;
            int titleBarHeight = 20;
            if (window.HasFlag(WindowFlags.WFLG_CLOSEGADGET))
            {
                var closeImage = NewObject(SYSICLASS,
                    (Tags.SYSIA_Which, SysImageType.Close)
                    );
                Gadget closeGadget = (Gadget)NewObject(FRBUTTONCLASS,
                    (Tags.GA_Left, 0),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 20),
                    (Tags.GA_Height, titleBarHeight),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_RelVerify, true),
                    (Tags.GA_SysGType, GadgetType.CLOSE),
                    (Tags.GA_Image, closeImage)
                    );
                window.AddGadget(closeGadget, -1);
                titleLeft += closeGadget.Width;
            }
            if (window.HasFlag(WindowFlags.WFLG_DEPTHGADGET))
            {
                var depthImage = NewObject(SYSICLASS,
                    (Tags.SYSIA_Which, SysImageType.Depth)
                    );
                Gadget depthGadget = (Gadget)NewObject(FRBUTTONCLASS,
                    (Tags.GA_RelRight, -23),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 24),
                    (Tags.GA_Height, titleBarHeight),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_RelVerify, true),
                    (Tags.GA_SysGType, GadgetType.WDEPTH),
                    (Tags.GA_Image, depthImage)
                    );
                window.AddGadget(depthGadget, -1);
                titleRight += depthGadget.Width;
            }
            if (window.HasFlag(WindowFlags.WFLG_HASZOOM))
            {
                var zoomImage = NewObject(SYSICLASS,
                    (Tags.SYSIA_Which, SysImageType.Zoom)
                    );
                Gadget zoomGadget = (Gadget)NewObject(FRBUTTONCLASS,
                    (Tags.GA_RelRight, -23 - 24),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 24),
                    (Tags.GA_Height, titleBarHeight),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_RelVerify, true),
                    (Tags.GA_SysGType, GadgetType.WZOOM),
                    (Tags.GA_Image, zoomImage)
                    );
                window.AddGadget(zoomGadget, -1);
                titleRight += zoomGadget.Width;
            }
            if (window.HasFlag(WindowFlags.WFLG_DRAGBAR))
            {
                var dragImage = NewObject(SYSICLASS,
                    (Tags.SYSIA_Which, SysImageType.Drag)
                    );
                Gadget dragGadget = (Gadget)NewObject(FRBUTTONCLASS,
                    (Tags.GA_Left, titleLeft),
                    (Tags.GA_Top, 0),
                    (Tags.GA_RelWidth, -(titleRight + titleLeft)),
                    (Tags.GA_Height, titleBarHeight),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_RelVerify, false),
                    (Tags.GA_SysGType, GadgetType.WDRAGGING),
                    (Tags.GA_Image, dragImage)
                );
                window.AddGadget(dragGadget, -1);
            }
            if (window.HasFlag(WindowFlags.WFLG_SIZEGADGET))
            {
                var sizeImage = NewObject(SYSICLASS,
                    (Tags.SYSIA_Which, SysImageType.Size)
                );
                Gadget sizeGadget = (Gadget)NewObject(FRBUTTONCLASS,
                    (Tags.GA_RelRight, -17),
                    (Tags.GA_RelBottom, -17),
                    (Tags.GA_Width, 18),
                    (Tags.GA_Height, 18),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_RelVerify, false),
                    (Tags.GA_BottomBorder, (window.Flags & WindowFlags.WFLG_SIZEBBOTTOM) == WindowFlags.WFLG_SIZEBBOTTOM),
                    (Tags.GA_RightBorder, (window.Flags & WindowFlags.WFLG_SIZEBRIGHT) == WindowFlags.WFLG_SIZEBRIGHT),
                    (Tags.GA_SysGType, GadgetType.SIZING),
                    (Tags.GA_Image, sizeImage)
                );
                window.AddGadget(sizeGadget, -1);
            }
        }

        private static void CheckRectFill(IGraphics rport, int left, int top, int right, int bottom, Color c)
        {
            int width = right - left + 1;
            int height = bottom - top + 1;
            if (width > 0 && height > 0)
            {
                rport.FillRectangle(left, top, width, height, c);
            }
        }

        private static void RenderWindow(Window window, IGraphics graphics)
        {
            graphics.SetTarget(window.Bitmap);
            RenderWindowBorder(graphics, window);
            RenderBorderGadgets(graphics, window);
            RenderWindowTitle(graphics, window);
            graphics.SetClip(window.BorderLeft, window.BorderTop, window.InnerWidth, window.InnerHeight);
            RenderInnerGadgets(graphics, window);
            graphics.ClearClip();
            RenderRequesters(graphics, window);
            graphics.ClearTarget();
            window.Validate();
        }

        private static void RenderRequesters(IGraphics graphics, Window window)
        {
            foreach (Requester req in window.Requesters)
            {
                RenderRequest(graphics, window, req);
            }
        }
        private static void RenderSysGadgets(IGraphics graphics, Window window)
        {
            foreach (Gadget gadget in window.Gadgets)
            {
                if ((gadget.GadgetType & GadgetType.SYSGADGET) == GadgetType.SYSGADGET)
                {
                    RenderGadget(window, null, gadget, graphics);
                }
            }
        }

        private static void RenderBorderGadgets(IGraphics graphics, Window window)
        {
            foreach (Gadget gadget in window.Gadgets)
            {
                if ((gadget.Activation & (GadgetActivation.BOTTOMBORDER | GadgetActivation.TOPBORDER | GadgetActivation.LEFTBORDER | GadgetActivation.RIGHTBORDER)) != GadgetActivation.NONE)
                {
                    RenderGadget(window, null, gadget, graphics);
                }
            }
        }

        private static void RenderInnerGadgets(IGraphics graphics, Window window)
        {
            foreach (Gadget gadget in window.Gadgets)
            {
                if ((gadget.Activation & (GadgetActivation.BOTTOMBORDER | GadgetActivation.TOPBORDER | GadgetActivation.LEFTBORDER | GadgetActivation.RIGHTBORDER)) == GadgetActivation.NONE)
                {
                    RenderGadget(window, null, gadget, graphics);
                }
            }
        }

        private static void RenderUserGadgets(IGraphics graphics, Window window)
        {
            foreach (Gadget gadget in window.Gadgets)
            {
                if ((gadget.GadgetType & GadgetType.SYSGADGET) != GadgetType.SYSGADGET)
                {
                    RenderGadget(window, null, gadget, graphics);
                }
            }
        }

        private static void RenderWindowTitle(IGraphics graphics, Window window)
        {
            if (window.Title != null)
            {
                int titleX = window.BorderLeft;
                int titleY = window.BorderTop / 2;
                if ((window.Flags & WindowFlags.WFLG_CLOSEGADGET) == WindowFlags.WFLG_CLOSEGADGET)
                {
                    titleX += 20;
                }
                graphics.RenderText(window.Title, titleX, titleY, Color.Black, HorizontalTextAlign.Left, VerticalTextAlign.Center);
            }
        }

        private static void RenderWindowBorder(IGraphics graphics, Window window)
        {
            bool active = window.HasFlag(WindowFlags.WFLG_WINDOWACTIVE);
            Color backColor = active ? colorActive : colorInactive;
            Color borderColorTopLeft = colorLightEdge;
            Color borderColorBottomRight = colorDarkEdge;
            int x = 0;
            int y = 0;
            int w = window.Width;
            int h = window.Height;
            graphics.FillRectangle(x, y, w, h, window.BgColor);
            if (window.BorderTop > 0)
            {
                CheckRectFill(graphics, 0, 0, w - 1, 0, borderColorTopLeft);
            }
            if (window.BorderLeft > 0)
            {
                CheckRectFill(graphics, 0, 0, 0, h - 1, borderColorTopLeft);
            }
            if (window.BorderRight > 1)
            {
                CheckRectFill(graphics, w - window.BorderRight, window.BorderTop, w - window.BorderRight, h - window.BorderBottom, borderColorTopLeft);
            }
            if (window.BorderBottom > 1)
            {
                CheckRectFill(graphics, window.BorderLeft, h - window.BorderBottom, w - window.BorderRight, h - window.BorderBottom, borderColorTopLeft);
            }
            if (window.BorderRight > 0)
            {
                CheckRectFill(graphics, w - 1, 1, w - 1, h - 1, borderColorBottomRight);
            }
            if (window.BorderBottom > 0)
            {
                CheckRectFill(graphics, 1, h - 1, w - 1, h - 1, borderColorBottomRight);
            }
            if (window.BorderLeft > 1)
            {
                CheckRectFill(graphics, window.BorderLeft - 1, window.BorderTop - 1, window.BorderLeft - 1, h - window.BorderBottom, borderColorBottomRight);
            }
            if (window.BorderTop > 1)
            {
                CheckRectFill(graphics, window.BorderLeft - 1, window.BorderTop - 1, w - window.BorderRight, window.BorderTop - 1, borderColorBottomRight);
            }
            if (window.BorderTop > 2)
            {
                CheckRectFill(graphics, 1, 1, w - 2, window.BorderTop - 2, backColor);
            }
            if (window.BorderLeft > 2)
            {
                CheckRectFill(graphics, 1, 1, window.BorderLeft - 2, h - 2, backColor);
            }
            if (window.BorderRight > 2)
            {
                CheckRectFill(graphics, w - window.BorderRight + 1, 1, w - 2, h - 2, backColor);
            }
            if (window.BorderBottom > 2)
            {
                CheckRectFill(graphics, 1, h - window.BorderBottom + 1, w - 2, h - 2, backColor);
            }
        }

        private static void RenderRequest(IGraphics graphics, Window window, Requester req)
        {
            if (req != null)
            {
                int x = 0;
                int y = 0;
                int w = window.Width;
                int h = window.Height;
                x += req.LeftEdge;
                y += req.TopEdge;
                w = req.Width;
                h = req.Height;
                graphics.FillRectangle(x, y, w, h, req.BackFill);
                DrawBorder(graphics, req.ReqBorder, x, y);
                DrawIntuiText(graphics, req.ReqText, x, y);
                foreach (var gad in req.ReqGadgets)
                {
                    RenderGadget(window, req, gad, graphics);
                }
            }
        }

        public static IBox GetGadgetDomain(Gadget gadget, IScreen screen, Window window, Requester req, ref IBox box)
        {
            if (box == null) box = new Box();
            if (window != null)
            {
                box.LeftEdge = 0;
                box.TopEdge = 0;
                box.Width = window.Width;
                box.Height = window.Height;
                if (gadget != null)
                {
                    switch (gadget.GadgetType & (GadgetType.GADGETTYPE & ~GadgetType.SYSGADGET))
                    {
                        case GadgetType.SCRGADGET:
                            box.LeftEdge = 0;
                            box.TopEdge = 0;
                            box.Width = screen.Width;
                            box.Height = screen.Height;
                            break;
                        case GadgetType.GZZGADGET:
                            box.LeftEdge = 0;
                            box.TopEdge = 0;
                            box.Width = window.Width;
                            box.Height = window.Height;
                            break;
                        case GadgetType.REQGADGET:
                            //box.LeftEdge = req.LeftEdge + window.BorderLeft;
                            //box.TopEdge = req.TopEdge + window.BorderTop;
                            box.LeftEdge = req.LeftEdge;
                            box.TopEdge = req.TopEdge;
                            box.Width = req.Width;
                            box.Height = req.Height;
                            break;
                        default:
                            if ((window.Flags & WindowFlags.WFLG_GIMMEZEROZERO) == WindowFlags.WFLG_GIMMEZEROZERO)
                            {
                                box.LeftEdge = window.BorderLeft;
                                box.TopEdge = window.BorderTop;
                                box.Width = window.Width - window.BorderLeft - window.BorderRight;
                                box.Height = window.Height - window.BorderTop - window.BorderBottom;
                            }
                            break;
                    }
                }
            }
            return box;
        }

        internal static GadgetInfo SetupGInfo(Window window, Requester req, Gadget gadget, IGraphics graphics)
        {
            IBox box = null;
            IScreen screen = window?.Screen ?? Screen;
            GadgetInfo gi = new GadgetInfo()
            {
                Screen = screen,
                Window = window,
                Requester = req,
                RastPort = graphics,
                Domain = GetGadgetDomain(gadget, screen, window, req, ref box),
                DrawInfo = screen.GetDrawInfo()
            };
            return gi;
        }

        private static void RefreshBoopsiGadget(Gadget gadget, Window window, Requester req, IGraphics graphics)
        {
            GadgetInfo gi = SetupGInfo(window, req, gadget, graphics);
            gadget.Render(gi, graphics, GadgetRedraw.Redraw);
        }

        private static void RenderGadget(Window window, Requester req, Gadget gadget, IGraphics graphics)
        {
            RefreshBoopsiGadget(gadget, window, req, graphics);
            //if ((gadget.Flags & GadgetFlags.BOOPSIGADGET) == GadgetFlags.BOOPSIGADGET)
            //{
            //    RefreshBoopsiGadget(gadget, window, req, graphics);
            //    return;
            //}
            //bool active = window.HasFlag(WindowFlags.WFLG_WINDOWACTIVE);
            //bool selected = gadget.Selected;
            //bool borderGadget = (gadget.Activation & (GadgetActivation.BOTTOMBORDER | GadgetActivation.LEFTBORDER | GadgetActivation.RIGHTBORDER | GadgetActivation.TOPBORDER)) != 0;
            //IBox box = null;
            //IScreen screen = window?.Screen;
            //gadget.GetWinGadgetIBox(screen, window, req, ref box);
            //int x = box.LeftEdge;
            //int y = box.TopEdge;
            //int w = box.Width;
            //int h = box.Height;
            //Color backColor = active ? colorActive : colorInactive;
            //Color borderColorTopLeft = selected ? colorDarkEdge : colorLightEdge;
            //Color borderColorBottomRight = selected ? colorLightEdge : colorDarkEdge;
            //Color bgColor = borderGadget ? backColor : gadget.BgColor;
            //switch (gadget.GadgetType & GadgetType.GTYPEMASK)
            //{
            //    case GadgetType.BOOLGADGET:
            //        graphics.FillRectangle(x, y, w, h, bgColor);
            //        DrawImage(graphics, gadget.Image, x, y);
            //        DrawBorder(graphics, gadget.Border, x, y);
            //        DrawIntuiText(graphics, gadget.GadgetText, x, y, x + 1, y + 1, w - 2, h - 2);
            //        break;
            //    case GadgetType.STRGADGET:
            //        graphics.FillRectangle(x, y, w, h, colorWindow);
            //        graphics.DrawLine(x, y + h, x + w, y + h, colorLightEdge);
            //        graphics.DrawLine(x + w, y, x + w, y + h, colorLightEdge);
            //        graphics.DrawRectangle(x + 1, y + 1, w - 2, h - 2, Color.Black);
            //        int txtX = x + 4;
            //        int txtY = y + 3;
            //        gadget.StringInfo.CLeft = txtX;
            //        gadget.StringInfo.CTop = txtY;
            //        graphics.SetClip(x + 1, y + 1, w - 2, h - 2);
            //        graphics.RenderText(gadget.StringInfo.Buffer, txtX, txtY, colorText, HorizontalTextAlign.Left, VerticalTextAlign.Top);
            //        graphics.ClearClip();
            //        if (cursorBlink && gadget == activeGadget)
            //        {
            //            DrawCursor(graphics, gadget);
            //        }
            //        DrawIntuiText(graphics, gadget.GadgetText, x, y);
            //        break;
            //    case GadgetType.PROPGADGET:
            //        PropGadget.RefreshPropGadget(gadget, window, req, graphics);
            //        break;

            //}
            //if ((gadget.Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED)
            //{
            //    DrawDisabled(graphics, x, y, w, h);
            //}
        }

        private static void DrawDisabled(IGraphics graphics, int x, int y, int w, int h)
        {
            Color color = new Color(128, 128, 128, 128);
            graphics.FillRectangle(x, y, w, h, color);
        }
        private static bool FindInput(InputEvent ie, out IScreen screen, out Window win, out Requester req, out Gadget gad)
        {
            win = null;
            req = null;
            gad = null;
            IBox box = new Box();
            screen = Screen;
            GadgetInfo gi = new GadgetInfo()
            {
                Screen = screen
            };
            if (screen != null && ie != null)
            {
                int x = ie.X;
                int y = ie.Y;
                foreach (var w in screen.Windows)
                {
                    if ((w.LeftEdge <= x) && (w.TopEdge <= y) && (w.LeftEdge + w.Width > x) && (w.TopEdge + w.Height > y))
                    {
                        win = w;
                    }
                }
                if (win != null)
                {
                    gi.Window = win;
                    int wx = x - win.LeftEdge;
                    int wy = y - win.TopEdge;
                    foreach (var r in win.Requesters)
                    {
                        if ((r.LeftEdge <= wx) && (r.TopEdge <= wy) && (r.LeftEdge + r.Width > wx) && (r.TopEdge + r.Height > wy))
                        {
                            req = r;
                        }
                    }
                    if (req != null)
                    {
                        gi.Requester = req;
                        int rx = wx - req.LeftEdge;
                        int ry = wy - req.TopEdge;

                        foreach (var g in req.ReqGadgets)
                        {
                            g.GetScrGadgetIBox(screen, win, req, ref box);
                            if (box.ContainsPoint(x, y))
                            {
                                if (g.HitTest(gi, rx, ry) == HitTestResult.GadgetHit)
                                {
                                    gad = g;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var g in win.Gadgets)
                        {
                            g.GetScrGadgetIBox(screen, win, req, ref box);
                            if (box.ContainsPoint(x, y))
                            {
                                if (g.HitTest(gi, wx, wy) == HitTestResult.GadgetHit)
                                {
                                    gad = g;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private static IBox GetKnobDimensions(Gadget gadget)
        {
            IBox knob = new Box();
            if (gadget != null)
            {
                IBox box = null;
                Window window = gadget.Window;
                IScreen screen = window?.Screen;
                Requester req = gadget.Requester;
                gadget.GetWinGadgetIBox(screen, window, req, ref box);
                PropGadget.CalcKnobSize(gadget, box, ref knob);
            }
            return knob;
        }

        private static void ModifyFlag(NewWindow newWindow, ValueTuple<Tags, object> tag, WindowFlags flag)
        {
            if ((bool)tag.Item2) { newWindow.Flags |= flag; } else { newWindow.Flags &= ~flag; }
        }

    }
}
