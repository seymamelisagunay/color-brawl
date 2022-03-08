namespace Game.UI
{
    public interface IPage
    {
        string Name { get; }
        bool Visible { get; }
        void Show();
        void Hide();
    }
}