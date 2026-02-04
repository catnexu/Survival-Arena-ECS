namespace Infrastructure
{
    public interface IUpdate : IController
    {
        void UpdateController(float deltaTime);
    }
}