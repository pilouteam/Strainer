using System;
using Cirrious.MvvmCross.ViewModels;

namespace Strainer.Presentation.ViewModels
{
    public class HomeViewModel
        : MvxViewModel
    {
        public HomeViewModel()
        {
        }

        public string Hello { get; set;} = "Strainer Hello";
    }
}

