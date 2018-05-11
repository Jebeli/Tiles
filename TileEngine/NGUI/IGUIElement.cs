using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public interface IGUIElement : IBoxProvider, IGUIDimensions
    {
        int ID { get; set; }
        bool Visible { get; set; }
        bool Selectable { get; set; }
        bool Enabled { get; set; }
        bool Hover { get; set; }
        bool Selected { get; set; }
        bool Active { get; set; }
        IGUIElement Target { get; set; }
        DrawInfo DrawInfo { get; set; }
        StateFlags State { get; set; }
        IGUIElement Parent { get; set; }
        IEnumerable<IGUIElement> Children { get; }
        Font Font { get; }
        void AddChild(IGUIElement element);
        void RemChild(IGUIElement element);
        void MoveToBack();
        void MoveToFront();
        void MoveChildToBack(IGUIElement element);
        void MoveChildToFront(IGUIElement element);
        void Init();
        void BeginUpdate();
        void EndUpdate();
        bool IsUpdating { get; }
        void Notify();
        void Update();
        bool HitTest(int x, int y);
        void HandleInput(InputEvent inputEvent);
        void RenderBack(IGraphics graphics);
        void RenderFront(IGraphics graphics);
        void BeginRender(IGraphics graphics);
        void EndRender(IGraphics graphics);
        void DebugRender(IGraphics graphics, Font font);
    }
}
