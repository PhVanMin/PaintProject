using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintProject {
    public interface IShape : ICloneable {
        void AddFirst(Point point);
        void AddSecond(Point point);
        UIElement Convert();
        void SetFill(SolidColorBrush colorFill);
        void SetStrokeColor(SolidColorBrush colorStroke);
        void SetThickness(double thickness);
    }
    public class MyShape : IShape {
        public Point First { get; set; }
        public Point Second { get; set; }
        public double Thickness { get; set; } = 1;
        public Color? ColorStroke { get; set; }
        public Color? ColorFill { get; set; }
        public virtual string? Icon { get; }

        public void AddFirst(Point point) {
            First = point;
        }

        public void AddSecond(Point point) {
            Second = point;
        }

        public object Clone() { return MemberwiseClone(); }

        public virtual UIElement Convert() => null;

        public void SetFill(SolidColorBrush colorFill) {
            ColorFill = colorFill.Color;
        }

        public void SetStrokeColor(SolidColorBrush colorStroke) {
            ColorStroke = colorStroke.Color;
        }

        public void SetThickness(double thickness) {
            Thickness = thickness;
        }
    }
    public class MyRightArrow : MyShape {
        public override string Icon => $"/Images/{nameof(MyRightArrow)}.png";

        public override UIElement Convert() {
            var item = new Path() {
                Data = Geometry.Parse("M0,50 L100,50 L100,0 L200,80 L100,160 L100,110 L0,110 L0,50"),
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                StrokeThickness = Thickness,
                Stretch = Stretch.Fill,
                Stroke = new SolidColorBrush(ColorStroke??Colors.Black),
                Fill = new SolidColorBrush(ColorFill ?? Colors.Transparent),
            };
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }
    public class MyLine : MyShape {
        public override string Icon => "/Images/Line.png";

        public override UIElement Convert() {
            return new Line() {
                X1 = First.X,
                Y1 = First.Y,
                X2 = Second.X,
                Y2 = Second.Y,
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black)
            };
        }
    }
    public class MyRectangle : MyShape {
        public override string Icon => "/Images/Rectangle.png";

        public override UIElement Convert() {
            var item = new Rectangle() {
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black),
                Fill = new SolidColorBrush(ColorFill ?? Colors.Transparent)
            };
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }
    public class MyEllipse : MyShape {
        public override string Icon => "/Images/Ellipse.png";
        public override UIElement Convert() {
            var item = new Ellipse() {
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black),
                Fill = new SolidColorBrush(ColorFill ?? Colors.Transparent),
            };
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }
    public class MyImage : MyShape {
        private BitmapSource _bitmapSource;
        public void SetSource(BitmapSource bitmapSource) {
            _bitmapSource = bitmapSource;
        }
        public override UIElement Convert() {
            return new Image() {
                Source = _bitmapSource,
            };
        }
    }
    public class MyText : MyShape {
        public override string Icon => "/Images/Text.png";
        public override UIElement Convert() {
            var item = new TextBox() {
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                Background = new SolidColorBrush(ColorFill ?? Colors.Transparent),
                Foreground = new SolidColorBrush(ColorStroke ?? Colors.Black),
                Focusable = true,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = (Math.Abs(First.Y - Second.Y) * 0.2) + 1
            };
            item.LostFocus += (object sender, RoutedEventArgs e) => {
                item.BorderThickness = new Thickness(0);
                item.Focusable = false;
            };
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }
}
