using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Strainer.Presentation.ViewModels;

namespace Otolane.Presentation
{
    public class StartApplicationObject : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<HomeViewModel>();
        }
    }
}

