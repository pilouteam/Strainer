using System;
using System.Threading.Tasks;

namespace Strainer.Infrastructure
{
    public interface IToastService
    {
        Task<IToast> Show(object dataContext);

        Task<IToast> Show(object dataContext, TimeSpan duration);

  
    }
    public interface IToast
    {
        Task Show();
        Task Hide();
    }
}

