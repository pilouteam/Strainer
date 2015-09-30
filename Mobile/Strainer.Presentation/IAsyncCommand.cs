using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.CrossCore;
using Strainer.Infrastructure;
using System.Reactive.Linq;
using System.Reactive;

namespace Strainer
{
    
    public interface IAsyncCommand :ICommand 
    {
        Task ExecuteAsync (object  paramerter);
    }
	
}
