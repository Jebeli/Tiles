using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Audio;
using TileEngine.Core;
using TileEngine.Files;
using TileEngine.Logging;

namespace SDLTiles
{
    public class SDLSounds : AbstractSounds
    {
        private FPoint lastPos;
        private Dictionary<int, Playback> playback;
        private Dictionary<string, int> channels;
        private SDL_mixer.ChannelFinishedDelegate channelFinished;

        public SDLSounds()
        {
            playback = new Dictionary<int, Playback>();
            channels = new Dictionary<string, int>();
            if (SDL_mixer.Mix_OpenAudio(22050, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 1024) != 0)
            {
                Logger.Error("Sound", $"Error during Mix_OpenAudio: {SDL.SDL_GetError()}");
            }
            else
            {
                Logger.Info("Sound", $"Using SDLSoundManager (SDL2, {SDL.SDL_GetCurrentAudioDriver()})");
                SDL_mixer.Mix_AllocateChannels(128);
                channelFinished = new SDL_mixer.ChannelFinishedDelegate(OnChannelFinished);
                SetSoundVolume(soundVolume);
            }
        }

        public override Music GetMusic(string fileId, IFileResolver fileResolver)
        {
            IntPtr chunk = SDL_mixer.Mix_LoadMUS(fileResolver.Resolve(fileId));
            if (!chunk.Equals(IntPtr.Zero))
            {
                return new SDLMusic(fileId, chunk);
            }
            return null;
        }

        public override Sound GetSound(string fileId, IFileResolver fileResolver)
        {
            IntPtr chunk = SDL_mixer.Mix_LoadWAV(fileResolver.Resolve(fileId));
            if (!chunk.Equals(IntPtr.Zero))
            {
                return new SDLSound(fileId, chunk);
            }
            return null;
        }

        public override void PauseAll()
        {
            SDL_mixer.Mix_Pause(-1);
            SDL_mixer.Mix_PauseMusic();

        }

        public override void ResumeAll()
        {
            SDL_mixer.Mix_Resume(-1);
            SDL_mixer.Mix_ResumeMusic();
        }

        public override void PlayMusic(Music music)
        {
            if (music != this.music)
            {
                this.music = music;
                if (music != null)
                {
                    SDL_mixer.Mix_VolumeMusic(musicVolume);
                    SDL_mixer.Mix_PlayMusic(music.GetChunk(), -1);
                    Logger.Info("Sound", $"Playing music {music}");
                }
                else
                {
                    SDL_mixer.Mix_HaltMusic();
                }
            }
        }

        public override void StopMusic()
        {
            if (music != null)
            {
                SDL_mixer.Mix_HaltMusic();
                music = null;
            }
        }

        public override void Update(FPoint pos)
        {
            lastPos = pos;
            List<int> cleanup = new List<int>();
            foreach (var it in playback)
            {
                int channel = it.Key;
                Playback play = it.Value;
                if (play.Finished)
                {
                    cleanup.Add(channel);
                    continue;
                }
                if (play.Location.X == 0 && play.Location.Y == 0)
                {
                    continue;
                }
                float v = CalcDist(pos, play.Location) / soundFallOff;
                if (play.Loop)
                {
                    if (v < 1.0f && play.Paused)
                    {
                        SDL_mixer.Mix_Resume(channel);
                        play.Paused = false;
                    }
                    else if (v > 1.0f && !play.Paused)
                    {
                        SDL_mixer.Mix_Pause(channel);
                        play.Paused = true;
                        continue;
                    }
                }
                v = Math.Min(Math.Max(v, 0.0f), 1.0f);
                byte dist = (byte)(255.0f * v);
                SetChannelPosition(channel, 0, dist);
            }
            while (cleanup.Count > 0)
            {
                int channel = cleanup[0];
                cleanup.RemoveAt(0);
                if (playback.TryGetValue(channel, out Playback play))
                {
                    playback.Remove(channel);
                    if (channels.TryGetValue(play.VirtualChannel, out int vcit))
                    {
                        channels.Remove(play.VirtualChannel);
                    }
                }
            }
        }

        public override void Reset()
        {
            foreach (var it in playback)
            {
                Playback play = it.Value;
                int channel = it.Key;
                if (play.Loop)
                {
                    Logger.Info("Sound", $"Stopping sound '{play.Sound}' on channel {channel} ({play.VirtualChannel})");
                    SDL_mixer.Mix_HaltChannel(channel);
                }
            }
            Update(new FPoint(0,0));
        }

        protected override void SetMusicVolume(int volume)
        {
            SDL_mixer.Mix_VolumeMusic(volume);
        }

        protected override void SetSoundVolume(int volume)
        {
            SDL_mixer.Mix_Volume(-1, volume);
        }

        protected override void Play(Playback play)
        {
            int channel = -1;
            bool setChannel = false;
            if (!play.VirtualChannel.Equals(Sound.GLOBAL_VIRTUAL_CHANNEL))
            {
                if (channels.TryGetValue(play.VirtualChannel, out int vc))
                {
                    SDL_mixer.Mix_HaltChannel(vc);
                    channels.Remove(play.VirtualChannel);
                }
                setChannel = true;
            }
            channel = SDL_mixer.Mix_PlayChannel(-1, play.Sound.GetChunk(), play.Loop ? -1 : 0);
            if (channel == -1)
            {
                Logger.Error("Sound", $"Failed to play sound '{play.Sound}', no more channels available");

            }
            else
            {
                SDL_mixer.Mix_ChannelFinished(channelFinished);
                Logger.Info("Sound", $"Playing sound '{play.Sound}' on channel {channel} ({play.VirtualChannel})");
            }
            byte dist;
            if (play.Location.X != 0 || play.Location.Y != 0)
            {
                float v = 255.0f * (CalcDist(lastPos, play.Location) / soundFallOff);
                v = Math.Min(Math.Max(v, 0.0f), 255.0f);
                dist = (byte)v;
            }
            else
            {
                dist = 0;
            }
            SetChannelPosition(channel, 0, dist);
            if (setChannel) channels[play.VirtualChannel] = channel;
            playback[channel] = play;
        }

        private void OnChannelFinished(int channel)
        {
            if (playback.TryGetValue(channel, out Playback play))
            {
                play.Finished = true;
            }
            SetChannelPosition(channel, 0, 0);
        }

        private void SetChannelPosition(int channel, short angle, byte distance)
        {
            SDL_mixer.Mix_SetPosition(channel, angle, distance);
        }

        private static float CalcDist(FPoint p1, FPoint p2)
        {
            return (float)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }
    }
}
