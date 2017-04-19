using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HSDHelper.Displays {
	public partial class Graph : UserControl {
		private Color _defaultColor = Colors.Aqua;
		public Graph() {
			this.InitializeComponent();

			for (var i = 0; i < 300; ++i) {
				_history.Add(new GraphPoint { color = _defaultColor, value = 0 });
				_historySecondary.Add(new GraphPoint { color = _defaultColor, value = 0 });
			}
		}
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Graph), new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));
		public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(Graph), new PropertyMetadata(0.0, new PropertyChangedCallback(OnMinValueChanged)));
		public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(Graph), new PropertyMetadata(100.0, new PropertyChangedCallback(OnMaxValueChanged)));
		public static readonly DependencyProperty MinValueSecondaryProperty = DependencyProperty.Register("MinValueSecondary", typeof(double), typeof(Graph), new PropertyMetadata(0.0, new PropertyChangedCallback(OnMinValueSecondaryChanged)));
		public static readonly DependencyProperty MaxValueSecondaryProperty = DependencyProperty.Register("MaxValueSecondary", typeof(double), typeof(Graph), new PropertyMetadata(100.0, new PropertyChangedCallback(OnMaxValueSecondaryChanged)));
		public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register("MinAngle", typeof(double), typeof(Graph), new PropertyMetadata(-220.0, new PropertyChangedCallback(OnMinAngleChanged)));
		public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register("MaxAngle", typeof(double), typeof(Graph), new PropertyMetadata(0.0, new PropertyChangedCallback(OnMaxAngleChanged)));
		public static readonly DependencyProperty UnitsProperty = DependencyProperty.Register("Units", typeof(string), typeof(Graph), new PropertyMetadata("", new PropertyChangedCallback(OnUnitsChanged)));
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<float>), typeof(Graph), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));

		public void Refresh() {
			canvas.Invalidate();
			gauge.Invalidate();
		}

		protected override Size MeasureOverride(Size availableSize) {
			return new Size(Math.Min(availableSize.Width, availableSize.Height), Math.Min(availableSize.Width, availableSize.Height));
		}
		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (!Equals((IList<float>)e.NewValue, (IList<float>)e.OldValue)) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnMinValueSecondaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnMaxValueSecondaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnMinAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnMaxAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if ((double)e.NewValue != (double)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
			} // else this was invoked because of boxing, do nothing
		}
		private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			if ((string)e.NewValue != (string)e.OldValue) {
				//value really changed, invoke your changed logic here
				((Graph)d).canvas.Invalidate();
				((Graph)d).gauge.Invalidate();
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
		public double MinValueSecondary {
			get { return (double)GetValue(MinValueSecondaryProperty); }
			set { SetValue(MinValueSecondaryProperty, value); }
		}
		public double MaxValueSecondary {
			get { return (double)GetValue(MaxValueSecondaryProperty); }
			set { SetValue(MaxValueSecondaryProperty, value); }
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
		public IList<float> ItemsSource {
			get { return (IList<float>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		protected void Canvas_OnDraw(CanvasControl sender, CanvasDrawEventArgs args) {
			Draw(sender, args);
		}

		private void Draw(CanvasControl sender, CanvasDrawEventArgs args) {
			var color = Color.FromArgb(64, 0, 170, 255);
			var gridColor = Color.FromArgb(32, 255, 255, 255);

			//RenderData(canvas, args , 1, ViewModel.BatteryChargeHistory, true);
			var cellWidth = (float)sender.ActualWidth / _history.Count;

			for (var x = 0; x <= 30; ++x) {
				using (var cpb = new CanvasPathBuilder(args.DrawingSession)) {
					cpb.BeginFigure(x * cellWidth * 10, 0);
					cpb.AddLine(x * cellWidth * 10, (float)sender.ActualHeight);
					cpb.EndFigure(CanvasFigureLoop.Open);
					args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), gridColor);
				}
			}
			for (var y = 0; y <= 15; ++y) {
				using (var cpb = new CanvasPathBuilder(args.DrawingSession)) {
					cpb.BeginFigure(0, y * (float)sender.ActualHeight / 15);
					cpb.AddLine((float)sender.ActualWidth, y * (float)sender.ActualHeight / 15);
					cpb.EndFigure(CanvasFigureLoop.Open);
					args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), gridColor);
				}
			}
			args.DrawingSession.DrawLine(new Vector2(0, (float)(sender.ActualHeight- 3 *sender.ActualHeight / 15)), new Vector2((float) sender.ActualWidth, (float)(sender.ActualHeight - 3 * sender.ActualHeight / 15)), color);
			args.DrawingSession.DrawLine(new Vector2(0, (float)(sender.ActualHeight- 5 *sender.ActualHeight / 15)), new Vector2((float) sender.ActualWidth, (float)(sender.ActualHeight - 5 * sender.ActualHeight / 15)), color);
			args.DrawingSession.DrawLine(new Vector2(0, (float)(sender.ActualHeight-9 * sender.ActualHeight / 15)), new Vector2((float) sender.ActualWidth, (float)(sender.ActualHeight - 9 * sender.ActualHeight / 15)), color);
			args.DrawingSession.DrawLine(new Vector2(0, (float)(sender.ActualHeight-13 * sender.ActualHeight / 15)), new Vector2((float) sender.ActualWidth, (float)(sender.ActualHeight - 13 * sender.ActualHeight / 15)), color);


			using (var cpb = new CanvasPathBuilder(args.DrawingSession)) {
				cpb.BeginFigure(new Vector2(0, (float)(sender.ActualHeight - sender.ActualHeight * (_historySecondary[0].value - MinValueSecondary) / (MaxValueSecondary - MinValueSecondary))));
				for (int i = 1; i < _history.Count; i++) {
					cpb.AddLine(new Vector2(i * cellWidth, (float)(sender.ActualHeight - sender.ActualHeight * (_historySecondary[i].value - MinValueSecondary) / (MaxValueSecondary - MinValueSecondary))));
				}

				cpb.AddLine(new Vector2(_history.Count * cellWidth, (float)sender.ActualHeight));
				cpb.AddLine(new Vector2(0, (float)sender.ActualHeight));
				cpb.EndFigure(CanvasFigureLoop.Closed);
				args.DrawingSession.FillGeometry(CanvasGeometry.CreatePath(cpb), Color.FromArgb(255 / 3, 0x07, 0x72, 0xA1));
			}

			var startVector = new Vector2(0, (float)(sender.ActualHeight - sender.ActualHeight * (_history[0].value - MinValue) / (MaxValue - MinValue)));
			for (int i = 1; i < _history.Count; i++) {
				var endVector = new Vector2(i * cellWidth, (float)(sender.ActualHeight - sender.ActualHeight * (_history[i].value - MinValue) / (MaxValue - MinValue)));
				args.DrawingSession.DrawLine(startVector, endVector, _history[i].color, 2);
				startVector = endVector;
				//cpb.AddLine(new Vector2(i * cellWidth, (float)(sender.ActualHeight - sender.ActualHeight * (_history[i].value - MinValue) / (MaxValue - MinValue))));
			}
			startVector = new Vector2(0, (float)(sender.ActualHeight - sender.ActualHeight * (_historySecondary[0].value - MinValueSecondary) / (MaxValueSecondary - MinValueSecondary)));
			for (int i = 1; i < _history.Count; i++) {
				var endVector = new Vector2(i * cellWidth, (float)(sender.ActualHeight - sender.ActualHeight * (_historySecondary[i].value - MinValueSecondary) / (MaxValueSecondary - MinValueSecondary)));
				args.DrawingSession.DrawLine(startVector, endVector, _historySecondary[i].color);
				startVector = endVector;
				//cpb.AddLine(new Vector2(i * cellWidth, (float)(sender.ActualHeight - sender.ActualHeight * (_history[i].value - MinValue) / (MaxValue - MinValue))));
			}
			//cpb.EndFigure(CanvasFigureLoop.Open);
			//args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), color, thickness);
		}

		private void Gauge_OnDraw(CanvasControl sender, CanvasDrawEventArgs args) {
			var lastPrimary = _history[_history.Count - 1];
			var lastSecondary = _historySecondary[_historySecondary.Count - 1];

			var primaryColor = Color.FromArgb(128, lastPrimary.color.R, lastPrimary.color.G, lastPrimary.color.B);
			var secondaryColor = Color.FromArgb(128, lastSecondary.color.R, lastSecondary.color.G, lastSecondary.color.B);

			Rect rect = new Rect(0, 0, sender.ActualWidth, sender.ActualHeight);
			args.DrawingSession.DrawRectangle(rect, Color.FromArgb(128, 0, 170, 255));
			Rect rect2 = new Rect(
				0,
				Math.Max(0, sender.ActualHeight - sender.ActualHeight * (lastPrimary.value - MinValue) / (MaxValue - MinValue)),
				sender.ActualWidth,
				Math.Max(0, sender.ActualHeight * (lastPrimary.value - MinValue) / (MaxValue - MinValue)));
			args.DrawingSession.FillRectangle(rect2, primaryColor);

			rect = new Rect(0, 0, sender.ActualWidth, sender.ActualHeight);
			args.DrawingSession.DrawRectangle(rect, Color.FromArgb(128, 0, 170, 255));
			rect2 = new Rect(
				0,
				Math.Max(0, sender.ActualHeight - sender.ActualHeight * (lastSecondary.value - MinValueSecondary) / (MaxValueSecondary - MinValueSecondary)),
				sender.ActualWidth,
				Math.Max(0, sender.ActualHeight * (lastSecondary.value - MinValueSecondary) / (MaxValueSecondary - MinValueSecondary)));
			args.DrawingSession.FillRectangle(rect2, secondaryColor);
		}

		public struct GraphPoint {
			public float value;
			public Color color;
		}
		protected List<GraphPoint> _history = new List<GraphPoint>(300);
		protected List<GraphPoint> _historySecondary = new List<GraphPoint>(300);
		public void RegisterValue(GraphPoint value) {
			_history.RemoveAt(0);
			_history.Add(value);
			Refresh();
		}
		public void RegisterSecondaryValue(GraphPoint value) {
			_historySecondary.RemoveAt(0);
			_historySecondary.Add(value);
			Refresh();
		}
	}
}
