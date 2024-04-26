
using Interfaces;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyImage {
    public class MyImage : BaseShape {
        public string Source { get; set; }

        public override UIElement Convert() {
            var bitmapSource = new BitmapImage(new Uri(Source, UriKind.Absolute));
            return new Image() {
                Source = bitmapSource,
                Stretch = Stretch.UniformToFill
            };
        }
    }

}
