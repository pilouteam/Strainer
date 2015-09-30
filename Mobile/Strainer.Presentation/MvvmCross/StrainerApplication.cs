using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;

namespace Strainer.MvvmCross
{
	public abstract class StrainerApplication : Cirrious.MvvmCross.ViewModels.MvxApplication
	{
		public abstract string ApplicationName { get; }

       

	}
}

