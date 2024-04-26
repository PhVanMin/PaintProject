using Interfaces;
using System.Windows.Controls;
using System.Windows.Input;

namespace PaintProject {
    public class RelayCommand : ICommand {
        private readonly Predicate<object> _canExecute;
        private readonly Action _execute;

        public RelayCommand(Predicate<object> canExecute, Action execute) {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object? parameter) {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter) {
            _execute();
        }

        public event EventHandler? CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public static RelayCommand Create(Action execute) {
            return new RelayCommand(p => true, execute);
        }
    }
    public class Shortcut
    {
        public List<KeyBinding> KeyBindings { get; }
        private Canvas _canvas;
        private Stack<BaseShape> _Prototypes;
        private Stack<BaseShape> _DeletedPrototypes;
        public static Shortcut Create(MainWindow window) {
            return new Shortcut(
                window.myCanvas, 
                window.Prototypes, 
                window.DeletedPrototypes
            );
        }

        private Shortcut(Canvas canvas, Stack<BaseShape> prototypes, Stack<BaseShape> deletedPrototype) {
            _canvas = canvas;
            _Prototypes = prototypes;
            _DeletedPrototypes = deletedPrototype;
            KeyBindings = new List<KeyBinding>() {
                new KeyBinding(RelayCommand.Create(Redo), new KeyGesture(Key.Z, ModifierKeys.Shift | ModifierKeys.Control)),
                new KeyBinding(RelayCommand.Create(Undo), new KeyGesture(Key.Z, ModifierKeys.Control)),
            };
        }

        private void Undo() {
            if (_Prototypes.Count > 0) {
                _DeletedPrototypes.Push(_Prototypes.Pop());
                _canvas.Children.RemoveAt(_canvas.Children.Count - 1);
            }
        }

        private void Redo() {
            if (_DeletedPrototypes.Count > 0) {
                _Prototypes.Push(_DeletedPrototypes.Pop());
                _canvas.Children.Add(_Prototypes.Peek().Convert());
            }
        }
    }
}
