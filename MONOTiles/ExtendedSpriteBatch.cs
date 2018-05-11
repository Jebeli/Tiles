/*
 * From an answer by https://stackoverflow.com/users/1009118/wronex on stackoverflow
 * 
 * https://stackoverflow.com/questions/2792694/draw-rectangle-with-xna/2792711
 * 
 * */

namespace MONOTiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An extended version of the SpriteBatch class that supports line and
    /// rectangle drawing.
    /// </summary>
    public class ExtendedSpriteBatch : SpriteBatch
    {
        /// <summary>
        /// The texture used when drawing rectangles, lines and other 
        /// primitives. This is a 1x1 white texture created at runtime.
        /// </summary>
        public Texture2D WhiteTexture { get; protected set; }
        private Dictionary<Color, Texture2D> gradients = new Dictionary<Color, Texture2D>();
        //public Texture2D GradTexture { get; protected set; }

        public ExtendedSpriteBatch(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new Color[] { Color.White });
            //GradTexture = new Texture2D(GraphicsDevice, 1, 2);
            //GradTexture.SetData(new Color[] { Color.White, Color.White });
        }

        /// <summary>
        /// Draw a line between the two supplied points.
        /// </summary>
        /// <param name="start">Starting point.</param>
        /// <param name="end">End point.</param>
        /// <param name="color">The draw color.</param>
        public void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            if ((int)start.X == (int)end.X)
            {
                int minY = (int)Math.Min(start.Y, end.Y);
                int maxY = (int)Math.Max(start.Y, end.Y);
                Draw(WhiteTexture, new Rectangle((int)start.X, minY, 1, 1 + maxY - minY), color);
            }
            else if ((int)start.Y == (int)end.Y)
            {
                int minX = (int)Math.Min(start.X, end.X);
                int maxX = (int)Math.Max(start.X, end.X);
                Draw(WhiteTexture, new Rectangle(minX, (int)start.Y, 1 + maxX - minX, 1), color);
            }
            else
            {
                float length = (end - start).Length();
                float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
                length = (float)Math.Round(length + 0.5f);
                Draw(WhiteTexture, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Draw a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The draw color.</param>
        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            Draw(WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width - 1, 1), color);
            Draw(WhiteTexture, new Rectangle(rectangle.Left, rectangle.Bottom - 1, rectangle.Width - 1, 1), color);
            Draw(WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height - 1), color);
            Draw(WhiteTexture, new Rectangle(rectangle.Right - 1, rectangle.Top, 1, rectangle.Height), color);
        }

        public void DrawRoundedRectangle(Rectangle rectangle, Color color)
        {
            Draw(WhiteTexture, new Rectangle(rectangle.Left + 1, rectangle.Top, rectangle.Width - 3, 1), color);
            Draw(WhiteTexture, new Rectangle(rectangle.Left + 1, rectangle.Bottom - 1, rectangle.Width - 3, 1), color);
            Draw(WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top + 1, 1, rectangle.Height - 3), color);
            Draw(WhiteTexture, new Rectangle(rectangle.Right - 1, rectangle.Top + 1, 1, rectangle.Height - 3), color);
        }

        /// <summary>
        /// Fill a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to fill.</param>
        /// <param name="color">The fill color.</param>
        public void FillRectangle(Rectangle rectangle, Color color)
        {
            Draw(WhiteTexture, rectangle, color);
        }

        private Texture2D GetGradient(Color color, Color color2, int height)
        {
            Texture2D tex = null;
            if (gradients.TryGetValue(color, out tex))
            {
                return tex;
            }
            tex = new Texture2D(GraphicsDevice, 1, height);
            Color[] colors = new Color[height];
            float start = color.R;
            float end = color2.R;
            float aStart = color.A;
            float aEnd = color2.A;
            float inc = (end - start) / height;
            float aInc = (aEnd - aStart) / height;
            float a = aStart;
            float c = start;
            for (int i = 0; i < height; i++)
            {
                Color ic = new Color((byte)c, (byte)c, (byte)c, (byte)a);
                colors[i] = ic;
                c += inc;
                a += aInc;
            }
            tex.SetData(colors);
            gradients[color] = tex;
            return tex;
        }

        public void FillRectangle(Rectangle rectangle, Color color, Color color2)
        {
            Texture2D grad = GetGradient(color, color2, 20);
            Draw(grad, rectangle, Color.White);
        }


    }
}
