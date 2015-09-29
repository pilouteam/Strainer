
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

namespace Strainer.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme="@style/Theme.AppCompat.Light", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation=Android.Content.PM.ScreenOrientation.Landscape)]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Create your application here
        }
    }
}

