namespace Unit
{
    public interface IUnit
    {
        UnitView View { get; }
        void Destroy();
    }
}