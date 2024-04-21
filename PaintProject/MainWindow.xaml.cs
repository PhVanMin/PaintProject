using Fluent;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PaintProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow {
        private bool _isDrawing = false;
        private bool _isClick;
        private bool _textBoxFocus = false;

        private IShape? _painter = null;
        IFactory? _dataFactory;
        IExporter? _dataExporter;
        IImporter? _dataImporter;
        ExportVisitor _exportVisitor;

        public List<IShape> Painters = new List<IShape>();
        public Stack<IShape> Prototypes { get; } = new Stack<IShape>();
        public Stack<IShape> DeletedPrototypes { get; } = new Stack<IShape>();

        public MainWindow() {
            InitializeComponent();
            _exportVisitor = new ExportVisitor(this);
            InputBindings.AddRange(Shortcut.Create(myCanvas, Prototypes, DeletedPrototypes).KeyBindings);        
        }
        private void StrokeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tuple<string, string>? selectedItem = (Tuple<string, string>)StrokeComboBox.SelectedItem;
            if (selectedItem != null)
            {
                string strokeType = selectedItem.Item1; 
                switch (strokeType)
                {
                    case "Solid":
                        foreach (var shape in _painters)
                        {
                            shape.SetStrokeType(StrokeType.Solid);
                        }
                        break;
                    case "Dash":
                        foreach (var shape in _painters)
                        {
                            shape.SetStrokeType(StrokeType.Dash);
                        }
                        break;
                    case "Dot":
                        foreach (var shape in _painters)
                        {
                            shape.SetStrokeType(StrokeType.Dot);
                        }
                        break;
                    case "Dash dot dot":
                        foreach (var shape in _painters)
                        {
                            shape.SetStrokeType(StrokeType.DashDotDot);
                        }
                        break;
                }
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (_textBoxFocus) {
                HandleTextBoxFocus(e.GetPosition(myCanvas));
            } else if (_painter != null) {
                _isClick = true;
                StartDrawingAt(e.GetPosition(myCanvas));
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            if (_isDrawing) {
                RemovePreviousPreview(myCanvas);
                DrawPreviewAt(e.GetPosition(myCanvas));
            }
        }
        
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (_isDrawing && !_isClick) {
                HandleTextBoxDrawing(myCanvas);
                FinishDrawing();
            } else {
                _isDrawing = false;
            }
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e) {
            SetUpColors();
            SetUpPainters();
        }

        private void SetUpPainters() {
            Painters.Add(new MyLine());
            Painters.Add(new MyRectangle());
            Painters.Add(new MyEllipse());
            Painters.Add(new MyRightArrow());
            Painters.Add(new MyText());
            ShapeList.ItemsSource = Painters;
        }

        private void SetUpColors() {
            var colors = MyColorBrush.GetColors();
            ColorList.ItemsSource = colors;
            ColorList.SelectedIndex = 0;
            FillColorList.ItemsSource = colors;
            ShapeList.ItemsSource = _painters;

            StrokeComboBox.ItemsSource = strokes;
        }

        private void RemoveFill_Click(object sender, RoutedEventArgs e) {
            FillColorList.SelectedIndex = -1;
        }

        private void Export() {
            _dataExporter = _dataFactory?.CreateExporter();
            _dataExporter?.Accept(_exportVisitor);
            _dataExporter?.Export();
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            _dataFactory = XmlFactory.Instance;
            Export();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            _dataFactory = PngFactory.Instance;
            Export();
        }

        private void Load_Click(object sender, RoutedEventArgs e) {
            _dataFactory = XmlFactory.Instance;
            _dataImporter = _dataFactory.CreateImporter();
            _dataImporter.SetUp(Painters);

            var list = (List<IShape>?) _dataImporter.Import();
            if (list != null) {
                list.Reverse();
                Prototypes.Clear();
                foreach (var type in list) {
                    Prototypes.Push(type);
                    myCanvas.Children.Add(type.Convert());
                }
            }
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e) {
            _dataFactory = PngFactory.Instance;
            _dataImporter = _dataFactory.CreateImporter();

            var image = (MyImage?) _dataImporter.Import();
            if (image != null) {
                Prototypes.Push(image);
                myCanvas.Children.Add(image.Convert());
            }
        }

        private void ChangePainter_Click(object sender, SelectionChangedEventArgs e) {
            _painter = (IShape?) ShapeList.SelectedItem;
        }

        private void HandleTextBoxFocus(Point pos) {
            var item = (MyShape)Prototypes.Peek();
            var minX = item.First.X < item.Second.X ? item.First.X : item.Second.X;
            var minY = item.First.Y < item.Second.Y ? item.First.Y : item.Second.Y;
            if ((pos.X < minX) ||
                (pos.X > (minX + Math.Abs(item.First.X - item.Second.X))) ||
                (pos.Y < minY) ||
                (pos.Y > (minY + Math.Abs(item.First.Y - item.Second.Y)))) {
                FocusManager.SetFocusedElement(this, null);
                _textBoxFocus = false;
            }
        }
        private void StartDrawingAt(Point pos) {
            if (_textBoxFocus)
                return;

            _isDrawing = true;
            _painter?.AddFirst(pos);
            _painter?.SetStrokeColor((SolidColorBrush)ColorList.SelectedItem);
            _painter?.SetFill((SolidColorBrush)FillColorList.SelectedItem ?? (new SolidColorBrush(Colors.Transparent)));
        }

        private void RemovePreviousPreview(Canvas canvas) {
            if (_isClick)
                _isClick = false;
            else
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
        }

        private void DrawPreviewAt(Point pos) {
            _painter?.AddSecond(pos);
            myCanvas.Children.Add(_painter?.Convert());
        }

        private void FinishDrawing() {
            _isDrawing = false;
            DeletedPrototypes.Clear();
            Prototypes.Push((IShape)_painter.Clone());
        }

        private void HandleTextBoxDrawing(Canvas canvas) {
            if (_painter?.GetType() == typeof(MyText)) {
                canvas.Children[canvas.Children.Count - 1].Focus();
                _textBoxFocus = true;
            }
        }
    }
}