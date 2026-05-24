using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Logic.UI.MainMenu;
using UnityEngine;

namespace _Project.Logic.UI
{
    public class WindowSwitcher : IWindowSwitcher
    {
        private WindowsRepository _repository;
        private readonly Stack<IWindowPresenter> _history = new();

        public WindowSwitcher(WindowsRepository repository)
        {
            _repository = repository;
        }

        public void Show<T>() where T : IWindowPresenter
        {
            var next = _repository.Get<T>();
            _history.TryPeek(out var current);
            current?.Hide();
            _history.Push(next);
            next.Show();
        }
        
        public void Back()
        {
            if (_history.Count <= 1) return;

            _history.Pop().Hide();
            _history.Peek().Show();
        }
        
        public void ShowRoot<T>() where T : IWindowPresenter
        {
            while (_history.Count > 0)
                _history.Pop().Hide();

            var root = _repository.Get<T>();
            _history.Push(root);
            root.Show();
        }
    }
}