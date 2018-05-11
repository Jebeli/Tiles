using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Logging;
using TileEngine.NGUI.SysGadgets;
using TileEngine.Screens;

namespace TileEngine.NGUI
{
    public static class GUISystem
    {
        private static Engine engine;
        private static IGUIElement hoverElement;
        private static IGUIElement selectedElement;
        private static IGUIElement activeElement;
        private static Window activeWindow;
        private static bool showDebugInfo = false;
        private static int mouseX;
        private static int mouseY;
        private static TimeSpan tickDuration = TimeSpan.FromMilliseconds(100);
        private static TimeSpan lastTick;

        public static void Init(Engine engine)
        {
            GUISystem.engine = engine;
        }
        public static bool ShowDebugInfo
        {
            get { return showDebugInfo; }
            set { showDebugInfo = value; }
        }
        public static event EventHandler<InputEvent> Input;

        public static void Update(TimeInfo time)
        {
            CheckTickTimer(time);
        }

        private static void CheckTickTimer(TimeInfo time)
        {
            var timeDiff = time.TotalGameTime - lastTick;
            if (timeDiff > tickDuration)
            {
                lastTick = time.TotalGameTime;
                if (activeElement != null)
                {
                    InputEvent ie = new InputEvent()
                    {
                        InputClass = InputClass.Timer,
                        GUIElement = activeElement,
                        Gadget = activeElement as Gadget,
                        Window = activeWindow,
                        MouseX = engine.Input.MouseX,
                        MouseY = engine.Input.MouseY
                    };
                    OnInput(ie);
                }
                else if (activeWindow != null)
                {

                }
            }
        }

        public static void Render(IGraphics graphics, TimeInfo time)
        {
            //Render(graphics, engine?.Screen);
        }

        public static void ActivateWindow(Window window)
        {
            SetActiveWindow(window);
        }

        public static void CloseWindow(Window window)
        {
            CloseWindow(window, null);
        }

        private static void CloseWindow(Window window, Window next)
        {
            if (window != null)
            {
                if (next == null)
                {
                    next = window.Close();
                }
                else
                {
                    window.Close();
                }
                SetActiveWindow(next);
            }
        }

        public static void Request(Requester req, Window window)
        {
            if (window != null)
            {
                window.BeginRequest(req);
                OnInput(new InputEvent()
                {
                    InputClass = InputClass.ReqSet,
                    Requester = req
                });
            }
        }

        public static void EndRequest(Requester req, Window window)
        {
            InternalEndRequest(req, window, null);
        }

        private static void InternalEndRequest(Requester req, Window window, Gadget gadget)
        {
            if (window != null)
            {
                window.EndRequest(req);
            }
            if (req != null && req.ReqFlags.HasFlag(ReqFlags.SysReq))
            {
                CloseWindow(window, req.Owner);
                if (req.Owner != null)
                {
                    req.Owner.EndRequest(window);
                    SetActiveWindow(req.Owner);
                }
                if (gadget != null)
                {
                    OnInput(new InputEvent()
                    {
                        InputClass = InputClass.ReqAuto,
                        Window = window,
                        Requester = req,
                        Gadget = gadget
                    });
                }
            }
        }


        public static bool EasyRequest(Window window, EasyStruct easy, params object[] args)
        {
            Window reqWin = BuildEasyRequest(window, easy, args);
            return reqWin != null;
        }

