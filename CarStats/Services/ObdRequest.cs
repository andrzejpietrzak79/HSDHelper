namespace HSDHelper.Services
{
	public class ObdRequest {
		/// <summary>
		/// <para>Force providing header and command. This is required to set up OBD header to talk to only one ECU. This speeds up OBD communication.</para>
		/// <para>Command should also specify how many frames of response are expected. This allows the OBD interface to skip the normal timeout period when it waits for a while to let all the ECUs respond</para>
		/// <para>Since we're always talking to one ECU, there is no need to wait.</para>
		/// </summary>
		/// <param name="header">ECU id</param>
		/// <param name="command">OBDII command</param>
		public ObdRequest(string header, string command) {
			Header = header;
			Command = command;
		}
		public string Header { get; private set; }
		public string Command { get; private set; }
	}
}