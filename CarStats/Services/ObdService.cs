using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using HSDHelper.Utils;
using Template10.Mvvm;

namespace HSDHelper.Services {
    public abstract class ObdService : ViewModelBase, IObdService {
        private int _currentProtocolNumber = 8;
        protected string _lastSentHeader;
        protected bool _lastSentHeaderWasCustom;
		protected Logger _obdLogger = new Logger();

	    public async Task SaveLog(string fileName)
	    {
		    await _obdLogger.Save(fileName);
	    }
		public abstract Task<string> Send(string command);
        public abstract Task<bool> Connect(string portName);

        public async Task<bool> Initialize() {
            try {
                ConnectionState = ConnectionStateEnum.Initializing;
                var response = await Send("ATZ");
                if (response == null)
                    throw new Exception("ATZ failed");

                response = await Send("ATE0"); // echo off
                if (response == null)
                    throw new Exception("ATE0 failed");

                response = await Send("ATL0"); // line feeds off
                if (response == null)
                    throw new Exception("ATL0 failed");

                response = await Send("ATH1"); // turn on headers
                if (response == null)
                    throw new Exception("ATH1 failed");
                ConnectionState = ConnectionStateEnum.Connected;
                return true;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            ConnectionState = ConnectionStateEnum.Disconnected;
            return false;
        }

        private ConnectionStateEnum _connectionState;
        public ConnectionStateEnum ConnectionState {
            get { return _connectionState; }
            protected set {
                _connectionState = value;
                RaisePropertyChanged();
            }
        }
        public async Task<ObdResponse> Send(ObdRequest request) {

            if (!string.IsNullOrEmpty(request.Header)) {
                if (_lastSentHeader != request.Header) {
                    var reply = await Send("ATSH " + request.Header);
                    _lastSentHeader = request.Header;
                    _lastSentHeaderWasCustom = true;
                }
            }
            else if (_lastSentHeaderWasCustom) {
                var reply = await Send(GetDefaultHeader());
                _lastSentHeader = "";
                _lastSentHeaderWasCustom = false;
            }
            var requestReply = await Send(request.Command);
            return new ObdResponse(requestReply);

        }
        private string GetDefaultHeader() {
            switch (_currentProtocolNumber) {
                case 1:
                    return "ATSH 61 6A F1";
                case 2:
                case 3:
                    return "ATSH 68 6A F1";
                case 4:
                case 5:
                    return "ATSH C2 33 F1";
                case 6:
                case 8:
                    return "ATSH 7DF";
                case 7:
                case 9:
                    return "ATSH DB 33 F1";
                case 11:
                    return "ATSH 81 10 FC";
            }
            return "ATSH 00";
        }

    }
}