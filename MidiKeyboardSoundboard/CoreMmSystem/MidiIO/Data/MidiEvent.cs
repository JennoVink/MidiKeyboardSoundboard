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

        public bool ButtonPressed => Status == 144;
        public bool ButtonReleased => Status == 128;

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

        public override string ToString() => $"Raw data: {Hex} |  {Status.ToString("X2").ToUpper()}  |  {(Status & 0xF0).ToString("X2").ToUpper()} | {((Status & 0x0F) + 1).ToString("X2").ToUpper()} |  {AllData[1].ToString("X2").ToUpper()}   |  {AllData[2].ToString("X2").ToUpper()}   | ";


    }
}