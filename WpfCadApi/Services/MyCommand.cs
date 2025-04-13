using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfCadApi.Services
{
    public  class MyCommand : ICommand
    {
        public Action<object> _execute;
        public Predicate<object> _canExcute;

        public MyCommand( Action<object> ExcuteCmd , Predicate<object> CanExcute) 
        {
            _execute = ExcuteCmd;
            _canExcute = CanExcute;
        }  
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExcute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
