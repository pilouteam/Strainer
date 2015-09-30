using System;

namespace Strainer.Infrastructure.Impl
{
    public class GestureProcessingService : IGestureProcessingService
    {
        private int left = 0;
        private int right = 0;
        private int up = 0;
        private int down = 0;

        public void Reset()
        {     
            right = 0;
            up = 0;
            left = 0;
            down = 0;
        }

        public bool ProcessKonamiCode(double deltaX, double deltaY)
        {
            var isMovementX = (Math.Abs(deltaX) - 100) > 0;
            var isMovementY = (Math.Abs(deltaY) - 100) > 0;


            if (isMovementX && isMovementY)
            {
                Reset();
            }
            else if (deltaX > 0 && isMovementX && (left == 1 || left == 2))
            {
                right++;
                if( right == 2)
                {
                    Reset();
                    return true;
                }
            }
            else if (deltaX < 0 && isMovementX && up == 2 && right < 2)
            {
                left++;
            }
            else if (deltaY > 0 && isMovementY && up < 2)
            {
                up++;
            }
            else if (deltaY < 0 && isMovementY && up == 2 && down < 2)
            {
                down++;
            }
            else
            {
                Reset();
            }

            return false;
        }

    }
}

