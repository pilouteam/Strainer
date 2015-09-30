// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Strainer.iOS.Views
{
	[Register ("TabView")]
	partial class TabView
	{
		[Outlet]
		UIKit.UITabBar tabBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tabBar != null) {
				tabBar.Dispose ();
				tabBar = null;
			}
		}
	}
}
