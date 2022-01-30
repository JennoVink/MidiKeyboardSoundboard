using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AudioSwitcher.AudioApi.CoreAudio;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using MidiKeyboardSoundboard.Model;
using MidiKeyboardSoundboard.Properties;
using Newtonsoft.Json;
using PureMidi.CoreMmSystem.MidiIO.Data;
using PureMidi.CoreMmSystem.MidiIO.Definitions;
using PureMidi.CoreMmSystem.MidiIO.DeviceInfo;
using InputDevice = PureMidi.CoreMmSystem.MidiIO.InputDevice;

namespace MidiKeyboardSoundboard.ViewModel
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly CoreAudioDevice _defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        private InputDevice _inputDevice;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Console.WriteLine(Settings.Default.SoundboardSettings);

            OpenConnectionCommand = new RelayCommand(OpenConnectionCommandExecuted);
            SelectSoundPathCommand = new RelayCommand<string>(SelectSoundPathCommandExecuted);
            RecordButtonCommand = new RelayCommand<string>(RecordButtonCommandExecuted);
            RemoveSoundCommand = new RelayCommand<string>(RemoveSoundCommandExecuted);
            AddNewKeyCommand = new RelayCommand(AddNewKeyCommandExecuted);
            RecordStopButtonCommand = new RelayCommand(RecordStopButtonCommandExecuted);
            RecordVolumeKnobCommand = new RelayCommand(RecordVolumeKnobCommandExecuted);

            MidiButtons = TryLoadSettings() ?? MidiButtons;

            MonitorLoad(this, EventArgs.Empty);
        }

        public bool IsConnected { get; set; }

        public MidiButtonCollection MidiButtons { get; set; }

        public int RecordingForSoundEntryId { get; set; } = -1;

        public ICommand OpenConnectionCommand { get; set; }
        public RelayCommand<string> SelectSoundPathCommand { get; set; }
        public RelayCommand<string> RecordButtonCommand { get; set; }
        public RelayCommand<string> RemoveSoundCommand { get; set; }
        public ICommand AddNewKeyCommand { get; set; }
        public ICommand RecordStopButtonCommand { get; set; }

        public bool IgnoreAutoSensingSignals { get; set; }

        public ObservableCollection<MidiInInfo> InputDevices { get; set; } = new ObservableCollection<MidiInInfo>();

        public MidiInInfo SelectedInputDevice { get; set; }

        public ICommand WindowClosing => new RelayCommand<CancelEventArgs>(SaveSettings);

        public ICommand RecordVolumeKnobCommand { get; set; }

        private MidiButtonCollection TryLoadSettings()
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.SoundboardSettings))
            {
                var deserialized = JsonConvert.DeserializeObject<MidiButtonCollection>(Settings.Default
                    .SoundboardSettings);

                if (deserialized.SoundButton == null)
                    deserialized.AddRange(DefaultButtonFactory.StartStopVolume);

                return deserialized;
            }

            return null;
        }

        private void RecordVolumeKnobCommandExecuted()
        {
            MidiButtons.VolumeKnob.IsRecording = true;
            RaisePropertyChanged(nameof(MidiButtons.VolumeKnob.IsRecording));
        }

        private void SetVolume(int volume)
        {
            foreach (var soundEntry in MidiButtons)
            {
                soundEntry.SetVolume(volume);
            }
        }

        private void RecordStopButtonCommandExecuted()
        {
            MidiButtons.StopButton.IsRecording = true;
            RaisePropertyChanged(nameof(MidiButtons.StopButton.IsRecording));
        }

        private void SaveSettings(CancelEventArgs cancelEventArgs)
        {
            Settings.Default.SoundboardSettings = JsonConvert.SerializeObject(MidiButtons);
            Settings.Default.Save();
        }

        private void AddNewKeyCommandExecuted()
        {
            MidiButtons.Add(new MidiButton());
        }

        private void RemoveSoundCommandExecuted(string soundId)
        {
            MidiButtons.Remove(MidiButtons.First(x => x.Id == int.Parse(soundId)));
        }

        private void SelectSoundPathCommandExecuted(string id)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".mp3",
                Filter = "mp3 Files (*.mp3)|*.mp3|WAV Files (*.wav)|*.wav|Midi Files (*.mid)|*.mid",
                Multiselect = true
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                MidiButtons.First(x => x.Id == int.Parse(id)).SoundPath = new Uri(dialog.FileName);
                foreach (var fileName in dialog.FileNames.Skip(1)) MidiButtons.Add(new MidiButton(new Uri(fileName)));
            }
        }

        private void OpenConnectionCommandExecuted()
        {
            if (!IsConnected)
                SwitchMonitorOn();
            else
                SwitchMonitorOff();
        }

        private void SwitchMonitorOn()
        {
            SwitchMonitorOff();
            if (SelectedInputDevice != null)
            {
                _inputDevice = new InputDevice(SelectedInputDevice.DeviceIndex);
                _inputDevice.OnMidiEvent += OnMidiEventHandle;
                _inputDevice.Start();

                IsConnected = true;
            }
            else
            {
                MessageBox.Show("input device must be selected.");
            }
        }

        private void OnMidiEventHandle(MidiEvent ev)
        {
            if (ev.MidiEventType == EMidiEventType.Short)
            {
                Console.WriteLine(ev.Hex + " |  "
                                         + ev.Status.ToString("X2").ToUpper() + "  |  " +
                                         (ev.Status & 0xF0).ToString("X2").ToUpper() + " | " +
                                         ((ev.Status & 0x0F) + 1).ToString("X2").ToUpper() + " |  " +
                                         ev.AllData[1].ToString("X2").ToUpper() + "   |  " +
                                         ev.AllData[2].ToString("X2").ToUpper() + "   | ");

                // ignore autosensing
                if (ev.Hex == "FE0000" && IgnoreAutoSensingSignals)
                    return;

                var pressedMidiKey = ev.AllData[1].ToString("X2");
                if (MidiButtons.VolumeKnob.IsRecording)
                {
                    MidiButtons.VolumeKnob.MidiKey = pressedMidiKey;
                    MidiButtons.VolumeKnob.IsRecording = false;
                    RaisePropertyChanged(nameof(MidiButtons.VolumeKnob.IsRecording));
                }

                if (MidiButtons.StopButton.IsRecording)
                {
                    MidiButtons.StopButton.MidiKey = pressedMidiKey;

                    Debug.WriteLine($"Stop button recording ended {ev.AllData[1]:X2}");

                    MidiButtons.StopButton.IsRecording = false;
                    RaisePropertyChanged(nameof(MidiButtons.StopButton.IsRecording));
                }
                else if (MidiButtons.SoundButton.IsRecording)
                {
                    MidiButtons.SoundButton.IsRecording = false;
                    Debug.WriteLine($"recording ended {pressedMidiKey}");

                    MidiButtons.First(x => x.Id == RecordingForSoundEntryId).MidiKey = pressedMidiKey;

                    // The frontend needs a MultiBinding, checking if RecordingForSoundEntryId equal is to the current Id of the SoundEntry.
                    // In the most optimal solution, the IsRecording["sound"] is also checked in the FE. Unfortunately this gave some problems.
                    // That's why it is set to -1.
                    RecordingForSoundEntryId = -1;
                    RaisePropertyChanged(nameof(MidiButtons.SoundButton.IsRecording));
                }
                else if (ev.Status == 144) // pressed button
                {
                    PlaySound(pressedMidiKey);
                }
                else if (pressedMidiKey == MidiButtons.StopButton.MidiKey)
                {
                    StopAllSounds();
                }
                else if (pressedMidiKey == MidiButtons.VolumeKnob.MidiKey)
                {
                    SetVolume((int)(int.Parse(ev.AllData[2].ToString()) / 127.0 * 100));
                }
            }
        }


        private void RecordButtonCommandExecuted(string soundEntryId)
        {
            MidiButtons.SoundButton.IsRecording = true;
            RecordingForSoundEntryId = int.Parse(soundEntryId);
            RaisePropertyChanged(nameof(MidiButtons.SoundButton.IsRecording));

            Debug.WriteLine("recording started");
        }

        private void PlaySound(string midiKeyIndex)
        {
            MidiButtons.FirstOrDefault(x => x.MidiKey == midiKeyIndex)?.Play();
        }

        private void StopAllSounds()
        {
            foreach (var soundEntry in MidiButtons) soundEntry.Stop();
        }

        public void SwitchMonitorOff()
        {
            if (_inputDevice != null && !_inputDevice.IsDisposed) _inputDevice.Dispose();
            IsConnected = false;
        }

        private void MonitorLoad(object sender, EventArgs e)
        {
            var inp = MidiInInfo.Informations;
            foreach (var midiInInfo in inp) InputDevices.Add(midiInInfo);

            SelectedInputDevice = InputDevices.FirstOrDefault();
        }
    }
}