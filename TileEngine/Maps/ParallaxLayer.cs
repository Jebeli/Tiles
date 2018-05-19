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

namespace TileEngine.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Graphics;

    public class ParallaxLayer
    {
        private Texture texture;
        private string mapLayer;
        private float speed;
        private float fixedSpeedX;
        private float fixedSpeedY;
        private float fixedOffsetX;
        private float fixedOffsetY;

        public ParallaxLayer(Texture texture)
        {
            this.texture = texture;
        }

        public Texture Texture
        {
            get { return texture; }
        }

        public string MapLayer
        {
            get { return mapLayer; }
            set { mapLayer = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float FixedSpeedX
        {
            get { return fixedSpeedX; }
            set { fixedSpeedX = value; }
        }

        public float FixedSpeedY
        {
            get { return fixedSpeedY; }
            set { fixedSpeedY = value; }
        }

        public float FixedOffsetX
        {
            get { return fixedOffsetX; }
            set { fixedOffsetX = value; }
        }

        public float FixedOffsetY
        {
            get { return fixedOffsetY; }
            set { fixedOffsetY = value; }
        }

        public void Update()
        {
            int width = texture.Width;
            int height = texture.Height;

            fixedOffsetX += fixedSpeedX;
            fixedOffsetY += fixedSpeedY;

            //float sX = speed;
            //float sY = speed;

            //if (fixedSpeedX != 0)
            //{
            //    sX = fixedSpeedX * sX;
            //}
            //if (fixedSpeedY != 0)
            //{
            //    sY = fixedSpeedY * sY;
            //}


            //fixedOffsetX += sX;
            //fixedOffsetY += sY;

            if (fixedOffsetX > width) { fixedOffsetX -= width; }
            if (fixedOffsetX < -width) { fixedOffsetX += width; }
            if (fixedOffsetY > height) { fixedOffsetY -= height; }
            if (fixedOffsetY < -height) { fixedOffsetY += height; }
        }
    }
}
