using Interfaces;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace MyText {
    public class MyText : BaseShape {
        public MyText() { IsFocusable = true; }
        public override string Icon => $"{_resourcePath}/Text.png";
        public string StringText { get; set; } = "";
        public Color? ColorStroke { get; set; }
        public Color? ColorFill { get; set; }
        public override UIElement Convert() {
            var item = new TextBox() {
                Width = Math.Abs(First.X - Second.X),
                Height = Math.Abs(First.Y - Second.Y),
                Background = new SolidColorBrush(ColorFill ?? Colors.Transparent),
                Foreground = new SolidColorBrush(ColorStroke ?? Colors.Black),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = (Math.Abs(First.Y - Second.Y) * 0.2) + 1
            };

            if (IsFocusable) {
                item.LostFocus += (object sender, RoutedEventArgs e) => {
                    item.BorderThickness = new Thickness(0);
                    item.Focusable = false;
                };
                item.TextChanged += (object sender, TextChangedEventArgs e) => {
                    StringText = item.Text;
                };
            } else {
                item.Text = StringText;
                item.BorderThickness = new Thickness(0);
                item.Focusable = false;
            }
            
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }

        public override void SetFill(SolidColorBrush colorFill) {
            ColorFill = colorFill.Color;
        }

        public override void SetStrokeColor(SolidColorBrush colorStroke) {
            ColorStroke = colorStroke.Color;
        }
    }

}
