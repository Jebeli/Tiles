using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Files;

namespace TileEngine.Audio
{
    public interface ISounds
    {
        int SoundFallOff { get; set; }
        int SoundVolume { get; set; }
        int MusicVolume { get; set; }

        Music GetMusic(string fileId, IFileResolver fileResolver);
        Sound GetSound(string fileId, IFileResolver fileResolver);

        void PauseAll();
        void ResumeAll();

        void PlayMusic(Music music);
        void StopMusic();

        void PlaySound(Sound sound);
        void PlaySound(Sound sound, FPoint pos, bool loop = false);
        void PlaySound(Sound sound, string channel);
        void PlaySound(Sound sound, string channel, FPoint pos, bool loop = false);

        void Update(FPoint pos);
        void Reset();


    }
}
