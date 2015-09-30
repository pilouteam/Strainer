using Cirrious.CrossCore.IoC;
using Strainer.Presentation.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Otolane.Presentation;

namespace Strainer
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            Mvx.LazyConstructAndRegisterSingleton<IMvxAppStart, StartApplicationObject>();
        }
    }
}