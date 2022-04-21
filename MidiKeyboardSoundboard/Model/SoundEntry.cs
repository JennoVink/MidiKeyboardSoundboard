using System;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace MidiKeyboardSoundboard.Model
{
    public class SoundEntry : ObservableObject
    {
        [JsonConstructor]
        public SoundEntry(Uri soundPath)
        {
            SoundPath = soundPath;
            Player = new MediaPlayer();
        }

        public SoundEntry() : this(new Uri(@"c:\"))
        {
        }

        public Uri SoundPath { get; set; }

        [JsonIgnore] public MediaPlayer Player { get; set; }

        public void Play()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Player.Open(SoundPath);
                Player.Play();
            });
        }

        public void Stop()
        {
            Application.Current.Dispatcher.Invoke(() => { Player.Stop(); });
        }

        /// <summary>
        /// Note that the volume has to be between 0 and 100.
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(int volume)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Player.Volume = volume / 100.0f;
            });
        }

    }
}