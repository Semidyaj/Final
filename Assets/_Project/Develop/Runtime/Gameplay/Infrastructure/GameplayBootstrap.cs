using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Gameplay.Features.ResultHandler;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
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

        private GameplayCycle _gameplayCycle;

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

            _gameplayCycle = _container.Resolve<GameplayCycle>();

            _gameplayCycle.GameEnd += OnGameEnded;
        }

        public override void Run()
        {
            _gameplayCycle.Start();
        }

        private void Update()
        {
            _gameplayCycle?.Update();
        }

        private void OnGameEnded(GameplayEndState endState)
        {
            GameplayResultHandler gameplayResultHandler = _container.Resolve<GameplayResultHandler>();
            gameplayResultHandler.Apply(endState);

            PlayerDataProvider playerDataProvider = _container.Resolve<PlayerDataProvider>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
            coroutinesPerformer.StartPerform(playerDataProvider.Save());

            GameplaySceneSwitcher gameplaySceneSwitcher = _container.Resolve<GameplaySceneSwitcher>();
            gameplaySceneSwitcher.SwitchBy(endState, _inputArgs);
        }

        private void OnDestroy()
        {
            _gameplayCycle.GameEnd -= OnGameEnded;
            _gameplayCycle.Dispose();
        }
    }
}
