using System;

namespace Strainer.Infrastructure
{
	public interface IPhoneService
	{
        void SendFeedbackErrorLog(string subject, string content = "");
        void ShowFeedbackPopup();
		void AddEventToCalendarAndReminder(string title, string addInfo, string place, DateTime startDate, DateTime alertDate);
		bool CanUseCalendarAPI();

        void OpenUrl(string url);
	}
}

