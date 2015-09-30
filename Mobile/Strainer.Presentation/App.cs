using Cirrious.CrossCore.IoC;
using Strainer.Presentation.ViewModels;

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
				
            RegisterAppStart<HomeViewModel>();
        }
    }
}