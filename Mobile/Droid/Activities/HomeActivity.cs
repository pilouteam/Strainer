
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
using Strainer.Presentation.ViewModels;
using Amp.Views;

namespace Strainer.Droid.Activities
{
    [Activity(Label = "Home")]
    public class HomeActivity : BaseActivity<HomeViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }
    }
}

