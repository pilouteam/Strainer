using System;
using System.Reactive.Disposables;

namespace Strainer.Extensions
{
    public static class DisposableExtensions
    {
        public static void DisposeWith(this IDisposable disposable, CompositeDisposable composite)
        {
            if (disposable == null) throw new ArgumentNullException("disposable");
            if (composite == null) throw new ArgumentNullException("composite");

            composite.Add(disposable);
        }
    }
}