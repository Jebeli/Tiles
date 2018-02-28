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

    public class TileAnim
    {
        private int tileId;
        private int currentFrame;
        private int duration;
        private List<int> frameDurations;
        private List<TextureRegion> textures;
        public TileAnim(int tileId)
        {
            this.tileId = tileId;
            frameDurations = new List<int>();
            textures = new List<TextureRegion>();
        }

        public bool Update()
        {
            duration++;
            if (duration >= frameDurations[currentFrame])
            {
                AdvanceFrame();
                return true;
            }
            return false;
        }

        private void AdvanceFrame()
        {
            duration = 0;
            currentFrame++;
            if (currentFrame >= textures.Count)
            {
                currentFrame = 0;
            }
        }

        public int TileId
        {
            get { return tileId; }
        }

        public IList<int> FrameDurations
        {
            get { return frameDurations; }
        }

        public IList<TextureRegion> Textures
        {
            get { return textures; }
        }

        public TextureRegion CurrentFrame
        {
            get
            {
                return textures[currentFrame];
            }
        }

        public TextureRegion AddFrame(Texture texture, int duration, int clipX, int clipY, int clipW, int clipH, int offsetX, int offsetY)
        {
            TextureRegion tile = new TextureRegion(texture, clipX, clipY, clipW, clipH, offsetX, offsetY);
            textures.Add(tile);
            frameDurations.Add(duration);
            return tile;
        }
    }
}
