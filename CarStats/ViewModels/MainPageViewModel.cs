using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using HSDHelper.Messages;
using HSDHelper.Models;
using HSDHelper.Services;
using HSDHelper.Utils;
using GalaSoft.MvvmLight.Messaging;

namespace HSDHelper.ViewModels {
    public class MainPageViewModel : ViewModelBase {
		private bool _isInitialized = false;
		private bool _timerRunning = false;
		private readonly Logger _telemetryLogger = new Logger();
		private Geolocator _geolocator = new Geolocator();
		public IObdService OBDService { get; private set; }
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

		public MainPageViewModel() {
			_telemetryLogger.AppendLine(
				$"now,ODO,Speed,Latitude,Longitude," +
				$"SpeedGPS,Altitude,BatteryVoltage,BatteryCurrent,StateOfCharge," +
				$"CoolantTemperature,RPMAverage,ICEPower,BrakeRegenTorque," +
				$"BrakeMastCylTorque,TripSeconds,TripEvSeconds,TripInMoveSeconds," +
				$"TripLength,TripEvLength,HSI,MG1RPM,IgnitionTiming," +
				$"LongTermFuelTrim,ShortTermFuelTrim,TripFuel,FuelFlowLitersPerHourAverage," +
				$"BatteryDischargeControl,BatteryChargeControl,gkwhAverage," +
				$"EngineLoad,InverterTemperature,BatteryTemperature," +
				$"MG1Temperature,MG2Temperature,AmbientTemperature"
				);
			if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
			}
			OBDService = ObdServiceRfcomm.Instance;

			_dt.Tick += TimerTickProc;
			_dt.Interval = TimeSpan.FromMilliseconds(500);
			CarState.HSDState = CarState.HSDStateEnum.S0;
		}

		/// <summary>
		/// Used to calculate distance
		/// </summary>
		private BasicGeoposition _prevPosition;
		
		/// <summary>
		/// Makes sure some code is executed every second time in timer handler
		/// </summary>
		private bool _flip = false;

		/// <summary>
		/// Timer that fires every 0.5s
		/// </summary>
		private readonly DispatcherTimer _dt = new DispatcherTimer();

		/// <summary>
		/// Updates car state values that are dependent on time (trip seconds, trip EV seconds, seconds while moving) and sends the message that forces graph to be refreshed every 0.5s
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerTickProc(object sender, object e) {
			_flip = !_flip;
			if (_flip)
			{
				var now = DateTime.Now;
				CarState.TripSeconds += 1;

				if (CarState.RPM < 900)
				{
					CarState.TripEvSeconds += 1;
				}
				if (CarState.Speed > 2)
				{
					CarState.TripInMoveSeconds += 1;
				}

				var logMessage =
					$"{now},{CarState.ODO},{CarState.Speed},{CarState.Latitude},{CarState.Longitude}," +
					$"{CarState.SpeedGPS},{CarState.Altitude},{CarState.BatteryVoltage},{CarState.BatteryCurrent},{CarState.StateOfCharge}," +
					$"{CarState.CoolantTemperature},{CarState.RPMAverage},{CarState.ICEPower},{CarState.BrakeRegenTorque}," +
					$"{CarState.BrakeMastCylTorque},{CarState.TripSeconds},{CarState.TripEvSeconds},{CarState.TripInMoveSeconds}," +
					$"{CarState.TripLength},{CarState.TripEvLength},{CarState.HSI},{CarState.MG1RPM},{CarState.IgnitionTiming}," +
					$"{CarState.LongTermFuelTrim},{CarState.ShortTermFuelTrim},{CarState.TripFuel},{CarState.FuelFlowLitersPerHourAverage}," +
					$"{CarState.BatteryDischargeControl},{CarState.BatteryChargeControl},{CarState.gkwhAverage}," +
					$"{CarState.EngineLoad},{CarState.InverterTemperature},{CarState.BatteryTemperature}," +
					$"{CarState.MG1Temperature},{CarState.MG2Temperature},{CarState.AmbientTemperature}";
				_telemetryLogger.AppendLine(logMessage);
			}
			Messenger.Default.Send(new DataRefreshedMessage(CarState));
		}
		
