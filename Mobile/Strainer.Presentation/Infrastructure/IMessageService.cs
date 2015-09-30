using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strainer.Infrastructure
{
    public interface IMessageService
    {
        Task Show(string title, string message);
        void Show(string title, string message, Action additionalAction);
        void Show(string title, string message, string positiveButtonTitle, Action positiveAction, string negativeButtonTitle, Action negativeAction);
        void Show(string title, string message, string positiveButtonTitle, Action positiveAction, string negativeButtonTitle, Action negativeAction, string neutralButtonTitle, Action neutralAction);
        void Show(string title, string message, List<KeyValuePair<string, Action>> additionalButton);
    }
}