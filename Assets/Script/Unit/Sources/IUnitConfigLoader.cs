namespace Unit
{
    public interface IUnitConfigLoader
    {
        T Load<T>(string id) where T : UnitConfig;
    }
}