using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Media.Imaging;

namespace PaintProject {
    public interface IImporter {
        object? Import();
        void SetUp(object data);
    }

    public class XmlImporter : IImporter {
        List<IShape>? _painters;
        public object? Import() {
            if (_painters == null)
                return null;

            var dialog = new OpenFileDialog();
            dialog.Title = "Load file";
            dialog.Multiselect = false;
            dialog.Filter = "Paint project | *.myext";
            if (dialog.ShowDialog() == DialogResult.OK) {
                List<Type> types = [];
                foreach (var s in _painters) {
                    types.Add(s.GetType());
                }

                XmlReader reader = XmlReader.Create(dialog.FileName);
                XmlSerializer serializer = new XmlSerializer(typeof(List<MyShape>), types.ToArray());

                List<MyShape>? list;
                list = serializer.Deserialize(reader) as List<MyShape>;
                reader.Close();
                return list;
            }

            return null;
        }

        public void SetUp(object data) {
            _painters = (List<IShape>) data;
        }
    }

    public class PngImporter : IImporter {
        public object? Import() {
            var dialog = new OpenFileDialog();
            dialog.Title = "Load image";
            dialog.Multiselect = false;
            dialog.Filter = "PNG files | *.png";
            if (dialog.ShowDialog() == DialogResult.OK) {
                //Stream imageStreamSource = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                //PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                //BitmapSource bitmapSource = decoder.Frames[0];

                var bitmapSource = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
                MyImage image = new MyImage();
                image.SetSource(bitmapSource);
                return image;
            }

            return null;
        }

        public void SetUp(object data) { }
    }
}
