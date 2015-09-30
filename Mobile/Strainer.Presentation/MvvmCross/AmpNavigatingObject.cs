using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Platform;
using System.Collections.Generic;

namespace Strainer.MvvmCross
{
    public class StrainerNavigatingObject : MvxNavigatingObject
    {

        public StrainerNavigatingObject() : base()
        {

        }
    
        protected void ShowViewModelAndClearTop<TViewModel>(object parameter = null) where TViewModel : IMvxViewModel
        {
            var dictionary = parameter.ToSimplePropertyDictionary();
            dictionary = dictionary ?? new Dictionary<string, string>();
            dictionary.Add(StrainerViewPresenterParameters.ClearTop, "notUsed");
            base.ShowViewModel<TViewModel>(dictionary);
        }
    }
}

