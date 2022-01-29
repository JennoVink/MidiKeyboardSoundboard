using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MidiKeyboardSoundboard.ViewModel
{
    /// <summary>
    /// A simple wrapper for managing a collection of midi buttons.
    /// Exposes a nice abstraction for several default buttons (sound, stop, volume knob).
    /// </summary>
    public class MidiButtonCollection : ObservableCollection<MidiButton>
    {
        /// <summary>
        /// Note: this is an abstraction. Used to store IsRecording. In contrary to the stop button and volume knob, which do represents physical midi inputs.
        /// </summary>
        public MidiButton SoundButton => this.First(x => x.Name == "sound");
        public MidiButton StopButton => this.First(x => x.Name == "stop");
        public MidiButton VolumeKnob => this.First(x => x.Name == "volume");

        /// <summary>
        /// Note that this also manages the ID of the <paramref name="button"/>.
        /// </summary>
        public new void Add(MidiButton button)
        {
            if (Count != 0)
            {
                button.Id = this.Max(x => x.Id) + 1;
            }
            base.Add(button);
        }
    }
}