using System.Windows.Input;
using Strainer.Extensions;
using Strainer.Performance;
using Strainer.MvvmCross.Extensions;

namespace Strainer.MvvmCross
{
    public abstract class PageViewModel : BaseViewModel
    {
        public virtual void OnViewLoaded()
        {
            Perf.TimestStrainerOnce("First PageViewModel.OnViewLoaded");

            this.Services().Tracking.TrackPage(GetType().Name);
        }

        public virtual void OnViewUnloaded()
        {
            Subscriptions.Clear();
        }

        public virtual ICommand CloseCommand
        {
            get
            {
                return this.GetCommand(() =>
                    {
                        Close(this);
                    });
            }
        }
    }
}