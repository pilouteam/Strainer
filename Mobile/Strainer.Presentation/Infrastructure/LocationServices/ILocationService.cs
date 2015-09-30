using System;
using System.Threading.Tasks;

namespace Strainer.Infrastructure.LocationServices
{
    public interface ILocationService
    {
        void Start();

        void Stop();

        void Restart();

        bool IsStarted { get; }

        IObservable<Position> Positions { get;}
        IObservable<Position> BestPositions { get;}

        IObservable<Position> GetNextBest(TimeSpan timeout);

        Task<Position> GetUserPosition();

        Position LastKnownPosition { get; }

        Position BestPosition { get; }

        void RequestLocationAuthorization();
    }
}

