using System;
using UIKit;
using Cirrious.MvvmCross.Views;
using System.Collections.ObjectModel;

namespace Strainer.iOS.Framework
{
    public class ViewControllerDefinition
    {
        private UIViewController _viewControllerInstance;
        public Type Type { get; set; } 
        public int Index { get; set; } 
        public string Title { get; set; } 
        public Func<object> DataContext { get; set; }

        public UIViewController GetViewController()
        {
            if (_viewControllerInstance == null)
            {
                var viewController = (UIViewController)Activator.CreateInstance(Type);
                viewController.Title = Title;
                var view = (IMvxView)viewController;
                view.DataContext = DataContext.Invoke();

                // Must acces view after setting DataContext or else we get a NullReferenceException in ViewDidLoad (MvvmCross)
                viewController.View.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

                _viewControllerInstance = viewController;
            }
            return _viewControllerInstance;
        }
    }

    public class ViewControllerDefinitionCollection: KeyedCollection<Type, ViewControllerDefinition>
    {
        protected override Type GetKeyForItem (ViewControllerDefinition item)
        {
            return item.Type;
        }
    }
}

