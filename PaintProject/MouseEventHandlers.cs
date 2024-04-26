using Interfaces;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PaintProject {
    internal class LeftButtonDownHandler {
        public static void HandleLeftButtonDown(MainWindow window, MouseEventArgs e) {
            var pos = e.GetPosition(window.myCanvas);
            if (HandleItemFocus(window, pos))
                return;

            if (window.painter != null) {
                window.isClick = true;
                window.isDrawing = true;
                window.painter.First = pos;
                window.painter.SetStrokeType((IStrokeType)window.StrokeComboBox.SelectedItem);
                window.painter.SetStrokeColor((SolidColorBrush)window.ColorList.SelectedItem);
                window.painter.SetFill((SolidColorBrush)window.FillColorList.SelectedItem ?? (new SolidColorBrush(Colors.Transparent)));
            }
        }
        private static bool HandleItemFocus(MainWindow window, Point pos) {
            if (!window.itemFocus)
                return false;

            var item = window.Prototypes.Peek();
            if (!item.IsFocusable) 
                return false;

            var minX = item.First.X < item.Second.X ? item.First.X : item.Second.X;
            var minY = item.First.Y < item.Second.Y ? item.First.Y : item.Second.Y;
            if ((pos.X < minX) ||
                (pos.X > (minX + Math.Abs(item.First.X - item.Second.X))) ||
                (pos.Y < minY) ||
                (pos.Y > (minY + Math.Abs(item.First.Y - item.Second.Y)))) {
                FocusManager.SetFocusedElement(window, null);
                window.itemFocus = false;
                item.IsFocusable = false;
                return true;
            }

            return false;
        }
    }

    internal class LeftButtonUpHandler {
        public static void HandleLeftButtonUp(MainWindow window, MouseEventArgs e) {
            if (window.isDrawing && !window.isClick) {
                window.DeletedPrototypes.Clear();
                var shape = (BaseShape) window.painter.Clone();
                window.myCanvas.Children.RemoveAt(window.myCanvas.Children.Count - 1);
                window.myCanvas.Children.Add(shape.Convert());
                window.Prototypes.Push(shape);

                if (shape.IsFocusable)
                    SetFocus(window);
            }
            window.isDrawing = false;
        }

        private static void SetFocus(MainWindow window) {
            window.myCanvas.Children[window.myCanvas.Children.Count - 1].Focus();
            window.itemFocus = true;
        }
    }

    internal class MouseMoveHandler {
        public static void HandleMouseMove(MainWindow window, MouseEventArgs e) {
            if (window.isDrawing) {
                if (window.isClick)
                    window.isClick = false;
                else
                    window.myCanvas.Children.RemoveAt(window.myCanvas.Children.Count - 1);

                window.painter.Second = e.GetPosition(window.myCanvas);
                window.myCanvas.Children.Add(window.painter?.Convert());
            }
        }
    }
}
