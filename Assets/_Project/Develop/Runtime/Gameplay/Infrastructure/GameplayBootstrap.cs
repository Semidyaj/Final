using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private GameplayInputArgs _inputArgs;

        private GameplayProcess _gameplayProcess;

        private GameplayEndState _gameplayEndState;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log($"Starting level with {_inputArgs.Type.ToString().ToLower()}");

            ConfigsProviderService configsProviderService = _container.Resolve<ConfigsProviderService>();

            yield return configsProviderService.LoadAsync();

            _gameplayProcess = new GameplayProcess(_container);

            _gameplayProcess.Won += OnWon;
            _gameplayProcess.Lost += OnLost;

            _gameplayEndState = GameplayEndState.None;
        }

        public override void Run()
        {
            _gameplayProcess.Start();
        }

        private void Update()
        {
            if (_gameplayEndState == GameplayEndState.None)
                _gameplayProcess?.Update();
            else
                SwitchScene();
        }

        private void OnWon()
        {
            Debug.Log("Congratulations! You won");
            Debug.Log("Press Space to go to menu");

            _gameplayEndState = GameplayEndState.Victory;
        }

        private void OnLost()
        {
            Debug.Log("Oops! You lost");
            Debug.Log("Press Space to try again");

            _gameplayEndState = GameplayEndState.Defeat;
        }

        private void SwitchScene()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

                switch (_gameplayEndState)
                {
                    case GameplayEndState.Victory:
                        coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
                        break;

                    case GameplayEndState.Defeat:
                        coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, _inputArgs));
                        break;

                    default:
                        throw new InvalidOperationException("Wrong type of GameplayEndState");
                }
            }
        }
    }
}
