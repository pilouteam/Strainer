 using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Strainer.Messages;
using Strainer.MvvmCross;
using Strainer.MvvmCross.Extensions;
using System.Threading.Tasks;

namespace Strainer.Extensions
{
    public static class ISubViewModelExtensions
    {
        public static async Task ReturnResult<TViewModel, TResult>(this TViewModel viewModel, TResult result) where TViewModel : MvxViewModel, ISubViewModel<TResult>
        {
            var message = new SubNavigationResultMessage<TResult>(viewModel, viewModel.MessageId, result);

            Dispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel));

            await Task.Yield();//for Android - the problem is that the double returnresult the second close is ignored
            await Task.Yield();//todo Yuk
            await Task.Yield();//added a third yield just in case

            viewModel.Services().MessengerHub.Publish(message);
        }

        public static async Task ReturnResultAfterMessage<TViewModel, TResult>(this TViewModel viewModel, TResult result) where TViewModel : MvxViewModel, ISubViewModel<TResult>
        {
            var message = new SubNavigationResultMessage<TResult>(viewModel,viewModel.MessageId, result);

            viewModel.Services().MessengerHub.Publish(message);

            Dispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel));
        }

        private static IMvxViewDispatcher Dispatcher
        {
            get
            {
                return ((IMvxViewDispatcher)MvxSingleton<IMvxMainThreadDispatcher>.Instance);
            }
        }
    }
}