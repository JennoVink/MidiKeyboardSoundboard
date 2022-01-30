using System;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace MidiKeyboardSoundboard.Model
{
    /// <summary>
    ///     An entry representing a sound and a key (on the midi keyboard/pad).
    /// </summary>
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
    }
}