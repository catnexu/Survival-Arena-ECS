using System;

namespace UI
{
    public interface IViewModel
    {
        event Action<IViewModel> OnClose;
        
        void Close();
    }
}