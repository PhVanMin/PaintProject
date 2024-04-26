namespace PaintProject {
    public interface IImportVisitor {
        void visitXml(XmlImporter importer);
        void visitPng(PngImporter importer);
    }

    public class ImportVisitor : IImportVisitor {
        private MainWindow _window;
        public ImportVisitor(MainWindow window) { _window = window; }

        public void visitPng(PngImporter importer) {
            importer.SetCanvas(_window.myCanvas);
            importer.Import();
        }

        public void visitXml(XmlImporter importer) {
            importer.SetCanvas(_window.myCanvas);
            importer.SetShapes(_window.Prototypes);
            importer.SetTypes(_window.SupportedType);
            importer.Import();
        }
    }
}
