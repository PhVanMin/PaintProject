using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    }
}
