using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private SceneSwitcherService _sceneSwitcherService;
        private ICoroutinesPerformer _coroutinesPerformer;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Initialization menu scene");

            _sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start menu scene");
            Debug.Log("Select gameplay mode: 1 - numbers, 2 - letters");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _coroutinesPerformer.StartPerform(_sceneSwitcherService
                    .ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(GameplayTypes.Numbers)));

            if (Input.GetKeyDown(KeyCode.Alpha2))
                _coroutinesPerformer.StartPerform(_sceneSwitcherService
                    .ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(GameplayTypes.Letters)));
        }
    }
}
