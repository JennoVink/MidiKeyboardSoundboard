using System.Text;

namespace PureMidi.CoreMmSystem.MidiIO.Exceptions
{
    internal sealed class InputDeviceException : MidiDeviceException
    {
       
        public InputDeviceException(int errCode) : base(errCode)
        {
            WindowsMultimediaDevice.midiInGetErrorText(errCode, ErrMsg, ErrMsg.Capacity);
        }       
        
    }
}

