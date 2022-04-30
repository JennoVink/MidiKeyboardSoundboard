using System;
using System.IO;
using PureMidi.CoreMmSystem.MidiIO.Data;

namespace MidiKeyboardSoundboard.ViewModel
{
    /// <summary>
    /// Note that we don't do expensive logging (lots of logs/sec). That's why a stream isn't kept open (File.AppendAllxxx opens+closes a stream internally). That overhead is neglectable.
    /// </summary>
    public class SimpleFileLogger
    {
        private bool _isRecording;

        private readonly string _outputPath;

        public SimpleFileLogger(string outputFolder)
        {
            _outputPath = Path.Combine(outputFolder, $"MidiKeyboardDebugLog {DateTime.Now:yy-MM-dd}.log");
        }

        /// <summary>
        /// Gets the current status of the logger to show to the user. 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _isRecording
            ? "Debug recording started"
            : $"Debug recording finished! Checkout the file in {_outputPath}";

        public void ToggleDebugRecording()
        {
            _isRecording = !_isRecording;
            Log(ToString());
        }

        public void Log(string line)
        {
            Console.WriteLine(line);
            if (_isRecording)
                File.AppendAllText(_outputPath, $@"[{DateTime.Now:HH:mm:ss.fff}] {line + Environment.NewLine}");
        }

        public void LogDebug(MidiEvent ev)
        {
            var note = ev.AllData[1].ToString("X2").ToUpper();
            Log($"note {note} {ev.PressedOrReleased}:");
            Log($"Raw data: {ev.Hex} |  {ev.Status.ToString("X2").ToUpper()}  |  {(ev.Status & 0xF0).ToString("X2").ToUpper()} | {((ev.Status & 0x0F) + 1).ToString("X2").ToUpper()} |  {ev.AllData[1].ToString("X2").ToUpper()}   |  {ev.AllData[2].ToString("X2").ToUpper()}   | ");
        }
    }
}