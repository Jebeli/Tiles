using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Files;

namespace TileEngine.Audio
{
    public abstract class AbstractSounds : ISounds
    {
        protected int soundVolume;
        protected int musicVolume;
        protected int soundFallOff;
        protected Music music;


        public AbstractSounds()
        {
            soundVolume = 128;
            musicVolume = 96;
            soundFallOff = 15;
        }

        public int SoundFallOff
        {
            get { return soundFallOff; }
            set { soundFallOff = value; }
        }

        public int SoundVolume
        {
            get { return soundVolume; }
            set
            {
                soundVolume = value;
                SetSoundVolume(value);
            }
        }

        public int MusicVolume
        {
            get { return musicVolume; }
            set
            {
                musicVolume = value;
                SetMusicVolume(value);
            }
        }

        public abstract Music GetMusic(string fileId, IFileResolver fileResolver);
        public abstract Sound GetSound(string fileId, IFileResolver fileResolver);

        public abstract void PauseAll();
        public abstract void ResumeAll();

        public abstract void PlayMusic(Music music);
        public abstract void StopMusic();

        public void PlaySound(Sound sound)
        {
            PlaySound(sound, Sound.GLOBAL_VIRTUAL_CHANNEL, new FPoint(), false);
        }

        public void PlaySound(Sound sound, FPoint pos, bool loop = false)
        {
            PlaySound(sound, Sound.GLOBAL_VIRTUAL_CHANNEL, pos, loop);
        }

        public void PlaySound(Sound sound, string channel)
        {
            PlaySound(sound, channel, new FPoint(), false);
        }

        public void PlaySound(Sound sound, string channel, FPoint pos, bool loop = false)
        {
            Play(new Playback(sound, channel, pos, loop));
        }

        public abstract void Update(FPoint pos);

        public abstract void Reset();


        protected abstract void SetSoundVolume(int volume);
        protected abstract void SetMusicVolume(int volume);
        protected abstract void Play(Playback play);

        protected class Playback
        {
            public Sound Sound { get; set; }
            public string VirtualChannel { get; set; }
            public FPoint Location { get; set; }
            public bool Loop { get; set; }
            public bool Paused { get; set; }
            public bool Finished { get; set; }

            public Playback(Sound sound, string channel, FPoint pos, bool loop)
            {
                Sound = sound;
                Location = pos;
                Loop = loop;
                Paused = false;
                Finished = false;
                VirtualChannel = channel;
            }
        }
    }
}
