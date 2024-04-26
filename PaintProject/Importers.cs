using Interfaces;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace PaintProject {
    public interface IImporter {
        void Import();
        void SetCanvas(Canvas canvas);
        void Accept(IImportVisitor visitor);
    }

    public class XmlImporter : IImporter {
        private List<Type> _types;
        private Stack<BaseShape> _prototypes;
        private Canvas _canvas;

        public void SetTypes(List<Type> type) { _types = type; }
        public void SetShapes(Stack<BaseShape> shapes) { _prototypes = shapes; }
        public void SetCanvas(Canvas canvas) { _canvas = canvas; }
        public void Import() {
            var dialog = new OpenFileDialog();
            dialog.Title = "Load file";
            dialog.Multiselect = false;
            dialog.Filter = "Paint project | *.myext";
            if (dialog.ShowDialog() == DialogResult.OK) {
                XmlReader reader = XmlReader.Create(dialog.FileName);
                XmlSerializer serializer = new XmlSerializer(typeof(List<BaseShape>),_types.ToArray());

                List<BaseShape>? list;
                list = serializer.Deserialize(reader) as List<BaseShape>;
                reader.Close();

                if (list != null) {
                    list.Reverse();
                    _prototypes.Clear();
                    foreach (var shape in list) {
                        _prototypes.Push(shape);
                        _canvas.Children.Add(shape.Convert());
                    }
                }
            }
        }

        public void Accept(IImportVisitor visitor) {
            visitor.visitXml(this);
        }
    }

    public class PngImporter : IImporter {
        private Canvas _canvas;
        public void SetCanvas(Canvas canvas) { _canvas = canvas; }
        public void Import() {
            var dialog = new OpenFileDialog();
            dialog.Title = "Load image";
            dialog.Multiselect = false;
            dialog.Filter = "PNG files | *.png";
            if (dialog.ShowDialog() == DialogResult.OK) {
                _canvas.Children.Clear();
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(dialog.FileName, UriKind.Relative));
                ib.Stretch = Stretch.Uniform;
                RenderOptions.SetBitmapScalingMode(ib, BitmapScalingMode.HighQuality);
                ib.Freeze();
                _canvas.Background = ib;
            }
        }

        public void Accept(IImportVisitor visitor) {
            visitor.visitPng(this);
        }
    }
}
