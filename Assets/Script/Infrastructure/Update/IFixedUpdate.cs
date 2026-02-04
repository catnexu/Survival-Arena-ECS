namespace Infrastructure
{
    public interface IFixedUpdate : IController
    {
        void UpdateController(float deltaTime);
    }
}