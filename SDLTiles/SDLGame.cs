using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using TileEngine;
using TileEngine.Graphics;
using TileEngine.Input;
using TileEngine.Logging;

namespace SDLTiles
{
    public class SDLGame
    {
        private const string MAPNAME1 = "maps/frontier_outpost.xml";
        private const string MAPNAME2 = "maps/ancient_temple.txt";

        public bool running = true;
        public IntPtr win;
        public IntPtr ren;
        public SDL.SDL_Event systemEvent;
        public SDLFileResolver fileResolver;
        public SDLGraphics graphics;
        public SDLFontEngine fontEngine;
        private Engine engine;
        private int mouseX;
        private int mouseY;

        public SDLGame()
        {
            Logger.AddLogger(new ConsoleLogger());
            fileResolver = new SDLFileResolver("Content");

            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "best");
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO);
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);

            win = SDL.SDL_CreateWindow("SDL Tiles", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            ren = SDL.SDL_CreateRenderer(win, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            fontEngine = new SDLFontEngine();
            graphics = new SDLGraphics(this, 800, 600, fontEngine, new DebugOptions()
            {
                ShowGrid = false,
                ShowHighlight = false,
                ShowSelected = true,
                ShowTileCounter = false,
                ShowCoordinates = false
            });
            engine = new Engine(fileResolver, graphics, fontEngine);
        }

        public void Events()
        {
            while (SDL.SDL_PollEvent(out systemEvent) != 0 && running)
            {
                switch (systemEvent.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                        engine.Input.MouseWheel(mouseX, mouseY, systemEvent.wheel.y);
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEMOTION:
                        mouseX = systemEvent.motion.x;
                        mouseY = systemEvent.motion.y;
                        engine.Input.MouseMove(mouseX, mouseY, MouseButton.None);
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        engine.Input.MouseDown(systemEvent.button.x, systemEvent.button.y, systemEvent.button.GetMouseButton());
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                        engine.Input.MouseUp(systemEvent.button.x, systemEvent.button.y, systemEvent.button.GetMouseButton());
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        engine.Input.KeyDown(ScanCode2Key(systemEvent.key.keysym.scancode));
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        engine.Input.KeyUp(ScanCode2Key(systemEvent.key.keysym.scancode));
                        break;
                    case SDL.SDL_EventType.SDL_WINDOWEVENT:
                        switch (systemEvent.window.windowEvent)
                        {
                            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
                                engine.SetViewSize(systemEvent.window.data1, systemEvent.window.data2);
                                break;
                            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                                engine.SetViewSize(systemEvent.window.data1, systemEvent.window.data2);
                                break;
                        }
                        break;
                }
            }
        }

        public bool Update()
        {
            return engine.Update();
        }

        public void Present()
        {
            SDL.SDL_SetWindowTitle(win, engine.DebugInfoText);
            graphics.BeginFrame(this);
            engine.Render();
            graphics.EndFrame(this);
            SDL.SDL_RenderPresent(ren);
        }
        public void Run()
        {
            engine.SetNextMap(MAPNAME1, 25, 25);
            engine.Start();
            while (running)
            {
                Events();
                if (Update())
                    Present();
            }
        }

        private static Key ScanCode2Key(SDL.SDL_Scancode sc)
        {
            switch (sc)
            {
                case SDL.SDL_Scancode.SDL_SCANCODE_RETURN:
                    return Key.Return;
                case SDL.SDL_Scancode.SDL_SCANCODE_0:
                    return Key.D0;
                case SDL.SDL_Scancode.SDL_SCANCODE_1:
                    return Key.D1;
                case SDL.SDL_Scancode.SDL_SCANCODE_2:
                    return Key.D2;
                case SDL.SDL_Scancode.SDL_SCANCODE_3:
                    return Key.D3;
                case SDL.SDL_Scancode.SDL_SCANCODE_4:
                    return Key.D4;
                case SDL.SDL_Scancode.SDL_SCANCODE_5:
                    return Key.D5;
                case SDL.SDL_Scancode.SDL_SCANCODE_6:
                    return Key.D6;
                case SDL.SDL_Scancode.SDL_SCANCODE_7:
                    return Key.D7;
                case SDL.SDL_Scancode.SDL_SCANCODE_8:
                    return Key.D8;
                case SDL.SDL_Scancode.SDL_SCANCODE_9:
                    return Key.D9;
                case SDL.SDL_Scancode.SDL_SCANCODE_LEFT:
                    return Key.Left;
                case SDL.SDL_Scancode.SDL_SCANCODE_RIGHT:
                    return Key.Right;
                case SDL.SDL_Scancode.SDL_SCANCODE_UP:
                    return Key.Up;
                case SDL.SDL_Scancode.SDL_SCANCODE_DOWN:
                    return Key.Down;
                case SDL.SDL_Scancode.SDL_SCANCODE_HOME:
                    return Key.Home;
                case SDL.SDL_Scancode.SDL_SCANCODE_END:
                    return Key.End;
                case SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN:
                    return Key.PageDown;
                case SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP:
                    return Key.PageUp;
                case SDL.SDL_Scancode.SDL_SCANCODE_BACKSPACE:
                    return Key.Back;
                case SDL.SDL_Scancode.SDL_SCANCODE_DELETE:
                    return Key.Delete;
                case SDL.SDL_Scancode.SDL_SCANCODE_SPACE:
                    return Key.Space;
                case SDL.SDL_Scancode.SDL_SCANCODE_SEMICOLON:
                    return Key.OemSemicolon;
                case SDL.SDL_Scancode.SDL_SCANCODE_KP_PLUS:
                    return Key.Oemplus;
                case SDL.SDL_Scancode.SDL_SCANCODE_KP_MINUS:
                    return Key.OemMinus;
                case SDL.SDL_Scancode.SDL_SCANCODE_KP_COLON:
                    return Key.Decimal;
                case SDL.SDL_Scancode.SDL_SCANCODE_A:
                    return Key.A;
                case SDL.SDL_Scancode.SDL_SCANCODE_B:
                    return Key.B;
                case SDL.SDL_Scancode.SDL_SCANCODE_C:
                    return Key.C;
                case SDL.SDL_Scancode.SDL_SCANCODE_D:
                    return Key.D;
                case SDL.SDL_Scancode.SDL_SCANCODE_E:
                    return Key.E;
                case SDL.SDL_Scancode.SDL_SCANCODE_F:
                    return Key.F;
                case SDL.SDL_Scancode.SDL_SCANCODE_G:
                    return Key.G;
                case SDL.SDL_Scancode.SDL_SCANCODE_H:
                    return Key.H;
                case SDL.SDL_Scancode.SDL_SCANCODE_I:
                    return Key.I;
                case SDL.SDL_Scancode.SDL_SCANCODE_J:
                    return Key.J;
                case SDL.SDL_Scancode.SDL_SCANCODE_K:
                    return Key.K;
                case SDL.SDL_Scancode.SDL_SCANCODE_L:
                    return Key.L;
                case SDL.SDL_Scancode.SDL_SCANCODE_M:
                    return Key.M;
                case SDL.SDL_Scancode.SDL_SCANCODE_N:
                    return Key.N;
                case SDL.SDL_Scancode.SDL_SCANCODE_O:
                    return Key.O;
                case SDL.SDL_Scancode.SDL_SCANCODE_P:
                    return Key.P;
                case SDL.SDL_Scancode.SDL_SCANCODE_Q:
                    return Key.Q;
                case SDL.SDL_Scancode.SDL_SCANCODE_R:
                    return Key.R;
                case SDL.SDL_Scancode.SDL_SCANCODE_S:
                    return Key.S;
                case SDL.SDL_Scancode.SDL_SCANCODE_T:
                    return Key.T;
                case SDL.SDL_Scancode.SDL_SCANCODE_U:
                    return Key.U;
                case SDL.SDL_Scancode.SDL_SCANCODE_V:
                    return Key.V;
                case SDL.SDL_Scancode.SDL_SCANCODE_W:
                    return Key.W;
                case SDL.SDL_Scancode.SDL_SCANCODE_X:
                    return Key.X;
                case SDL.SDL_Scancode.SDL_SCANCODE_Y:
                    return Key.Y;
                case SDL.SDL_Scancode.SDL_SCANCODE_Z:
                    return Key.Z;
            }
            return (Key)sc;
        }
    }
}
