using Fluent;
using Fluent.Localization.Languages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PaintProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow {
        string _fileName = "data.myext";
        bool _isDrawing = false;
        bool _newElement;
        IShape? _painter = null;
        List<IShape> _painters = new List<IShape>();
        public Stack<IShape> Prototypes { get; } = new Stack<IShape>();
        public Stack<IShape> DeletedPrototypes { get; } = new Stack<IShape>();
        public MainWindow() {
            InitializeComponent();
            Shortcut shortcut = new Shortcut(this);
            InputBindings.AddRange(shortcut.KeyBindings);        
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            _painter = (IShape?) ShapeList.SelectedItem;
            if (_painter == null) 
                return;
            DeletedPrototypes.Clear();
            _isDrawing = true;
            _newElement = true;
            _painter.AddFirst(e.GetPosition(myCanvas));
            _painter.SetStrokeColor((SolidColorBrush)ColorList.SelectedItem);
            _painter.SetFill((SolidColorBrush)FillColorList.SelectedItem??(new SolidColorBrush(Colors.Transparent)));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            if (_isDrawing) {
                if (_newElement) {
                    _newElement = false;
                } else {
                    myCanvas.Children.RemoveAt(myCanvas.Children.Count - 1);
                }

                _painter?.AddSecond(e.GetPosition(myCanvas));
                myCanvas.Children.Add(_painter.Convert());
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (_isDrawing) {
                _isDrawing = false;
                Prototypes.Push((IShape)_painter.Clone());
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
            _painters.Add(new MyRightArrow());

            //LoadData();

            ColorList.ItemsSource = colors;
            ColorList.SelectedIndex = 0;
            FillColorList.ItemsSource = colors;
            ShapeList.ItemsSource = _painters;
        }

        private void RemoveFill_Click(object sender, RoutedEventArgs e) {
            FillColorList.SelectedIndex = -1;
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            
        }

        private void SaveData(string path) {
            XmlWriter writer = XmlWriter.Create(path + _fileName);
            List<Type> types = new();
            foreach (var s in _painters) {
                types.Add(s.GetType());
            }   
            List<MyShape> list = new List<MyShape>();
            foreach (var shape in Prototypes) {
                list.Add((MyShape) Convert.ChangeType(shape, shape.GetType()));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<MyShape>), types.ToArray());
            serializer.Serialize(writer, list);
            writer.Close();
        }

        private void LoadData(string filepath) {
            if (File.Exists(filepath)) {
                XmlReader reader = XmlReader.Create(filepath);
                List<MyShape>? list;
                List<Type> types = [];
                foreach (var s in _painters) {
                    types.Add(s.GetType());
                }
                XmlSerializer serializer = new XmlSerializer(typeof(List<MyShape>), types.ToArray());
                list = serializer.Deserialize(reader) as List<MyShape>;
                if (list != null) {
                    list.Reverse();
                    foreach (var type in list) {
                        Prototypes.Push(type);
                        myCanvas.Children.Add(type.Convert());
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFolderDialog();
            dialog.Title = "Save file location";
            if (dialog.ShowDialog() == true) {
                string filePath = dialog.FolderName + "\\";
                SaveData(filePath);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {

        }

        private void Load_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Title = "Load file";
            dialog.Multiselect = false;
            dialog.Filter = "Paint project | *.myext";
            if (dialog.ShowDialog() == true) {
                LoadData(dialog.FileName);
            }
        }
        private void LoadImage_Click(object sender, RoutedEventArgs e) {

        }
    }
}