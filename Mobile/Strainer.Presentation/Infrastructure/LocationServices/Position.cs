using System;

namespace Strainer.Infrastructure.LocationServices
{
 public class Position
    {
        public float Error  { get; set; }

        public DateTime Time  { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("({3:T}) - Position: [{0},{1}, Error: {2}]", Latitude, Longitude, Error, Time);
        }
    }

    public static class PositionExtensions
    {
        public static TimeSpan ValidCoordinateTime = new TimeSpan(0, 0, 30);

        public static bool IsActive(this Position postion)
        {
            return postion.Time > DateTime.Now.Subtract (ValidCoordinateTime);
        }

        public static bool IsBetterThan(this Position trueIfBetter, Position falseIfBetter)
        {
            if (falseIfBetter == null)
            {
                return true;
            }

            if (trueIfBetter == null)
            {
                return false;
            }

            if((falseIfBetter.Time - trueIfBetter.Time).Duration() > ValidCoordinateTime)            
            {
                return false;
            }

            return trueIfBetter.Error <= falseIfBetter.Error;
        }
    }
}

