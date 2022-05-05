using System;
using Newtonsoft.Json;

namespace MidiKeyboardSoundboard.Model
{
    /// <summary>
    ///     A midi button is a wrapper around a soundEntry.
    ///     For now, this construction is reasonable. I would really like that the soundEntry is a field (instead of an
    ///     inheritance relationship).
    ///     Because the SoundEntry has a filepath I didn't do that. Please raise an issue on github to start a discussion about
    ///     this.
    /// </summary>
    public class MidiButton : SoundEntry
    {
        public MidiButton()
        {
        }

        public MidiButton(Uri soundEntry) : base(soundEntry)
        {
        }

        public MidiButton(string name, string midiKey = "", bool isHidden = false)
        {
            Name = name;
            MidiKey = midiKey;
            IsHidden = isHidden;
        }

        public int Id { get; set; }

        [JsonIgnore] public bool IsRecording { get; set; }

        public string MidiKey { get; set; } = "(none)";

        /// <summary>
        ///     Press and hold the key for sound. Otherwise, a simple 'tap' is needed to activate the sound.
        /// </summary>
        public bool PressAndHold { get; set; }


        /// <summary>
        ///     Standard buttons (volume knobs, stop button) are hidden by default.
        /// </summary>
        public bool IsHidden { get; set; }

        public string Name { get; set; }

        public virtual void ButtonPressed()
        {
            if (LoopSound && IsPlaying)
                Stop();
            else
                Play();
        }

        public void ButtonReleased()
        {
            if (PressAndHold)
                Stop();
        }
    }
}