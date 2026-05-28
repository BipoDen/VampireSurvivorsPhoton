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
    public class InputBehaviour : SimulationBehaviour, IBeforeUpdate
    {
        private NetInput _accumulatedInput;
        private bool _resetInput;
        
        private SceneLoader _sceneLoader;
        private NetworkRunnerCallbacksAdapter _runnerAdapter;
        private JoystickInput _input;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, NetworkRunner runner, NetworkRunnerCallbacksAdapter runnerAdapter, JoystickInput input)
        {
            _sceneLoader = sceneLoader;
            _runnerAdapter = runnerAdapter;
            _input = input;
            RegisterOnRunner(runner);
            
            _runnerAdapter.Shutdown += OnShutdown;
            _runnerAdapter.Input += OnInput;
        }

        private void RegisterOnRunner(NetworkRunner runner)
        {
            if (runner.IsRunning) {
                runner.AddGlobal(this);
            }
        }
        
        private void RemoveFromRunner() {
            if (Runner.IsRunning) {
                Runner.RemoveGlobal(this);
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