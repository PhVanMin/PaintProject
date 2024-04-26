namespace PaintProject {
    public interface IExportVisitor {
        void visitXml(XmlExporter exporter);
        void visitPng(PngExporter exporter);
    }

    public class ExportVisitor : IExportVisitor {
        private MainWindow _window;
        public ExportVisitor(MainWindow window) {
            _window = window;
        }
        public void visitPng(PngExporter exporter) {
            exporter.SetCanvas(_window.myCanvas);
            exporter.Export();
        }

        public void visitXml(XmlExporter exporter) {
            exporter.SetTypes(_window.SupportedType);
            exporter.SetShapes(_window.Prototypes); 
            exporter.Export();
        }
    }
}
