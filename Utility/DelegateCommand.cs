using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utility {
public class DelegateCommand : ICommand
{
    private readonly Predicate<object> mCanExecute;
    private readonly Action<object> mExecute;
 
    public event EventHandler CanExecuteChanged;
 
    public DelegateCommand(Action<object> execute) 
                   : this(execute, null)
    {
    }
 
    public DelegateCommand(Action<object> execute, 
                   Predicate<object> canExecute)
    {
        mExecute = execute;
        mCanExecute = canExecute;
    }
 
    public bool CanExecute(object parameter)
    {
        if (mCanExecute == null)
        {
            return true;
        }
 
        return mCanExecute(parameter);
    }
 
    public void Execute(object parameter)
    {
        mExecute(parameter);
    }
 
    public void RaiseCanExecuteChanged()
    {
        if( CanExecuteChanged != null )
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
}
