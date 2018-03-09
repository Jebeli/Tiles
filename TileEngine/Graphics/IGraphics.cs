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

namespace TileEngine.Graphics
{
    using Core;
    using Files;

    public enum HorizontalTextAlign
    {
        Center,
        Left,
        Right
    }

    public enum VerticalTextAlign
    {
        Center,
        Top,
        Bottom
    }

    public interface IGraphics
    {
        int Width { get; }
        int Height { get; }
        int ViewWidth { get; }
        int ViewHeight { get; }
        float ViewScale { get; }
        long FrameId { get; }
        bool InFrame { get; }
        DebugOptions DebugOptions { get; set; }

        void SetTarget(Texture tex);
        void ClearTarget();

        void BeginFrame();
        void EndFrame();
        void SetSize(int width, int height);
        void SetScale(float scale);
        void ClearScreen();
        void ClearScreen(Color color);
        void DrawTextures(Texture texture, int[] vertices, int offset, int count);
        void Render(Texture texture, int x, int y, int trans=0);
        void Render(TextureRegion textureRegion, int x, int y);
        void Render(TextureRegion textureRegion, int x, int y, int width, int height);
        void Render(Texture texture, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight, int trans=0);
        void RenderText(string text, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center);
        int MeasureTextWidth(string text);
        void RenderWidget(int x, int y, int width, int height, bool enabled, bool hover, bool pressed);
        void DrawRectangle(int x, int y, int width, int height, Color color);
        void FillRectangle(int x, int y, int width, int height, Color color);
        void DrawLine(int x1, int y1, int x2, int y2, Color color);
        void DrawText(string text, int x, int y);
        void DrawTileGrid(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric);
        void DrawTileSelected(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric);

        Texture CreateTexture(string textureId, int width, int height);
        Texture GetTexture(string textureId, IFileResolver fileResolver);
        void ExitRequested();
    }
}
