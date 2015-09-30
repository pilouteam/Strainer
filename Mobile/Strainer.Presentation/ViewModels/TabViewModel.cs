using System;
using Amp.MvvmCross;
using Cirrious.MvvmCross.ViewModels;

namespace Strainer.Presentation.ViewModels
{
    public class TabViewModel : PageViewModel, ISubViewModel<string>
    {
        public TabViewModel()
        {
            Home = new HomeViewModel();
            Share = new ShareViewModel();
            Settings = new SettingsViewModel();
        }
        public override void Start()
        {
            base.Start();
        }
        public override void OnViewStarted(bool firstTime)
        {
            base.OnViewStarted(firstTime);
            SelectedIndex = 0;
        }
        public HomeViewModel Home
        {
            get;
            private set;
        }
        public ShareViewModel Share
        {
            get;
            private set;
        }
        public SettingsViewModel Settings
        {
            get;
            private set;
        }

        int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged(() => SelectedIndex);
            }
        }

    }
}

