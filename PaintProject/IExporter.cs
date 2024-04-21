using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Xml;

namespace PaintProject {
    public interface IExporter {
        void Export();
        void Accept(ExportVisitor visitor);
    }

    public class XmlExporter : IExporter {
        Stack<IShape>? _shapes;

        public void Accept(ExportVisitor visitor) {
            visitor.VisitXmlExporter(this);
        }

        public void Export() {
            if (_shapes == null)
                return;

            var dialog = new SaveFileDialog();
            dialog.Title = "Save file";
            dialog.Filter = "Paint project | *.myext";
            if (dialog.ShowDialog() == DialogResult.OK) {
                HashSet<Type> types = new();
                List<MyShape> list = new();
                foreach (var shape in _shapes) {
                    types.Add(shape.GetType());
                    list.Add((MyShape)Convert.ChangeType(shape, shape.GetType()));
                }

                XmlWriter writer = XmlWriter.Create($"{dialog.FileName}");
                XmlSerializer serializer = new XmlSerializer(typeof(List<MyShape>), types.ToArray());
                serializer.Serialize(writer, list);
                writer.Close();
            }
        }

        public void SetPrototypes(Stack<IShape> Prototypes) {
            _shapes = Prototypes;
        }
    }

    public class PngExporter : IExporter {
        Canvas? _canvas;

        public void Accept(ExportVisitor visitor) {
            visitor.VisitPngExporter(this);
        }

        public void Export() {
            if (_canvas == null)
                return;

            var dialog = new SaveFileDialog();
            dialog.Title = "Save as PNG";
            dialog.Filter = "PNG files | *.png";
            if (dialog.ShowDialog() == DialogResult.OK) {
                Stream myStream;
                double width = _canvas.RenderSize.Width;
                double height = _canvas.RenderSize.Height;
                if ((myStream = dialog.OpenFile()) != null) {
                    RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                        (int)width, (int)height,
                        96d, 96d,
                    PixelFormats.Pbgra32
                    );
                    renderBitmap.Render(_canvas);

                    PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    pngEncoder.Save(myStream);
                    myStream.Close();
                }
            }
        }

        public void SetCanvas(Canvas canvas) {
            _canvas = canvas;
        }
    }
}
