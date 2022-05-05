using System;

namespace MidiKeyboardSoundboard.Model
{
    /// <summary>
    ///     A custom action button for e.g. stopping all the sounds.
    ///     No inheritance because the ViewModel needs to call the StopAll method (and that's defined there).
    /// </summary>
    public class CustomActionButton : MidiButton
    {
        private readonly Action _keyPressAction;

        public CustomActionButton(string name, string midiKey, bool isHidden, Action keyPressAction) : base(name,
            midiKey, isHidden)
        {
            _keyPressAction = keyPressAction;
        }

        public override void ButtonPressed()
        {
            _keyPressAction();
        }
    }
}