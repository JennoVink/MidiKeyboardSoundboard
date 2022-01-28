using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Media;

namespace MidiKeyboardSoundboard.ViewModel
{
    /// <summary>
    /// An entry representing a sound and a key (on the midi keyboard/pad).
    /// </summary>
    public class SoundEntry : ObservableObject
    {
        public int Id { get; set; }
        public string KeyId { get; set; }
        public Uri SoundPath { get; set; }

        [JsonIgnore]
        public MediaPlayer Player { get; set; }

        [JsonConstructor]
        public SoundEntry(int id, string keyId, Uri soundPath)
        {
            Id = id;
            KeyId = keyId;
            SoundPath = soundPath;
            Player = new MediaPlayer();
        }

        public SoundEntry(int id) : this(id, "", new Uri(@"c:\")) { }

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                Player.Stop();
            });
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
