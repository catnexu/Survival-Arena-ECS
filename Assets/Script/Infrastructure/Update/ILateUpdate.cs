namespace Infrastructure
{
    public interface ILateUpdate : IController
    {
        void UpdateController(float deltaTime);
    }
}