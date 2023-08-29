
namespace Knv.Serial
{
    using System;
    using System.IO.Ports;
    using System.Threading;

    public class Serial: IDisposable
    { 
        readonly SerialPort _serialPort;
        bool _disposed = false;
        readonly bool _simualtion = false;

        public string LastErrorMessage { get; set; }

        public Serial(string port, int baudrate, bool simulation)
        {
            _simualtion = simulation;
            if(_simualtion)
                return;
            _serialPort = new SerialPort(port);
            _serialPort.BaudRate = baudrate;
            _serialPort.ReadTimeout = 1000;
            _serialPort.Open();
            _serialPort.DiscardInBuffer();
        }


        public void Write(byte[] data)
        { 
            if(_simualtion)
                return;
            _serialPort.Write(data, 0, data.Length);
        }

        public void Read(out byte[] data, int count)
        { 
            if(_simualtion)
            {
                data = new byte[count];
                for(int i = 0; i < count; i++)
                    data[i] = (byte)new Random().Next(0, 255);
                return;
            }
            try
            {
                _serialPort.Read(data = new byte[count], 0, count);
            }
            catch (Exception ex)
            { 
                LastErrorMessage = ex.Message;
                data = new byte[count];
            }
        }

        public int Read()
        {
            if (_simualtion)
                return new Random().Next(0, 255);
            int i = 0;
            try
            {
                i = _serialPort.ReadByte();
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
            }
            return i;
        }   

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if(_serialPort != null)
                  if(_serialPort.IsOpen)
                    _serialPort.Close();
                _serialPort?.Dispose();
            }
            _disposed = true;
        }
    }
}
