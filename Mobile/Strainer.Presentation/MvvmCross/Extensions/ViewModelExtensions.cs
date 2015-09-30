using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Linq.Expressions;
using Cirrious.CrossCore.Core;

namespace Strainer.MvvmCross.Extensions
{
    public static class ViewModelExtensions
    {
        public static IObservable<string> ObservePropertyChanged(this INotifyPropertyChanged vm)
        {
            return Observable.FromEventPattern(vm, "PropertyChanged")
                .Select(v=>(PropertyChangedEventArgs)v.EventArgs)
                .Select(a=>a.PropertyName);
        }
        public static IObservable<string> ObservePropertyChanged<T>(this INotifyPropertyChanged vm, Expression<Func<T>> property)
        {
            string propertyNameFromExpression = vm.GetPropertyNameFromExpression(property);

            return vm.ObservePropertyChanged().Where(pName => pName == propertyNameFromExpression);
        }
    }
}

