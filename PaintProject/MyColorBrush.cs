using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PaintProject {
    internal static class MyColorBrush {
        public static List<SolidColorBrush> GetColors() {
            return new List<SolidColorBrush>() {
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.Green),
                new SolidColorBrush(Colors.Orange),
                new SolidColorBrush(Colors.Orchid),
                new SolidColorBrush(Colors.Yellow),
                new SolidColorBrush(Colors.Purple),
            };
        }
    }
}
