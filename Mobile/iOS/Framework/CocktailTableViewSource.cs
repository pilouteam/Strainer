using System;
using Cirrious.MvvmCross.Binding.Touch.Views;
using UIKit;
using Foundation;
using Strainer.iOS.Controls;
using SWTableViewCell;
using System.Windows.Input;
using Strainer.Common.Contracts;

namespace Strainer.iOS.Framework
{
    public class CocktailTableViewSource : MvxSimpleTableViewSource
    {

        public CocktailTableViewSource(UITableView tableView, string key)
            :base (tableView, key)
        {
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (SwipeableCocktailCell)base.GetCell(tableView, indexPath);
            cell.SetLeftUtilityButtons(LeftButtons(), 80.0f);
            cell.Delegate = new CellDelegate(DeleteCell, ShareCell);
            return cell;
        }

        private UIButton[] LeftButtons()
        {
            var leftUtilityButtons  = new NSMutableArray();
            leftUtilityButtons.AddUtilityButton(UIColor.FromRGB(242, 38, 19), UIImage.FromBundle("delete_big.png"));
            leftUtilityButtons.AddUtilityButton(UIColor.FromRGB(30, 130, 76), UIImage.FromBundle("share_big.png"));
            return NSArray.FromArray<UIButton>(leftUtilityButtons);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (!tableView.Editing)
            {
                tableView.DeselectRow(indexPath, true);
            }
        }

        public ICommand DeleteCell{ get; set; }
        public ICommand ShareCell { get; set; }

        private class CellDelegate : SWTableViewCellDelegate
        {
            private ICommand _delete;
            private ICommand _share;

            public CellDelegate (ICommand delete, ICommand share)
            {
                this._share = share;
                this._delete = delete;
                
            }
            public override void DidTriggerLeftUtilityButton(SWTableViewCell.SWTableViewCell cell, nint index)
            {
                var c = cell as SwipeableCocktailCell;
                switch ((int)index)
                {
                    case 0:
                        _delete.Execute((Cocktail)c.DataContext);
                        break;
                    case 1:
                        _share.Execute((Cocktail)c.DataContext);
                        break;
                }
            }

            public override bool ShouldHideUtilityButtonsOnSwipe(SWTableViewCell.SWTableViewCell cell)
            {
                // allow just one cell's utility button to be open at once
                return true;
            }
        }
    }
}

