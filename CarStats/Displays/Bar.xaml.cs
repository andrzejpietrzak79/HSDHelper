using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HSDHelper.Displays {
    public sealed partial class Bar : UserControl {
        public Bar() {
            this.InitializeComponent();
        }

		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Bar), new PropertyMetadata(Orientation.Vertical, new PropertyChangedCallback(OnOrientationChanged)));

	    private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	    {
		    var bar = d as Bar;
		    if (bar == null)
			    return;
		    if (bar.Orientation == Orientation.Horizontal)
		    {
			    bar.FirstValueRect.Height = bar.ActualHeight;
			    bar.FirstValueRect.Width = Math.Max(0,
				    bar.ActualWidth*(bar.FirstValue - bar.FirstValueMin)/(bar.FirstValueMax - bar.FirstValueMin));
			    bar.SecondValueRect.Height = bar.ActualHeight;
			    bar.SecondValueRect.Width = Math.Max(0,
				    bar.ActualWidth*(bar.SecondValue - bar.SecondValueMin)/(bar.SecondValueMax - bar.SecondValueMin));
		    }
		    else
		    {
				bar.FirstValueRect.Width = bar.ActualWidth;
				bar.FirstValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
				bar.SecondValueRect.Width = bar.ActualWidth;
				bar.SecondValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));

			}
		}

	    public Orientation Orientation {
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}


		public static readonly DependencyProperty FirstValueProperty = DependencyProperty.Register("FirstValue", typeof(double), typeof(Bar), new PropertyMetadata(0.0, new PropertyChangedCallback(OnFirstValueChanged)));
        private static void OnFirstValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var bar = d as Bar;
            if (bar == null)
                return;
			if (bar.Orientation == Orientation.Horizontal) {
				bar.FirstValueRect.Height = bar.ActualHeight;
				bar.FirstValueRect.Width = Math.Max(0,
					bar.ActualWidth * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
			}
			else {
				bar.FirstValueRect.Width = bar.ActualWidth;
				bar.FirstValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
			}
		}
		public double FirstValue {
            get { return (double)GetValue(FirstValueProperty); }
            set { SetValue(FirstValueProperty, value); }
        }


        public static readonly DependencyProperty FirstValueMinProperty = DependencyProperty.Register("FirstValueMin", typeof(double), typeof(Bar), new PropertyMetadata(0.0, new PropertyChangedCallback(OnFirstValueMinChanged)));
        private static void OnFirstValueMinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var bar = d as Bar;
            if (bar == null)
                return;
			if (bar.Orientation == Orientation.Horizontal) {
				bar.FirstValueRect.Height = bar.ActualHeight;
				bar.FirstValueRect.Width = Math.Max(0,
					bar.ActualWidth * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
			}
			else {
				bar.FirstValueRect.Width = bar.ActualWidth;
				bar.FirstValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
			}
		}
		public double FirstValueMin {
            get { return (double)GetValue(FirstValueMinProperty); }
            set { SetValue(FirstValueMinProperty, value); }
        }


        public static readonly DependencyProperty FirstValueMaxProperty = DependencyProperty.Register("FirstValueMax", typeof(double), typeof(Bar), new PropertyMetadata(1.0, new PropertyChangedCallback(OnFirstValueMaxChanged)));
        private static void OnFirstValueMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var bar = d as Bar;
            if (bar == null)
                return;
			if (bar.Orientation == Orientation.Horizontal) {
				bar.FirstValueRect.Height = bar.ActualHeight;
				bar.FirstValueRect.Width = Math.Max(0,
					bar.ActualWidth * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
			}
			else {
				bar.FirstValueRect.Width = bar.ActualWidth;
				bar.FirstValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.FirstValue - bar.FirstValueMin) / (bar.FirstValueMax - bar.FirstValueMin));
			}
		}
		public double FirstValueMax {
            get { return (double)GetValue(FirstValueMaxProperty); }
            set { SetValue(FirstValueMaxProperty, value); }
        }


        public static readonly DependencyProperty FirstValueBrushProperty = DependencyProperty.Register("FirstValueBrush", typeof(SolidColorBrush), typeof(Bar), new PropertyMetadata(new SolidColorBrush(Colors.GreenYellow), new PropertyChangedCallback(OnFirstValueBrushChanged)));
        private static void OnFirstValueBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Bar)d).FirstValueRect.Fill = ((Bar)d).FirstValueBrush;
        }
        public SolidColorBrush FirstValueBrush {
            get { return (SolidColorBrush)GetValue(FirstValueBrushProperty); }
            set { SetValue(FirstValueBrushProperty, value); }
        }


        public static readonly DependencyProperty SecondValueProperty = DependencyProperty.Register("SecondValue", typeof(double), typeof(Bar), new PropertyMetadata(0.0, new PropertyChangedCallback(OnSecondValueChanged)));
        private static void OnSecondValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var bar = d as Bar;
            if (bar == null)
                return;
			if (bar.Orientation == Orientation.Horizontal) {
				bar.SecondValueRect.Height = bar.ActualHeight;
				bar.SecondValueRect.Width = Math.Max(0,
					bar.ActualWidth * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));
			}
			else {
				bar.SecondValueRect.Width = bar.ActualWidth;
				bar.SecondValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));
			}

		}
		public double SecondValue {
            get { return (double)GetValue(SecondValueProperty); }
            set { SetValue(SecondValueProperty, value); }
        }


        public static readonly DependencyProperty SecondValueMinProperty = DependencyProperty.Register("SecondValueMin", typeof(double), typeof(Bar), new PropertyMetadata(0.0, new PropertyChangedCallback(OnSecondValueMinChanged)));
        private static void OnSecondValueMinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var bar = d as Bar;
            if (bar == null)
                return;
			if (bar.Orientation == Orientation.Horizontal) {
				bar.SecondValueRect.Height = bar.ActualHeight;
				bar.SecondValueRect.Width = Math.Max(0,
					bar.ActualWidth * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));
			}
			else {
				bar.SecondValueRect.Width = bar.ActualWidth;
				bar.SecondValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));
			}

		}
		public double SecondValueMin {
            get { return (double)GetValue(SecondValueMinProperty); }
            set { SetValue(SecondValueMinProperty, value); }
        }


        public static readonly DependencyProperty SecondValueMaxProperty = DependencyProperty.Register("SecondValueMax", typeof(double), typeof(Bar), new PropertyMetadata(1.0, new PropertyChangedCallback(OnSecondValueMaxChanged)));
        private static void OnSecondValueMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var bar = d as Bar;
            if (bar == null)
                return;
			if (bar.Orientation == Orientation.Horizontal) {
				bar.SecondValueRect.Height = bar.ActualHeight;
				bar.SecondValueRect.Width = Math.Max(0,
					bar.ActualWidth * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));
			}
			else {
				bar.SecondValueRect.Width = bar.ActualWidth;
				bar.SecondValueRect.Height = Math.Max(0,
					bar.ActualHeight * (bar.SecondValue - bar.SecondValueMin) / (bar.SecondValueMax - bar.SecondValueMin));
			}

		}
		public double SecondValueMax {
            get { return (double)GetValue(SecondValueMaxProperty); }
            set { SetValue(SecondValueMaxProperty, value); }
        }


        public static readonly DependencyProperty SecondValueBrushProperty = DependencyProperty.Register("SecondValueBrush", typeof(SolidColorBrush), typeof(Bar), new PropertyMetadata(new SolidColorBrush(Colors.LightSkyBlue), new PropertyChangedCallback(OnSecondValueBrushChanged)));
        private static void OnSecondValueBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Bar)d).SecondValueRect.Fill = ((Bar)d).SecondValueBrush;
        }
        public SolidColorBrush SecondValueBrush {
            get { return (SolidColorBrush)GetValue(SecondValueBrushProperty); }
            set { SetValue(SecondValueBrushProperty, value); }
        }

        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(SolidColorBrush), typeof(Bar), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray), new PropertyChangedCallback(OnBackgroundBrushChanged)));
        private static void OnBackgroundBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Bar) d).BackgroundRect.Fill = ((Bar)d).BackgroundBrush;
        }
        public SolidColorBrush BackgroundBrush {
            get { return (SolidColorBrush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }

    }
}
