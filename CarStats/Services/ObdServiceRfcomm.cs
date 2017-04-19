using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HSDHelper.Services {
	/// <summary>
	/// This class implements communication over Bluetooth
	/// </summary>
	public class ObdServiceRfcomm : ObdService {
		public static IObdService Instance { get; } = new ObdServiceRfcomm();

		private StreamSocket _socket = new StreamSocket();
		private readonly object sendLock = new object();

		public override async Task<bool> Connect(string portName) {
			try
			{
				ConnectionState = ConnectionStateEnum.Connecting;
				var devices =
					(await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort)))
						.ToList();
				if (devices.Count == 0)
				{
					ConnectionState = ConnectionStateEnum.Disconnected;
					return false;
				}

				var srv = devices.SingleOrDefault(x => x.Name == portName || x.Name == "SPP");
				if (srv == null)
				{
					ConnectionState = ConnectionStateEnum.Disconnected;
					return false;
				}
				var service = await RfcommDeviceService.FromIdAsync(srv.Id);
				_socket.Control.KeepAlive = true;
				await _socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName);
				Debug.WriteLine($"Connected to Rfcomm: {portName}");

				return true;
			}
			catch
			{
				ConnectionState = ConnectionStateEnum.Disconnected;
				return false;
			}
		}
		/// <summary>
		/// Sends a command to OBD interface over Bluetooth and returns the response
		/// </summary>
		/// <param name="command">OBDII command</param>
		/// <returns>Response to the command</returns>
		public override async Task<string> Send(string command) {
			var start = DateTime.Now;

			//await Task.Delay(50);
			if (ConnectionState != ConnectionStateEnum.Connected) return "";
			var array = command.ToCharArray();
			var byteArray = new byte[array.Length + 1];
			for (var i = 0; i < array.Length; ++i) {
				byteArray[i] = (byte)array[i];
			}
			byteArray[array.Length] = 13;
			var response = "";
			var responseLen = -1;
			try
			{
			await _socket.OutputStream.WriteAsync(GetBufferFromByteArray(byteArray));
			Debug.WriteLine($"SEND: {command}");
			_obdLogger.Append($"{command}---");
				do
				{
					var buffer = await _socket.InputStream.ReadAsync(new Windows.Storage.Streams.Buffer(1), 1, InputStreamOptions.None);
					response += (char) (buffer.ToArray()[0]);
					++responseLen;
				} while (response[responseLen] != '>'); // read until '>' encountered
			}
			catch
			{
				response = "";
				responseLen = 0;
			}
			var resp = response.Substring(0, responseLen).Trim();
			var end = DateTime.Now;

			Debug.WriteLine($"RCV: {resp}");

			_obdLogger.AppendLine($"{response.Replace("\r","\\r").Replace("\n", "\\n")}>---{(end - start).TotalMilliseconds} ms");
			return response;
		}

		/// <summary>
		/// Converts byte array to IBuffer
		/// </summary>
		/// <param name="source">Source byte array</param>
		/// <returns>IBuffer containing source bytes</returns>
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

