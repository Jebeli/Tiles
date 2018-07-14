using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Files;

namespace TileEngine.Audio
{
    public class NoSounds : AbstractSounds
    {
        public override Music GetMusic(string fileId, IFileResolver fileResolver)
        {
            return null;    
        }

        public override Sound GetSound(string fileId, IFileResolver fileResolver)
        {
            return null;            
        }

        public override void PauseAll()
        {
            
        }

        public override void ResumeAll()
        {
            
        }

        public override void PlayMusic(Music music)
        {
            
        }

        public override void StopMusic()
        {
            
        }

        public override void Update(FPoint pos)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override void SetMusicVolume(int volume)
        {
            
        }

        protected override void SetSoundVolume(int volume)
        {
            
        }

        protected override void Play(Playback play)
        {
            
        }
    }
}
