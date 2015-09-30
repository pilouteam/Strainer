using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;

namespace Strainer.Infrastructure.LocationServices
{
    public abstract class BaseLocationService :ILocationService
    {
        public abstract void Start();

        public abstract void Stop();


        public virtual void Restart()
        {
            Stop();
            Start();
        }


        public abstract bool IsStarted { get; }

        public IObservable<Position> Positions { get; protected set; }
        public IObservable<Position> BestPositions { get; protected set; }

        public IObservable<Position> GetNextBest(TimeSpan timeout)
        {
            return Positions.TakeLast(timeout).Select(_ => BestPosition);
        }

        private IObservable<Position> GetNextPosition(TimeSpan timeout, float maxAccuracy)
        {
            if (!IsStarted)
            {
                Start();
            }
            return Positions.Where(p => p.IsActive() && p.Error <= maxAccuracy).Take(timeout).Take(1);
        }

        public async Task<Position> GetUserPosition()
        {
            // TODO: Handle when location services are not available
            if(  (BestPosition != null) && BestPosition.IsActive() )
            {
                return BestPosition;
            }

            var position = await GetNextPosition(TimeSpan.FromSeconds(15), 50)
                .Take(1)
                .DefaultIfEmpty() // Will return null in case of a timeout
                .ToTask();

            if (position != null)
            {
                return position;
            }

            // between the first call to BestPosition, we might have received a position if LocationService was started by GetNextPosition()
            return BestPosition;
        }

        public abstract void RequestLocationAuthorization();

        public abstract Position LastKnownPosition { get; }

        public abstract Position BestPosition { get; }        
    }
}

