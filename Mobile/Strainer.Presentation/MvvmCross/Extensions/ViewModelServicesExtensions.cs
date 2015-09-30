using Strainer.Infrastructure;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using Strainer.Tracking;

namespace Strainer.MvvmCross.Extensions
{
    public static class ViewModelServicesExtensions
    {
        public static ServicesExtensionPoint Services(this MvxNavigatingObject navigatingObject)
        {
            return new ServicesExtensionPoint();
        }
    }

    public class ServicesExtensionPoint
    {
        public IMessageService Message { get { return Mvx.Resolve<IMessageService>(); } }

        public ILocalization Localize { get { return Mvx.Resolve<ILocalization>(); } }

        public IMvxMessenger MessengerHub { get { return Mvx.Resolve<IMvxMessenger>(); } }

        public ICacheService Cache { get { return Mvx.Resolve<ICacheService>(); } }

        public IPackageInfo PackageInfo { get { return Mvx.Resolve<IPackageInfo>(); } }

		public IPhoneService Phone { get { return Mvx.Resolve<IPhoneService>(); } }

        public ITrackingService Tracking { get { return Mvx.Resolve<ITrackingService>(); } }

    }
}