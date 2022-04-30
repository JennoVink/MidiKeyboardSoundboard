using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace MidiKeyboardSoundboard.Model
{
    public class SoundEntry : ObservableObject
    {
        public Uri SoundPath { get; set; }

        [JsonIgnore] public string FileName => Path.GetFileName(SoundPath.ToString());
        [JsonIgnore] public MediaPlayer Player { get; set; }
        [JsonIgnore] public bool IsPlaying { get; set; }

        public bool LoopSound { get; set; }


        [JsonConstructor]
        public SoundEntry(Uri soundPath)
        {
            SoundPath = soundPath;
            Player = new MediaPlayer();
            Player.MediaEnded += Media_Ended;
        }

        public SoundEntry() : this(new Uri(@"c:\"))
        {
        }

        public void Play()
        {
            IsPlaying = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Player.Open(SoundPath);
                Player.Play();
            });
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            IsPlaying = false;

            if (!LoopSound)
            {
                return;
            }

            IsPlaying = true; // to trigger the flashing animation.

            Player.Position = TimeSpan.Zero;
            Player.Play();
        }

        public void Stop()
        {
            IsPlaying = false;
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