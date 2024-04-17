using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Windows.Forms.LinkLabel;

namespace PaintProject {
    public enum StrokeType
    {
        Solid,
        Dash,
        Dot,
        DashDotDot
    }
    public interface IShape : ICloneable {
        void AddFirst(Point point);
        void AddSecond(Point point);
        void SetThickness(double thickness);
        void SetStrokeColor(SolidColorBrush colorStroke);
        void SetFill(SolidColorBrush colorFill);
        void SetStrokeType(StrokeType strokeType);
        UIElement Convert();
    }
    public class MyShape : IShape {
        public Point First { get; set; }
        public Point Second { get; set; }
        public double Thickness { get; set; } = 1;
        public Color? ColorStroke { get; set; }
        public Color? ColorFill { get; set; }
        public StrokeType LineStrokeType { get; set; } = StrokeType.Solid;
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
        public void SetStrokeType(StrokeType strokeType)
        {
            LineStrokeType = strokeType;
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
            switch (LineStrokeType) 
            {
                case StrokeType.Solid:
                    item.StrokeDashArray = null;
                    break;
                case StrokeType.Dash:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2 });
                    break;
                case StrokeType.Dot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });
                    break;
                case StrokeType.DashDotDot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2, 1, 2 });
                    break;
                default:
                    item.StrokeDashArray = null;
                    break;
            }
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }
    public class MyLine : MyShape {
        public override string Icon => "/Images/Line.png";

        public override UIElement Convert()
        {
            var item = new Line()
            {
                X1 = First.X,
                Y1 = First.Y,
                X2 = Second.X,
                Y2 = Second.Y,
                StrokeThickness = Thickness,
                Stroke = new SolidColorBrush(ColorStroke ?? Colors.Black)
            };

            switch (LineStrokeType)
            {
                case StrokeType.Solid:
                    item.StrokeDashArray = null;
                    break;
                case StrokeType.Dash:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2 });
                    break;
                case StrokeType.Dot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });
                    break;
                case StrokeType.DashDotDot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2, 1, 2 });
                    break;
                default:
                    item.StrokeDashArray = null;
                    break;
            }

            return item;
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
                Fill = new SolidColorBrush(ColorFill ?? Colors.Transparent),
            };
            switch (LineStrokeType)
            {
                case StrokeType.Solid:
                    item.StrokeDashArray = null;
                    break;
                case StrokeType.Dash:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2 });
                    break;
                case StrokeType.Dot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });
                    break;
                case StrokeType.DashDotDot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2, 1, 2 });
                    break;
                default:
                    item.StrokeDashArray = null;
                    break;
            }
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
            switch (LineStrokeType)
            {
                case StrokeType.Solid:
                    item.StrokeDashArray = null;
                    break;
                case StrokeType.Dash:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2 });
                    break;
                case StrokeType.Dot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });
                    break;
                case StrokeType.DashDotDot:
                    item.StrokeDashArray = new DoubleCollection(new double[] { 4, 2, 1, 2 });
                    break;
                default:
                    item.StrokeDashArray = null;
                    break;
            }
            Canvas.SetLeft(item, First.X < Second.X ? First.X : Second.X);
            Canvas.SetTop(item, First.Y < Second.Y ? First.Y : Second.Y);
            return item;
        }
    }
}
