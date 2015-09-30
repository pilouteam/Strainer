using System;
using Cirrious.MvvmCross.ViewModels;

namespace Strainer.MvvmCross
{
	public class LimitedStringLengthViewModel: MvxNotifyPropertyChanged
	{
		readonly int _maxLength;

		public LimitedStringLengthViewModel(int maxLength)
		{
			_maxLength = maxLength;
			_remainingCharacters = maxLength;
		}


		public int MaxLength
		{
			get {return _maxLength; }
		}

		private string _value;
		public string Value
		{
			get { return _value; }
			set
			{ 
				var text = value ?? string.Empty;
				var truncatedText = text.Substring(0, Math.Min(text.Length, _maxLength));
				if(truncatedText != _value)
				{
					_value = truncatedText;
					RaisePropertyChanged(() => Value);
					RemainingCharacters = _maxLength - truncatedText.Length;
				}
			} 
		}

		private int _remainingCharacters;
		public int RemainingCharacters
		{
			get { return _remainingCharacters; }
			private set
			{ 
				if (value != _remainingCharacters)
				{
					_remainingCharacters = value;
					RaisePropertyChanged(() => RemainingCharacters);
				}
			} 
		}
	}
}

