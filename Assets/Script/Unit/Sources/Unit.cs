namespace Unit
{
    public abstract class Unit : IUnit
    {
        private bool _destroyed;
        public UnitView View { get; }

        protected Unit(UnitView view)
        {
            View = view;
        }

        public void Destroy()
        {
            if (_destroyed)
                return;
            _destroyed = true;
            OnDestroyProtected();
        }

        protected abstract void OnDestroyProtected();
    }
}