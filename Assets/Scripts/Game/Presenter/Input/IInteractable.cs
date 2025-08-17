
namespace TowerDefense.Presenter
{
    public interface IClickHandler
    {
        void HandleClick();
    }

    public interface IHoverHandler
    {
        void HandleHoverEnter();
        void HandleHoverExit();
    }
}
