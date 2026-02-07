using Infrastructure;

namespace Unit
{
    public class PooledUnit : Unit
    {
        private readonly IPoolService _poolService;
        private readonly UnitView _view;

        public PooledUnit(string name, IPoolService poolService, UnitView view) : base(name, view.transform)
        {
            _poolService = poolService;
            _view = view;
        }

        protected override void OnDestroyProtected()
        {
            _poolService.Destroy(_view.gameObject);
        }
    }
}