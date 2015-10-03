// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Strainer.iOS.Controls
{
	[Register ("CocktailListItemView")]
	partial class CocktailListItemView
	{
		[Outlet]
		UIKit.UIImageView imgCocktail { get; set; }

		[Outlet]
		UIKit.UILabel lblCocktailAlcool { get; set; }

		[Outlet]
		UIKit.UILabel lblCocktailTitle { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgCocktail != null) {
				imgCocktail.Dispose ();
				imgCocktail = null;
			}

			if (lblCocktailTitle != null) {
				lblCocktailTitle.Dispose ();
				lblCocktailTitle = null;
			}

			if (lblCocktailAlcool != null) {
				lblCocktailAlcool.Dispose ();
				lblCocktailAlcool = null;
			}
		}
	}
}
