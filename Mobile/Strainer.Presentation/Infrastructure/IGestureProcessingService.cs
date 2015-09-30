using System;

namespace Strainer.Infrastructure
{
    public interface IGestureProcessingService
    {
        void Reset();

        bool ProcessKonamiCode(double deltaX, double deltaY);
    }
}