        public static Window BuildEasyRequest(Window window, EasyStruct easy, params object[] args)
        {
            IScreen screen = window != null ? GetScreen(window) : engine.Screen;
            string bodyText = args.Length > 0 ? string.Format(easy.TextFormat, args[0]) : easy.TextFormat;
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
                Gadget gad = new Gadget()
                {
                    Left = posX,
                    Top = height - gadHeight - 8,
                    Width = gadWidth,
                    Height = gadHeight,
                    GadgetFlags = GadgetFlags.RelVerify | GadgetFlags.EndGadget,
                    ID = count < numGads - 1 ? count + 1 : 0,
                    Text = gadText
                };
                gadgets.Add(gad);
                posX += (gadWidth + 4);
                width = posX;
                count++;
            }
            Requester req = new Requester()
            {
                Width = width,
                Height = height,
                Text = bodyText,
                Left = 4,
                Top = 20,
                BackFill = Color.Gray,
                ReqFlags = ReqFlags.SysReq,
                Owner = window
            };
            foreach (var g in gadgets)
            {
                req.AddChild(g);
            };
            string title = easy.Title;
            int x = 0;
            int y = 0;
            width += 8;
            height += 24;
            if (easy.EasyReqFlags.HasFlag(EasyReqFlags.CenterScreen))
            {
                //x = screen.Width / 2 - width / 2;
                //y = screen.Height / 2 - height / 2;
            }
            else if (easy.EasyReqFlags.HasFlag(EasyReqFlags.CenterWindow))
            {
                var box = window.ScrBox;
                x = box.CenterX - width / 2;
                y = box.CenterY - height / 2;
            }
            Window win = new Window()
            {
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                WindowFlags = WindowFlags.DepthGadget | WindowFlags.DragGadget | WindowFlags.Activate,
                Title = title
            };
            //screen.AddChild(win);
            Request(req, win);
            if (easy.EasyReqFlags.HasFlag(EasyReqFlags.Blocking) && window != null)
            {
                window.BeginRequest(win);
            }
            return win;
        }
        public static void AddSysGadgets(Window window)
        {
            if (window.WindowFlags.HasFlag(WindowFlags.CloseGadget)) { window.AddChild(new CloseGadget(window)); }
            if (window.WindowFlags.HasFlag(WindowFlags.ZoomGadget)) { window.AddChild(new ZoomGadget(window)); }
            if (window.WindowFlags.HasFlag(WindowFlags.DepthGadget)) { window.AddChild(new DepthGadget(window)); }
            if (window.WindowFlags.HasFlag(WindowFlags.DragGadget)) { window.AddChild(new DragGadget(window)); }
            if (window.WindowFlags.HasFlag(WindowFlags.SizeGadget)) { window.AddChild(new SizeGadget(window)); }
        }
        private static void Render(IGraphics graphics, IGUIElement element)
        {
            if (element != null && element.Visible)
            {
                element.BeginRender(graphics);
                element.RenderBack(graphics);
                if (showDebugInfo && element.Selectable) { element.DebugRender(graphics, element.Font); }
                foreach (var child in element.Children)
                {
                    Render(graphics, child);
                }
                element.RenderFront(graphics);
                element.EndRender(graphics);
            }
        }

        private static void SetHoverElement(IGUIElement element)
        {
            if (element != hoverElement)
            {
                if (hoverElement != null)
                {
                    hoverElement.Hover = false;
                    Logger.Detail("GUI", $"Not Hovering \"{hoverElement}\"");
                }
                hoverElement = element;
                if (hoverElement != null)
                {
                    hoverElement.Hover = true;
                    Logger.Detail("GUI", $"Hovering \"{hoverElement}\"");
                }
            }
        }

        private static void SetSelectedElement(IGUIElement element)
        {
            if (element != selectedElement)
            {
                if (selectedElement != null)
                {
                    selectedElement.Selected = false;
                    Logger.Detail("GUI", $"Not Selected \"{selectedElement}\"");
                }
                selectedElement = element;
                if (selectedElement != null)
                {
                    selectedElement.Selected = true;
                    Logger.Detail("GUI", $"Selected \"{selectedElement}\"");
                }
            }
        }

        private static void SetActiveElement(IGUIElement element)
        {
            if (!(element is Window))
            {
                if (element != activeElement)
                {
                    if (activeElement != null)
                    {
                        activeElement.Active = false;
                        Logger.Detail("GUI", $"Not Activated \"{activeElement}\"");
                    }
                    activeElement = element;
                    if (activeElement != null)
                    {
                        activeElement.Active = true;
                        Logger.Detail("GUI", $"Activated \"{activeElement}\"");
                    }
                }
                else if (element != null && !element.Active)
                {
                    activeElement = element;
                    activeElement.Active = true;
                    Logger.Detail("GUI", $"Activated \"{activeElement}\"");
                }
            }
        }

        private static void SetActiveWindow(Window window)
        {
            if (window?.ReqWindow != null) window = window?.ReqWindow;
            if (window != activeWindow)
            {
                if (activeWindow != null)
                {
                    activeWindow.Active = false;
                    Logger.Detail("GUI", $"Not Activated \"{activeWindow}\"");
                    OnInput(new InputEvent()
                    {
                        InputClass = InputClass.WindowInactive,
                        GUIElement = activeWindow,
                        Window = activeWindow
                    });
                }
                activeWindow = window;
                if (activeWindow != null)
                {
                    activeWindow.Active = true;
                    Logger.Detail("GUI", $"Activated \"{activeWindow}\"");
                    var screen = GetScreen(activeWindow);
                    //if (screen != null) { screen.ActiveWindow = activeWindow; }
                    OnInput(new InputEvent()
                    {
                        InputClass = InputClass.WindowActive,
                        GUIElement = activeWindow,
                        Window = activeWindow
                    });
                }
            }
        }

