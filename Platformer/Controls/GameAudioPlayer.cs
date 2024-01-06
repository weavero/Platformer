using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Platformer.Controls
{
    class GameAudioPlayer : MediaPlayer
    {
        double gameVolume = Config.GameSoundVolume;

        public GameAudioPlayer()
        {
            gameVolume = Config.GameSoundVolume;
        }

        public void PlayStopGameMusic()
        {
            Open(Config.GameMusic);
            Play();
        }

        public void LoadMusic(Uri MusicSourceUri)
        {
            Open(MusicSourceUri);
        }

        public void PlaySound()
        {

        }
    }
}
