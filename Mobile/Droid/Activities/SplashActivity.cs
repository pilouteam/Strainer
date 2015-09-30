
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace Strainer.Droid.Activities
{
    [Activity(
        Label = "@string/app_name"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme="@style/Theme.AppCompat.Light"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : MvxSplashScreenActivity
    {
        public SplashActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void TriggerFirstNavigate()
        {
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();
        }
    }
}

