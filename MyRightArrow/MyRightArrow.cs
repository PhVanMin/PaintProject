using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Interfaces;
using System.Windows.Shapes;

namespace MyRightArrow {
    public class MyRightArrow : BaseShape {
        public override string Icon => $"{_resourcePath}/MyRightArrow.png";
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
            var item = new Path() {
                Data = Geometry.Parse("M0,50 L100,50 L100,0 L200,80 L100,160 L100,110 L0,110 L0,50"),
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                StrokeThickness = Thickness,
                Stretch = Stretch.Fill,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black),
                Fill = new SolidColorBrush(ColorFill ?? Colors.Transparent),
                StrokeDashArray = LineStrokeType,
            };

            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }

}
