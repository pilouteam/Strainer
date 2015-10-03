
using System;

using Foundation;
using UIKit;
using Amp.Views;
using Strainer.Presentation.ViewModels;
using ObjCRuntime;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Strainer.iOS.Framework;
using System.Linq;
using CoreGraphics;

namespace Strainer.iOS.Views
{
    public partial class TabView : BaseViewController<TabViewModel>
    {
        readonly ViewControllerDefinitionCollection _viewControllerDefinitions;
        private UIBarButtonItem _addBarButtonItem;

        public TabView()
            : base("TabView", null)
        {

            _viewControllerDefinitions = new ViewControllerDefinitionCollection {
                new ViewControllerDefinition
                {
                    Type = typeof(HomeView),
                    Index = 0,
                    Title = "Strainer",
                    DataContext = () => ViewModel.Home,
                },
                new ViewControllerDefinition
                {
                    Type = typeof(ShareView),
                    Index = 1,
                    Title = "Share",
                    DataContext = () => ViewModel.Share,
                },
                new ViewControllerDefinition
                {
                    Type = typeof(SettingsView),
                    Index = 2,
                    Title = "Settings",
                    DataContext = () => ViewModel.Settings,
                },

            };
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            Title = _viewControllerDefinitions [0].Title;
            SetViewController (_viewControllerDefinitions [0].GetViewController ());
            tabBar.SelectedItem = tabBar.Items[0];

            tabBar.BarTintColor = Colors.NavigationBar;
            tabBar.TintColor = UIColor.White;

            tabBar.ItemSelected += (sender, e) => 
                {
                    var item = (UITabBarItem)e.Item;
                    Title = _viewControllerDefinitions [(int)item.Tag].Title;
                    ViewModel.SelectedIndex = (int)item.Tag;
                };
            
            var set = this.CreateBindingSet<TabView, TabViewModel> ();

            set.Bind ()
                .For (v => v.SelectedIndex)
                .To (vm => vm.SelectedIndex);

            set.Apply ();

        }

        int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    SelectTab(value);                
                }
            }
        }

        private UIViewController _currentViewController;
        private async void SetViewController(UIViewController viewController)
        {
            var bounds = new CGRect(0, NavigationController.NavigationBar.Frame.Height + NavigationController.NavigationBar.Frame.Y, View.Bounds.Width, View.Bounds.Height - NavigationController.NavigationBar.Frame.Height - tabBar.Frame.Height - NavigationController.NavigationBar.Frame.Y);
            if (_currentViewController != null)
            {
                _currentViewController.WillMoveToParentViewController (null);
                AddChildViewController (viewController);

                viewController.View.Frame = bounds;
                await TransitionAsync (_currentViewController, viewController, 0.2, UIViewAnimationOptions.TransitionCrossDissolve, () => {
                    View.BringSubviewToFront(tabBar);
                });

                _currentViewController.RemoveFromParentViewController ();
                viewController.DidMoveToParentViewController (this);

            }
            else
            {
                AddChildViewController (viewController);
                viewController.View.Frame = bounds;
                View.InsertSubviewAbove(viewController.View, tabBar);
                View.BringSubviewToFront(tabBar);
                viewController.DidMoveToParentViewController (this);

            }
            _currentViewController = viewController;
        }

        public void SelectTab(int index)
        {
            var definition = _viewControllerDefinitions.FirstOrDefault (x => x.Index == index);

            if(definition != null)
            {
                SetViewController (definition.GetViewController ());
            }
        }
    }
}

