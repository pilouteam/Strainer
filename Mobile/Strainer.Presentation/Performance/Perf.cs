using System;
using System.Runtime.CompilerServices;
using System.Reactive.Disposables;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using System.Diagnostics;

namespace Strainer.Performance
{
    public static class Perf
    {
        static readonly PerfTools Tools = new PerfTools();
        static readonly Action<string> _timestStrainerOnce;

        static Perf()
        {
            _timestStrainerOnce = Once<string>(Tools.TimestStrainer);
        }

        public static IDisposable Measure([CallerMemberName]string metric = null)
        {
            return Tools.Measure(metric);
        }

        public static void TimestStrainer([CallerMemberName]string metric = null)
        {
            Tools.TimestStrainer(metric);
        }

        public static void TimestStrainerOnce([CallerMemberName]string metric = null)
        {
            _timestStrainerOnce(metric);
        }


        public static Action<A> Once<A>(this Action<A> action)
        {
            var l = new List<A>();
            return a=> 
            {
                if (!l.Contains(a))
                {
                    action(a);
                    l.Add(a);
                }
            };
        }

        public static Func<A, R> Memoize<A, R>(this Func<A, R> f)
        {
            var d = new Dictionary<A, R>();
            return a=> 
            {
                R r;
                if (!d.TryGetValue(a, out r))
                {
                    r = f(a);
                    d.Add(a, r);
                }
                return r;
            };
        }

        public static Func<A, B, R> Memoize<A, B, R>(this Func<A, B, R> f)
        {
            return f.Tuplify().Memoize().Untuplify();
        }

        private static Func<Tuple<A, B>, R> Tuplify<A, B, R>(this Func<A, B, R> f)
        {
            return t => f(t.Item1, t.Item2);
        }

        private static Func<A, B, R> Untuplify<A, B, R>(this Func<Tuple<A, B>, R> f)
        {
            return (a, b) => f(Tuple.Create(a, b));
        }

    }

    internal class PerfTools
    {
        public PerfTools()
        {

        }

        public IDisposable Measure(string metric)
        {
            var stopwatch = new Stopwatch();

            var disposable = Disposable.Create(() =>
            {
                stopwatch.Stop();
                MvxTrace.TaggedTrace("Strainer.Measure", metric + ": " + stopwatch.ElapsedMilliseconds + "ms");
            });

            stopwatch.Start();
            return disposable;
        }

        public void TimestStrainer(string metric)
        {
            MvxTrace.TaggedTrace("Strainer.TimestStrainer",  metric);
        }


    }
}

