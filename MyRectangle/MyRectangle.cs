
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using Interfaces;

namespace MyRectangle {
    public class MyRectangle : BaseShape {
        public override string Icon => $"{_resourcePath}/Rectangle.png";
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
            var item = new Rectangle() {
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black),
                Fill = new SolidColorBrush(ColorFill ?? Colors.Transparent),
                StrokeDashArray = LineStrokeType
            };
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }

}
