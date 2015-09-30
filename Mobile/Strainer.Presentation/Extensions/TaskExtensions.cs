using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Cirrious.CrossCore;
using Strainer.Infrastructure;

namespace Strainer.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch(Exception e)
            {
                Mvx.Resolve<ILogger>().LogError(e);
            }
        }

        public static async Task Safe(this Task task)
        {
            try
            {
                await task;
            }
            catch(Exception e)
            {
                Mvx.Resolve<ILogger>().LogError(e);
            }
        }

        public static async Task<T> SafeOrDefault<T>(this Task<T> task, T defaultValue = default(T))
        {
            try
            {
                return await task;
            }
            catch(Exception e)
            {
                Mvx.Resolve<ILogger>().LogError(e);
                return defaultValue;
            }
        }

    }
}

