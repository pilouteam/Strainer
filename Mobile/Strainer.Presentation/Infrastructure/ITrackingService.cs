using System;
using Strainer.Infrastructure;

namespace Strainer.Tracking
{
	public interface ITrackingService
	{
        void Error(Exception e);

        void Track(string @event, string key, string value);

        void TrackPage<T>();
        void TrackPage(string name);
    }

}

