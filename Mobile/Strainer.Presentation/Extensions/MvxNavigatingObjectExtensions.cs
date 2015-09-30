using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Strainer.MvvmCross;
using System.Runtime.CompilerServices;
using Cirrious.MvvmCross.Platform;
using System.Collections.Generic;

namespace Strainer.Extensions
{
    public static class MvxNavigatingObjectExtensions
    {
		public static ICommand GetCommand(this BaseViewModel viewModel, Action execute, IObservable<bool> canExecute = null, [CallerMemberName]string propertyName = null)
        {
			var existingcommand = viewModel.GetCommand (propertyName);
			if (existingcommand != null)
			{
				return existingcommand;
			}

			return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
        }

		public static ICommand GetCommand<T>(this BaseViewModel viewModel, Action<T> execute, IObservable<bool> canExecute = null, [CallerMemberName]string propertyName = null)
        {
			var existingcommand = viewModel.GetCommand (propertyName);
			if (existingcommand != null)
			{
				return existingcommand;
			}

			return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
        }

		public static ICommand GetCommand(this BaseViewModel viewModel, Func<Task> execute, IObservable<bool> canExecute = null, [CallerMemberName]string propertyName = null)
        {
			var existingcommand = viewModel.GetCommand (propertyName);
			if (existingcommand != null)
			{
				return existingcommand;
			}

			return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
        }

		public static ICommand GetCommand<T>(this BaseViewModel viewModel, Func<T, Task> execute, IObservable<bool> canExecute = null, [CallerMemberName]string propertyName = null)
        {
			var existingcommand = viewModel.GetCommand (propertyName);
			if (existingcommand != null)
			{
				return existingcommand;
			}

			return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
        }

		public static ICommand GetCommand(this BaseViewModel viewModel, Action execute, Func<bool> canExecute, [CallerMemberName]string propertyName = null)
		{
			var existingcommand = viewModel.GetCommand (propertyName);
			if (existingcommand != null)
			{
				return existingcommand;
			}

			return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
		}

        public static ICommand GetCommand(this BaseViewModel viewModel, Func<Task> execute, Func<bool> canExecute, [CallerMemberName]string propertyName = null)
        {
            var existingcommand = viewModel.GetCommand (propertyName);
            if (existingcommand != null)
            {
                return existingcommand;
            }

            return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
        }
        public static ICommand GetCommand<T>(this BaseViewModel viewModel, Func<T, Task> execute, Func<object, bool> canExecute, [CallerMemberName]string propertyName = null)
        {
            var existingcommand = viewModel.GetCommand (propertyName);
            if (existingcommand != null)
            {
                return existingcommand;
            }

            return WrapCommand (AsyncCommand.Create (execute, canExecute), viewModel, propertyName);
        }

		private static AsyncCommand<T> WrapCommand<T>(AsyncCommand<T> command, BaseViewModel viewModel, string propertyName)
		{
			viewModel.RegisterCommand (command, propertyName);
			return command;
		}            
    }
}