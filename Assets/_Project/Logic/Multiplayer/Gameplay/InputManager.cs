using System;
using System.Collections.Generic;
using _Project.Logic.Constants;
using _Project.Logic.Input;
using _Project.Logic.Services;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Multiplayer.Gameplay
{
    public class InputManager : SimulationBehaviour, IBeforeUpdate
    {
        private NetInput _accumulatedInput;
        private bool _resetInput;

        private NetworkRunner _runner;
        private SceneLoader _sceneLoader;
        private NetworkRunnerCallbacksAdapter _runnerAdapter;
        private JoystickInput _input;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, NetworkRunnerCallbacksAdapter runnerAdapter, JoystickInput input, NetworkRunner runner)
        {
            _sceneLoader = sceneLoader;
            _runnerAdapter = runnerAdapter;
            _input = input;
            _runner = runner;
            
            _runnerAdapter.Shutdown += OnShutdown;
            _runnerAdapter.Input += OnInput;
        }

        public void RegisterOnRunner()
        {
            if (_runner.IsRunning) {
                _runner.AddGlobal(this);
            }
        }
        
        public void RemoveFromRunner() {
            if (_runner.IsRunning) {
                _runner.RemoveGlobal(this);
            }
        }
        
        public void BeforeUpdate()
        {
            if (_resetInput)
            {
                _resetInput = false;
                _accumulatedInput = default;
            }
            
            Vector2 moveDirection = _input.Move();

            _accumulatedInput.Direction = moveDirection;
        }

        private async void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
            {
                await runner.Shutdown();
                _sceneLoader.LoadScene(GameConstants.MAIN_MENU_SCENE_NAME);
            }
        }

        private void OnInput(NetworkRunner runner, NetworkInput input)
        {
            _accumulatedInput.Direction.Normalize();
            input.Set(_accumulatedInput);
            _resetInput = true;
        }

        private void OnDestroy()
        {
            RemoveFromRunner();
        }
    }
}