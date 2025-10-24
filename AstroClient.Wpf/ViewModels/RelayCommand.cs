using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AstroClient.Wpf.ViewModels
{
    public sealed class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool>? _can; //store the logic for commond can execute or not

        public RelayCommand(Action action, Func<bool>? can = null)
        {
            _action = action;
            _can = can;
        }
        public bool CanExecute(object? parameter) => _can?.Invoke() ?? true;
        public void Execute(object? parameter) => _action();
        public event EventHandler? CanExecuteChanged;

        // notify UI to update its state.
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
