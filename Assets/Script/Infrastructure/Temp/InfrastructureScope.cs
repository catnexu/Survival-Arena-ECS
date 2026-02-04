namespace Infrastructure
{
    public static class InfrastructureScope
    {
        public static void Build(IServiceLocator locator)
        {
            TimeManager timeManager = new TimeManager();
            locator.Register<ITimeManager>(timeManager);
            TickController tickController = new TickController(timeManager);
            locator.Register<ITickController>(tickController);
            locator.Register<IPoolService>(new PoolService(tickController));
            locator.Register<IRandomizer>(new Randomizer());
        }
    }
}