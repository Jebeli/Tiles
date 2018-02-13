using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Graphics
{
    public class TextureBatch : IBatch
    {
        private IGraphics graphics;
        private int[] vertices;
        private bool drawing;
        private int index;
        private Texture lastTexture;
        private int renderCalls;
        private int totalRenderCalls;
        private int maxSpritesInBatch;
        private const int NUM_VERTICES = 10;
        private const int DEFAULT_SIZE = 2000;

        public TextureBatch(IGraphics graphics)
                   : this(graphics, DEFAULT_SIZE)
        {

        }

        public TextureBatch(IGraphics graphics, int size)
        {
            this.graphics = graphics;
            vertices = new int[size * NUM_VERTICES];
        }

        public bool IsDrawing { get { return drawing; } }

        public void Begin()
        {
            if (drawing) throw new InvalidOperationException("Batch.End must be called before Begin.");
            renderCalls = 0;
            drawing = true;

        }
        public void End()
        {
            if (!drawing) throw new InvalidOperationException("Batch.Begin must be called before End.");
            if (index > 0) Flush();
            lastTexture = null;
            drawing = false;
        }
        public void Flush()
        {
            if (index == 0) return;
            renderCalls++;
            totalRenderCalls++;
            int count = index / NUM_VERTICES;
            if (count > maxSpritesInBatch) { maxSpritesInBatch = count; }
            graphics.DrawTextures(lastTexture, vertices, 0, count);
            index = 0;
        }
        public void Draw(TextureRegion region, int x, int y)
        {
            Draw(region.Texture, x + region.OffsetX, y + region.OffsetY, region.Width, region.Height, region.X, region.Y, region.Width, region.Height);
        }

        public void Draw(Texture texture, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            if (!drawing) throw new InvalidOperationException("Batch.Begin must be called before Draw.");
            if (texture != lastTexture)
            {
                SwitchTexture(texture);
            }
            else if (index == vertices.Length)
            {
                Flush();
            }
            vertices[index + 0] = x;
            vertices[index + 1] = y;
            vertices[index + 2] = width;
            vertices[index + 3] = height;
            vertices[index + 4] = srcX;
            vertices[index + 5] = srcY;
            vertices[index + 6] = srcWidth;
            vertices[index + 7] = srcHeight;
            vertices[index + 8] = 0; // reserved for transparency
            vertices[index + 9] = 0xFFFFFF; // reserved for tint
            index += NUM_VERTICES;
        }

        private void SwitchTexture(Texture texture)
        {
            Flush();
            lastTexture = texture;
        }
    }
}
