using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;

namespace TileEngine.Graphics
{
    public class AnimationSet : NamedObject
    {
        private Animation defaultAnimation;
        private AnimationSet parent;
        private List<Animation> animations;
        private Texture image;

        public AnimationSet(string name, Texture image)
            : base(name)
        {
            this.image = image;
            animations = new List<Animation>();
        }

        public void AddAnimation(Animation a)
        {
            animations.Add(a);
            if (animations.Count == 1) { defaultAnimation = a; }
        }


        public Animation AddAnimation(string name, AnimationType type)
        {
            Animation anim = new Animation(name, type, image, BlendMode.Normal, 255, Color.White);
            animations.Add(anim);
            if (animations.Count == 1) { defaultAnimation = anim; }
            return anim;
        }

        public void SetDefaultAnimation(string startingAnim)
        {
            if (!string.IsNullOrEmpty(startingAnim))
            {
                Animation a = GetAnimation(startingAnim);
                if (a != null)
                {
                    defaultAnimation = a;
                }
            }
        }

        public Animation GetAnimation(string name)
        {
            Animation anim = Find(animations, name);
            if (anim != null)
            {
                return new Animation(anim);
            }
            return new Animation(defaultAnimation);
        }

        public AnimationSet Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public int GetAnimationFrames(string name)
        {
            Animation anim = Find(animations, name);
            if (anim != null)
            {
                return anim.FrameCount;
            }
            return defaultAnimation.FrameCount;
        }
    }
}
