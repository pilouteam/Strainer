using System;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Bindings;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace Strainer.iOS.Controls
{
    public class SwipeableCocktailCell : SWTableViewCell.SWTableViewCell, IMvxBindable, IMvxBindingContextOwner, IMvxDataConsumer
    {
        public object DataContext
        {
            get
            {
                return this.BindingContext.DataContext;
            }
            set
            {
                this.BindingContext.DataContext = value;
            }
        }
        public IMvxBindingContext BindingContext { get; set; }

        public SwipeableCocktailCell(IEnumerable<MvxBindingDescription> bindingDescriptions, UITableViewCellStyle cellStyle, NSString cellIdentifier, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None) : base(cellStyle, cellIdentifier)
        {
            this.Accessory = tableViewCellAccessory;
            this.CreateBindingContext(bindingDescriptions);
        }

        public SwipeableCocktailCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None) : base(cellStyle, cellIdentifier)
        {
            this.Accessory = tableViewCellAccessory;
            this.CreateBindingContext(bindingText);
        }

        public SwipeableCocktailCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle) : base(handle)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public SwipeableCocktailCell(string bindingText, IntPtr handle) : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public SwipeableCocktailCell(IntPtr handle) : this(string.Empty, handle)
        {
        }

        public SwipeableCocktailCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public SwipeableCocktailCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public SwipeableCocktailCell() : this(string.Empty)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }
    }
}

