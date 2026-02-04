using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.StatisticsService;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private GameModeChooseService _gameModeChooseService;

        private ResetStatistics _resetStatistics;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _gameModeChooseService = _container.Resolve<GameModeChooseService>();

            _gameModeChooseService.GameModeChosen += OnModeChosen;

            _resetStatistics = _container.Resolve<ResetStatistics>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Select gameplay mode: 1 - numbers, 2 - letters");
        }

        private void Update()
        {
            _gameModeChooseService?.Update();
        }

        private void OnModeChosen(GameplayTypes type)
        {
            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(type)));
        }

        private void OnDestroy()
        {
            _gameModeChooseService.GameModeChosen -= OnModeChosen;
        }
    }
}
