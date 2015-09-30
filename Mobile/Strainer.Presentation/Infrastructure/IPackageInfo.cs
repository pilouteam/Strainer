using System;

namespace Strainer.Infrastructure
{
	public interface IPackageInfo
	{
        string Platform { get; }

		string Version { get; }
        string UserAgent { get; }
	}
}
