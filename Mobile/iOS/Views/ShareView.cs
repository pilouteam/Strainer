
using System;

using Foundation;
using UIKit;
using Amp.Views;
using Strainer.Presentation.ViewModels;
using ObjCRuntime;
using CoreGraphics;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Strainer.iOS.Views
{
    public partial class ShareView : BaseViewController<ShareViewModel>
    {
        public ShareView()
            : base("ShareView", null)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ParentViewController.NavigationItem.LeftBarButtonItem.Title = "";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }
            View.BackgroundColor = UIColor.Red;
            var label = new UILabel(new CGRect(10, 10, 300, 40));
            Add(label);
            var textField = new UITextField(new CGRect(10, 50, 300, 40));
            Add(textField);

            var set = this.CreateBindingSet<ShareView, ShareViewModel>();
            set.Bind(label).To(vm => vm.Hello);
            set.Bind(textField).To(vm => vm.Hello);
            set.Apply();
        }
    }
}

