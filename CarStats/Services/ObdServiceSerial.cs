using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace HSDHelper.Services {
	/// <summary>
	/// This class is used for testing on a PC.
	/// Since in UWP we can't access virtual COM port, this connector connects to localhost at port 8000. There is a service which listens on this port and routes messages between virtual COM and this app.
	/// </summary>
    public class ObdServiceSerial : ObdService {
        public static IObdService Instance { get; } = new ObdServiceSerial();

        private Socket _socket = null;
        public override async Task<bool> Connect(string portName) {
            ConnectionState = ConnectionStateEnum.Connecting;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.Connect("127.0.0.1", 8000);
                return true;
            }
            catch (Exception ex)
            {
                
            }
            ConnectionState = ConnectionStateEnum.Disconnected;
            return false;
        }

        public override async Task<string> Send(string command) {
            var array = command.ToCharArray();
            var byteArray = new byte[array.Length + 1];
            for (var i = 0; i < array.Length; ++i) {
                byteArray[i] = (byte)array[i];
            }
            byteArray[array.Length] = 13;
            _socket.Send(byteArray);

            var buffer = new byte[128];
            var bufferLen = _socket.Receive(buffer);
            string result = System.Text.Encoding.UTF8.GetString(buffer);

            return result;
        }

   


        private IBuffer GetBufferFromByteArray(byte[] source) {
            IBuffer result;
            using (var writer = new DataWriter()) {
                writer.WriteBytes(source);
                result = writer.DetachBuffer();
            }
            return result;
        }
    }
}