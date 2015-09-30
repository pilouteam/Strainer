using System;

namespace Strainer.Infrastructure
{
    public interface IProgressService
    {
        void Show(bool show);
        void ShowNonModal(bool show);

        IDisposable Show();
        IDisposable ShowNonModal();
    }
}