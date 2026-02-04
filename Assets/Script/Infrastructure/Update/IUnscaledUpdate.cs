namespace Infrastructure
{
    public interface IUnscaledUpdate : IController
    {
        void UpdateController(float deltaTime);
    }
}