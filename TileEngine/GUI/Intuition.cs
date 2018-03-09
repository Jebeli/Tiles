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
    using TileEngine.Screens;

    public static class Intuition
    {

        private static Engine engine;
        private static IScreen Screen { get { return engine.Screen; } }
        private static Window activeWindow;
        private static Gadget activeGadget;
        private static Gadget selectedGadget;
        private static int mouseStartX;
        private static int mouseStartY;
        private static Color colorText = Color.Black;
        private static Color colorActive = new Color(62, 92, 154);
        private static Color colorInactive = Color.Gray;
        private static Color colorDarkEdge = Color.Black;
        private static Color colorLightEdge = Color.White;
        private static Color colorWindow = Color.DimGray;
        private static TimeSpan blinkDuration = TimeSpan.FromMilliseconds(666);
        private static TimeSpan lastBlink;
        private static bool cursorBlink;
        private static Image sizeNormal;
        private static Image sizeSelected;
        private static Image closeNormal;
        private static Image closeSelected;
        private static Image depthNormal;
        private static Image depthSelected;
        private static Image zoomNormal;
        private static Image zoomSelected;
        private static Image titleNormal;
        private static Image titleSelected;
        private static NinePatch button9P;
        private static NinePatch button9PS;
        private static AutoRequestPositionMode autoRequestPositionMode;

        public static event EventHandler<IntuiMessage> Message;

        private static void OnMessage(Gadget gadget, IDCMPFlags msg, int code = 0)
        {
            OnMessage(gadget.Window, new IntuiMessage(msg, gadget, code));
        }

        private static void OnMessage(Window window, IDCMPFlags msg, int code = 0)
        {
            if ((window.IDCMPFlags & msg) == msg)
            {
                OnMessage(window, new IntuiMessage(msg, null, code));
            }
        }
        private static void OnMessage(Window window, IntuiMessage msg)
        {
            if ((window.IDCMPFlags & msg.Message) == msg.Message)
            {
                Message?.Invoke(window, msg);
            }
        }

        public static AutoRequestPositionMode AutoRequestPositionMode
        {
            get { return autoRequestPositionMode; }
            set { autoRequestPositionMode = value; }
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

        public static void Init(Engine engine)
        {
            Intuition.engine = engine;
            Texture sizeTex = engine.GetTexture("gui/Size.png");
            sizeNormal = new Image(sizeTex.GetRegion(0, 0, sizeTex.Width / 2, sizeTex.Height));
            sizeSelected = new Image(sizeTex.GetRegion(sizeTex.Width / 2, 0, sizeTex.Width / 2, sizeTex.Height));
            Texture closeTex = engine.GetTexture("gui/Close.png");
            closeNormal = new Image(closeTex.GetRegion(0, 0, closeTex.Width / 2, closeTex.Height));
            closeSelected = new Image(closeTex.GetRegion(closeTex.Width / 2, 0, closeTex.Width / 2, closeTex.Height));
            Texture depthTex = engine.GetTexture("gui/Depth.png");
            depthNormal = new Image(depthTex.GetRegion(0, 0, depthTex.Width / 2, depthTex.Height));
            depthSelected = new Image(depthTex.GetRegion(depthTex.Width / 2, 0, depthTex.Width / 2, depthTex.Height));
            Texture zoomTex = engine.GetTexture("gui/Zoom.png");
            zoomNormal = new Image(zoomTex.GetRegion(0, 0, zoomTex.Width / 2, zoomTex.Height));
            zoomSelected = new Image(zoomTex.GetRegion(zoomTex.Width / 2, 0, zoomTex.Width / 2, zoomTex.Height));
            Texture titleTex = engine.GetTexture("gui/Title.png");
            NinePatch tN9P = new NinePatch(titleTex, 3, 3, 3, 3);
            titleNormal = new Image(tN9P);
            Texture titleSTex = engine.GetTexture("gui/TitleS.png");
            NinePatch tS9P = new NinePatch(titleSTex, 3, 3, 3, 3);
            titleSelected = new Image(tS9P);
            Texture buttonTex = engine.GetTexture("gui/Button.png");
            button9P = new NinePatch(buttonTex, 3, 3, 3, 3);
            Texture buttonSTex = engine.GetTexture("gui/ButtonS.png");
            button9PS = new NinePatch(buttonSTex, 3, 3, 3, 3);
        }

        internal static void InitGadget(Gadget gadget)
        {
            switch (gadget.GadgetType & ~GadgetType.GADGETTYPE)
            {
                case GadgetType.BOOLGADGET:
                    gadget.GadgetImage = new Image(button9P);
                    gadget.GadgetImage.Width = Math.Abs(gadget.Width);
                    gadget.GadgetImage.Height = Math.Abs(gadget.Height);
                    gadget.SelectImage = new Image(button9PS);
                    gadget.SelectImage.Width = Math.Abs(gadget.Width);
                    gadget.SelectImage.Height = Math.Abs(gadget.Height);
                    break;
                case GadgetType.PROPGADGET:
                    break;
            }
            if (gadget.Width <= 0)
            {
                gadget.Flags |= GadgetFlags.GRELWIDTH;
            }
            if (gadget.Height <= 0)
            {
                gadget.Flags |= GadgetFlags.GRELHEIGHT;
            }
        }

        public static Window OpenWindowTags(NewWindow newWindow, params ValueTuple<WATags, object>[] tags)
        {
            if (newWindow == null) newWindow = new NewWindow();
            int opacity = 255;
            Color bgColor = colorWindow;
            Color fgColor = colorText;
            if (tags != null)
            {
                newWindow.Flags |= ((WindowFlags)tags.GetTagData(WATags.WA_Flags) & ~WindowFlags.WFLG_PRIVATEFLAGS);
                foreach (var tag in tags)
                {
                    switch (tag.Item1)
                    {
                        case WATags.WA_Left:
                            newWindow.LeftEdge = (int)tag.Item2;
                            break;
                        case WATags.WA_Top:
                            newWindow.TopEdge = (int)tag.Item2;
                            break;
                        case WATags.WA_Width:
                            newWindow.Width = (int)tag.Item2;
                            break;
                        case WATags.WA_Height:
                            newWindow.Height = (int)tag.Item2;
                            break;
                        case WATags.WA_IDCMP:
                            newWindow.IDCMPFlags = (IDCMPFlags)tag.Item2;
                            break;
                        case WATags.WA_MinWidth:
                            newWindow.MinWidth = (int)tag.Item2;
                            break;
                        case WATags.WA_MinHeight:
                            newWindow.MinHeight = (int)tag.Item2;
                            break;
                        case WATags.WA_MaxWidth:
                            newWindow.MaxWidth = (int)tag.Item2;
                            break;
                        case WATags.WA_MaxHeight:
                            newWindow.MaxHeight = (int)tag.Item2;
                            break;
                        case WATags.WA_Gadgets:
                            newWindow.Gadgets = (IList<Gadget>)tag.Item2;
                            break;
                        case WATags.WA_Title:
                            newWindow.Title = (string)tag.Item2;
                            break;
                        case WATags.WA_Screen:
                            newWindow.Screen = (IScreen)tag.Item2;
                            break;
                        case WATags.WA_SizeGadget:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_SIZEGADGET);
                            break;
                        case WATags.WA_DragBar:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_DRAGBAR);
                            break;
                        case WATags.WA_DepthGadget:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_DEPTHGADGET);
                            break;
                        case WATags.WA_CloseGadget:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_CLOSEGADGET);
                            break;
                        case WATags.WA_Backdrop:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_BACKDROP);
                            break;
                        case WATags.WA_ReportMouse:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_REPORTMOUSE);
                            break;
                        case WATags.WA_NoCareRefresh:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_NOCAREREFRESH);
                            break;
                        case WATags.WA_Borderless:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_BORDERLESS);
                            break;
                        case WATags.WA_Activate:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_ACTIVATE);
                            break;
                        case WATags.WA_RMBTrap:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_RMBTRAP);
                            break;
                        case WATags.WA_WBenchWindow:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_WBENCHWINDOW);
                            break;
                        case WATags.WA_SizeBRight:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_SIZEBRIGHT);
                            break;
                        case WATags.WA_SizeBBottom:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_SIZEBBOTTOM);
                            break;
                        case WATags.WA_GimmeZeroZero:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_GIMMEZEROZERO);
                            break;
                        case WATags.WA_NewLookMenus:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_NEWLOOKMENUS);
                            break;
                        case WATags.WA_ToolBox:
                            ModifyFlag(newWindow, tag, WindowFlags.WFLG_TOOLBOX);
                            break;
                        case WATags.WA_Opacity:
                            opacity = (int)tag.Item2;
                            break;
                        case WATags.WA_BackgroundColor:
                            bgColor = (Color)tag.Item2;
                            break;
                        case WATags.WA_ForegroundColor:
                            fgColor = (Color)tag.Item2;
                            break;
                        case WATags.WA_Zoom:

                            break;
                        case WATags.WA_SimpleRefresh:
                            if ((bool)tag.Item2)
                            {
                                newWindow.Flags &= ~WindowFlags.WFLG_REFRESHBITS;
                                newWindow.Flags |= WindowFlags.WFLG_SIMPLE_REFRESH;
                            }
                            break;
                        case WATags.WA_SmartRefresh:
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
            window.BgColor = bgColor;
            window.FgColor = fgColor;
            window.Screen.Windows.Add(window);
            AddSysGadgets(window);
            AddGList(window, newWindow.Gadgets, -1, -1);
            if (window.HasFlag(WindowFlags.WFLG_ACTIVATE))
            {
                ActivateWindow(window);
            }
            return window;
        }
        public static Window OpenWindow(NewWindow newWindow)
        {
            return OpenWindowTags(newWindow, null);
        }

        public static void CloseWindow(Window window)
        {
            window.Screen.Windows.Remove(window);
            window.Bitmap?.Dispose();
        }

        public static void Update(TimeInfo time)
        {
            var timeDiff = time.TotalGameTime - lastBlink;
            if (timeDiff > blinkDuration)
            {
                lastBlink = time.TotalGameTime;
                cursorBlink = !cursorBlink;
                if (activeWindow != null && activeGadget != null)
                {
                    activeWindow.Invalidate();
                }
            }
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

        private static void RenderWindowBitmap(Window window, IGraphics graphics)
        {
            graphics.Render(window.Bitmap, window.LeftEdge, window.TopEdge, 255 - window.Opacity);
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
                    activeGadget = gadget;
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
            IList<Window> windows = window.Screen.Windows;
            windows.Remove(window);
            for (int i = 0; i < windows.Count; i++)
            {
                if (!windows[i].HasFlag(WindowFlags.WFLG_BACKDROP))
                {
                    windows.Insert(i, window);
                    break;
                }
            }
        }

        public static void WindowToFront(Window window)
        {
            IList<Window> windows = window.Screen.Windows;
            windows.Remove(window);
            windows.Add(window);
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
            Gadget posGad = Gadget.MakeBoolGadget(posText, width / 4, height / 4);
            posGad.Activation |= GadgetActivation.ENDGADGET | GadgetActivation.TOGGLESELECT;
            posGad.SetPosition(5, 2 * height / 3);
            posGad.GadgetId = 1;
            Gadget negGad = Gadget.MakeBoolGadget(negText, width / 4, height / 4, GadgetFlags.GRELRIGHT);
            negGad.Activation |= GadgetActivation.ENDGADGET | GadgetActivation.TOGGLESELECT;
            negGad.SetPosition(-(5 + width / 4), 2 * height / 3);
            negGad.GadgetId = 0;
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
            Window reqWin = OpenWindowTags(null,
                T(WATags.WA_Left, x),
                T(WATags.WA_Top, y),
                T(WATags.WA_Width, width + 2 * 4),
                T(WATags.WA_Height, height + 2 * 4 + 16),
                T(WATags.WA_Flags, WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_ACTIVATE),
                T(WATags.WA_IDCMP, IDCMPFlags.GADGETUP),
                T(WATags.WA_Title, title),
                T(WATags.WA_Screen, screen)
                );
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
            req.ReqGadgets = new[] { posGad, negGad };
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
            foreach (var fs in formats)
            {
                object arg = null;
                if (args.Length > count) arg = args[1 + count];
                string gadText = string.Format(fs, arg);
                Gadget gad = Gadget.MakeBoolGadget(gadText, gadWidth, gadHeight);
                gad.Activation |= GadgetActivation.ENDGADGET | GadgetActivation.TOGGLESELECT;
                gad.GadgetId = count < numGads ? count + 1 : 0;
                gad.LeftEdge = posX;
                gad.TopEdge = height - gadHeight - 8;
                gadgets.Add(gad);
                posX += gadWidth;
                posX += 4;
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
            Window reqWin = OpenWindowTags(null,
                T(WATags.WA_Left, x),
                T(WATags.WA_Top, y),
                T(WATags.WA_Width, width + 2 * 4),
                T(WATags.WA_Height, height + 2 * 4 + 16),
                T(WATags.WA_Flags, WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_ACTIVATE),
                T(WATags.WA_IDCMP, IDCMPFlags.GADGETUP),
                T(WATags.WA_Title, title),
                T(WATags.WA_Screen, screen)
                );
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
            Request(req, reqWin);

            return null;
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

        public static void KeyDown(Key key, char code)
        {

        }

        public static void KeyUp(Key key, char code)
        {
            if (activeGadget != null)
            {
                Gadget gadget = activeGadget;
                if ((gadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.STRGADGET)
                {
                    string str = gadget.StringInfo.Buffer;
                    int pos = gadget.StringInfo.BufferPos;
                    switch (key)
                    {
                        case Key.Enter:
                            OnMessage(gadget, IDCMPFlags.GADGETUP);
                            UnselectGadget(gadget);
                            activeGadget = null;
                            break;
                        case Key.Left:
                            gadget.StringInfo.BufferPos--;
                            break;
                        case Key.Right:
                            gadget.StringInfo.BufferPos++;
                            break;
                        case Key.Delete:
                            if (pos < str.Length)
                            {
                                str = str.Remove(pos, 1);
                                gadget.StringInfo.Buffer = str;
                            }
                            break;
                        case Key.Back:
                            if (pos > 0)
                            {
                                str = str.Remove(pos - 1, 1);
                                gadget.StringInfo.Buffer = str;
                                gadget.StringInfo.BufferPos--;
                            }
                            break;
                        default:
                            if (!char.IsControl(code) || key == Key.Space)
                            {

                                str = str.Insert(pos, "" + code);
                                if (str.Length < gadget.StringInfo.MaxChars)
                                {
                                    gadget.StringInfo.Buffer = str;
                                    gadget.StringInfo.BufferPos++;
                                }
                            }
                            break;
                    }
                }
                gadget.Window.Invalidate();
            }
        }

        public static void MouseMove(int x, int y, MouseButton button)
        {
            if (selectedGadget != null)
            {
                Gadget gadget = selectedGadget;
                Window window = gadget.Window;
                int deltaX = x - mouseStartX;
                int deltaY = y - mouseStartY;
                mouseStartX = x;
                mouseStartY = y;
                switch (gadget.GadgetType & ~GadgetType.GADGETTYPE)
                {
                    case GadgetType.SIZING:
                        SizeWindow(window, deltaX, deltaY);
                        break;
                    case GadgetType.WDRAGGING:
                        MoveWindow(window, deltaX, deltaY);
                        break;
                    case GadgetType.PROPGADGET:
                        if ((gadget.PropInfo.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ)
                        {
                            int posX = x - gadget.PropInfo.LeftBorder;
                            posX *= PropInfo.MAXPOT;
                            posX /= gadget.PropInfo.CWidth;
                            gadget.PropInfo.HorizPot = posX;
                        }
                        if ((gadget.PropInfo.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT)
                        {
                            int posY = y - gadget.PropInfo.TopBorder;
                            posY *= PropInfo.MAXPOT;
                            posY /= gadget.PropInfo.CHeight;
                            gadget.PropInfo.VertPot = posY;
                        }
                        break;
                }
                window.Invalidate();
            }
            else
            {
                Window window = GetMouseWindow(x, y);
                Requester req = GetMouseRequester(window, x, y);
                Gadget gadget = GetMouseGadget(window, req, x, y);
            }
        }
        public static void MouseDown(int x, int y, MouseButton button)
        {
            Window window = GetMouseWindow(x, y);
            Requester req = GetMouseRequester(window, x, y);
            Gadget gadget = GetMouseGadget(window, req, x, y);
            mouseStartX = x;
            mouseStartY = y;
            ActivateWindow(window);
            if (gadget != null)
            {
                if ((gadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.PROPGADGET)
                {
                    int kx;
                    int ky;
                    int kw;
                    int kh;

                    GetKnobDimensions(window, req, gadget, out kx, out ky, out kw, out kh);
                    if (x >= kx && y >= ky && x <= kx + kw && y <= ky + kh)
                    {
                        gadget.PropInfo.Flags |= PropFlags.KNOBHIT;
                    }
                    else
                    {
                        gadget.PropInfo.Flags &= ~PropFlags.KNOBHIT;
                    }
                }
                if ((gadget.Activation & GadgetActivation.GADGIMMEDIATE) == GadgetActivation.GADGIMMEDIATE)
                {
                    OnMessage(gadget, IDCMPFlags.GADGETDOWN);
                }
            }
            SelectGadget(gadget);
        }
        public static void MouseUp(int x, int y, MouseButton button)
        {
            Window window = GetMouseWindow(x, y);
            Requester req = GetMouseRequester(window, x, y);
            Gadget gadget = GetMouseGadget(window, req, x, y);
            if (activeGadget != null)
            {
                if ((activeGadget.Activation & GadgetActivation.RELVERIFY) == GadgetActivation.RELVERIFY)
                {
                    if (activeGadget != gadget)
                    {
                        UnselectGadget(activeGadget);
                        return;
                    }
                }
                Window activeWindow = activeGadget.Window;
                if (activeWindow == null) { activeWindow = window; }
                IList<Window> windows = activeWindow.Screen.Windows;
                if ((activeGadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.WDEPTH)
                {
                    if (windows.IndexOf(activeWindow) >= windows.Count - 1)
                    {
                        WindowToBack(activeWindow);
                    }
                    else
                    {
                        WindowToFront(activeWindow);
                    }
                }
                else if ((activeGadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.WZOOM)
                {
                    activeWindow.Zip();
                }
                else if ((activeGadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.CLOSE)
                {
                    OnMessage(activeGadget, IDCMPFlags.CLOSEWINDOW);
                }
                else if ((activeGadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.PROPGADGET)
                {
                    int kx;
                    int ky;
                    int kw;
                    int kh;

                    GetKnobDimensions(activeWindow, req, activeGadget, out kx, out ky, out kw, out kh);
                    if (x >= kx && y >= ky && x <= kx + kw && y <= ky + kh)
                    {
                        activeGadget.PropInfo.Flags |= PropFlags.KNOBHIT;
                    }
                    else
                    {
                        activeGadget.PropInfo.Flags &= ~PropFlags.KNOBHIT;
                        if (x < kx && ((activeGadget.PropInfo.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ))
                        {
                            activeGadget.PropInfo.HorizPot -= activeGadget.PropInfo.HorizBody;

                        }
                        if (x > kx + kw && ((activeGadget.PropInfo.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ))
                        {
                            activeGadget.PropInfo.HorizPot += activeGadget.PropInfo.HorizBody;
                        }
                        if (y < ky && ((activeGadget.PropInfo.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT))
                        {
                            activeGadget.PropInfo.VertPot -= activeGadget.PropInfo.VertBody;

                        }
                        if (y > ky + kh && ((activeGadget.PropInfo.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT))
                        {
                            activeGadget.PropInfo.VertPot += activeGadget.PropInfo.VertBody;
                        }
                    }
                }
                if ((activeGadget.Activation & GadgetActivation.RELVERIFY) == GadgetActivation.RELVERIFY)
                {
                    OnMessage(activeGadget, IDCMPFlags.GADGETUP);
                }
                if ((activeGadget.Activation & GadgetActivation.ENDGADGET) == GadgetActivation.ENDGADGET)
                {
                    InternalEndRequest(req, window, activeGadget);
                }
                UnselectGadget(activeGadget);
            }
            else
            {
                UnselectGadget(gadget);
            }
        }

        private static void DrawIntuiText(IGraphics graphics, IntuiText text, int x, int y)
        {
            while (text != null)
            {
                graphics.RenderText(text.IText, x + text.LeftEdge, y + text.TopEdge, text.FrontPen, text.HorizontalTextAlign, text.VerticalTextAlign);
                text = text.NextText;
            }
        }

        private static void DrawBorder(IGraphics graphics, Border border, int x, int y)
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

        private static void SelectGadget(Gadget gadget)
        {
            if (gadget != null)
            {
                gadget.Selected = true;
                gadget.Window.Invalidate();
            }
            activeGadget = gadget;
            selectedGadget = gadget;
        }

        private static void UnselectGadget(Gadget gadget)
        {
            if (gadget != null)
            {
                gadget.Selected = false;
                gadget.Window.Invalidate();
            }
            selectedGadget = null;
        }

        private static void AddSysGadgets(Window window)
        {
            int titleLeft = 0;
            int titleRight = 0;
            if (window.HasFlag(WindowFlags.WFLG_CLOSEGADGET))
            {
                Gadget closeGadget = new Gadget();
                closeGadget.LeftEdge = 0;
                closeGadget.TopEdge = 0;
                closeGadget.Width = closeNormal.Width;
                closeGadget.Height = closeNormal.Height;
                closeGadget.Flags = GadgetFlags.GADGIMAGE;
                closeGadget.Activation = GadgetActivation.RELVERIFY;
                closeGadget.GadgetType = GadgetType.SYSGADGET | GadgetType.CLOSE;
                closeGadget.GadgetImage = closeNormal;
                closeGadget.SelectImage = closeSelected;
                window.AddGadget(closeGadget, -1);
                titleLeft += closeGadget.Width;
            }
            if (window.HasFlag(WindowFlags.WFLG_DEPTHGADGET))
            {
                Gadget depthGadget = new Gadget();
                depthGadget.LeftEdge = -depthNormal.Width;
                depthGadget.TopEdge = 0;
                depthGadget.Width = depthNormal.Width;
                depthGadget.Height = depthNormal.Height;
                depthGadget.Flags = GadgetFlags.GRELRIGHT | GadgetFlags.GADGIMAGE;
                depthGadget.Activation = GadgetActivation.RELVERIFY;
                depthGadget.GadgetType = GadgetType.SYSGADGET | GadgetType.WDEPTH;
                depthGadget.GadgetImage = depthNormal;
                depthGadget.SelectImage = depthSelected;
                window.AddGadget(depthGadget, -1);
                titleRight += depthGadget.Width;
            }
            if (window.HasFlag(WindowFlags.WFLG_HASZOOM))
            {
                Gadget zoomGadget = new Gadget();
                zoomGadget.LeftEdge = -(depthNormal.Width + zoomNormal.Width);
                zoomGadget.TopEdge = 0;
                zoomGadget.Width = zoomNormal.Width;
                zoomGadget.Height = zoomNormal.Height;
                zoomGadget.Flags = GadgetFlags.GRELRIGHT | GadgetFlags.GADGIMAGE;
                zoomGadget.Activation = GadgetActivation.RELVERIFY;
                zoomGadget.GadgetImage = zoomNormal;
                zoomGadget.SelectImage = zoomSelected;
                zoomGadget.GadgetType = GadgetType.SYSGADGET | GadgetType.WZOOM;
                window.AddGadget(zoomGadget, -1);
                titleRight += zoomGadget.Width;
            }
            if (window.HasFlag(WindowFlags.WFLG_DRAGBAR))
            {
                Gadget dragGadget = new Gadget();
                dragGadget.LeftEdge = titleLeft;
                dragGadget.TopEdge = 0;
                dragGadget.Width = -(titleRight);
                dragGadget.Height = titleNormal.Height;
                dragGadget.Flags = GadgetFlags.GRELWIDTH | GadgetFlags.GADGIMAGE;
                dragGadget.GadgetImage = titleNormal;
                dragGadget.SelectImage = titleSelected;
                dragGadget.GadgetType = GadgetType.SYSGADGET | GadgetType.WDRAGGING;
                window.AddGadget(dragGadget, -1);
            }
            if (window.HasFlag(WindowFlags.WFLG_SIZEGADGET))
            {
                Gadget sizeGadget = new Gadget();
                sizeGadget.Width = sizeNormal.Width;
                sizeGadget.Height = sizeNormal.Height;
                sizeGadget.LeftEdge = -sizeNormal.Width;
                sizeGadget.TopEdge = -sizeNormal.Height;
                sizeGadget.Flags = GadgetFlags.GRELRIGHT | GadgetFlags.GRELBOTTOM | GadgetFlags.GADGIMAGE;
                sizeGadget.GadgetType = GadgetType.SYSGADGET | GadgetType.SIZING;
                sizeGadget.GadgetImage = sizeNormal;
                sizeGadget.SelectImage = sizeSelected;
                window.AddGadget(sizeGadget, -1);

            }
        }

        private static void RenderWindow(Window window, IGraphics graphics)
        {
            graphics.SetTarget(window.Bitmap);
            bool active = window.HasFlag(WindowFlags.WFLG_WINDOWACTIVE);
            Color backColor = active ? colorActive : colorInactive;
            Color borderColorTopLeft = colorLightEdge;
            Color borderColorBottomRight = colorDarkEdge;
            //int x = window.LeftEdge;
            //int y = window.TopEdge;
            int x = 0;
            int y = 0;
            int w = window.Width;
            int h = window.Height;
            graphics.FillRectangle(x, y, w, h, window.BgColor);
            if (window.BorderLeft > 0)
            {
                graphics.DrawLine(x, y, x, y + h - 1, borderColorTopLeft);
                for (int i = 1; i < window.BorderLeft - 1; i++)
                {
                    graphics.DrawLine(x + i, y, x + i, y + h - 1, backColor);
                }
                graphics.DrawLine(x + window.BorderLeft - 1, y, x + window.BorderLeft - 1, y + h - 1, borderColorBottomRight);
            }
            if (window.BorderRight > 0)
            {
                graphics.DrawLine(x + w - 1, y, x + w - 1, y + h - 1, borderColorBottomRight);
                for (int i = 1; i < window.BorderRight - 1; i++)
                {
                    graphics.DrawLine(x + w - 1 - i, y, x + w - 1 - i, y + h - 1, backColor);
                }
                graphics.DrawLine(x + w - window.BorderRight, y, x + w - window.BorderRight, y + h - 1, borderColorTopLeft);
            }
            if (window.BorderBottom > 0)
            {
                graphics.DrawLine(x + 1, y + h - 1, x + w - 1, y + h - 1, borderColorBottomRight);
                for (int i = 1; i < window.BorderBottom - 1; i++)
                {
                    graphics.DrawLine(x + 1, y + h - i - 1, x + w - 1, y + h - i - 1, backColor);
                }
                graphics.DrawLine(x + window.BorderLeft, y + h - window.BorderBottom, x + w - window.BorderRight - 1, y + h - window.BorderBottom, borderColorTopLeft);
            }
            if (window.BorderTop > 0)
            {
                graphics.DrawLine(x + 1, y, x + w - 1, y, borderColorTopLeft);
                for (int i = 1; i < window.BorderTop - 1; i++)
                {
                    graphics.DrawLine(x + 1, y + i, x + w - 1, y + i, backColor);
                }
                graphics.DrawLine(x + 1, y + window.BorderTop - 1, x + w - 1, y + window.BorderTop - 1, borderColorBottomRight);
            }

            foreach (Gadget gadget in window.Gadgets)
            {
                RenderGadget(window, null, gadget, graphics);
            }
            if (window.Title != null)
            {
                int titleX = window.BorderLeft;
                if ((window.Flags & WindowFlags.WFLG_CLOSEGADGET) == WindowFlags.WFLG_CLOSEGADGET)
                {
                    titleX += closeNormal.Width;
                }
                graphics.RenderText(window.Title, x + titleX, y + titleNormal.Height / 2, Color.Black, HorizontalTextAlign.Left, VerticalTextAlign.Center);
            }
            foreach (Requester req in window.Requesters)
            {
                RenderRequest(graphics, window, req);
            }
            graphics.ClearTarget();
            window.Validate();
        }

        private static void RenderRequest(IGraphics graphics, Window window, Requester req)
        {
            if (req != null)
            {
                //int x = window.LeftEdge;
                //int y = window.TopEdge;
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

        private static void RenderGadget(Window window, Requester req, Gadget gadget, IGraphics graphics)
        {
            bool active = window.HasFlag(WindowFlags.WFLG_WINDOWACTIVE);
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            GetGadgetDimensions(window, req, gadget, out x, out y, out w, out h);
            x -= window.LeftEdge;
            y -= window.TopEdge;
            Color backColor = active ? colorActive : colorInactive;
            Color borderColorTopLeft = colorLightEdge;
            Color borderColorBottomRight = colorDarkEdge;
            bool selected = gadget.Selected;
            if ((gadget.GadgetType & GadgetType.SYSGADGET) == GadgetType.SYSGADGET)
            {
                if (selected)
                {
                    borderColorBottomRight = colorLightEdge;
                    borderColorTopLeft = colorDarkEdge;
                }
                switch (gadget.GadgetType & ~GadgetType.GADGETTYPE)
                {
                    case GadgetType.SIZING:
                    case GadgetType.CLOSE:
                    case GadgetType.WDEPTH:
                    case GadgetType.WZOOM:
                        DrawImage(graphics, gadget.Image, x, y);
                        break;
                }
            }
            switch (gadget.GadgetType & ~GadgetType.GADGETTYPE)
            {
                case GadgetType.PROPGADGET:
                    int kx;
                    int ky;
                    int kw;
                    int kh;

                    GetKnobDimensions(window, req, gadget, out kx, out ky, out kw, out kh);
                    kx -= window.LeftEdge;
                    ky -= window.TopEdge;
                    graphics.FillRectangle(x, y, w, h, colorWindow);
                    if ((gadget.PropInfo.Flags & PropFlags.PROPBORDERLESS) != PropFlags.PROPBORDERLESS)
                    {
                        graphics.DrawLine(x, y, x + w, y, borderColorBottomRight);
                        graphics.DrawLine(x, y, x, y + h, borderColorBottomRight);
                        graphics.DrawLine(x, y + h, x + w, y + h, borderColorTopLeft);
                        graphics.DrawLine(x + w, y, x + w, y + h, borderColorTopLeft);
                    }
                    if ((gadget.PropInfo.Flags & PropFlags.AUTOKNOB) == PropFlags.AUTOKNOB)
                    {
                        graphics.FillRectangle(kx, ky, kw, kh, selected ? colorInactive : colorWindow);
                        graphics.DrawLine(kx, ky, kx + kw, ky, borderColorTopLeft);
                        graphics.DrawLine(kx, ky, kx, ky + kh, borderColorTopLeft);
                        graphics.DrawLine(kx, ky + kh, kx + kw, ky + kh, borderColorBottomRight);
                        graphics.DrawLine(kx + kw, ky, kx + kw, ky + kh, borderColorBottomRight);
                    }
                    else
                    {

                    }
                    break;
                case GadgetType.BOOLGADGET:
                    graphics.FillRectangle(x, y, w, h, gadget.BgColor);
                    DrawImage(graphics, gadget.Image, x, y);
                    DrawIntuiText(graphics, gadget.GadgetText, x, y);
                    break;
                case GadgetType.STRGADGET:
                    graphics.FillRectangle(x, y, w, h, colorWindow);
                    graphics.DrawLine(x, y, x + w, y, borderColorBottomRight);
                    graphics.DrawLine(x, y, x, y + h, borderColorBottomRight);
                    graphics.DrawLine(x, y + h, x + w, y + h, borderColorTopLeft);
                    graphics.DrawLine(x + w, y, x + w, y + h, borderColorTopLeft);
                    int txtX = x + 2;
                    int txtY = y + 2;
                    gadget.StringInfo.CLeft = txtX;
                    gadget.StringInfo.CTop = txtY;
                    graphics.RenderText(gadget.StringInfo.Buffer, txtX, txtY, colorText, HorizontalTextAlign.Left, VerticalTextAlign.Top);
                    if (cursorBlink && gadget == activeGadget)
                    {
                        DrawCursor(graphics, gadget);
                    }
                    break;
            }
            if ((gadget.Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED)
            {
                DrawDisabled(graphics, x, y, w, h);
            }
        }

        private static void DrawDisabled(IGraphics graphics, int x, int y, int w, int h)
        {
            Color color = new Color(128, 128, 128, 128);
            graphics.FillRectangle(x, y, w, h, color);
        }

        private static void DrawCursor(IGraphics graphics, Gadget gadget)
        {
            if (gadget != null && gadget.StringInfo != null)
            {
                int pos = gadget.StringInfo.BufferPos;
                int startPos = gadget.StringInfo.DispPos;
                string part = gadget.StringInfo.Buffer.Substring(startPos, pos - startPos);
                int width = graphics.MeasureTextWidth(part);
                int x = gadget.StringInfo.CLeft + width + 1;
                int y = gadget.StringInfo.CTop + 1;
                graphics.DrawLine(x, y, x, y + 14, colorText);
            }
        }

        private static Gadget GetMouseGadget(Window window, Requester req, int x, int y)
        {
            Gadget gad = null;
            if (window != null)
            {
                if (req != null && window.HasFlag(WindowFlags.WFLG_INREQUEST))
                {
                    foreach (Gadget gadget in req.ReqGadgets)
                    {
                        if ((gadget.Flags & GadgetFlags.GADGDISABLED) != GadgetFlags.GADGDISABLED)
                        {
                            int gx = 0;
                            int gy = 0;
                            int gw = 0;
                            int gh = 0;
                            GetGadgetDimensions(window, req, gadget, out gx, out gy, out gw, out gh);
                            if ((x >= gx) && (y >= gy) && (x <= gx + gw) && (y <= gy + gh))
                            {
                                gad = gadget;
                            }
                        }
                    }
                }
                else if (window.HasFlag(WindowFlags.WFLG_INREQUEST))
                {
                    foreach (Gadget gadget in window.Gadgets)
                    {
                        if (((gadget.Flags & GadgetFlags.GADGDISABLED) != GadgetFlags.GADGDISABLED) &&
                            ((gadget.GadgetType & GadgetType.SYSGADGET) == GadgetType.SYSGADGET))
                        {
                            int gx = 0;
                            int gy = 0;
                            int gw = 0;
                            int gh = 0;
                            GetGadgetDimensions(window, req, gadget, out gx, out gy, out gw, out gh);
                            if ((x >= gx) && (y >= gy) && (x <= gx + gw) && (y <= gy + gh))
                            {
                                gad = gadget;
                            }
                        }
                    }
                }
                else if (!window.HasFlag(WindowFlags.WFLG_INREQUEST))
                {
                    foreach (Gadget gadget in window.Gadgets)
                    {
                        if ((gadget.Flags & GadgetFlags.GADGDISABLED) != GadgetFlags.GADGDISABLED)
                        {
                            int gx = 0;
                            int gy = 0;
                            int gw = 0;
                            int gh = 0;
                            GetGadgetDimensions(window, req, gadget, out gx, out gy, out gw, out gh);
                            if ((x >= gx) && (y >= gy) && (x <= gx + gw) && (y <= gy + gh))
                            {
                                gad = gadget;
                            }
                        }
                    }
                }
            }
            return gad;
        }

        private static Requester GetMouseRequester(Window window, int x, int y)
        {
            Requester req = null;
            if (window != null)
            {
                foreach (Requester requester in window.Requesters)
                {
                    int rx = 0;
                    int ry = 0;
                    int rw = 0;
                    int rh = 0;
                    GetRequesterDimensions(window, requester, out rx, out ry, out rw, out rh);
                    if ((x >= rx) && (y >= ry) && (x <= rx + rw) && (y <= ry + rh))
                    {
                        req = requester;
                    }
                }
            }
            return req;
        }


        private static Window GetMouseWindow(int x, int y)
        {
            Window window = null;
            if (Screen != null)
            {
                foreach (Window w in Screen.Windows)
                {
                    if ((w.LeftEdge <= x) && (w.TopEdge <= y) && (w.LeftEdge + w.Width > x) && (w.TopEdge + w.Height > y))
                    {
                        window = w;
                    }
                }
            }
            return window;
        }

        private static void GetKnobDimensions(Window window, Requester req, Gadget gadget, out int x, out int y, out int w, out int h)
        {
            GetGadgetDimensions(window, req, gadget, out x, out y, out w, out h);
            if ((gadget.GadgetType & ~GadgetType.GADGETTYPE) == GadgetType.PROPGADGET)
            {
                if ((gadget.PropInfo.Flags & PropFlags.PROPBORDERLESS) == PropFlags.PROPBORDERLESS)
                {
                }
                else
                {
                    x++;
                    y++;
                    w -= 2;
                    h -= 2;
                }
                gadget.PropInfo.CWidth = w;
                gadget.PropInfo.CHeight = h;
                gadget.PropInfo.LeftBorder = x;
                gadget.PropInfo.TopBorder = y;
                gadget.PropInfo.HPotRes = PropInfo.MAXPOT / gadget.PropInfo.HorizBody;
                gadget.PropInfo.VPotRes = PropInfo.MAXPOT / gadget.PropInfo.VertBody;
                if ((gadget.PropInfo.Flags & PropFlags.AUTOKNOB) == PropFlags.AUTOKNOB)
                {
                    w = gadget.PropInfo.CWidth * gadget.PropInfo.HorizBody / PropInfo.MAXBODY;
                    h = gadget.PropInfo.CHeight * gadget.PropInfo.VertBody / PropInfo.MAXBODY;
                    x = gadget.PropInfo.LeftBorder + gadget.PropInfo.CWidth * gadget.PropInfo.HorizPot / PropInfo.MAXPOT;
                    y = gadget.PropInfo.TopBorder + gadget.PropInfo.CHeight * gadget.PropInfo.VertPot / PropInfo.MAXPOT;
                    x = Math.Min(x, gadget.PropInfo.LeftBorder + gadget.PropInfo.CWidth - w);
                    y = Math.Min(y, gadget.PropInfo.TopBorder + gadget.PropInfo.CHeight - h);
                }
            }
        }

        private static void GetRequesterDimensions(Window window, Requester req, out int x, out int y, out int w, out int h)
        {
            x = window.LeftEdge + req.LeftEdge;
            y = window.TopEdge + req.TopEdge;
            w = req.Width;
            h = req.Height;
        }

        private static void GetGadgetDimensions(Window window, Requester req, Gadget gadget, out int x, out int y, out int w, out int h)
        {
            x = window.LeftEdge;
            y = window.TopEdge;
            if (req != null)
            {
                x += req.LeftEdge;
                y += req.TopEdge;
            }
            if ((gadget.Flags & GadgetFlags.GRELRIGHT) == GadgetFlags.GRELRIGHT)
            {
                if (req != null)
                {
                    x += (req.Width + gadget.LeftEdge);
                }
                else
                {
                    x += (window.Width + gadget.LeftEdge);
                }
            }
            else
            {
                x += gadget.LeftEdge;
            }
            if ((gadget.Flags & GadgetFlags.GRELBOTTOM) == GadgetFlags.GRELBOTTOM)
            {
                if (req != null)
                {
                    y += (req.Height + gadget.TopEdge);
                }
                else
                {
                    y += (window.Height + gadget.TopEdge);
                }
            }
            else
            {
                y += gadget.TopEdge;
            }
            if ((gadget.Flags & GadgetFlags.GRELWIDTH) == GadgetFlags.GRELWIDTH)
            {
                int right = window.LeftEdge + window.Width + gadget.Width;
                w = right - x;
            }
            else
            {
                w = gadget.Width;
            }
            if ((gadget.Flags & GadgetFlags.GRELHEIGHT) == GadgetFlags.GRELHEIGHT)
            {
                int bottom = window.TopEdge + window.Height + gadget.Height;
                h = bottom - y;// window.Height + gadget.Height;
            }
            else
            {
                h = gadget.Height;
            }
        }

        private static object GetTagData(this ValueTuple<WATags, object>[] tags, WATags tag)
        {
            if (tags != null)
            {
                foreach (var t in tags)
                {
                    if (t.Item1 == tag)
                    {
                        return t.Item2;
                    }
                }
            }
            return 0;
        }
        private static void ModifyFlag(NewWindow newWindow, ValueTuple<WATags, object> tag, WindowFlags flag)
        {
            if ((bool)tag.Item2)
            {
                newWindow.Flags |= flag;
            }
            else
            {
                newWindow.Flags &= ~flag;
            }
        }

        public static ValueTuple<WATags, object> T(this WATags tag, object value)
        {
            return ValueTuple.Create<WATags, object>(tag, value);
        }
    }
}
