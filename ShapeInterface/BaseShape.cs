using System.Windows.Media;
using System.Windows;

namespace Interfaces {
    public abstract class BaseShape : ICloneable {
        protected string _resourcePath => $"/{GetType().Namespace};Component";
        public virtual string? Icon => null;
        public Point First { get; set; }
        public Point Second { get; set; }
        public bool IsFocusable { get; set; } = false;
        public virtual void SetStrokeColor(SolidColorBrush colorStroke) {}
        public virtual void SetThickness(double thickness) {}
        public virtual void SetFill(SolidColorBrush colorFill) {}
        public virtual void SetStrokeType(IStrokeType strokeType) {}
        public abstract UIElement Convert();
        public object Clone() => MemberwiseClone();
    }
}
