using System;

namespace Strainer.Infrastructure
{
	public interface ILocalization
	{
		string this[string key] { get; }
		bool Exists(string key);
	}
}
