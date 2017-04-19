using System;
using System.Collections.Generic;
using HSDHelper.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI;
using Windows.UI.Core;
using HSDHelper.Displays;
using HSDHelper.Messages;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Template10.Common;


namespace HSDHelper.Views {
    public sealed partial class MainPage : Page {
        public MainPage() {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Messenger.Default.Register<DataRefreshedMessage>(this, DataRefreshedMessageReceived);
			SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
		}

	    private async void MainPage_BackRequested(object sender, BackRequestedEventArgs e) {
		    var vm = DataContext as MainPageViewModel;
			if (vm == null) { return; }
			//await vm.SaveLog();
		    e.Handled = true;
			BootStrapper.Current.Exit();
	    }

	    private void DataRefreshedMessageReceived(DataRefreshedMessage obj) {
		    var speedColor = Colors.Red;
		    if (obj.State.L100 <= 0.5)
		    {
			    speedColor = obj.State.BatteryPower > 1 ? Color.FromArgb(255,0,128,255) : Colors.LawnGreen;
		    }
				
			SpeedHistogram.RegisterValue(new Graph.GraphPoint {value = obj.State.Speed, color = speedColor});
			SpeedHistogram.RegisterSecondaryValue(new Graph.GraphPoint { value = (float)obj.State.L100Avg, color = Colors.DeepSkyBlue});
			/*
            SoCHistogram.RegisterValue(obj.State.BatteryCharge);
			CurrentHistogram.RegisterValue(obj.State.BatteryCurrent);

            BatteryGauge.Value = obj.State.BatteryCharge;
            SpeedGauge.Value = obj.State.Speed;
            RPMGauge.Value = obj.State.RPM;
	        CoolantGauge.Value = obj.State.CoolantTemp;
	        CurrentGauge.Value = obj.State.BatteryCurrent;
	        LoadGauge.Value = obj.State.CalculatedLoad;

			SoCHistogram.Refresh();
			SpeedHistogram.Refresh();
			CurrentHistogram.Refresh();
	        
			BatteryGauge.Refresh();
            RPMGauge.Refresh();
            SpeedGauge.Refresh();
			CoolantGauge.Refresh();
			CurrentGauge.Refresh();
			LoadGauge.Refresh();
            */
		}

		public double CalcPowerHeight(double value, double minValue, double maxValue) {
            return (value - minValue) /(maxValue - minValue) * PowerGrid.ActualHeight;
            //return (value-minValue)/(maxValue-minValue) * PowerGrid.ActualHeight;
        }
    }
}
