namespace PaintProject {
    public interface IFactory {
        IExporter CreateExporter();
        IImporter CreateImporter();
    }

    public class XmlFactory : IFactory {
        private static XmlFactory? _instance = null;
        public IExporter CreateExporter() {
            return new XmlExporter();
        }

        public IImporter CreateImporter() {
            return new XmlImporter();
        }

        public static IFactory Instance {
            get {
                if (_instance == null) {
                    _instance = new XmlFactory();
                }
                return _instance;
            }
        }
    }

    public class PngFactory : IFactory {
        private static PngFactory? _instance = null;
        public IExporter CreateExporter() {
            return new PngExporter();
        }

        public IImporter CreateImporter() {
            return new PngImporter();
        }

        public static IFactory Instance {
            get {
                if (_instance == null) {
                    _instance = new PngFactory();
                }
                return _instance;
            }
        }
    }
}
