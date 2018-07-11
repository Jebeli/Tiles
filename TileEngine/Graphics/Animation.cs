using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;

namespace TileEngine.Graphics
{
    public enum AnimationType
    {
        None,
        PlayOnce,
        Looped,
        BackForth
    }

    public enum BlendMode
    {
        Normal = 0,
        Add = 1
    }

    public class Animation : NamedObject
    {
        private Texture image;
        private AnimationType type;
        private int numberFrames;
        private int curFrame;
        private int curFrameIndex;
        private int curFrameDuration;
        private float curFrameIndexF;
        private int maxKinds;
        private int additonalData;
        private int timesPlayed;
        private TextureRegion[] regions;
        private int[] frames;
        private List<int> activeFrames;
        private bool activeFrameTriggered;
        private int elapsedFrames;
        private int frameCount;
        private float speed;
        private BlendMode blendMode;
        private byte alphaMod;
        private Color colorMod;

        public Animation(string name, AnimationType type, Texture image, BlendMode blendMode, byte alphaMod, Color colorMod)
            : base(name)
        {
            this.type = type;
            this.image = image;
            this.blendMode = blendMode;
            this.alphaMod = alphaMod;
            this.colorMod = colorMod;
            numberFrames = 0;
            curFrame = 0;
            curFrameIndex = 0;
            curFrameIndexF = 0;
            curFrameDuration = 0;
            maxKinds = 0;
            additonalData = 0;
            timesPlayed = 0;
            regions = new TextureRegion[0];
            frames = new int[0];
            activeFrames = new List<int>();
            activeFrameTriggered = false;
            elapsedFrames = 0;
            frameCount = 0;
            speed = 1.0f;
        }

        public Animation(Animation other)
                   : base(other)
        {
            type = other.type;
            image = other.image;
            blendMode = other.blendMode;
            alphaMod = other.alphaMod;
            colorMod = other.colorMod;
            numberFrames = other.numberFrames;
            curFrame = 0;
            curFrameIndex = other.curFrameIndex;
            curFrameIndexF = other.curFrameIndexF;
            curFrameDuration = other.curFrameDuration;
            maxKinds = other.maxKinds;
            additonalData = other.additonalData;
            timesPlayed = 0;
            regions = new TextureRegion[other.regions.Length];
            Array.Copy(other.regions, regions, regions.Length);
            frames = new int[other.frames.Length];
            Array.Copy(other.frames, frames, frames.Length);
            activeFrames = new List<int>(other.activeFrames);
            activeFrameTriggered = false;
            elapsedFrames = 0;
            frameCount = 0;
            speed = other.speed;
        }

        public void Setup(int numFrames, int duration, int maxKinds = 8)
        {
            frameCount = numFrames;
            List<int> frameList = new List<int>();
            if (numFrames > 0 && duration % numFrames == 0)
            {
                int divided = duration / numFrames;
                for (int i = 0; i < numFrames; i++)
                {
                    for (int j = 0; j < divided; j++)
                    {
                        frameList.Add(i);
                    }
                }
            }
            else
            {
                int x0 = 0;
                int y0 = 0;
                int x1 = duration - 1;
                int y1 = numFrames - 1;

                int dx = x1 - x0;
                int dy = y1 - y0;

                int D = 2 * dy - dx;

                frameList.Add(y0);

                int x = x0 + 1;
                int y = y0;

                while (x <= x1)
                {
                    if (D > 0)
                    {
                        y++;
                        frameList.Add(y);
                        D = D + ((2 * dy) - (2 * dx));
                    }
                    else
                    {
                        frameList.Add(y);
                        D = D + (2 * dy);
                    }
                    x++;
                }
            }
            if (frameList.Count > 0)
            {
                frames = frameList.ToArray();
                numberFrames = frames.Length;
            }
            if (type == AnimationType.PlayOnce)
            {
                additonalData = 0;
            }
            else if (type == AnimationType.Looped)
            {
                additonalData = 0;
            }
            else if (type == AnimationType.BackForth)
            {
                numberFrames = 2 * numberFrames;
                additonalData = 1;
            }
            curFrame = 0;
            curFrameIndex = 0;
            curFrameIndexF = 0;
            this.maxKinds = maxKinds;
            timesPlayed = 0;
            activeFrames.Add((numberFrames - 1) / 2);
            int index = maxKinds * numFrames;
            regions = new TextureRegion[index];
        }

