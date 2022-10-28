using PureMidi.CoreMmSystem.MidiIO.Data;
using PureMidi.CoreMmSystem.MidiIO.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiDeviceMonitor
{
    /// <summary>
    /// Simple application that lists the available midi devices. Just to be sure that https://github.com/JennoVink/MidiKeyboardSoundboard/issues/16 it isn't a frontend bug.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var midiDevices = GetMidiDevices();

            if (midiDevices.Count > 0)
            {
                int midiDeviceIndex = ParseMidiIndex(midiDevices);

                var _inputDevice = new PureMidi.CoreMmSystem.MidiIO.InputDevice(midiDeviceIndex);
                _inputDevice.OnMidiEvent += OnMidiEventHandle;
                _inputDevice.Start();

                Console.WriteLine("Listening... Press any key on the midi keyboard.");
                Console.WriteLine("Press ctrl + c to exit...");


                while (true)
                {
                    Thread.Sleep(2000); // poor design.
                }
            }
        }

        private static int ParseMidiIndex(List<MidiInInfo> midiDevices)
        {
            if (midiDevices.Count == 1)
            {
                return midiDevices.First().DeviceIndex;
            }

            Console.WriteLine("Mulitiple devices found:");
            for (int i = 0; i < midiDevices.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {midiDevices[i].ProductName}");
            }
            Console.WriteLine("Please enter a number and press enter...");
            var response = Console.ReadLine();
            if (int.TryParse(response, out int result))
            {
                if (result >= 1 && result <= midiDevices.Count)
                {
                    return midiDevices[result - 1].DeviceIndex;
                }
            }
            Console.WriteLine($"Option {response} invalid... Please restart the program to try again. Press enter to exit.");
            Console.ReadLine();
            Environment.Exit(13);
            return -1;
        }

        private static void OnMidiEventHandle(MidiEvent ev)
        {
            var note = ev.AllData[1].ToString("X2").ToUpper();
            Console.WriteLine($"note {note} {ev.PressedOrReleased}: {ev}");
        }

        private static List<MidiInInfo> GetMidiDevices()
        {
            Console.WriteLine("Scanning connected midi devices...");

            var inp = MidiInInfo.Informations;
            foreach (var midiInInfo in inp)
            {
                Console.WriteLine($"Midi device found: {midiInInfo.ProductName}");
            }

            if (!inp.ToList().Any())
            {
                Console.WriteLine("No midi devices found");
            }
            return inp.ToList();
        }

    }
}
