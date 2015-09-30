using System;
using System.Net;

namespace Strainer.Infrastructure.Network
{
    //https://github.com/xamarin/monotouch-sStrainerles/blob/master/ReachabilitySStrainerle/reachability.cs
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

}
