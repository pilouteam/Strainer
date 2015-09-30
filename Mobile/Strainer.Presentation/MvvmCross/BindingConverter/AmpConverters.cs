using System;
using Strainer.BindingConverter;

namespace Strainer.MvvmCross.BindingConverter
{
	public class StrainerConverters
	{
		public readonly BoolInverter BoolInverter = new BoolInverter();
        public readonly StringFormat StringFormat = new StringFormat();
	}
}