		/// <summary>
		/// Updates car state values related to GPS
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args) {
			CarState.SpeedGPS = args.Position.Coordinate.Speed != null ? (int) (args.Position.Coordinate.Speed*3600/1000) : 0;
			CarState.Altitude = args.Position.Coordinate.Point.Position.Altitude;
			CarState.Latitude = args.Position.Coordinate.Point.Position.Latitude;
			CarState.Longitude = args.Position.Coordinate.Point.Position.Longitude;

			var distance = CalcDistance(_prevPosition, args.Position.Coordinate.Point.Position);
			CarState.TripLength += distance;
			if (CarState.RPM < 900)
				CarState.TripEvLength += distance;
			_prevPosition = args.Position.Coordinate.Point.Position;
		}
	
		/// <summary>
		/// Converts degrees to radians
		/// </summary>
		/// <param name="deg">Degrees to convert to radians</param>
		/// <returns></returns>
		private double ToRadian(double deg) {
			// deg * pi /180
			return deg*0.01745329251994329576923690768489;
		}

		/// <summary>
		/// Calculates spherical distance between two points on Earth. Distance is measured along the great circle of the sphere
		/// </summary>
		/// <param name="pos1">First position</param>
		/// <param name="pos2">Second position</param>
		/// <returns>Spherical distance between first and second position on Earth</returns>
		private double CalcDistance(BasicGeoposition pos1, BasicGeoposition pos2) {
			if (pos1.Latitude == 0.0)
				return 0.0;
			var R = 6371.0;
			var dLat = ToRadian(pos2.Latitude - pos1.Latitude);
			var dLon = ToRadian(pos2.Longitude - pos1.Longitude);
			var a = Math.Sin(dLat/2)*Math.Sin(dLat/2) +
			        Math.Cos(ToRadian(pos1.Latitude))*Math.Cos(ToRadian(pos2.Latitude))*
			        Math.Sin(dLon/2)*Math.Sin(dLon/2);
			var c = 2*Math.Asin(Math.Min(1, Math.Sqrt(a)));
			var d = R*c;
			return d;
		}

		/// <summary>
		/// Connects to the OBD Interface
		/// </summary>
		public async void Reconnect() {
			await Connect();
			//NavigationService.Navigate(typeof(Views.SettingsPage), 0);
		}
		/// <summary>
		/// Connects to the OBD interface asynchronously
		/// </summary>
		/// <returns></returns>
		private async Task Connect() {
			if (OBDService.ConnectionState != ConnectionStateEnum.Connected)
				if (await OBDService.Connect("OBDII"))
				{
					await OBDService.Initialize();
				}

		}

		public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
			var dr = new Windows.System.Display.DisplayRequest();
			dr.RequestActive();
			var accessStatus = await Geolocator.RequestAccessAsync();
			if (accessStatus == GeolocationAccessStatus.Allowed)
			{
				_geolocator = new Geolocator() {ReportInterval = 1000};
				_geolocator.DesiredAccuracy = PositionAccuracy.High;
				_geolocator.PositionChanged += Geolocator_PositionChanged;
			}
			if (!_timerRunning)
			{
				_dt.Start();
				_timerRunning = false;
			}
			if (suspensionState.Any())
			{
			}
			await Connect();
			if (!_isInitialized)
			{
				InitSequence();
				_isInitialized = true;
			}
			CarState.TripSeconds = 0;
			CarState.TripEvSeconds = 0;
			CarState.TripFuel = 0;
			CarState.TripInMoveSeconds = 0;
			CarState.TripLength = 0;
			CarState.TripEvLength = 0;

			await Task.Factory.StartNew(ProcessSequence, TaskCreationOptions.LongRunning);
			//await Task.CompletedTask;
			RaisePropertyChanged();


		}
		public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending) {
			if (suspending)
			{
			}
			await Task.CompletedTask;
		}
		public override async Task OnNavigatingFromAsync(NavigatingEventArgs args) {
			args.Cancel = false;
			_tokenSource.Cancel();
			_dt.Stop();
			_timerRunning = false;
			await Task.CompletedTask;
		}

		/// <summary>
		/// Resets trip statistics
		/// </summary>
		public void Reset() {
			CarState.Reset();
		}

		/// <summary>
		/// Saves trip log to a file
		/// </summary>
		/// <returns></returns>
        public async Task SaveLog() {
            var now = DateTime.Now;
            var dateString = $"{now.Year}-{now.Month}-{now.Day}-{now.Hour}-{now.Minute}";
            await _telemetryLogger.Save($"{dateString}-telemetry.csv");
            //await OBDService.SaveLog($"{dateString}-obd.txt");
            // Get the logical root folder for all external storage devices.
        }
		/// <summary>
		/// CarState instance
		/// </summary>
        public CarState CarState { get; } = new CarState();

		/// <summary>
		/// List that holds all the PIDS to be queried
		/// </summary>
        private List<ObdSequenceItem> _obdSequence = new List<ObdSequenceItem>();
		/// <summary>
		/// Initializes the list of PIDS that are to be queried for.
		/// </summary>
        private void InitSequence() {
            // engine load
            //_obdSequence.Add(new ObdSequenceItem {
            //    Request = new ObdRequest("7E0","0104"),
            //    EveryNthTime = 1,
            //    SequenceCounter = 0, // block
            //    Prc = r => { Process0104_7E0(r); }
            //});

            // ------------------------------- 7C0 -------------------------------
            // tank level
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7C0","2129 1"),
                EveryNthTime = 99,
                IsEnabled = true,
                Prc = r => { Process2129_7C0(r); }
            });
            // HSI indicator
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7C0","212B 1"),
                EveryNthTime = 3,
                IsEnabled = false,
                Prc = r => { Process212B_7C0(r); }
            });

            // ------------------------------- 7E0 -------------------------------
            // ICE torque
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E0","2149 3"),
                EveryNthTime = 1,
                IsEnabled = true,
                Prc = r => { Process2149_7E0(r); }
            });
            // injector volume
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E0","213C 1"),
                EveryNthTime = 1,
                IsEnabled = true,
                Prc = r => { Process213C_7E0(r); }
            });
            // ------------------------------- 7E2 -------------------------------
            // speed, rpm, charge, coolant temperature
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2101 4"),
                EveryNthTime = 1,
                IsEnabled = true,
                Prc = r => { Process2101_7E2(r); }
            });
            // ice power, braking torques
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2146 5"),
                EveryNthTime = 1,
                IsEnabled = true,
                Prc = r => { Process2146_7E2(r); }
            });
            // voltage before boosting
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2174 2"),
                EveryNthTime = 1,
                IsEnabled = false,
                Prc = r => { Process2174_7E2(r); }
            });
            // battery current
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2198 2"),
                EveryNthTime = 1,
                IsEnabled = true,
                Prc = r => { Process2198_7E2(r); }
            });
            // MG1 temperature, MG1 rpm
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2161 1"),
                EveryNthTime = 2,
                IsEnabled = true,
                Prc = r => { Process2161_7E2(r); }
            });
            // MG2 temperature, MG2 rpm
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2162 1"),
                EveryNthTime = 2,
                IsEnabled = false,
                Prc = r => { Process2162_7E2(r); }
            });
            // inverter temperature
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2175 2"),
                EveryNthTime = 10,
                IsEnabled = true,
                Prc = r => { Process2175_7E2(r); }
            });
            // fan mode
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","219B 1"),
                EveryNthTime = 11,
                IsEnabled = true,
                Prc = r => { Process219B_7E2(r); }
            });
            // odometer
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2128 1"),
                EveryNthTime = 15,
                IsEnabled = false,
                Prc = r => { Process2128_7E2(r); }
            });
            // battery temperature, battery air temperature
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2187 2"),
                EveryNthTime = 29,
                IsEnabled = true,
                Prc = r => { Process2187_7E2(r); }
            });
            // inverter MG1 temperature
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2170 1"),
                EveryNthTime = 31,
                IsEnabled = false,
                Prc = r => { Process2170_7E2(r); }
            });
            // inverter MG2 temperature
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2171 1"),
                EveryNthTime = 31,
                IsEnabled = false,
                Prc = r => { Process2171_7E2(r); }
            });
            // battery voltage
            _obdSequence.Add(new ObdSequenceItem {
                Request = new ObdRequest("7E2","2181 4"),
                EveryNthTime = 30,
                IsEnabled = true,
                Prc = r => { Process2181_7E2(r); }
            });
        }

		#region PID Handlers
		private void Process2101_7E2(ObdResponse r) {
            CarState.EngineLoad = r.A / 2.55;
            CarState.AmbientTemperature = r.D - 40;
            CarState.CoolantTemperature = r.F - 40;
            CarState.RPM = (r.G * 256 + r.H) / 4;
            CarState.Speed = r.I;
            CarState.AcceleratorPedal = r.M/2.55;
            CarState.StateOfCharge = r.V / 2.0;
        }
        private void Process2128_7E2(ObdResponse r) { CarState.ODO = r.A * 256 * 256 + r.B * 256 + r.C; }
        private void Process2129_7C0(ObdResponse r) { CarState.TankLevel = r.A / 2.0f; }
        private void Process212B_7C0(ObdResponse r) { CarState.HSI = (r.A & 1) * 256 + r.B - (r.A & 2) * 512; }
        private void Process213C_7E0(ObdResponse r) { CarState.Inj_uL = (r.A * 256 + r.B) / 32.0f; }
        private void Process2146_7E2(ObdResponse r) {
            //CarState.ICEPower = (r.E * 256 + r.F) * 10 / 1000.0f;
            CarState.BrakeRegenTorque = (r.P * 256 + r.Q) / 8.0f - 4096;
            CarState.BrakeMastCylTorque = (r.R * 256 + r.S) / 8.0f - 4096;
        }
        private void Process2149_7E0(ObdResponse r) { CarState.ICETorque = r.D * 256 + r.E - 32768.0f; }
        private void Process2161_7E2(ObdResponse r) {
            CarState.MG1Temperature = r.A - 40;
            CarState.MG1RPM = r.D * 256 + r.E - 32768;
        }
        private void Process2162_7E2(ObdResponse r) {
            CarState.MG2Temperature = r.A - 40;
            CarState.MG2RPM = r.D * 256 + r.E - 32768;
        }
        private void Process2170_7E2(ObdResponse r) { CarState.MG1InverterTemperature = r.A - 40; }
        private void Process2171_7E2(ObdResponse r) { CarState.MG2InverterTemperature = r.A - 40; }
        private void Process2174_7E2(ObdResponse r) { CarState.BatteryVoltage = (r.F * 256 + r.G) / 2.0f; }
        private void Process2175_7E2(ObdResponse r) { CarState.InverterTemperature = r.D - 40; }
        private void Process2181_7E2(ObdResponse r) {

            CarState.BatteryVoltage = ((
                                r.A +
                                r.C +
                                r.E +
                                r.G +
                                r.I +
                                r.K +
                                r.M +
                                r.O +
                                r.Q +
                                r.S +
                                r.U +
                                r.W +
                                r.Y +
                                r.AA) * 256 +

                                r.B +
                                r.D +
                                r.F +
                                r.H +
                                r.J +
                                r.L +
                                r.N +
                                r.P +
                                r.R +
                                r.T +
                                r.V +
                                r.X +
                                r.Z +
                                r.AB

                                ) * 79.99f / 65535;

        }
        private void Process2187_7E2(ObdResponse r) {
            CarState.BatteryAirTemperature = (int)((r.A * 256 + r.B) * 255.9f / 65535 - 50);
            CarState.BatteryTemperature = (int)Math.Max((r.C * 256 + r.D) * 255.9f / 65535 - 50,
                    Math.Max(
                        (r.E * 256 + r.F) * 255.9f / 65535 - 50,
                        (r.G * 256 + r.H) * 255.9f / 65535 - 50
                    )
                );
        }
        private void Process2198_7E2(ObdResponse r) {
            CarState.BatteryCurrent = (r.A * 256 + r.B) / 100.0f - 327.68f;
            CarState.BatteryChargeControl = r.C / 2.0f - 64;
            CarState.BatteryDischargeControl = r.D / 2.0f - 64;
        }
        private void Process219B_7E2(ObdResponse r) { CarState.FanMode = r.B; }
        private void Process21A3_7B0(ObdResponse r) { CarState.BrakeColor = r.B > 0 ? CarState._redBrush : CarState._greyBrush; }
		#endregion

		/// <summary>
		/// Main loop of the program.
		/// <para>Each element in sequence, element is executed every nth time (defined for each element separately)</para>
		/// </summary>
		private async void ProcessSequence() {
            while (true) {
                try {
                    foreach (var item in _obdSequence) { // for each element in sequence
                        if (_tokenSource.IsCancellationRequested)
                            return;
                        if (!item.IsEnabled) // skip if element disabled
                            continue;
                        if (item.SequenceCounter == 1) { // if counter reached then 1 execute element
                            item.SequenceCounter = item.EveryNthTime; // reset sequence counter
                            if (OBDService.ConnectionState == ConnectionStateEnum.Connected) { // make sure the connection is active
                                var response = await OBDService.Send(item.Request); // send the OBD request
                                await Dispatcher.DispatchAsync(() => { // make sure the response is processed on the UI thread
                                    item.Prc(response);
                                });
                            }
							// test display if not connected to the OBD interface
                            else { 
                                await Dispatcher.DispatchAsync(() => {
                                    CarState.Speed = (int)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 50 + 50);
                                    CarState.RPM = (int)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 1000 + 2000);
                                    CarState.StateOfCharge = ((int)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 20 + 60) * 2) / 2.0f;
                                    CarState.CoolantTemperature = (int)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 35 + 65);
                                    CarState.InverterTemperature = (int)(Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 20 + 60);
                                    CarState.BatteryVoltage = 8 * 16;
                                    CarState.BatteryCurrent = (double)(Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 200);
                                    CarState.BatteryAirTemperature = (int)(Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 10 + 30);
                                    CarState.BatteryTemperature = (int)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 15 + 45);
                                    CarState.ICETorque = (double)(Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 50 + 100);
                                    CarState.ICEPower = (int)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 50 + 50);
                                    CarState.BrakeRegenTorque = (double)(Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 8 + 0);
                                    CarState.BrakeMastCylTorque = (double)(Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 8 + 0);
                                    CarState.FanMode = DateTime.Now.Second % 7;
                                    CarState.Inj_uL = Math.Max(0,(double)(Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Math.PI * 8 / 60) * 100 + 80));
                                    Messenger.Default.Send(new DataRefreshedMessage(CarState));
                                });
                                await Task.Delay(50);
                            }
                        }
                        else item.SequenceCounter -= 1; // sequence counter > 1, simply decrement
                    }
                    CarState._EvalBrake(); // evaluate braking since it needs to be done even if braking torques did not change
                }
                catch (Exception ex) { // log problems
                    RuntimeLogger.Warning("Exception caught in ProcessSequence", ex);                    
                }
            }
        }

		/// <summary>
		/// Class that represents OBD request and its handler
		/// </summary>
        private class ObdSequenceItem {
			/// <summary>
			/// The request to be sent to OBD interface
			/// </summary>
            public ObdRequest Request { get; set; }
			/// <summary>
			/// Keeps track of when to execute this sequence item
			/// </summary>
            public byte SequenceCounter { get; set; } = 1;
			/// <summary>
			/// How often this sequence item should be processed
			/// </summary>
            public byte EveryNthTime { get; set; }
			/// <summary>
			/// Sequence item will only be processed if it is enabled
			/// </summary>
            public bool IsEnabled { get; set; }
			/// <summary>
			/// Delegate that handles OBD response
			/// </summary>
			/// <param name="response">OBD Response to be handled</param>
            public delegate void Process(ObdResponse response);

            public Process Prc;
        }

    }
}

