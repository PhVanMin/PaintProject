using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintProject {
    public interface IShape : ICloneable {
        void AddFirst(Point point);
        void AddSecond(Point point);
        void SetThickness(double thickness);
        void SetStroke(SolidColorBrush colorStroke);
        void SetFill(SolidColorBrush colorFill);
        UIElement Convert();
        string Icon { get; }
    }

    public class MyLine : IShape {
        private Point _first;
        private Point _second;
        private double _thickness = 1;
        private SolidColorBrush _colorStroke = new SolidColorBrush(Colors.Black);

        public string Icon => "/Images/Line.png";

        public void AddFirst(Point point) {
            _first = point;
        }

        public void AddSecond(Point point) {
            _second = point;
        }

        public object Clone() {
            return MemberwiseClone();
        }

        public UIElement Convert() {
            return new Line() {
                X1 = _first.X,
                Y1 = _first.Y,
                X2 = _second.X,
                Y2 = _second.Y,
                StrokeThickness = _thickness,
                Stroke = _colorStroke
            };
        }

        public void SetFill(SolidColorBrush colorFill) {
            throw new NotImplementedException();
        }

        public void SetStroke(SolidColorBrush colorBrush) {
            _colorStroke = colorBrush;
        }

        public void SetThickness(double thickness) {
            _thickness = thickness;
        }
    }
    public class MyRectangle : IShape {
        private Point _first;
        private Point _second;
        private double _thickness = 1;
        private SolidColorBrush _colorStroke = new SolidColorBrush(Colors.Black);
        private SolidColorBrush _colorFill = new SolidColorBrush(Colors.Transparent);
        public string Icon => "/Images/Rectangle.png";

        public void AddFirst(Point point) {
            _first = point;
        }

        public void AddSecond(Point point) {
            _second = point;
        }

        public UIElement Convert() {
            var item = new Rectangle() {
                Width = Math.Abs(_first.X - _second.X),
                Height = Math.Abs(_first.Y - _second.Y),
                StrokeThickness = _thickness,
                Stroke = _colorStroke,
                Fill = _colorFill,
            };
            Canvas.SetLeft(item, _first.X < _second.X ? _first.X : _second.X);
            Canvas.SetTop(item, _first.Y < _second.Y ? _first.Y : _second.Y);
            return item;
        }

        public object Clone() {
            return MemberwiseClone();
        }

        public void SetFill(SolidColorBrush colorFill) {
            _colorFill = colorFill;
        }

        public void SetStroke(SolidColorBrush colorBrush) {
            _colorStroke = colorBrush;
        }

        public void SetThickness(double thickness) {
            _thickness = thickness;
        }
    }
    public class MyEllipse : IShape {
        private Point _first;
        private Point _second;
        private double _thickness = 1;
        private SolidColorBrush _colorStroke = new SolidColorBrush(Colors.Black);
        private SolidColorBrush _colorFill = new SolidColorBrush(Colors.Transparent);
        public string Icon => "/Images/Ellipse.png";

        public void AddFirst(Point point) {
            _first = point;
        }

        public void AddSecond(Point point) {
            _second = point;
        }

        public object Clone() {
            return MemberwiseClone();
        }

        public UIElement Convert() {
            var item = new Ellipse() {
                Width = Math.Abs(_first.X - _second.X),
                Height = Math.Abs(_first.Y - _second.Y),
                StrokeThickness = _thickness,
                Stroke = _colorStroke,
                Fill = _colorFill,
            };
            Canvas.SetLeft(item, _first.X < _second.X ? _first.X : _second.X);
            Canvas.SetTop(item, _first.Y < _second.Y ? _first.Y : _second.Y);
            return item;
        }

        public void SetFill(SolidColorBrush colorFill) {
            _colorFill = colorFill;
        }

        public void SetStroke(SolidColorBrush colorBrush) {
            _colorStroke = colorBrush;
        }

        public void SetThickness(double thickness) {
            _thickness = thickness;
        }
    }
}
