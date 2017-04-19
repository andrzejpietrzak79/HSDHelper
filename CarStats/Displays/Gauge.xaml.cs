using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HSDHelper.Displays {
    public abstract partial class Gauge : UserControl {
        protected Gauge() {
            this.InitializeComponent();
        }
        public void Refresh() {
            canvas.Invalidate();
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Gauge), new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(Gauge), new PropertyMetadata(0.0, new PropertyChangedCallback(OnMinValueChanged)));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(Gauge), new PropertyMetadata(100.0, new PropertyChangedCallback(OnMaxValueChanged)));
        public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register("MinAngle", typeof(double), typeof(Gauge), new PropertyMetadata(-220.0, new PropertyChangedCallback(OnMinAngleChanged)));
        public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register("MaxAngle", typeof(double), typeof(Gauge), new PropertyMetadata(0.0, new PropertyChangedCallback(OnMaxAngleChanged)));
        public static readonly DependencyProperty UnitsProperty = DependencyProperty.Register("Units", typeof(string), typeof(Gauge), new PropertyMetadata("", new PropertyChangedCallback(OnUnitsChanged)));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Gauge), new PropertyMetadata("", new PropertyChangedCallback(OnTitleChanged)));



        protected override Size MeasureOverride(Size availableSize) {
            return new Size(Math.Min(availableSize.Width, availableSize.Height), Math.Min(availableSize.Width, availableSize.Height));
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // ReSharper disable once CompareOfdoublesByEqualityOperator
            if ((double)e.NewValue != (double)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
                //canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }
        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // ReSharper disable once CompareOfdoublesByEqualityOperator
            if ((double)e.NewValue != (double)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }
        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // ReSharper disable once CompareOfdoublesByEqualityOperator
            if ((double)e.NewValue != (double)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }
        private static void OnMinAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // ReSharper disable once CompareOfdoublesByEqualityOperator
            if ((double)e.NewValue != (double)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }
        private static void OnMaxAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // ReSharper disable once CompareOfdoublesByEqualityOperator
            if ((double)e.NewValue != (double)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }
        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if ((string)e.NewValue != (string)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if ((string)e.NewValue != (string)e.OldValue) {
                //value really changed, invoke your changed logic here
                ((Gauge)d).canvas.Invalidate();
            } // else this was invoked because of boxing, do nothing
        }

        public double Value {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public double MinValue {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public double MaxValue {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public double MinAngle {
            get { return (double)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }
        public double MaxAngle {
            get { return (double)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }
        public string Units {
            get { return (string)GetValue(UnitsProperty); }
            set { SetValue(UnitsProperty, value); }
        }
        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        //private Vector2[] arcPoints = new Vector2[2];
        private double arcRadiusX;
        private double arcRadiusY;
        protected static double DegreesToRadians(double angle) {
            return angle * (double)Math.PI / 180;
        }
        private double ArcRotation { get; set; }
        private CanvasArcSize ArcSize { get; set; }
        private CanvasSweepDirection ArcSweepDirection { get; set; }
        protected abstract void Draw(CanvasControl sender, CanvasDrawEventArgs args);
        protected void Canvas_OnDraw(CanvasControl sender, CanvasDrawEventArgs args) {
            Draw(sender, args);
        }
    }
}
