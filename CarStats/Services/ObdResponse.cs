using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using HSDHelper.Utils;
using Microsoft.HockeyApp;

namespace HSDHelper.Services
{
    public class ObdResponse {
		/// <summary>
		/// Holds the parsed response
		/// </summary>
        private byte[] _data = new byte[128];

	    /// <summary>
		/// <para>Ensures that response string is well formatted. Depending on OBD initialization response may not contain spaces. This method ensures that there is a space between each hex byte.</para>
		/// <para>&quot;7EA 06410F0102 03 04&quot; will be converted to &quot;07 EA 06 41 0F 01 02 03 04&quot;</para>
		/// </summary>
		/// <param name="line">Input line</param>
		/// <returns>A well formed string representing the response</returns>
		private string EnsureLineFormat(string line) {
            line = line.Trim().Replace(" ", ""); // trim and remove spaces
            if (line.Length % 2 == 1) {
                line = "0" + line;
            }
            var sb = new StringBuilder();
            for (var i = 0; i < line.Length; ++i) {
                sb.Append(line[i]);
                if (i % 2 == 1)
                    sb.Append(' ');
            }
            return sb.ToString().TrimEnd();
        }
		/// <summary>
		/// Parse OBD request reply and construct OBD response. See https://www.elmelectronics.com/DSheets/ELM327DS.pdf pages 38, 43 for format specification
		/// There is an assumption here that only one ECU responds. OBDRequest enforces that header is always set.
		/// </summary>
		/// <param name="requestReply">Data received from OBD interface</param>
		public ObdResponse(string requestReply) {
            try
            {
	            IsValid = true; // will be set to false if anything goes wrong
                var lines = requestReply.Replace("\r\r>", "")
                    .Replace("\n", "")
                    .Split(new[] {'\r'}, StringSplitOptions.RemoveEmptyEntries);

                var numBytes = 0;
                foreach (var line in lines)
                {
                    var l = EnsureLineFormat(line);
                    var strings = l.Split(' ');

                    //0  1  2  3  4  5  6  7  8  
                    //07 EA 06 41 0F 01 02 03 04
                    if (strings[2][0] == '0') // single frame response, for example 
                    {
                        numBytes = strings[2][1] - '0';
                        for (var i = 0; i < numBytes - 2; ++i)
                        {
                            _data[i] = Convert.ToByte(strings[i + 5], 16);
                        }
                    }
                    else
                    {
                        //0  1  2  3  4  5  6  7  8  9

                        //07 EA 10 18 61 01 01 02 03 04 \r
                        //07 EA 21 05 06 07 08 09 0A 0B \r
                        //07 EA 22 0C 0D 0E 0F 10 11 12 \r
                        //07 EA 23 13 14 15 16 17 18 00 \r\r>";
                        if (strings[2][0] == '1') // first frame of multi frame response
                        {
                            numBytes = Convert.ToByte(strings[3], 16);
                            for (var i = 6; i <= 9; ++i) // data starts at 7th byte, 4 bytes of data
                            {
                                _data[i - 6] = Convert.ToByte(strings[i], 16);
                            }
                        }
                        else // following frame of multi frame response
                        {
                            var frameNum = strings[2][1] - '0'; // frames can be received in random order
                            for (var i = 3; i <= 9; ++i) // data starts at 4th byte, 7 bytes of data
                            {
                                var x = (frameNum - 1)*7 + i - 3 + 4;
                                if (x >= numBytes)
                                    break;
                                _data[x] = Convert.ToByte(strings[i], 16);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // ignore any errors
            {
                //var properties = new Dictionary<string, string>();
                //properties.Add("response", requestReply);
                //HockeyClient.Current.TrackException(ex, properties);
                //RuntimeLogger.Error($"OBDResponse constructor failed. Request reply was {requestReply}");
	            IsValid = false;
            }
        }
		/// <summary>
		/// True if response was successfully interpreted, false otherwise
		/// </summary>
	    public bool IsValid { get; set; }

		// using letters in formulas instead of indexes for improved readability. Adhere to convention used on https://en.wikipedia.org/wiki/OBD-II_PIDs and internet forums
		public byte A => _data[0];
        public byte B => _data[1];
        public byte C => _data[2];
        public byte D => _data[3];
        public byte E => _data[4];
        public byte F => _data[5];
        public byte G => _data[6];
        public byte H => _data[7];
        public byte I => _data[8];
        public byte J => _data[9];
        public byte K => _data[10];
        public byte L => _data[11];
        public byte M => _data[12];
        public byte N => _data[13];
        public byte O => _data[14];
        public byte P => _data[15];
        public byte Q => _data[16];
        public byte R => _data[17];
        public byte S => _data[18];
        public byte T => _data[19];
        public byte U => _data[20];
        public byte V => _data[21];
        public byte W => _data[22];
        public byte X => _data[23];
        public byte Y => _data[24];
        public byte Z => _data[25];
        public byte AA => _data[26];
        public byte AB => _data[27];
    }
}