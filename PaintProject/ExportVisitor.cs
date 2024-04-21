using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintProject {
    public class ExportVisitor {
        private MainWindow _mainWindow;
        public ExportVisitor(MainWindow mainWindow) {
            _mainWindow = mainWindow;
        }

        public void VisitXmlExporter(XmlExporter exporter) {
            exporter.SetPrototypes(_mainWindow.Prototypes);
        }

        public void VisitPngExporter(PngExporter exporter) {
            exporter.SetCanvas(_mainWindow.myCanvas);
        }
    }
}
