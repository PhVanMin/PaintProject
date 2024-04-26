
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using Interfaces;
using System.Windows.Controls;

namespace MyLine {
    public class MyLine : BaseShape {
        public override string Icon => $"{_resourcePath}/Line.png";
        public double Thickness { get; set; } = 1;
        public Color? ColorStroke { get; set; }
        public Color? ColorFill { get; set; }
        public DoubleCollection? LineStrokeType { get; set; } = null;

        public override void SetFill(SolidColorBrush colorFill) {
            ColorFill = colorFill.Color;
        }

        public override void SetStrokeColor(SolidColorBrush colorStroke) {
            ColorStroke = colorStroke.Color;
        }

        public override void SetThickness(double thickness) {
            Thickness = thickness;
        }
        public override void SetStrokeType(IStrokeType strokeType) {
            LineStrokeType = strokeType.Convert();
        }

        public override UIElement Convert() {
            var item = new Line() {
                X1 = Second.X > First.X ? 0 : Math.Abs(Second.X - First.X),
                Y1 = Second.Y > First.Y ? 0 : Math.Abs(Second.Y - First.Y),
                X2 = Second.X < First.X ? 0 : Math.Abs(Second.X - First.X),
                Y2 = Second.Y < First.Y ? 0 : Math.Abs(Second.Y - First.Y),
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black),
                StrokeDashArray = LineStrokeType  
            };
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }

}
