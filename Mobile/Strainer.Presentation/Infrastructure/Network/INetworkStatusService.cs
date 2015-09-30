using System;

namespace Strainer.Infrastructure.Network
{
    public interface INetworkStatusService
    {
        NetworkStatus InternetConnectionStatus();

		IObservable<NetworkStatus> ObserveNetworkStatusChanged();
    }
}

