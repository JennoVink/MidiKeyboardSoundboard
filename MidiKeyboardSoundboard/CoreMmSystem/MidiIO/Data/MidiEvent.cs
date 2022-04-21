using System.Text;
using PureMidi.CoreMmSystem.MidiIO.Definitions;

namespace PureMidi.CoreMmSystem.MidiIO.Data
{
    public class MidiEvent
    {
        public MidiEvent(byte[] data)
        {
            AllData = data;
        }

        public readonly byte[] AllData;

        public string Hex
        {
            get
            {
                var sb = new StringBuilder();
                for (int i = 0; i < AllData.Length; i++)
                {
                    sb.Append(AllData[i].ToString("X2").ToUpper());
                }
                return sb.ToString();
            }
        }

        public byte Status => AllData[0];

        /// <summary>
        /// Returns "pressed", "released" or the status (if not pressed or released).
        /// </summary>
        public string PressedOrReleased => Status == 144 ? "pressed" : Status == 128 ? "released" : Status.ToString("X2");

        public EMidiEventType MidiEventType
        {
            get
            {
                switch (AllData[0])
                {
                    case 0xFF:
                        return EMidiEventType.Meta;
                    case 0xF0:
                        return EMidiEventType.Sysex;
                    case 0xF7:
                        return EMidiEventType.Sysex;
                    case 0:
                        return EMidiEventType.Empty;
                    default:
                        return EMidiEventType.Short;
                }
            }
        }

    }
}