using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.CrossCore;
using Strainer.Infrastructure;
using System.Reactive.Linq;
using System.Reactive;
using Strainer.Extensions;

namespace Strainer
{
    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Action execute)
        {
            return new AsyncCommand<object>(Wrap(execute), Observable.Return<bool>(true));
        }

        public static AsyncCommand<object> Create(Action execute, IObservable<bool> canExecute)
        {
            return new AsyncCommand<object>(Wrap(execute), canExecute);
        }

		public static AsyncCommand<object> Create(Action execute, Func<bool> canExecute)
		{
			return new AsyncCommand<object>(Wrap(execute), parameter => canExecute());
		}

        public static AsyncCommand<T> Create<T>(Action<T> execute)
        {
            return new AsyncCommand<T>(Wrap(execute), default(IObservable<bool>));
        }

        public static AsyncCommand<T> Create<T>(Action<T> execute, IObservable<bool> canExecute)
        {
            return new AsyncCommand<T>(Wrap(execute), canExecute);
        }

        public static AsyncCommand<object> Create(Func<Task> execute)
        {
            return new AsyncCommand<object>(Wrap(execute), default(IObservable<bool>));
        }

        public static AsyncCommand<object> Create(Func<Task> execute, IObservable<bool> canExecute)
        {
            return new AsyncCommand<object>(Wrap(execute), canExecute);
        }

        public static AsyncCommand<object> Create(Func<Task> execute, Func<bool> canExecute)
        {
            return new AsyncCommand<object>(Wrap(execute), parameter=>canExecute());
        }

        public static AsyncCommand<T> Create<T>(Func<T, Task> execute)
        {
            return new AsyncCommand<T>(execute, default(IObservable<bool>));
        }

        public static AsyncCommand<T> Create<T>(Func<T, Task> execute, IObservable<bool> canExecute)
        {
            return new AsyncCommand<T>(execute, canExecute);
        }

        public static AsyncCommand<T> Create<T>(Func<T, Task> execute, Func<object, bool> canExecute)
        {
            return new AsyncCommand<T>(execute, canExecute);
        }

        private static TaskScheduler GetTaskScheduler()
        {
            try
            {
                return TaskScheduler.FromCurrentSynchronizationContext();
            }
            catch(Exception)
            {
                return TaskScheduler.Default;
            }
        }

        private static Func<T, Task> Wrap<T>(Action<T> execute)
        {
            return p =>
            {
                return Task.Factory.StartNew(() => execute(p),
                    default(CancellationToken),
                    TaskCreationOptions.None,
                    GetTaskScheduler());
            };
        }

        private static Func<object, Task> Wrap(Action execute)
        {
            return p =>
            {
                return Task.Factory.StartNew(() => execute(),
                    default(CancellationToken),
                    TaskCreationOptions.None,
                    GetTaskScheduler());
            };
        }

        private static Func<object, Task> Wrap(Func<Task> execute)
        {
            return async p =>
            {
                await execute();
            };
        }
    }
    public class AsyncCommand<T> : IAsyncCommand, IDisposable
	{
        private ISubject<bool> _isExecuting = new Subject<bool>();
		private Func<T, Task> _execute;
        private IDisposable _subscription;

        Func<object, bool> _canExecute;

		public AsyncCommand(Func<T, Task> execute)
            : this(execute, (IObservable<bool>)null)
		{
		}


        public AsyncCommand(Func<T, Task> execute, IObservable<bool> canExecute)
		{
			_execute = execute;

            _canExecute = (_ => true);

            var canExecuteObservable = (canExecute ?? Observable.Never<bool>()).StartWith(true);
            _subscription = canExecuteObservable
                .CombineLatest(_isExecuting.StartWith(false), (ce, ie) => ce && !ie)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(ce =>
            {
                _canExecute = obj => ce;
                OnCanExecuteChanged();
            });
		}

        public AsyncCommand(Func<T, Task> execute, Func<object, bool> canExecute)
        {
            _execute = execute;

            _canExecute = canExecute ?? (_ => true);       
        }

		public bool CanExecute(object parameter)
		{
            return _canExecute(parameter);
		}

		public bool CanExecute()
		{
			return CanExecute(null);
		}
  

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                _isExecuting.OnNext(true);

                try
                {
                    await _execute((T)parameter);
                }
                catch(Exception e)
                {
                    OnError(e);
                }
                finally
                {
                    _isExecuting.OnNext(false);
                }
            }
        }

		public void Execute(object parameter)
		{
			ExecuteAsync(parameter).FireAndForget();
		}

		public void Execute()
		{
			Execute(null);
		}

		public event EventHandler CanExecuteChanged;

		protected virtual void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, new EventArgs());
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_execute = null;
                var disp = Interlocked.Exchange(ref _subscription, null);
                if (disp != null)
                {
                    disp.Dispose();
                }
			}
		}

		public event EventHandler<ExceptionEventArgs> ErrorOccurred;

		private void OnError(Exception e)
        {
			if (ErrorOccurred != null)
			{
				ErrorOccurred(this, new ExceptionEventArgs(e));
			}
        }
	}

	public class ExceptionEventArgs : EventArgs 
	{
		public ExceptionEventArgs(Exception Exception) 
		{
			this.Exception = Exception;
		}

		public Exception Exception { get; private set; }
	}
}