        public void SetUncompressed(int renderX, int renderY, int renderOffsetX, int renderOffsetY, int position, int frames, int durarion, int maxKinds = 8)
        {
            Setup(frames, durarion, maxKinds);
            for (int i = 0; i < frames; i++)
            {
                int baseIndex = maxKinds * i;
                for (int kind = 0; kind < maxKinds; kind++)
                {
                    var reg = image.GetRegion(renderX * (position + i), renderY * kind, renderX, renderY, renderOffsetX, renderOffsetY);
                    regions[baseIndex + kind] = reg;
                }
            }
        }

        public void AddFrame(int index, int kind, int renderX, int renderY, int renderWidth, int renderHeight, int offsetX, int offsetY)
        {
            if (index >= regions.Length / maxKinds) return;
            if (kind >= maxKinds) return;
            int i = maxKinds * index + kind;
            var reg = image.GetRegion(renderX, renderY, renderWidth, renderHeight, offsetX, offsetY);
            regions[i] = reg;
        }

        public void AdvanceFrame()
        {
            if (frames.Length == 0)
            {
                curFrameIndex = 0;
                curFrameIndexF = 0;
                timesPlayed++;
                return;
            }

            int lastBaseIndex = frames.Length - 1;
            switch (type)
            {
                case AnimationType.PlayOnce:
                    if (curFrameIndex < lastBaseIndex)
                    {
                        curFrameIndexF += speed;
                        curFrameIndex = (int)curFrameIndexF;
                    }
                    else
                    {
                        timesPlayed = 1;
                    }
                    break;
                case AnimationType.Looped:
                    if (curFrameIndex < lastBaseIndex)
                    {
                        curFrameIndexF += speed;
                        curFrameIndex = (int)curFrameIndexF;
                    }
                    else
                    {
                        curFrameIndexF += speed;
                        curFrameIndexF -= lastBaseIndex;
                        curFrameIndex = (int)curFrameIndexF;
                        timesPlayed++;
                    }
                    break;
                case AnimationType.BackForth:
                    if (additonalData == 1)
                    {
                        if (curFrameIndex < lastBaseIndex)
                        {
                            curFrameIndexF += speed;
                            curFrameIndex = (int)curFrameIndexF;
                        }
                        else
                        {
                            additonalData = -1;
                        }
                    }
                    else if (additonalData == -1)
                    {
                        if (curFrameIndex > 0)
                        {
                            curFrameIndexF -= speed;
                            curFrameIndex = (int)curFrameIndexF;
                        }
                        else
                        {
                            additonalData = 1;
                            timesPlayed++;

                        }
                    }
                    break;
                case AnimationType.None:
                default:
                    break;
            }
            curFrameIndex = Math.Max(0, curFrameIndex);
            curFrameIndex = curFrameIndex > lastBaseIndex ? lastBaseIndex : curFrameIndex;
            if (curFrame != frames[curFrameIndex]) elapsedFrames++;
            curFrame = frames[curFrameIndex];
        }

        public bool SyncTo(Animation other)
        {
            curFrame = other.curFrame;
            curFrameIndex = other.curFrameIndex;
            curFrameIndexF = other.curFrameIndexF;
            timesPlayed = other.timesPlayed;
            additonalData = other.additonalData;
            elapsedFrames = other.elapsedFrames;
            if (curFrameIndex >= frames.Length)
            {
                if (frames.Length == 0)
                {
                    curFrameIndex = 0;
                    curFrameIndexF = 0;
                    return false;
                }
                else
                {
                    curFrameIndex = frames.Length - 1;
                    curFrameIndexF = curFrameIndex;
                    return false;
                }
            }
            return true;
        }

