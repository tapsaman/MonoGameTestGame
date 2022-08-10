using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace MonoGameTestGame
{
    public static class Music
    {
        public static float Volume { get; set; } = 0.4f;
        private static float _currentVolume;
        private static float _fadeOutTime;
        private static float _elapsedFadeTime;
        private static bool _fadingOut;
        private static Song _currentSong;
        private static Song _nextSong;

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
                MediaPlayer.Volume = Volume;
                MediaPlayer.IsRepeating = true;
            }
        }
        
        public static void PlayOnce(Song song)
        {
            Play(song);
            MediaPlayer.IsRepeating = false;
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
            if (_fadingOut)
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

            MediaPlayer.Volume = _currentVolume;
        }
    }
}