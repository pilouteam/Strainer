using System;
using Cirrious.MvvmCross.Droid.Platform;
using Android.Content;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Amp;

namespace Strainer.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        App _app;

        protected override IMvxApplication CreateApp()
        {

            MvxSimpleIoCContainer.Initialize ();
            AmpModule.Init(ApplicationContext);
            _app = new App();

            return _app;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}

