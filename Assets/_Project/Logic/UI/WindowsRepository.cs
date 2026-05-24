using System;
using System.Collections.Generic;
using _Project.Logic.UI.MainMenu;

namespace _Project.Logic.UI
{
    public class WindowsRepository
    {
        private readonly Dictionary<Type, IWindowPresenter> _presenters = new();

        public void Register<T>(T presenter) where T : IWindowPresenter
            => _presenters[typeof(T)] = presenter;

        public T Get<T>() where T : IWindowPresenter
            => (T)_presenters[typeof(T)];
        
    }
}