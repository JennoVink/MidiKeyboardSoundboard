using System;
using System.IO;
using System.Timers;
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
            Player.MediaEnded += Media_Ended;
            ButtonFlashAnimationTimer = new Timer(100)
            {
                AutoReset = false
            };
            ButtonFlashAnimationTimer.Elapsed += (sender, args) =>
            {
                RaisePropertyChanged(nameof(ButtonFlashAnimationTimer));
            };
        }

        public SoundEntry() : this(new Uri(@"c:\"))
        {
        }

        public Uri SoundPath { get; set; }

        [JsonIgnore] public string FileName => Path.GetFileName(SoundPath.ToString());
        [JsonIgnore] public MediaPlayer Player { get; set; }
        [JsonIgnore] public bool IsPlaying { get; set; }
        [JsonIgnore] public Timer ButtonFlashAnimationTimer { get; set; }

        public bool LoopSound { get; set; }

        public void Play()
        {
            IsPlaying = true;
            ButtonFlashAnimationTimer.Start();
            RaisePropertyChanged(nameof(ButtonFlashAnimationTimer));
            Application.Current.Dispatcher.Invoke(() =>
            {
                Player.Open(SoundPath);
                Player.Play();
            });
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            IsPlaying = false;

            if (!LoopSound) return;

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
        ///     Note that the volume has to be between 0 and 100.
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(int volume)
        {
            Application.Current.Dispatcher.Invoke(() => { Player.Volume = volume / 100.0f; });
        }
    }
}