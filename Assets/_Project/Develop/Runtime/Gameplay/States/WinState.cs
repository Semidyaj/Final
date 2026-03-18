using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.PauseFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.ResultHandler;
using Assets._Project.Develop.Runtime.Gameplay.Features.RewardsService;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class WinState : EndGameState, IUpdatableState
    {
        private readonly LevelsProgressionService _levelsProgressionService;
        private readonly GameplayInputArgs _gameplayInputArgs;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameplayResultHandler _gameplayResultHandler;
        private readonly GameplayRewardsService _gameplayRewardsService;
        private readonly GameplayPopupService _gameplayPopupService;

        public WinState(
            IInputService inputService,
            IPauseService pauseService,
            LevelsProgressionService levelsProgressionService,
            GameplayInputArgs gameplayInputArgs,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            GameplayResultHandler gameplayResultHandler,
            GameplayRewardsService gameplayRewardsService,
            GameplayPopupService gameplayPopupService) : base(inputService, pauseService)
        {
            _levelsProgressionService = levelsProgressionService;
            _gameplayInputArgs = gameplayInputArgs;
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _gameplayResultHandler = gameplayResultHandler;
            _gameplayRewardsService = gameplayRewardsService;
            _gameplayPopupService = gameplayPopupService;
        }

        public override void Enter()
        {
            base.Enter();

            _gameplayResultHandler.Apply(GameplayEndState.Victory);
            _gameplayRewardsService.ApplyVictoryReward();

            _levelsProgressionService.AddLevelToCompleted(_gameplayInputArgs.LevelNumber);

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAsync());

            _gameplayPopupService.OpenWinPopup();
        }

        public void Update(float deltaTime)
        {
        }
    }
}
