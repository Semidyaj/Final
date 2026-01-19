using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplaySceneSwitcher
    {
        private SceneSwitcherService _sceneSwitcherService;
        private ICoroutinesPerformer _coroutinesPerformer;

        public GameplaySceneSwitcher(SceneSwitcherService sceneSwitcherService, ICoroutinesPerformer coroutinesPerformer)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void SwitchBy(GameplayEndState endState, GameplayInputArgs inputArgs)
        {
            switch (endState)
            {
                case GameplayEndState.Victory:
                    _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
                    break;

                case GameplayEndState.Defeat:
                    _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, inputArgs));
                    break;

                default:
                    throw new InvalidOperationException("Wrong type of GameplayEndState");
            }
        }
    }
}
