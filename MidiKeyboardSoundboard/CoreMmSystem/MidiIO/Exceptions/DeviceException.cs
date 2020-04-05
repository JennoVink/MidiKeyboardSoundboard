using System;

namespace PureMidi.CoreMmSystem.MidiIO.Exceptions
{
    internal class DeviceException : ApplicationException
    {
        private readonly int _errorCode;

        public DeviceException(int errorCode)
        {
            _errorCode = errorCode;
        }

        public int ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }
    }
}

