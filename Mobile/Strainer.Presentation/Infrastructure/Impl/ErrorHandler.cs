using System;
using Cirrious.MvvmCross.Views;
using System.Net.Http;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Strainer.Extensions;

namespace Strainer.Infrastructure.Impl
{
    public class ErrorHandler : IErrorHandler
    {

        readonly IMvxViewPresenter _viewPresenter;

        readonly ILogger _logger;

        IMessageService _messageService;

        ILocalization _localizationService;

        IPhoneService _phoneService;

        public ErrorHandler(
            IMvxViewPresenter viewPresenter, 
            ILogger logger, 
            IMessageService messageService, 
            ILocalization localizationService,
            IPhoneService phoneService)
        {
            this._phoneService = phoneService;
            this._localizationService = localizationService;
            this._messageService = messageService;
            _logger = logger;
            _viewPresenter = viewPresenter;
        }

        public async Task HandleResponse(HttpResponseMessage message)
        {
            _logger.LogMessage("HttpResponseMessage Not OK - " + message.RequestMessage +  message.ToString());

            if(message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogMessage("Unauthorized");

                _phoneService.ShowFeedbackPopup();
            }

            var content = await message.Content.ReadAsStringAsync().SafeOrDefault();

            _logger.LogMessage("Content : " +content);    
        }

        public void Handle(Exception ex)
        {  
            _logger.LogError(ex);  
        }
    }
}

