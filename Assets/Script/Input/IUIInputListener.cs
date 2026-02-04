using UnityEngine;

namespace Input
{
    public interface IUIInputListener : IInputListener
    {
        void StartSubmit();
        void CancelSubmit();
        void Cancel();
        void Navigate(Vector2 value);
        void OnScroll(Vector2 value);
    }
}