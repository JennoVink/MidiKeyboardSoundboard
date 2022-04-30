using System;
using System.Collections.Generic;

namespace MidiKeyboardSoundboard.Model
{
    /// <summary>
    ///     Factory for the start and stop knob + volume button.
    ///     Maybe in the future I'll add sound effects (like distortion, attack, etc).
    ///     This design will allow polymorphic behaviour.
    /// </summary>
    public static class DefaultButtonFactory
    {
        public static List<MidiButton> SoundStopVolume => new List<MidiButton>
        {
            new MidiButton("sound", isHidden: true),
            new MidiButton("stop", "03", true),
            new MidiButton("volume", "02", true)
        };

        public static IList<MidiButton> SoundAndVolumeButton => new List<MidiButton> { new MidiButton("sound", isHidden: true), new MidiButton("volume", "02", true) };

        public static MidiButton StopButton(Action action) => new CustomActionButton("stop", "03", true, action);
    }
}