using System;
using Strainer.Infrastructure;
using System.Threading.Tasks;

namespace Strainer.Infrastructure
{
	public interface ISerializer
	{
        T Deserialize<T>(string serialized);

        string Serialize(object item);
	}

}

