using System;
using System.Threading.Tasks;
using Strainer.Infrastructure;
using Cirrious.CrossCore;

namespace Strainer.Extensions
{
    public static class IProgressServiceExtensions
    {
        public static async Task Run(this IProgressService progress, Func<Task> function)
        {
            var task = function();
            if(task.IsFaulted)
            {
                throw task.Exception.InnerException;
            }
            if (task.IsCompleted)
            {
                return;
            }
            using(progress.Show())
            {
                await task;
            }
        }

        public static async Task<TResult> Run<TResult>(this IProgressService progress, Func<Task<TResult>> function)
        {
            var task = function();
            if(task.IsFaulted)
            {
                throw task.Exception.InnerException;
            }
            if (task.IsCompleted)
            {
                return task.Result;
            }
            using(progress.Show())
            {
                return await task;
            }
        }

        public static async Task ShowProgress(this Task task)
        {
            if(task.IsFaulted)
            {
                throw task.Exception.InnerException;
            }
            if (task.IsCompleted)
            {
                return;
            }
            var progress = Mvx.Resolve<IProgressService>();
            using(progress.Show())
            {
                await task;
            }
        }

        public static async Task<TResult> ShowProgress<TResult>(this Task<TResult> task)
        {
            if(task.IsFaulted)
            {
                throw task.Exception.InnerException;
            }
            if (task.IsCompleted)
            {
                return task.Result;
            }
            var progress = Mvx.Resolve<IProgressService>();
            using(progress.Show())
            {
                return await task;
            }
        }
    }
}

