using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Strainer.Extensions;
using Strainer.Infrastructure;
using Strainer.Messages;
using Strainer.MvvmCross.Extensions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Windows.Input;
using System.Reactive.Subjects;
using Strainer.Collections;
using Strainer.Tracking;
using System.Threading.Tasks;

namespace Strainer.MvvmCross
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        readonly IList<Tuple<BaseViewModel, bool>> _childViewModels = new List<Tuple<BaseViewModel, bool>>();
        readonly IList<Func<IDisposable>> _disposableFactories = new List<Func<IDisposable>>();
        readonly CompositeDisposable _factorySubsciptions = new CompositeDisposable();
        readonly IDictionary<string, object> _backingFields = new Dictionary<string, object>();
		readonly IDictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        protected readonly CompositeDisposable Subscriptions = new CompositeDisposable();

		public BaseViewModel Parent { get; set; }

       

        public virtual void OnViewStarted(bool firstTime)
        {
            foreach (var cvm in _childViewModels.Where(x => x.Item2))
            {
                cvm.Item1.OnViewStarted(firstTime);
            }
            foreach (var factory in _disposableFactories)
            {
                _factorySubsciptions.Add(factory.Invoke());
            }
        }

        public virtual void OnViewStopped()
        {
            foreach (var cvm in _childViewModels.Where(x => x.Item2))
            {
                cvm.Item1.OnViewStopped();
            }
            _factorySubsciptions.Clear();
        }

        protected void Observe<T>(IObservable<T> observable, Action<T> onNext)
        {
            // Buffer to hold the last value produce while VM is stopped
            var buffer = new FixedSizedQueue<T>(1);

            // subject used to record values produced while VM is stopped
            var recordingSubject = new Subject<T>();

            var offline = recordingSubject.Publish();
            offline.Subscribe(x => buffer.Enqueue(x));
            var offlineSubscription = offline.Connect();

            // Start recording now 
            observable.Subscribe(recordingSubject);
            // subject used to record values produced while VM is started
            var liveSubject = new Subject<T>();
            observable.Subscribe(liveSubject);

            var connection = Observable.Create<T>(o =>
            {
                // Stop recording offline values;
                offlineSubscription.Dispose();

                // Produce an observable sequence starting with the last value
                var subscription = liveSubject
                    .StartWith(buffer.ToArray())
                    .Subscribe(o);

                buffer.Clear();

                // This will reconnect to this offline observable when subscription is disposed
                var goOffline = Disposable.Create(() => {
                    offlineSubscription = offline.Connect();
                });

                return new CompositeDisposable(new IDisposable[] {
                    subscription,
                    goOffline
                });


            }).Publish();

            connection
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => onNext(x));

            _disposableFactories.Add(() => connection.Connect());
        }

        protected T GetValue<T>(T defaultValue = default(T), [CallerMemberName]string propertyName = null)
        {
            object value;
            return _backingFields.TryGetValue(propertyName, out value)
                ? (T)value
                : defaultValue;
        }

        protected bool SetValue<T>(T value, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(GetValue<T>(propertyName: propertyName), value))
            {
                _backingFields[propertyName] = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

		public ICommand GetCommand(string propertyName)
		{
			ICommand value;
			return _commands.TryGetValue(propertyName, out value)
				? value
				: null;
		}

		public void RegisterCommand<T>(AsyncCommand<T> command, string propertyName)
		{
			command.ErrorOccurred += (s, e) => 
			{ 
				Logger.LogError(e.Exception);
				bool isSilent = e.Exception is TaskCanceledException;
				if(isSilent)
				{
					return;
				}


				#if DEBUG
				this.Services ().Message.Show (this.Services().Localize["GenericError_Title"], e.Exception.Message);
				#else
                    this.Services().Phone.ShowFeedbackPopup ();				
				#endif

			};
			_commands[propertyName] = command;
		}

        protected new void RaisePropertyChanged([CallerMemberName]string whichProperty = null)
        {
            base.RaisePropertyChanged(whichProperty);
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }

        protected ILogger Logger { get { return Mvx.Resolve<ILogger>(); } }

        protected ILocalization Localize { get { return this.Services().Localize; } }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Subscriptions.Dispose();
            }
        }

        protected bool ShowSubViewModel<TViewModel, TResult>(Action<TResult> onResult)
            where TViewModel : MvxViewModel, ISubViewModel<TResult>
        {
            return ShowSubViewModel<TViewModel, TResult>(null, onResult);
        }

        protected bool ShowSubViewModel<TViewModel, TResult>(object parameterValuesObject, Action<TResult> onResult)
            where TViewModel : MvxViewModel, ISubViewModel<TResult>
        {
            return ShowSubViewModel<TViewModel, TResult>(parameterValuesObject.ToSimplePropertyDictionary(), onResult);
        }

        protected bool ShowSubViewModel<TViewModel, TResult>(IDictionary<string, string> parameterValues,
            Action<TResult> onResult)
            where TViewModel : MvxViewModel, ISubViewModel<TResult>
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();

            if (parameterValues.ContainsKey("messageId"))
                throw new ArgumentException("parameterValues cannot contain an item with the key 'messageId'");

            string messageId = Guid.NewGuid().ToString();
            parameterValues["messageId"] = messageId;

            MvxSubscriptionToken token = null;
            token = this.Services().MessengerHub.Subscribe<SubNavigationResultMessage<TResult>>(msg =>
            {
                if(msg.MessageId != messageId)
                {
                    // We are not concerned with this message
                    return;
                }
                if (token != null)
                {
                    this.Services().MessengerHub.Unsubscribe<SubNavigationResultMessage<TResult>>(token);
                }

                onResult(msg.Result);
            }, MvxReference.Strong);

            return ShowViewModel<TViewModel>(parameterValues);
        }

		protected void ShowViewModelAndRemoveFromHistory<TViewModel>(object parameter = null) where TViewModel : IMvxViewModel
        {
            var dictionary = parameter.ToSimplePropertyDictionary();
            dictionary = dictionary ?? new Dictionary<string, string>();
            dictionary.Add("removeFromHistory", "notUsed");
            base.ShowViewModel<TViewModel>(dictionary);
        }

        protected TViewModel AddChild<TViewModel>(Func<TViewModel> builder, bool forwardParentLifecycleEvents = true)
            where TViewModel : BaseViewModel
        {
            var viewModel = builder.Invoke();
			viewModel.Parent = this;
            viewModel.DisposeWith(Subscriptions);
            _childViewModels.Add(Tuple.Create<BaseViewModel, bool>(viewModel, forwardParentLifecycleEvents));
            return viewModel;
        }

        protected virtual TViewModel AddChild<TViewModel>(bool forwardParentLifecycleEvents = true)
			where TViewModel : BaseViewModel
		{
            return AddChild<TViewModel>(() => Mvx.IocConstruct<TViewModel>(), forwardParentLifecycleEvents);
		}

        public override void Start()
        {
            base.Start();
            foreach (var child in _childViewModels)
            {
                try
                {
                    child.Item1.Start();
                }
                catch(Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
            if (parameters.Data.ContainsKey("messageId"))
            {
                this.MessageId = parameters.Data["messageId"];
            }

            foreach (var child in _childViewModels)
            {
                try
                {
                    child.Item1.CallBundleMethods("Init", parameters);
                }
                catch(Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }

		protected bool Close (BaseViewModel viewModel)
		{
			while (viewModel.Parent != null)
			{
				viewModel = viewModel.Parent;
			}
			return this.ChangePresentation (new MvxClosePresentationHint (viewModel));
		}

        public string MessageId { get; private set; }
    }
}