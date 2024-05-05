using Fluent;
using Interfaces;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System;

namespace PaintProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow {
        public bool isDrawing = false;
        public bool isClick;
        public bool itemFocus = false;

        public BaseShape? painter = null;
        private IFactory? _exporterFactory;
        private IExporter? _dataExporter;
        private IImporter? _dataImporter;
        private ExportVisitor _exporterVisitor;
        private ImportVisitor _importerVisitor;
        private double _factor = 1;
        private Point _lastMousePositionOnTarget;

        public delegate void MouseEventHandler(MainWindow window, MouseEventArgs e);
        private event MouseEventHandler? _leftButtonDownHandler;
        private event MouseEventHandler? _mouseMoveHandler;
        private event MouseEventHandler? _leftButtonUpHandler;

        public List<Type> SupportedType { get; } = new();
        private List<BaseShape> _painters = new();
        public Stack<BaseShape> Prototypes { get; set; } = new();
        public Stack<BaseShape> DeletedPrototypes { get; } = new();
        public MainWindow() {
            InitializeComponent();
            _exporterVisitor = new ExportVisitor(this);
            _importerVisitor = new ImportVisitor(this);
            
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            _leftButtonDownHandler?.Invoke(this, e);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            _mouseMoveHandler?.Invoke(this, e);
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            _leftButtonUpHandler?.Invoke(this, e);
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e) {
            SetUpMouseHanlders();
            SetUpKeyBindings();
            SetUpColors();
            SetUpPainters();
            SetUpStrokes();
        }

        private void SetUpMouseHanlders() {
            _leftButtonDownHandler += LeftButtonDownHandler.HandleLeftButtonDown;
            _mouseMoveHandler += MouseMoveHandler.HandleMouseMove;
            _leftButtonUpHandler += LeftButtonUpHandler.HandleLeftButtonUp;
        }

        private void SetUpKeyBindings() {
            InputBindings.AddRange(Shortcut.Create(this).KeyBindings);
        }

        private void SetUpPainters() {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            var fis = new DirectoryInfo(folder).GetFiles("*.dll");

            foreach (var fi in fis) {
                // Lấy tất cả kiểu dữ liệu trong dll
                var assembly = Assembly.LoadFrom(fi.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types) {
                    if ((type.IsClass) && (type != typeof(BaseShape))
                        && (typeof(BaseShape).IsAssignableFrom(type))) {
                        var shape = (BaseShape)Activator.CreateInstance(type)!;
                        if (shape.Icon != null) {
                            _painters.Add(shape);
                        }
                        SupportedType.Add(shape.GetType());
                    }
                }
            }
            ShapeList.ItemsSource = _painters;
        }

        private void SetUpColors() {
            var colors = MyColorBrush.GetColors();
            ColorList.ItemsSource = colors;
            ColorList.SelectedIndex = 0;
            FillColorList.ItemsSource = colors;
        }

        private void SetUpStrokes() {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            var fis = new DirectoryInfo(folder).GetFiles("*.dll");

            foreach (var fi in fis) {
                // Lấy tất cả kiểu dữ liệu trong dll
                var assembly = Assembly.LoadFrom(fi.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types) {
                    if ((type.IsClass)
                        && (typeof(IStrokeType).IsAssignableFrom(type))) {
                        var stroke = (IStrokeType)Activator.CreateInstance(type)!;
                        StrokeComboBox.Items.Add(stroke);
                    }
                }
            }
            StrokeComboBox.SelectedIndex = 0;
        }

        private void RemoveFill_Click(object sender, RoutedEventArgs e) {
            FillColorList.SelectedIndex = -1;
        }

        private void Export() {
            _dataExporter = _exporterFactory?.CreateExporter();
            _dataExporter?.Accept(_exporterVisitor);
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            _exporterFactory = XmlFactory.Instance;
            Export();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            _exporterFactory = PngFactory.Instance;
            Export();
        }

        private void Import() {
            _dataImporter = _exporterFactory?.CreateImporter();
            Prototypes.Clear();
            DeletedPrototypes.Clear();
            _dataImporter?.Accept(_importerVisitor);
        }

        private void Load_Click(object sender, RoutedEventArgs e) {
            _exporterFactory = XmlFactory.Instance;
            Import();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e) {
            _exporterFactory = PngFactory.Instance;
            Import();
        }

        private void ChangePainter_Click(object sender, SelectionChangedEventArgs e) {
            painter = (BaseShape?)ShapeList.SelectedItem;
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e) {
            _lastMousePositionOnTarget = Mouse.GetPosition(grid);

            if (e.Delta > 0) {
                _factor += 0.1;
                if (_factor > 3) _factor = 3;
            }
            if (e.Delta < 0) {
                _factor -= 0.1;
                if (_factor < 0.2) _factor = 0.2;
            }
            ScaleValue.Text = $"{Math.Round(100 * _factor)}%";
            scaleTransform.ScaleX = _factor;
            scaleTransform.ScaleY = _factor;
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0) {
                Point targetBefore;
                Point targetNow;


                targetBefore = _lastMousePositionOnTarget;
                targetNow = Mouse.GetPosition(grid);

                double dXInTargetPixels = targetNow.X - targetBefore.X;
                double dYInTargetPixels = targetNow.Y - targetBefore.Y;

                double multiplicatorX = e.ExtentWidth / grid.ActualWidth;
                double multiplicatorY = e.ExtentHeight / grid.ActualHeight;

                double newOffsetX = s.HorizontalOffset -
                                    dXInTargetPixels * multiplicatorX;
                double newOffsetY = s.VerticalOffset -
                                    dYInTargetPixels * multiplicatorY;

                if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY)) {
                    return;
                }

                s.ScrollToHorizontalOffset(newOffsetX);
                s.ScrollToVerticalOffset(newOffsetY);
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e) {

        }
    }
}