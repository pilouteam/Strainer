
using System;

using Foundation;
using UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Strainer.Common.Contracts;
using Cirrious.MvvmCross.Binding.BindingContext;
using Strainer.iOS.Converters;
using Cirrious.CrossCore.Core;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Bindings;
using CoreGraphics;

namespace Strainer.iOS.Controls
{
    public partial class CocktailListItemView : SwipeableCocktailCell
    {
        public static readonly UINib Nib = UINib.FromName("CocktailListItemView", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("CocktailListItemView");

        public CocktailListItemView(IntPtr handle)
            : base(handle)
        {
        }

        public static CocktailListItemView Create()
        {
            return (CocktailListItemView)Nib.Instantiate(null, null)[0];
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            imgCocktail.Layer.CornerRadius = imgCocktail.Frame.Width / 2;
            imgCocktail.ClipsToBounds = true;

            var set = this.CreateBindingSet<CocktailListItemView, Cocktail>();

            set.Bind(lblCocktailTitle)
                .To(vm => vm.Name);

            set.Bind(lblCocktailAlcool)
                .To(vm => vm.Alcool);

            set.Bind(imgCocktail)
                .For(v => v.Image)
                .To(vm => vm.Img)
                .WithConversion(StrainerConverters.StringToUIImage);

            set.Apply();
        }
    }
}

