using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MidiKeyboardSoundboard.Model;

namespace MidiKeyboardSoundboard.ViewModel
{
    /// <summary>
    ///     A simple wrapper for managing a collection of midi buttons.
    ///     Exposes a nice abstraction for several default buttons (sound, stop, volume knob).
    /// </summary>
    public class MidiButtonCollection : ObservableCollection<MidiButton>
    {
        /// <summary>
        ///     Note: this is an abstraction. Used to store IsRecording. In contrary to the stop button and volume knob, which do
        ///     represents physical midi inputs.
        /// </summary>
        public MidiButton SoundButton => this.FirstOrDefault(x => x.Name == "sound");

        public MidiButton StopButton => this.FirstOrDefault(x => x.Name == "stop");
        public MidiButton VolumeKnob => this.FirstOrDefault(x => x.Name == "volume");

        /// <summary>
        ///     Why does the ObservableCollection doesn't have an add range method? I don't know.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>The new collection</returns>
        public void AddRange(IEnumerable<MidiButton> items)
        {
            foreach (var midiButton in items) Add(midiButton);
        }

        /// <summary>
        ///     Note that this also manages the ID of the <paramref name="button" />.
        /// </summary>
        public new void Add(MidiButton button)
        {
            if (Count != 0) button.Id = this.Max(x => x.Id) + 1;
            base.Add(button);
        }
    }
}