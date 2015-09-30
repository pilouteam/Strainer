using System;
using Cirrious.MvvmCross.ViewModels;
using Amp.MvvmCross;

namespace Strainer.Presentation.ViewModels
{
    public class HomeViewModel : PageViewModel
    {
        public HomeViewModel()
        {
        }

        public string Hello { get; set;} = "Strainer Hello";
    }
}

