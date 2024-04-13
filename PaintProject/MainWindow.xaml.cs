﻿using Fluent;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow {
        bool _isDrawing = false;
        bool _newElement;
        IShape? _painter = null;

        List<IShape> _painters = new List<IShape>();
        List<IShape> _prototypes = new List<IShape>();
        public MainWindow() {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            _painter = (IShape?) ShapeList.SelectedItem;
            if (_painter == null) 
                return;
            _isDrawing = true;
            _newElement = true;
            _painter.AddFirst(e.GetPosition(myCanvas));
            _painter.SetStroke((SolidColorBrush)ColorList.SelectedItem);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            if (_isDrawing) {
                if (_newElement) {
                    _newElement = false;
                } else {
                    myCanvas.Children.RemoveAt(myCanvas.Children.Count - 1);
                }

                _painter.AddSecond(e.GetPosition(myCanvas));
                myCanvas.Children.Add(_painter.Convert());
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (_isDrawing) {
                _isDrawing = false;
                _prototypes.Add((IShape)_painter.Clone());
            }
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e) {
            List<SolidColorBrush> colors = new List<SolidColorBrush>() {
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Blue),
            };

            _painters.Add(new MyLine());
            _painters.Add(new MyRectangle());
            _painters.Add(new MyEllipse());

            ColorList.ItemsSource = colors;
            ColorList.SelectedIndex = 0;
            ShapeList.ItemsSource = _painters;
        }
    }
}