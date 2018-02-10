using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Files;

namespace TileEngine.Graphics
{
    public interface IGraphics
    {
        int Width { get; }
        int Height { get; }
        int ViewWidth { get; }
        int ViewHeight { get; }
        float ViewScale { get; }
        long FrameId { get; }
        bool InFrame { get; }

        void BeginFrame();
        void EndFrame();
        void SetSize(int width, int height);
        void SetScale(float scale);
        void ClearScreen();
        Texture CreateTexture(string textureId, int width, int height);
        Texture GetTexture(string textureId, IFileResolver fileResolver);
    }
}
