
using System;

using Foundation;
using UIKit;
using Amp.Views;
using ObjCRuntime;
using CoreGraphics;
using Cirrious.MvvmCross.Binding.BindingContext;
using Strainer.Presentation.ViewModels;

namespace Strainer.iOS.Views
{
    public partial class SettingsView : BaseViewController<SettingsViewModel>
    {
        public SettingsView()
            : base("SettingsView", null)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }
            View.BackgroundColor = UIColor.Green;
            var label = new UILabel(new CGRect(10, 10, 300, 40));
            Add(label);
            var textField = new UITextField(new CGRect(10, 50, 300, 40));
            Add(textField);

            var set = this.CreateBindingSet<SettingsView, SettingsViewModel>();
            set.Bind(label).To(vm => vm.Hello);
            set.Bind(textField).To(vm => vm.Hello);
            set.Apply();
        }
    }
}

