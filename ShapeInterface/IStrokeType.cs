using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Interfaces {
    public interface IStrokeType {
        string Name { get; }
        string Image { get; }
        DoubleCollection? Convert();
    }

    public class SolidStroke : IStrokeType {
        public string Name => "Solid";

        public string Image => "/Interfaces;Component/Solid.png";

        public DoubleCollection? Convert() {
            return null;
        }
    }

    public class DotStroke : IStrokeType {
        public string Name => "Dot";

        public string Image => "/Interfaces;Component/Dot.png";

        public DoubleCollection? Convert() {
            return new() { 2, 2 };
        }
    }

    public class DashStroke : IStrokeType {
        public string Name => "Dash";

        public string Image => "/Interfaces;Component/Dash.png";

        public DoubleCollection? Convert() {
            return new() { 4, 2 };
        }
    }

    public class DashDotStroke : IStrokeType {
        public string Name => "Dash Dot";

        public string Image => "/Interfaces;Component/DashDot.png";

        public DoubleCollection? Convert() {
            return new() { 4, 2, 2, 2 };
        }
    }
}
