using Cirrious.CrossCore.IoC;
using Strainer.Presentation.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Otolane.Presentation;
using Amp.MvvmCross;

namespace Strainer
{
    public class App : AmpApplication
    {
        public override string ApplicationName
        {
            get
            {
                return "Strainer";
            }
        }
        public App()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            Mvx.LazyConstructAndRegisterSingleton<IMvxAppStart, StartApplicationObject>();
            Mvx.RegisterSingleton<AmpApplication>(this);

        }
    }
}