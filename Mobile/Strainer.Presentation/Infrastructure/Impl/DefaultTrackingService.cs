using System;
using Strainer.Tracking;

namespace Strainer.Infrastructure.Impl
{
    public class DefaultTrackingService : ITrackingService
    {
        public DefaultTrackingService()
        {
        }

        public void Error(Exception e)
        {
        }

        public void Track(string @event, string key, string value)
        {
        }

        public void TrackPage<T>()
        {
        }

        public void TrackPage(string name)
        {
        }

    }
}

