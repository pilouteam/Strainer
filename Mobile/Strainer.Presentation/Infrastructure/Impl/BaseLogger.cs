using System;
using Cirrious.CrossCore;
using System.Globalization;
using Strainer.Tracking;
using System.Threading.Tasks;
using Strainer.Infrastructure;

namespace Strainer.Infrastructure.Impl
{
	public abstract class BaseLogger : ILogger
	{
        private static ITrackingService TrackingService {get { return Mvx.Resolve<ITrackingService>();}}

        public async Task ShowErrorLog()
        {
            var fileservice = Mvx.Resolve<IFileManager>();
            var messageService = Mvx.Resolve<IMessageService>();

            var logPath = GetErrorLogPath();

            if (fileservice.FileExists(logPath))
            {
                var log = await fileservice.Read(logPath);
                await messageService.Show("Error Log", log);
            }
            else
            {
                await messageService.Show("Error Log", "File Not found");
            }
        }

		public void LogMessage (string message, params object[] args)
		{
			if ((args != null) && (args.Length > 0)) {
				message = string.Format (message, args);
			}

			LogMessage (message);
		}

		public void LogError( Exception ex )
		{
			LogError(ex, 0);
		}

		private void LogError(Exception ex, int indent)
		{
			var indentStr = "";
			for (int i = 0; i < indent; i++)
			{
				indentStr += "   ";
			}
			if (indent == 0)
			{
                Write(indentStr + "Error on " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
			}

			Write(indentStr + "Message : " + ex.Message);
			Write(indentStr + "Stack : " + ex.StackTrace);

			if (ex.InnerException != null)
			{
				LogError(ex.InnerException, indent++);
			}

            TrackingService.Error(ex);
		}

		public void LogMessage( string message )
		{
            Write("Message on " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " : " + message);
		}

		protected abstract void Write(string message);

		public abstract string GetErrorLogPath();
	}
}

