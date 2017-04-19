using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace HSDHelper.Displays {
    class DialMeter : Gauge {
        protected override void Draw(CanvasControl sender, CanvasDrawEventArgs args) {
            // draw arc

            //arcPoints[0] = new Vector2(40);
            //arcPoints[1] = Vector2.Min(new Vector2(480, 320), canvas.Size.ToVector2() - new Vector2(40));

            float arcStartAngle = (float)MinAngle;
            float arcSweepAngle = (float)MaxAngle - (float)MinAngle;

            //arcRadiusX = Vector2.Distance(arcPoints[0], arcPoints[1]);
            //arcRadiusY = arcRadiusX;

            var ds = args.DrawingSession;

            var centerPoint = new Vector2((float)sender.ActualWidth / 2, (float)sender.ActualHeight / 2);

            // Compute positions.
            var ellipseRadius = new Vector2((float)sender.ActualWidth / 2 - 4, (float)sender.ActualHeight / 2 - 4);

            ellipseRadius.X = Math.Abs(ellipseRadius.X);
            ellipseRadius.Y = Math.Abs(ellipseRadius.Y);

            float startAngle = (float)DegreesToRadians(arcStartAngle);
            float sweepAngle = (float)DegreesToRadians(arcSweepAngle);

            var startPoint = centerPoint + Vector2.Transform(Vector2.UnitX, Matrix3x2.CreateRotation(startAngle)) * ellipseRadius;

            // Draw the arcs
            using (var builder = new CanvasPathBuilder(sender)) {
                builder.BeginFigure(startPoint);
                builder.AddArc(centerPoint, ellipseRadius.X, ellipseRadius.Y, startAngle, sweepAngle);
                builder.EndFigure(CanvasFigureLoop.Open);

                using (var geometry = CanvasGeometry.CreatePath(builder)) {
                    ds.DrawGeometry(geometry, Color.FromArgb(40, 0, 0, 255), ellipseRadius.X * 0.06f, null);
                }
            }
            
            // draw dial markers wide
            using (var builder = new CanvasPathBuilder(sender)) {
                for (var i = 0; i <= arcSweepAngle / 10; i += 2) {
                    var val = i * 10;
                    var angle = DegreesToRadians((float)(MinAngle + i * 10));
                    //var angle = DegreesToRadians((float)((MaxAngle - MinAngle) * (val - MinValue) / (MaxValue - MinValue) + MinAngle));
                    var point = centerPoint + Vector2.Transform(Vector2.UnitX, Matrix3x2.CreateRotation((float)angle)) * ellipseRadius;


                    builder.BeginFigure(point - (centerPoint - point) / 10);
                    builder.AddLine(point + (centerPoint - point) / 8);
                    builder.EndFigure(CanvasFigureLoop.Open);
                }

                using (var geometry = CanvasGeometry.CreatePath(builder)) {
                    ds.DrawGeometry(geometry, Color.FromArgb(128, 255, 0, 255), ellipseRadius.X * 0.04f, null);
                }
            }
            // draw dial markers narrow
            using (var builder = new CanvasPathBuilder(sender)) {

                for (var i = 1; i <= arcSweepAngle / 10; i += 2) {
                    var val = i * 10;
                    var angle = DegreesToRadians((float)(MinAngle + i * 10));
                    var point = centerPoint + Vector2.Transform(Vector2.UnitX, Matrix3x2.CreateRotation((float)angle)) * ellipseRadius;


                    builder.BeginFigure(point - (centerPoint - point) / 10);
                    builder.AddLine(point + (centerPoint - point) / 12);
                    builder.EndFigure(CanvasFigureLoop.Open);
                }

                using (var geometry = CanvasGeometry.CreatePath(builder)) {
                    ds.DrawGeometry(geometry, Color.FromArgb(128, 255, 0, 255), ellipseRadius.X * 0.02f, null);
                }
            }
            // draw units
            ds.DrawText(Units, centerPoint + new Vector2(0, (float)(-0.2 * sender.ActualHeight)), Color.FromArgb(255, 128, 128, 128));
            ds.DrawText(Value.ToString("0.0"), centerPoint, Color.FromArgb(255, 128, 128, 128));
            ds.DrawText(Title, centerPoint + new Vector2(0, (float)(0.2 * sender.ActualHeight)), Color.FromArgb(255, 128, 128, 128));

            // Draw the meter arc
            using (var builder = new CanvasPathBuilder(sender)) {
                var angle = DegreesToRadians((float)((MaxAngle - MinAngle) * (Value - MinValue) / (MaxValue - MinValue) + MinAngle));
                builder.BeginFigure(startPoint);
                builder.AddArc(centerPoint, ellipseRadius.X, ellipseRadius.Y, startAngle, (float)(angle -startAngle));
                builder.EndFigure(CanvasFigureLoop.Open);

                using (var geometry = CanvasGeometry.CreatePath(builder)) {
                    ds.DrawGeometry(geometry, Color.FromArgb(80, 0, 0, 255), ellipseRadius.X * 0.06f, null);
                }
            }
        }
    }
}
