using System;
using Windows.Security.Cryptography.Core;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.Xaml.Media;
using HSDHelper.Utils;

namespace HSDHelper.Models {
	public class DerivedProperties : ViewModelBase {
		private bool _EV;
		public bool EV { get { return _EV; } set { _EV = value; RaisePropertyChanged(); } }
		private bool _ICESpinning;
		public bool ICESpinning { get { return _ICESpinning; } set { _ICESpinning = value; RaisePropertyChanged(); } }
		private bool _ICEOff;
		public bool ICEOff { get { return _ICEOff; } set { _ICEOff = value; RaisePropertyChanged(); } }
		private bool _Braking;
		public bool Braking { get { return _Braking; } set { _Braking = value; RaisePropertyChanged(); } }
		private bool _BrakingWhileMoving;
		public bool BrakingWhileMoving { get { return _BrakingWhileMoving; } set { _BrakingWhileMoving = value; RaisePropertyChanged(); } }
		private bool _AcceleratorPressed;
		public bool AcceleratorPressed { get { return _AcceleratorPressed; } set { _AcceleratorPressed = value; RaisePropertyChanged(); } }
		private bool _Moving;
		public bool Moving { get { return _Moving; } set { _Moving = value; RaisePropertyChanged(); } }
		private bool _Coasting;
		public bool Coasting { get { return _Coasting; } set { _Coasting = value; RaisePropertyChanged(); } }
		private bool _Paragliding;
		public bool Paragliding { get { return _Paragliding; } set { _Paragliding = value; RaisePropertyChanged(); } }
		private bool _Gliding;
		public bool Gliding { get { return _Gliding; } set { _Gliding = value; RaisePropertyChanged(); } }
		private bool _Accelerating;
		public bool Accelerating { get { return _Accelerating; } set { _Accelerating = value; RaisePropertyChanged(); } }
		private bool _Pulsing;
		public bool Pulsing { get { return _Pulsing; } set { _Pulsing = value; RaisePropertyChanged(); } }
		private bool _Neutral;
		public bool Neutral { get { return _Neutral; } set { _Neutral = value; RaisePropertyChanged(); } }
		private bool _Heretical;
		public bool Heretical { get { return _Heretical; } set { _Heretical = value; RaisePropertyChanged(); } }
		private bool _Sweetspot;
		public bool Sweetspot { get { return _Sweetspot; } set { _Sweetspot = value; RaisePropertyChanged(); } }
		private bool _EVRegen;
		public bool EVRegen { get { return _EVRegen; } set { _EVRegen = value; RaisePropertyChanged(); } }
		private bool _EVTraction;
		public bool EVTraction { get { return _EVTraction; } set { _EVTraction = value; RaisePropertyChanged(); } }
		private bool _ExcessiveEV;
		public bool ExcessiveEV { get { return _ExcessiveEV; } set { _ExcessiveEV = value; RaisePropertyChanged(); } }
		private bool _HSIPower;
		public bool HSIPower { get { return _HSIPower; } set { _HSIPower = value; RaisePropertyChanged(); } }
		private bool _HSIUpperEco;
		public bool HSIUpperEco { get { return _HSIUpperEco; } set { _HSIUpperEco = value; RaisePropertyChanged(); } }
		private bool _HSILowerEco;
		public bool HSILowerEco { get { return _HSILowerEco; } set { _HSILowerEco = value; RaisePropertyChanged(); } }
		private bool _HSICharge;
		public bool HSICharge { get { return _HSICharge; } set { _HSICharge = value; RaisePropertyChanged(); } }
		private bool _GoodBrake;
		public bool GoodBrake { get { return _GoodBrake; } set { _GoodBrake = value; RaisePropertyChanged(); } }
		private bool _BadBrake;
		public bool BadBrake { get { return _BadBrake; } set { _BadBrake = value; RaisePropertyChanged(); } }
		private bool _BatterySensor1;
		public bool BatterySensor1 { get { return _BatterySensor1; } set { _BatterySensor1 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor2;
		public bool BatterySensor2 { get { return _BatterySensor2; } set { _BatterySensor2 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor3;
		public bool BatterySensor3 { get { return _BatterySensor3; } set { _BatterySensor3 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor4;
		public bool BatterySensor4 { get { return _BatterySensor4; } set { _BatterySensor4 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor5;
		public bool BatterySensor5 { get { return _BatterySensor5; } set { _BatterySensor5 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor6;
		public bool BatterySensor6 { get { return _BatterySensor6; } set { _BatterySensor6 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor7;
		public bool BatterySensor7 { get { return _BatterySensor7; } set { _BatterySensor7 = value; RaisePropertyChanged(); } }
		private bool _BatterySensor8;
		public bool BatterySensor8 { get { return _BatterySensor8; } set { _BatterySensor8 = value; RaisePropertyChanged(); } }
		private bool _ICERunning;
		public bool ICERunning { get { return _ICERunning; } set { _ICERunning = value; RaisePropertyChanged(); } }
	}
	public class CarState : ViewModelBase {
		public CarState() {
			_fuelFlowLitersPerHourArray.Initialize(3, -1);
			_gkwhArray.Initialize(3, -1);
			_rpmArray.Initialize(3, -1);
			_l100Array.Initialize(3, -1);
		}



		public static readonly SolidColorBrush _greenBrush = new SolidColorBrush(Color.FromArgb(255, 111, 255, 0));
		public static readonly SolidColorBrush _greyBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
		public static readonly SolidColorBrush _lightBlueBrush = new SolidColorBrush(Color.FromArgb(255, 128, 255, 255));
		public static readonly SolidColorBrush _redBrush = new SolidColorBrush(Color.FromArgb(255, 255, 51, 0));
		public static readonly SolidColorBrush _yellowBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));

		public SolidColorBrush _stateOfChargeColor = _greenBrush;
		private double _altitude;
		private int _ambientTemperature;
		private int _batteryAirTemperature;
		private double _batteryChargeControl;
		private double _batteryCurrent;
		private double _batteryDischargeControl;
		private SolidColorBrush _batteryPowerColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
		private int _batteryTemperature;
		private SolidColorBrush _batteryTemperatureColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
		private double _batteryVoltage;
		private SolidColorBrush _brakeColor = _greyBrush;
		private SolidColorBrush _brakeH1Color = _greyBrush;
		private SolidColorBrush _brakeH2Color = _greyBrush;
		private SolidColorBrush _brakeH3Color = _greyBrush;
		private SolidColorBrush _brakeH4Color = _greyBrush;
		private SolidColorBrush _brakeH5Color = _greyBrush;
		private double _brakeMastCylTorque;
		private double _brakeRegenTorque;
		private int _coolantTemperature = 17;
		private SolidColorBrush _coolantTemperatureColor = _lightBlueBrush;
		private HSDStateEnum _currentState;
		private double _engineLoad;
		private string _enginePhase = "S0";
		private SolidColorBrush _enginePowerColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
		private SolidColorBrush _eVH1Color = _greyBrush;
		private SolidColorBrush _eVH2Color = _greyBrush;
		private SolidColorBrush _eVH3Color = _greyBrush;
		private SolidColorBrush _eVH4Color = _greyBrush;
		private SolidColorBrush _eVH5Color = _greyBrush;
		private SolidColorBrush _fanColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
		private int _fanMode = 6;
		private double _fuelConsumption = 4.3f;
		private SolidColorBrush _fuelConsumptionColor = _greyBrush;
		private double _fuelFlowLitersPerHour;
		private readonly CircularArray _fuelFlowLitersPerHourArray = new CircularArray();
		private double _fuelFlowLitersPerHourAverage;
		private double _gkwh;
		private readonly CircularArray _gkwhArray = new CircularArray();
		private int _hsi;
		private double _icePower;
		private SolidColorBrush _icePowerColor = _greyBrush;
		private double _iceTorque;
		private double _ignitionTiming;
		private double _inj_UL;
		private int _inverterTemperature = 35;
		private SolidColorBrush _inverterTemperatureColor = _greenBrush;
		private readonly CircularArray _l100Array = new CircularArray();
		private double _l100;
		private double _l100Avg;
		private double _latitude;
		private double _longitude;
		private double _longTermFuelTrim;
		private int _mg1InverterTemperature;
		private int _mg1Rpm;
		private int _mg1Temperature;
		private int _mg2InverterTemperature;
		private int _mg2Rpm;
		private int _mg2Temperature;
		private int _odo;
		private int _rpm = 0;
		private readonly CircularArray _rpmArray = new CircularArray();
		private SolidColorBrush _rpmColor = _greyBrush;
		private double _shortTermFuelTrim;
		private int _speed = 0;
		private SolidColorBrush _speedColor = _greyBrush;
		private int _speedGps;
		private double _stateOfCharge = 0f;
		private double _tankLevel;
		private double _tripEvLength;
		private int _tripEvSeconds;
		private double _tripFuel;
		private double _tripFuelAvg;
		private int _tripInMoveSeconds;
		private double _tripLength;
		private int _tripSeconds;
		public enum HSDStateEnum {
			None = 0,
			S0 = 1,
			S1 = 2,
			S1A = 3,
			End_S1A = 4,
			S1B = 5,
			S2 = 6,
			S3 = 7,
			S4 = 8
		}

		public enum BrakeState {
			None = 0,
			Regen = 1,
			Pad = 2,
		}

		public double Altitude {
			get { return _altitude; }
			set {
				_altitude = value;
				RaisePropertyChanged();
			}
		}
		public int AmbientTemperature {
			get { return _ambientTemperature; }
			set {
				_ambientTemperature = value;
				RaisePropertyChanged();
			}
		}
		public int BatteryAirTemperature {
			get { return _batteryAirTemperature; }
			set {
				_batteryAirTemperature = value;
				RaisePropertyChanged();
			}
		}
		public double BatteryCharge => BatteryCurrent > 0 ? 0 : -BatteryCurrent * BatteryVoltage / 1000;
		public double BatteryChargeControl {
			get { return _batteryChargeControl; }
			set {
				_batteryChargeControl = value;
				RaisePropertyChanged();
			}
		}
		public double BatteryCurrent {
			get { return _batteryCurrent; }
			set {
				_batteryCurrent = value;
				RaisePropertyChanged();
				RaisePropertyChanged("BatteryPower");
				RaisePropertyChanged("BatteryCharge");
				RaisePropertyChanged("BatteryDischarge");
			}
		}
		public double BatteryDischarge => BatteryCurrent < 0 ? 0 : BatteryCurrent * BatteryVoltage / 1000;
		public double BatteryDischargeControl {
			get { return _batteryDischargeControl; }
			set {
				_batteryDischargeControl = value;
				RaisePropertyChanged();
			}
		}
		public double BatteryPower => BatteryVoltage * BatteryCurrent / 1000;
		public SolidColorBrush BatteryPowerColor {
			get { return _batteryPowerColor; }
			set {
				_batteryPowerColor = value;
				RaisePropertyChanged();
			}
		}
		public int BatteryTemperature {
			get { return _batteryTemperature; }
			set {
				_batteryTemperature = value;
				RaisePropertyChanged();
				byte r, g, b;
				if (value < 36) {
					HsvToRgb(120, 1, 1, out r, out g, out b);
				}
				else if (value > 50) {
					HsvToRgb(0, 1, 1, out r, out g, out b);
				}
				else {
					HsvToRgb(120 - (value - 36.0) / 14 * 120, 1, 1, out r, out g, out b);
				}

				BatteryTemperatureColor = new SolidColorBrush(Color.FromArgb(255, r, g, b));
			}
		}
		public SolidColorBrush BatteryTemperatureColor {
			get { return _batteryTemperatureColor; }
			set {
				_batteryTemperatureColor = value;
				RaisePropertyChanged();
			}
		}
		public double BatteryVoltage {
			get { return _batteryVoltage; }
			set {
				_batteryVoltage = value;
				RaisePropertyChanged();
				RaisePropertyChanged("BatteryPower");
				RaisePropertyChanged("BatteryCharge");
				RaisePropertyChanged("BatteryDischarge");
			}
		}
		public SolidColorBrush BrakeColor {
			get { return _brakeColor; }
			set {
				_brakeColor = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush BrakeH1Color {
			get { return _brakeH1Color; }
			set {
				_brakeH1Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush BrakeH2Color {
			get { return _brakeH2Color; }
			set {
				_brakeH2Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush BrakeH3Color {
			get { return _brakeH3Color; }
			set {
				_brakeH3Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush BrakeH4Color {
			get { return _brakeH4Color; }
			set {
				_brakeH4Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush BrakeH5Color {
			get { return _brakeH5Color; }
			set {
				_brakeH5Color = value;
				RaisePropertyChanged();
			}
		}
		public double BrakeMastCylTorque {
			get { return _brakeMastCylTorque; }
			set {
				_brakeMastCylTorque = value;
				RaisePropertyChanged();
				//_EvalBrake();

			}
		}
		public double BrakeRegenTorque {
			get { return _brakeRegenTorque; }
			set {
				_brakeRegenTorque = value;
				RaisePropertyChanged();
				//_EvalBrake();
			}
		}
		public int CoolantTemperature {
			get { return _coolantTemperature; }
			set {
				if (value < 40)
					CoolantTemperatureColor = _lightBlueBrush;
				else if (value < 70)
					CoolantTemperatureColor = _yellowBrush;
				else if (value < 95)
					CoolantTemperatureColor = _greenBrush;
				else
					CoolantTemperatureColor = _redBrush;
				_coolantTemperature = value;
				CalcHSDState(CoolantTemperature, RPM, ICEPower, Speed);
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush CoolantTemperatureColor {
			get { return _coolantTemperatureColor; }
			set {
				_coolantTemperatureColor = value;
				RaisePropertyChanged();
			}
		}
		public double EngineLoad {
			get { return _engineLoad; }
			set {
				_engineLoad = value;
				RaisePropertyChanged();
			}
		}
		public string EnginePhase {
			get { return _enginePhase; }
			set {
				_enginePhase = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush EnginePowerColor {
			get { return _enginePowerColor; }
			set {
				_enginePowerColor = value;
				RaisePropertyChanged();
			}
		}
		public double EnginePowerMax { get; set; } = 20;
		public double EnginePowerMin { get; set; } = 0;
		public SolidColorBrush EVColor {
			get {
				if (RPM >= 900) return _greyBrush;
				if (StateOfCharge < 60) return _redBrush;
				return _greenBrush;
			}
		}
		public SolidColorBrush EVH1Color {
			get { return _eVH1Color; }
			set {
				_eVH1Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush EVH2Color {
			get { return _eVH2Color; }
			set {
				_eVH2Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush EVH3Color {
			get { return _eVH3Color; }
			set {
				_eVH3Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush EVH4Color {
			get { return _eVH4Color; }
			set {
				_eVH4Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush EVH5Color {
			get { return _eVH5Color; }
			set {
				_eVH5Color = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush FanColor {
			get { return _fanColor; }
			set {
				_fanColor = value;
				RaisePropertyChanged();
			}
		}
		public int FanMode {
			get { return _fanMode; }
			set {
				_fanMode = value;
				RaisePropertyChanged();
			}
		}
		public double FuelConsumption {
			get { return _fuelConsumption; }
			set {
				_fuelConsumption = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush FuelConsumptionColor {
			get { return _fuelConsumptionColor; }
			set {
				_fuelConsumptionColor = value;
				RaisePropertyChanged();
			}
		}
		public double FuelFlowLitersPerHour {
			get { return _fuelFlowLitersPerHour; }
			set {
				_fuelFlowLitersPerHour = value;
				RaisePropertyChanged();
				FuelFlowLitersPerHourAverage = _fuelFlowLitersPerHourArray.UpdateValue(value);
			}
		}
		public double FuelFlowLitersPerHourAverage {
			get { return _fuelFlowLitersPerHourAverage; }
			set {
				_fuelFlowLitersPerHourAverage = value;
				RaisePropertyChanged();
				_CalcGKWH();
				L100 = Speed > 0.0 ? 100.0 / Speed * _fuelFlowLitersPerHourAverage : (_fuelFlowLitersPerHourAverage == 0.0 ? 0.0 : 99.0);
			}
		}
		public double GKWH {
			get { return _gkwh; }
			set {
				_gkwh = value;
				RaisePropertyChanged();
				gkwhAverage = _gkwhArray.UpdateValue(_gkwh);
				if (RPM < 900)
				{
					ICEPowerColor = _greyBrush;

				}
				else
				{
					byte r, g, b;
					var h = 0.0;

					if (_gkwh < 230) h = 120;
					else if (_gkwh > 290) h = 0;
					else
					{
						h = (290 - _gkwh)/60*120;
					}
					HsvToRgb(h, 1, 1, out r, out g, out b);
					ICEPowerColor = new SolidColorBrush(Color.FromArgb(255, r, g, b));
				}
			}
		}
		public double gkwhAverage { get; set; }
		public HSDStateEnum HSDState {
			get { return _currentState; }
			set {
				_currentState = value;
				RaisePropertyChanged();
			}
		}
		public int HSI {
			get { return _hsi; }
			set {
				_hsi = value;
				RaisePropertyChanged();
			}
		}
		public double ICEPower {
			get { return _icePower; }
			set {
				_icePower = value;
				RaisePropertyChanged();
				_CalcGKWH();
				CalcHSDState(CoolantTemperature, RPM, ICEPower, Speed);
			}
		}
		public SolidColorBrush ICEPowerColor {
			get { return _icePowerColor; }
			set { _icePowerColor = value; RaisePropertyChanged(); }
		}
		public double ICETorque {
			get { return _iceTorque; }
			set {
				_iceTorque = value;
				RaisePropertyChanged();
				_CalcICEPower();
			}
		}
		public double IgnitionTiming {
			get { return _ignitionTiming; }
			set {
				_ignitionTiming = value;
				RaisePropertyChanged();
			}
		}
		public double Inj_uL {
			get { return _inj_UL; }
			set {
				_inj_UL = value;
				RaisePropertyChanged();
				_CalcFuel();
			}
		}
		public int InverterTemperature {
			get { return _inverterTemperature; }
			set {
				_inverterTemperature = value;
				InverterTemperatureColor = _greenBrush;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush InverterTemperatureColor {
			get { return _inverterTemperatureColor; }
			set {
				_inverterTemperatureColor = value;
				RaisePropertyChanged();
			}
		}
		public double L100 {
			get { return _l100; }
			set {
				_l100 = value;
				RaisePropertyChanged();
				L100Avg = _l100Array.UpdateValue(value);
			}
		}
		public double L100Avg {
			get { return _l100Avg < 99.9 ? _l100Avg : 99.9; }
			set {
				_l100Avg = value;
				RaisePropertyChanged();
			}
		}
		public double Latitude {
			get { return _latitude; }
			set {
				_latitude = value;
				RaisePropertyChanged();
			}
		}
		public double Longitude {
			get { return _longitude; }
			set {
				_longitude = value;
				RaisePropertyChanged();
			}
		}
		public double LongTermFuelTrim {
			get { return _longTermFuelTrim; }
			set {
				_longTermFuelTrim = value;
				RaisePropertyChanged();
			}
		}
		public int MG1InverterTemperature {
			get { return _mg1InverterTemperature; }
			set {
				_mg1InverterTemperature = value;
				RaisePropertyChanged();
			}
		}
		public int MG1RPM {
			get { return _mg1Rpm; }
			set {
				_mg1Rpm = value;
				RaisePropertyChanged();
			}
		}
		public int MG1Temperature {
			get { return _mg1Temperature; }
			set {
				_mg1Temperature = value;
				RaisePropertyChanged();
			}
		}
		public int MG2InverterTemperature {
			get { return _mg2InverterTemperature; }
			set {
				_mg2InverterTemperature = value;
				RaisePropertyChanged();
			}
		}
		public int MG2RPM {
			get { return _mg2Rpm; }
			set {
				_mg2Rpm = value;
				RaisePropertyChanged();
			}
		}
		public int MG2Temperature {
			get { return _mg2Temperature; }
			set {
				_mg2Temperature = value;
				RaisePropertyChanged();
			}
		}
		public int ODO {
			get { return _odo; }
			set {
				_odo = value;
				RaisePropertyChanged();
			}
		}
		public int RPM {
			get { return _rpm; }
			set {

				if (value == 0)
					RPMColor = _greyBrush;
				/*
				else {
					if (Math.Abs(FuelConsumption) < 0.01f)
						RPMColor = _yellowBrush;
					else {
						if (value < 1600)
							RPMColor = _redBrush;
						else if (value < 2700)
							RPMColor = _greenBrush;
						else
							RPMColor = _yellowBrush;
					}
				}
				*/
				_rpm = value;
				RaisePropertyChanged();
				//RaisePropertyChanged("EnginePower");
				_CalcICEPower();
				CalcHSDState(CoolantTemperature, _rpm, ICEPower, Speed);
				_CalcFuel();
				RPMAverage = _rpmArray.UpdateValue(_rpm);
				RaisePropertyChanged("EVColor");
			}
		}
		public double RPMAverage { get; set; }
		public SolidColorBrush RPMColor {
			get { return _rpmColor; }
			set {
				_rpmColor = value;
				RaisePropertyChanged();
			}
		}
		public double ShortTermFuelTrim {
			get { return _shortTermFuelTrim; }
			set {
				_shortTermFuelTrim = value;
				RaisePropertyChanged();
			}
		}
		public double LastSpeed { get; set; }
		public int Speed {
			get { return _speed; }
			set {
				if (_speed == 0)
					SpeedColor = _greyBrush;
				else if (_speed < 30) // 0-30
					SpeedColor = _greenBrush;
				else if (_speed < 40) // 30-40
					SpeedColor = _yellowBrush;
				else if (_speed < 50) // 40-50
					SpeedColor = _greenBrush;
				else if (_speed < 60) // 50-60
					SpeedColor = _yellowBrush;
				else if (_speed < 90) // 60-90
					SpeedColor = _greenBrush;
				else if (_speed < 100) // 90-100
					SpeedColor = _yellowBrush;
				else if (_speed < 130) // 100-130
					SpeedColor = _greenBrush;
				else if (_speed < 140) // 130-140
					SpeedColor = _yellowBrush;
				else // 140+
					SpeedColor = _greenBrush;
				LastSpeed = _speed;
				_speed = value;
				RaisePropertyChanged();
				CalcHSDState(CoolantTemperature, RPM, ICEPower, Speed);
			}
		}
		public SolidColorBrush SpeedColor {
			get { return _speedColor; }
			set {
				_speedColor = value;
				RaisePropertyChanged();
			}
		}
		public int SpeedGPS {
			get { return _speedGps; }
			set {
				_speedGps = value;
				RaisePropertyChanged();
			}
		}
		public double StateOfCharge {
			get { return _stateOfCharge; }
			set {
				byte r, g, b;
				if (value < 50) {
					HsvToRgb(0, 1, 1, out r, out g, out b);
				}
				else if (value > 80) {
					HsvToRgb(120, 1, 1, out r, out g, out b);
				}
				else {
					HsvToRgb((value - 50.0) / 30 * 120, 1, 1, out r, out g, out b);
				}

				StateOfChargeColor = new SolidColorBrush(Color.FromArgb(255, r, g, b));
				_stateOfCharge = value;
				RaisePropertyChanged();
			}
		}
		public SolidColorBrush StateOfChargeColor {
			get { return _stateOfChargeColor; }
			set {
				_stateOfChargeColor = value;
				RaisePropertyChanged();
			}
		}
		public double TankLevel {
			get { return _tankLevel; }
			set {
				_tankLevel = value;
				RaisePropertyChanged();
			}
		}
		public double TripEvLength {
			get { return _tripEvLength; }
			set {
				_tripEvLength = value;
				RaisePropertyChanged();
				RaisePropertyChanged("TripEvLengthPercentage");
			}
		}
		public int TripEvLengthPercentage => TripLength == 0 ? 0 : (int)(TripEvLength * 100 / TripLength);
		public int TripEvSeconds {
			get { return _tripEvSeconds; }
			set {
				_tripEvSeconds = value;
				RaisePropertyChanged();
				RaisePropertyChanged("TripEvSecondsPercentage");
			}
		}
		public int TripEvSecondsPercentage => TripSeconds == 0 ? 0 : TripEvSeconds * 100 / TripSeconds;
		public double TripFuel {
			get { return _tripFuel; }
			set {
				_tripFuel = value;
				RaisePropertyChanged();
			}
		}
		public double TripFuelAvg {
			get { return _tripFuelAvg; }
			set {
				_tripFuelAvg = value;
				RaisePropertyChanged();
			}
		}
		public int TripInMoveSeconds {
			get { return _tripInMoveSeconds; }
			set {
				_tripInMoveSeconds = value;
				RaisePropertyChanged();
			}
		}
		public double TripLength {
			get { return _tripLength; }
			set {
				_tripLength = value;
				RaisePropertyChanged();
				RaisePropertyChanged("TripEvLengthPercentage");
			}
		}
		public int TripSeconds {
			get { return _tripSeconds; }
			set {
				_tripSeconds = value;
				RaisePropertyChanged();
				RaisePropertyChanged("TripEvSecondsPercentage");
			}
		}

		public double AcceleratorPedal {
			get { return _acceleratorPedal; }
			set { _acceleratorPedal = value; }
		}

		public HSDStateEnum CalcHSDState(int coolantTemperature, int rpm, double icePower, int obdSpeed) {
			switch (HSDState) {
				case HSDStateEnum.None /*0*/:
					HSDState = HSDStateEnum.None;
					break;

				case HSDStateEnum.S0: /*1*/
					if (coolantTemperature <= 73) {
						if (rpm <= 0) {
							HSDState = HSDStateEnum.S0;
							break;
						}
						HSDState = HSDStateEnum.S1A;
						break;
					}
					HSDState = HSDStateEnum.S4;
					break;

				case HSDStateEnum.S1 /*2*/:
					HSDState = HSDStateEnum.S1A;
					break;

				case HSDStateEnum.S1A /*3*/:
					if (rpm != 0) {
						if (icePower > 3.0d) {
							HSDState = HSDStateEnum.End_S1A;
							break;
						}
					}
					HSDState = HSDStateEnum.S2;
					break;

				case HSDStateEnum.End_S1A: /*4*/
					if (coolantTemperature <= 60) {
						HSDState = HSDStateEnum.S1B;
						break;
					}
					_currentState = HSDStateEnum.S2;
					break;

				case HSDStateEnum.S1B: /*5*/
					if ((rpm != 0 && rpm <= 1450) || coolantTemperature < 40) {
						HSDState = HSDStateEnum.S1B;
						break;
					}
					HSDState = HSDStateEnum.S2;
					break;

				case HSDStateEnum.S2: /*6*/
					if (coolantTemperature < 70) {
						if (coolantTemperature >= 39) {
							HSDState = HSDStateEnum.S2;
							break;
						}
						HSDState = HSDStateEnum.S1B;
						break;
					}
					HSDState = HSDStateEnum.S3;
					break;

				case HSDStateEnum.S3: /*7*/
					if (rpm == 0) {
						HSDState = HSDStateEnum.S4;
					}
					break;

				case HSDStateEnum.S4: /*8*/
					if (coolantTemperature > 40) {
						HSDState = HSDStateEnum.S4;
						break;
					}
					HSDState = HSDStateEnum.S0;
					break;
			}
			return HSDState;
		}

		private int _regenBrakeCounter = 0, _cylinderBrakeCounter = 0, _brakeCounter = 0;
		public void _EvalBrake() {
			var brakeBad = false;
			var brakeHalfBad = false;
			var brakeGood = false;

			if (BrakeRegenTorque >= 0 && BrakeMastCylTorque >= 0) // we're not braking anymore
			{
				BrakeColor = _greyBrush;
				if (_brakeCounter > 0) {
					if (_cylinderBrakeCounter < _brakeCounter / 6)
						brakeGood = true;
					else if (_cylinderBrakeCounter < _regenBrakeCounter)
						brakeHalfBad = true;
					else
						brakeBad = true;
					_regenBrakeCounter = 0;
					_cylinderBrakeCounter = 0;
					_brakeCounter = 0;
					AddBrakeHistory(brakeGood, brakeBad, brakeHalfBad);
				}
			}
			else {
				var badBrake = BrakeMastCylTorque < BrakeRegenTorque;
				BrakeColor = badBrake ? _redBrush : _greenBrush;
				if (Speed <= 10) return;

				if (badBrake) _cylinderBrakeCounter += 1;
				else _regenBrakeCounter += 1;
				_brakeCounter += 1;
			}
		}

		private void AddBrakeHistory(bool brakeGood, bool brakeBad, bool brakeHalfBad) {
			BrakeH1Color = BrakeH2Color;
			BrakeH2Color = BrakeH3Color;
			BrakeH3Color = BrakeH4Color;
			BrakeH4Color = BrakeH5Color;
			if (brakeGood) BrakeH5Color = _greenBrush;
			if (brakeBad) BrakeH5Color = _redBrush;
			if (brakeHalfBad) BrakeH5Color = _yellowBrush;
		}

		private DateTime _prevFuelCalcTime = DateTime.MinValue;
		private void _CalcFuel() {
			FuelFlowLitersPerHour = RPM * Inj_uL * 120 / 1.0e7;
			if (_prevFuelCalcTime == DateTime.MinValue) // ignore first calculation
			{
				_prevFuelCalcTime = DateTime.Now;
				return;
			}
			TripFuel += (DateTime.Now - _prevFuelCalcTime).Milliseconds*RPM/60000.0*Inj_uL/10000*2;
			_prevFuelCalcTime = DateTime.Now;
			TripFuelAvg = TripFuel/TripLength/10;
		}
		private void _CalcGKWH() {
			if (Math.Abs(_icePower) < 0.01)
				GKWH = 0;
			else
				GKWH = (750.0d * _fuelFlowLitersPerHourAverage) / _icePower;
		}

		private void _CalcICEPower() {
			ICEPower = ICETorque * 2 * Math.PI * RPM / 60 / 1000;
		}
		private byte Clamp(int i) {
			if (i < 0) return 0;
			if (i > 255) return 255;
			return (byte)i;
		}
		private void HsvToRgb(double h, double S, double V, out byte r, out byte g, out byte b) {
			// ######################################################################
			// T. Nathan Mundhenk
			// mundhenk@usc.edu
			// C/C++ Macro HSV to RGB

			var H = h;
			while (H < 0) {
				H += 360;
			}
			;
			while (H >= 360) {
				H -= 360;
			}
			;
			double R, G, B;
			if (V <= 0) {
				R = G = B = 0;
			}
			else if (S <= 0) {
				R = G = B = V;
			}
			else {
				var hf = H / 60.0;
				var i = (int)Math.Floor(hf);
				var f = hf - i;
				var pv = V * (1 - S);
				var qv = V * (1 - S * f);
				var tv = V * (1 - S * (1 - f));
				switch (i) {
					// Red is the dominant color

					case 0:
						R = V;
						G = tv;
						B = pv;
						break;

					// Green is the dominant color

					case 1:
						R = qv;
						G = V;
						B = pv;
						break;

					case 2:
						R = pv;
						G = V;
						B = tv;
						break;

					// Blue is the dominant color

					case 3:
						R = pv;
						G = qv;
						B = V;
						break;

					case 4:
						R = tv;
						G = pv;
						B = V;
						break;

					// Red is the dominant color

					case 5:
						R = V;
						G = pv;
						B = qv;
						break;

					// Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

					case 6:
						R = V;
						G = tv;
						B = pv;
						break;

					case -1:
						R = V;
						G = pv;
						B = qv;
						break;

					// The color is not defined, we should throw an error.

					default:
						//LFATAL("i Value error in Pixel conversion, Value is %d", i);
						R = G = B = V; // Just pretend its black/white
						break;
				}
			}
			r = Clamp((int)(R * 255.0));
			g = Clamp((int)(G * 255.0));
			b = Clamp((int)(B * 255.0));
		}
		public DerivedProperties DerivedProperties = new DerivedProperties();
		private double _acceleratorPedal;

		public void CalcState() {
			if (RPM < 900) {
				DerivedProperties.EV = true;
				DerivedProperties.ICERunning = false;
			}
			else {
				DerivedProperties.EV = false;
				DerivedProperties.ICERunning = true;
			}
			DerivedProperties.Accelerating = true;
			DerivedProperties.Accelerating = false;
			DerivedProperties.AcceleratorPressed = AcceleratorPedal > 0;
			DerivedProperties.ICESpinning = ICETorque < 0 && !DerivedProperties.AcceleratorPressed && RPM > 0;
			DerivedProperties.ICEOff = DerivedProperties.EV || DerivedProperties.ICESpinning;
			DerivedProperties.Braking = BrakeRegenTorque < 0 || BrakeMastCylTorque < 0;
			DerivedProperties.Moving = Speed < 5;
			DerivedProperties.BrakingWhileMoving = DerivedProperties.Braking && DerivedProperties.Moving;

			if (DerivedProperties.Braking) {
				DerivedProperties.BadBrake = BrakeMastCylTorque < BrakeRegenTorque;
				DerivedProperties.GoodBrake = !DerivedProperties.BadBrake;
			}
			else {
				DerivedProperties.BadBrake = false;
				DerivedProperties.GoodBrake = false;
			}

			if (DerivedProperties.Moving && !DerivedProperties.AcceleratorPressed) {
				if (!DerivedProperties.Braking && DerivedProperties.ICEOff)
					DerivedProperties.Coasting = true;
				else {
					DerivedProperties.Coasting = DerivedProperties.ICESpinning;
				}
			}

			DerivedProperties.Paragliding = DerivedProperties.AcceleratorPressed && DerivedProperties.ICEOff && BatteryCurrent < 5.0 && BatteryCurrent > 1.5 && Speed > 10.0;
			DerivedProperties.Gliding = DerivedProperties.AcceleratorPressed && DerivedProperties.ICEOff && _hsi >= 0 && _hsi <= 1;
			DerivedProperties.Accelerating = Speed > LastSpeed;

			if (!DerivedProperties.AcceleratorPressed || !DerivedProperties.Moving || DerivedProperties.Braking) DerivedProperties.Pulsing = false;
			else if (!DerivedProperties.EV && BatteryCurrent > -5.0 && BatteryCurrent < -1.5) {
				DerivedProperties.Pulsing = true;
			}
			else { // 3 sources:                            
				DerivedProperties.Pulsing = false;
			}
			if (DerivedProperties.Moving && this._hsi == -512) {
				DerivedProperties.Neutral = true;
			}
			else {
				DerivedProperties.Neutral = false;
			}

			if (MG1RPM >= 0) DerivedProperties.Heretical = false;
			else if (!DerivedProperties.ICEOff) {
				DerivedProperties.Heretical = true;
			}
			else {
				DerivedProperties.Heretical = false;
			}
			if (DerivedProperties.Heretical && this._rpm < 1100) {
				DerivedProperties.Sweetspot = true;
			}
			else {
				DerivedProperties.Sweetspot = false;
			}
			if (DerivedProperties.EV && BatteryCurrent < 0.0) {
				DerivedProperties.EVRegen = true;
			}
			else {
				DerivedProperties.EVRegen = false;
			}
			if (!DerivedProperties.EV || !DerivedProperties.AcceleratorPressed) DerivedProperties.EVTraction = false;
			else if (!DerivedProperties.EVRegen && DerivedProperties.Moving) {
				DerivedProperties.EVTraction = true;
			}
			else {
				DerivedProperties.EVTraction = false;
			}

			if (DerivedProperties.EVTraction && BatteryCharge < 60.0f) {
				DerivedProperties.ExcessiveEV = true;
			}
			else {
				DerivedProperties.ExcessiveEV = false;
			}

			DerivedProperties.HSICharge = false;
			DerivedProperties.HSILowerEco = false;
			DerivedProperties.HSIUpperEco = false;
			DerivedProperties.HSIPower = false;
			if (this._hsi >= 100) {
				DerivedProperties.HSIPower = true;
			}
			else if (this._hsi > 50) {
				DerivedProperties.HSIUpperEco = true;
			}
			else if (this._hsi >= 0 || this._hsi == -512) {
				DerivedProperties.HSILowerEco = true;
			}
			else {
				DerivedProperties.HSICharge = true;
			}
		}

		public void Reset() {
			_prevFuelCalcTime = DateTime.MinValue;
			TripSeconds = 0;
			TripEvSeconds = 0;
			TripInMoveSeconds = 0;
			TripLength = 0;
			TripEvLength = 0;
		}
	}
}