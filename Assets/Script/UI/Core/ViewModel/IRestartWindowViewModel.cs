namespace UI
{
    public interface IRestartWindowViewModel : IViewModel
    {
        int Score { get; }
        void Start();
    }
}