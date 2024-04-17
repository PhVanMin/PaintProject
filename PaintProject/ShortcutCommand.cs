using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PaintProject {
    public class ShortcutCommand : ICommand {
        private readonly Predicate<object> _canExecute;
        private readonly Action _execute;

        public ShortcutCommand(Predicate<object> canExecute, Action execute) {
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

        public static ShortcutCommand Create(Action execute) {
            return new ShortcutCommand(p => true, execute);
        }
    }
    public class Shortcut
    {
        MainWindow _window;
        public List<KeyBinding> KeyBindings { get; }
        public Shortcut(MainWindow window) {
            _window = window;
            KeyBindings = new List<KeyBinding>() {
                new KeyBinding(ShortcutCommand.Create(Redo), new KeyGesture(Key.Z, ModifierKeys.Shift | ModifierKeys.Control)),
                new KeyBinding(ShortcutCommand.Create(Undo), new KeyGesture(Key.Z, ModifierKeys.Control)),
            };
        }

        private void Undo() {
            if (_window.Prototypes.Count > 0) {
                _window.DeletedPrototypes.Push(_window.Prototypes.Pop());
                _window.myCanvas.Children.RemoveAt(_window.myCanvas.Children.Count - 1);
            }
        }

        private void Redo() {
            if (_window.DeletedPrototypes.Count > 0) {
                _window.Prototypes.Push(_window.DeletedPrototypes.Pop());
                _window.myCanvas.Children.Add(_window.Prototypes.Peek().Convert());
            }
        }
    }
}
