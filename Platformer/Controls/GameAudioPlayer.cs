using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Platformer.Controls
{
    class GameAudioPlayer : MediaPlayer
    {
        double Volume = Config.GameSoundVolume;

        

        public GameAudioPlayer()
        {
            Volume = Config.GameSoundVolume;
        }

        //MediaPlayer Jump;
        //MediaPlayer GameMusic;

        //public GameAudioPlayer()
        //{
        //    GameMusic = new MediaPlayer();
        //    GameMusic.Volume = Config.GameSoundVolume;
        //}

        //public void PlayStopGameMusic()
        //{
        //    GameMusic.Open(Config.GameMusic);
        //    GameMusic.Play();
        //}

        //public void LoadMusic(Uri MusicSourceUri)
        //{
        //    GameMusic.Open(MusicSourceUri);
        //}

        //public void PlaySound()
        //{
            
        //}
    }
}
