using _Project.Logic.UI.MainMenu;

namespace _Project.Logic.UI
{
    public interface IWindowSwitcher
    {
        void Show<T>() where T : IWindowPresenter;
        void Back();
        void ShowRoot<T>() where T : IWindowPresenter;
    }
}