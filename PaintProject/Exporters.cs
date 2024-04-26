using Interfaces;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace PaintProject {
    public interface IExporter {
        void Export();
        void Accept(IExportVisitor visitor);
    }

    public class XmlExporter : IExporter {
        private List<Type> _types;
        private List<BaseShape> _shapes;
        public void Accept(IExportVisitor visitor) {
            visitor.visitXml(this);
        }
        public void SetTypes(List<Type> types) { _types = types; }
        public void SetShapes(Stack<BaseShape> shapes) { _shapes = shapes.ToList(); }

        public void Export() {
            var dialog = new SaveFileDialog();
            dialog.Title = "Save file";
            dialog.Filter = "Paint project | *.myext";
            if (dialog.ShowDialog() == DialogResult.OK) {
                XmlWriter writer = XmlWriter.Create(dialog.FileName);
                XmlSerializer serializer = new XmlSerializer(typeof(List<BaseShape>), _types.ToArray());
                serializer.Serialize(writer, _shapes);
                writer.Close();
            }
        }
    }

    public class PngExporter : IExporter {
        private Canvas _canvas;

        public void SetCanvas(Canvas canvas) { _canvas = canvas; }
        public void Accept(IExportVisitor visitor) {
            visitor.visitPng(this);
        }

        public void Export() {
            var dialog = new SaveFileDialog();
            dialog.Title = "Save as PNG";
            dialog.Filter = "PNG files | *.png";
            if (dialog.ShowDialog() == DialogResult.OK) {
                Stream mystream;
                double width = _canvas.RenderSize.Width;
                double height = _canvas.RenderSize.Height;
                if ((mystream = dialog.OpenFile()) != null) {
                    RenderTargetBitmap renderbitmap = new RenderTargetBitmap(
                        (int)width, (int)height,
                        96d, 96d,
                    PixelFormats.Pbgra32
                    );
                    renderbitmap.Render(_canvas);

                    PngBitmapEncoder pngencoder = new PngBitmapEncoder();
                    pngencoder.Frames.Add(BitmapFrame.Create(renderbitmap));
                    pngencoder.Save(mystream);
                    mystream.Close();
                }
            }
        }
    }
}
