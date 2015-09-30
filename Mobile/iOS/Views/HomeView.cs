﻿using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using Strainer.Presentation.ViewModels;

namespace Strainer.iOS.Views
{
    public partial class HomeView : MvxViewController
    {
        public HomeView()
            : base("HomeView", null)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            View = new UIView { BackgroundColor = UIColor.White };
            base.ViewDidLoad();

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }

            var label = new UILabel(new CGRect(10, 10, 300, 40));
            Add(label);
            var textField = new UITextField(new CGRect(10, 50, 300, 40));
            Add(textField);

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(label).To(vm => vm.Hello);
            set.Bind(textField).To(vm => vm.Hello);
            set.Apply();
        }
    }
}