        public TextureRegion GetFirstFrame(int direction)
        {
            if (frames.Length > 0)
            {
                int index = maxKinds * frames[0] + direction;
                return regions[index];
            }
            return null;
        }

        public TextureRegion GetCurrentFrame(int direction)
        {
            if (frames.Length > 0)
            {
                int index = maxKinds * frames[curFrameIndex] + direction;
                return regions[index];
            }
            return null;
        }

        public BlendMode BlendMode
        {
            get { return blendMode; }
            set { blendMode = value; }
        }

        public Color ColorMod
        {
            get { return colorMod; }
            set { colorMod = value; }
        }

        public byte AlphaMod
        {
            get { return alphaMod; }
            set { alphaMod = value; }
        }

        public bool IsFirstFrame
        {
            get { return curFrameIndex == 0; }
        }

        public bool IsLastFrame
        {
            get { return curFrameIndex == GetLastFrameIndex(numberFrames - 1); }
        }

        public bool IsSecondLastFrame
        {
            get { return curFrameIndex == GetLastFrameIndex(numberFrames - 2); }
        }

        public bool IsActiveFrame
        {
            get
            {
                if (type == AnimationType.BackForth)
                {
                    if (activeFrames.Contains(elapsedFrames))
                        return curFrameIndex == GetLastFrameIndex(curFrame);
                }
                else
                {
                    if (activeFrames.Contains(curFrame))
                    {
                        if (curFrameIndex == GetLastFrameIndex(curFrame))
                        {
                            if (type == AnimationType.PlayOnce)
                            {
                                activeFrameTriggered = true;
                            }
                            return true;
                        }
                    }
                }
                return (IsLastFrame && type == AnimationType.PlayOnce && !activeFrameTriggered && activeFrames.Count > 0);
            }
        }

        public int TimesPlayed
        {
            get { return timesPlayed; }
        }

        public void Reset()
        {
            curFrame = 0;
            curFrameIndex = 0;
            curFrameIndexF = 0;
            timesPlayed = 0;
            additonalData = 1;
            elapsedFrames = 0;
            activeFrameTriggered = false;
        }

        public int Duration
        {
            get { return (int)(frames.Length / speed); }
        }

        public void SetActiveFrames(IList<int> activeFrames)
        {
            if (activeFrames == null || (activeFrames.Count == 1 && activeFrames[0] == -1))
            {
                this.activeFrames.Clear();
                for (int i = 0; i < numberFrames; i++)
                {
                    this.activeFrames.Add(i);
                }
            }
            else
            {
                this.activeFrames = new List<int>(activeFrames);
            }
            bool haveLastFrame = this.activeFrames.Contains(numberFrames - 1);
            for (int i = 0; i < this.activeFrames.Count; i++)
            {
                if (this.activeFrames[i] >= numberFrames)
                {
                    if (haveLastFrame)
                    {
                        this.activeFrames.RemoveAt(i);
                    }
                    else
                    {
                        this.activeFrames[i] = numberFrames - 1;
                        haveLastFrame = true;
                    }
                }
            }
        }

        public bool IsFinished
        {
            get
            {
                if (IsCompleted) return true;
                //if (stance == AnimationStance.Stance || stance == AnimationStance.Run)
                //{
                //    return false;
                //}
                return timesPlayed > 0;
            }
        }

        public bool IsCompleted
        {
            get { return type == AnimationType.PlayOnce && timesPlayed > 0; }
        }

        public int FrameCount { get { return frameCount; } }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private int GetLastFrameIndex(int frame)
        {
            if (frames.Length == 0 || frame < 0) return 0;
            if (type == AnimationType.BackForth && additonalData == -1)
            {
                for (int i = 0; i < frames.Length; i++)
                {
                    if (frames[i] == frame) return i;
                }
                return 0;
            }
            else
            {
                for (int i = frames.Length - 1; i >= 0; i--)
                {
                    if (frames[i] == frame) return i;
                }
                return frames.Length - 1;
            }
        }     
    }
}
