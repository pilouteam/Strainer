using System;
using System.Windows.Input;

namespace Strainer.Extensions
{
    public static class ICommandExtensions
    {
        public static void ExecuteIfPossible(this ICommand thisCommand, object parameter=null)
        {
            if(thisCommand != null && thisCommand.CanExecute(parameter))
            {
                thisCommand.Execute(parameter);
            }
        }
    }
}

