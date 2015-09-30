using System;
using System.Threading.Tasks;

namespace Strainer.Infrastructure
{
	public interface ILogger
	{
		string GetErrorLogPath();
        Task ShowErrorLog();

		void LogError(Exception ex);
		void LogMessage(string message);
		void LogMessage(string message, params object[] args);
	}
}