        public static void OnInput(InputEvent ie)
        {
            UpdateInputEvent(ie);
            Input?.Invoke(null, ie);
        }

        private static void UpdateDeltaMouse(InputEvent ie)
        {
            ie.DeltaX = ie.MouseX - mouseX;
            ie.DeltaY = ie.MouseY - mouseY;
            mouseX = ie.MouseX;
            mouseY = ie.MouseY;
        }

        internal static Window GetWindow(IGUIElement element)
        {
            while (element != null && !(element is Window))
            {
                element = element.Parent;
            }
            return (Window)element;
        }
        internal static Requester GetRequester(IGUIElement element)
        {
            while (element != null && !(element is Requester))
            {
                element = element.Parent;
            }
            return (Requester)element;
        }

        internal static IScreen GetScreen(IGUIElement element)
        {
            while (element != null && !(element is IScreen))
            {
                element = element.Parent;
            }
            return (IScreen)element;
        }

        private static void UpdateInputEvent(InputEvent ie)
        {
            switch (ie.InputClass)
            {
                case InputClass.MouseDown:
                    UpdateDeltaMouse(ie);
                    ie.GUIElement = FindGUIElement(ie.MouseX, ie.MouseY);
                    SetActiveWindow(GetWindow(ie.GUIElement));
                    SetHoverElement(ie.GUIElement);
                    SetSelectedElement(ie.GUIElement);
                    SetActiveElement(ie.GUIElement);
                    if (ie.GUIElement is Gadget)
                    {
                        OnInput(new InputEvent(ie)
                        {
                            InputClass = InputClass.GadgetDown,
                            Gadget = (Gadget)ie.GUIElement
                        });
                    }
                    break;
                case InputClass.MouseMove:
                    UpdateDeltaMouse(ie);
                    ie.GUIElement = FindGUIElement(ie.MouseX, ie.MouseY);
                    SetHoverElement(ie.GUIElement);
                    break;
                case InputClass.MouseUp:
                    UpdateDeltaMouse(ie);
                    ie.GUIElement = FindGUIElement(ie.MouseX, ie.MouseY);
                    SetHoverElement(ie.GUIElement);
                    bool gadUp = false;
                    Gadget gad = activeElement as Gadget;
                    if (gad != null)
                    {
                        if (gad.GadgetFlags.HasFlag(GadgetFlags.RelVerify))
                        {
                            if (gad == ie.GUIElement)
                            {
                                gadUp = true;
                            }
                        }
                        else
                        {
                            gadUp = true;
                        }
                    }
                    if (gadUp)
                    {
                        OnInput(new InputEvent(ie)
                        {
                            InputClass = InputClass.GadgetUp,
                            Gadget = gad
                        });
                    }
                    SetSelectedElement(null);
                    break;
                case InputClass.KeyDown:
                    ie.GUIElement = activeElement;
                    break;
                case InputClass.KeyUp:
                    ie.GUIElement = activeElement;
                    break;
                case InputClass.GadgetUp:
                    if (ie.Gadget.GadgetFlags.HasFlag(GadgetFlags.EndGadget))
                    {
                        var req = ie.Gadget.Requester;
                        var win = ie.Gadget.Window;
                        InternalEndRequest(req, win, ie.Gadget);
                    }
                    break;
                case InputClass.WindowClose:
                    return;
            }
            activeElement?.HandleInput(ie);
        }

        private static IGUIElement FindGUIElement(int x, int y)
        {
            return null;
            //return FindGUIElement(engine?.Screen, x, y);
        }

        private static IGUIElement FindGUIElement(IGUIElement element, int x, int y)
        {
            IGUIElement result = null;
            if (element != null && element.Visible && element.Enabled)
            {
                if (element.HitTest(x, y))
                {
                    result = element;
                    foreach (var child in element.Children)
                    {
                        IGUIElement subElement = FindGUIElement(child, x, y);
                        if (subElement != null && subElement.Selectable) { result = subElement; }
                    }
                }
            }
            return result;
        }

        internal static Texture GetWindowBitmap(Window window)
        {
            IBox box = window.WinBox;
            return engine.Graphics.CreateTexture("Window_" + window.Title, box.Width, box.Height);
        }
    }
}
