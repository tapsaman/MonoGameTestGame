using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;
using ZA6.Utilities;

namespace ZA6
{
    public static class Music
    {
        public static float Volume { get; set; } = 0.4f;
        public static bool IsPaused { get; private set; }
        public static MusicScrambler Scrambler { get; set; }
        private const float _VOLUME_MULTIPLIER = 0.4f;
        private static Song _currentSong;
        private static Song _nextSong;
        private static float _currentVolume;
        private static float _fadeOutTime;
        private static float _elapsedFadeTime;
        private static bool _fadingOut;
        private static object _pausedBy;

        public static void Play(Song song)
        {
            Sys.Debug("Playing song " + song.Name);

            if (_fadingOut)
            {
                _nextSong = song;
            }
            else if (_currentSong != song)
            {
                _currentSong = song;
                MediaPlayer.Play(song);
                MediaPlayer.Volume = Volume * _VOLUME_MULTIPLIER;
                MediaPlayer.IsRepeating = true;
            }
        }
        
        public static void PlayOnce(Song song)
        {
            Play(song);
            MediaPlayer.IsRepeating = false;
        }

        public static void Pause(object pauser)
        {
            if (IsPaused || MediaPlayer.State != MediaState.Playing)
                return;
            
            IsPaused = true;
            MediaPlayer.Pause();
            _pausedBy = pauser;
        }

        public static void Resume(object pauser)
        {
            if (!IsPaused || _pausedBy != pauser)
                return;
            
            IsPaused = false;
            MediaPlayer.Resume();
            _pausedBy = null;
        }

        public static void FadeOut(float timeInSeconds)
        {
            _fadeOutTime = timeInSeconds;
            _elapsedFadeTime = 0;
            _fadingOut = true;
        }

        public static void FadeOutTo(Song song, float timeInSeconds)
        {
            if (_currentSong == song)
                return;

            FadeOut(timeInSeconds);
            _nextSong = song;
        }

        public static void Stop()
        {
            _fadingOut = false;
            _currentSong = null;
            _nextSong = null;
            MediaPlayer.Stop();
        }

        public static void Update(GameTime gameTime)
        {
            if (IsPaused)
                return;
            
            if (Scrambler != null)
            {
                Scrambler.Update(gameTime);

                if (Scrambler.IsDone)
                    Scrambler = null;
            }
            else if (_fadingOut)
            {
                _elapsedFadeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedFadeTime < _fadeOutTime)
                {
                    _currentVolume = Volume - Volume * (_elapsedFadeTime / _fadeOutTime);
                }
                else
                {
                    _fadingOut = false;
                    _currentVolume = Volume;

                    if (_nextSong != null)
                    {
                        Play(_nextSong);
                        _nextSong = null;
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }
                }
            }
            else
            {
                _currentVolume = Volume;
            }

            MediaPlayer.Volume = _currentVolume * _VOLUME_MULTIPLIER;

        if (MediaPlayer.Queue.ActiveSong != null)
                Sys.Debug(MediaPlayer.Queue.ActiveSong.Name.ToString());
        }
    }
}