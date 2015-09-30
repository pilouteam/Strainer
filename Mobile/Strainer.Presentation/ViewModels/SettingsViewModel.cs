using System;
using Amp.MvvmCross;

namespace Strainer.Presentation.ViewModels
{
    public class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel()
        {
        }
        public string Hello { get; set;} = "Settings";
    }
}

