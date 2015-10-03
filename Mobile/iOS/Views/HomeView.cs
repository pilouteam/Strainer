using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using Strainer.Presentation.ViewModels;
using Amp.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Strainer.iOS.Controls;
using System.Windows.Input;
using Amp.TableViews;
using Amp.Extensions;
using System;
using System.Collections.Generic;
using SWTableViewCell;
using System.Linq;
using Strainer.iOS.Framework;

namespace Strainer.iOS.Views
{
    public partial class HomeView : BaseViewController<HomeViewModel>
    {
        private UIBarButtonItem _addBarButtonItem;
        private UIBarButtonItem _editBarButtonItem;

        public HomeView()
            : base("HomeView", null)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (ParentViewController.NavigationItem.LeftBarButtonItem == null)
            {
                ConfigureNavBar();
            }
            else
            {
                ParentViewController.NavigationItem.LeftBarButtonItem.Title = "Edit"; 
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (_isEditing)
                StopEditing();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }
           
            var tableViewSource = new CocktailTableViewSource(tblCocktail, CocktailListItemView.Key);

            tblCocktail.Source = tableViewSource;
            tblCocktail.RowHeight = 80;
            tblCocktail.AllowsMultipleSelectionDuringEditing = true;

            var _pullToRefreshHandler = tblCocktail.ActivatePullToRefresh(this);

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();

            set.Bind(tableViewSource)
                .For(v => v.ItemsSource)
                .To(vm => vm.Cocktails);
            
            set.Bind(_pullToRefreshHandler)
                .For(v => v.Command)
                .To(vm => vm.Refresh);

            set.Bind(tableViewSource)
                .For(v => v.DeleteCell)
                .To(vm => vm.DeleteCocktail);

            set.Bind(tableViewSource)
                .For(v => v.ShareCell)
                .To(vm => vm.ShareCocktail);
            
            set.Apply();
        }

        private bool _isEditing = false;
        private void ConfigureNavBar()
        {
            ParentViewController.NavigationController.NavigationBar.BarTintColor = Colors.NavigationBar;
            ParentViewController.NavigationController.NavigationBar.TintColor = UIColor.White;

            _addBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("add"), UIBarButtonItemStyle.Plain, null);

            var deleteBarButtonItem = new UIBarButtonItem (UIImage.FromBundle ("delete"), UIBarButtonItemStyle.Plain, null);
            deleteBarButtonItem.Clicked += (sender, e) => ReturnResult(delete:false);

            var shareBarButtonItem = new UIBarButtonItem (UIImage.FromBundle ("share"), UIBarButtonItemStyle.Plain, null);
            shareBarButtonItem.Clicked += (sender, e) => ReturnResult(delete:false);

            _editBarButtonItem = new UIBarButtonItem(){ Title = "Edit" };
            _editBarButtonItem.Clicked += (sender, e) => 
                {
                    if(!string.IsNullOrEmpty(_editBarButtonItem.Title))
                    {
                        if(!_isEditing)
                        {
                            _editBarButtonItem.Title = "Done";
                            ParentViewController.NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[]{shareBarButtonItem,deleteBarButtonItem}, false);
                            tblCocktail.SetEditing (true, true);
                            _isEditing = true;
                        }
                        else
                        {
                            StopEditing();
                        }
                    }
                };
            
            ParentViewController.NavigationItem.SetRightBarButtonItem(_addBarButtonItem, false);
            ParentViewController.NavigationItem.SetLeftBarButtonItem(_editBarButtonItem, false);
        }

        private void ReturnResult(bool delete)
        {
            var homeViewModel = (HomeViewModel)DataContext;
            var indexes = tblCocktail.IndexPathsForSelectedRows?.Select( ip => ip.Row)?.ToList();
            if (indexes == null)
            {
                StopEditing(); 
            }
            else
            {
                if (delete)
                    homeViewModel.DeleteCocktails.Execute(indexes);
                else
                    homeViewModel.ShareCocktails.Execute(indexes);
            }
        }

        private void StopEditing()
        {
            _editBarButtonItem.Title = "Edit";
            ParentViewController.NavigationItem.RightBarButtonItems = null;
            ParentViewController.NavigationItem.SetRightBarButtonItem(_addBarButtonItem, false);
            tblCocktail.SetEditing (false, true);
            _isEditing = false;
        }
    }
}

