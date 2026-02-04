namespace Infrastructure
{
    public interface IServiceLocator
    {
        void Register<T>(T instance) where T : class;

        T Resolve<T>();
    }
}