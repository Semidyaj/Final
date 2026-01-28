using Assets._Project.Develop.Runtime.Gameplay.Features.RewardsService;
using Assets._Project.Develop.Runtime.Gameplay.Features.StatisticsService;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ResultHandler
{
    public class GameplayResultHandler
    {
        private readonly GameplayRewardsService _rewardService;
        private readonly GameplayStatisticsService _statisticsService;

        public GameplayResultHandler(GameplayRewardsService rewardService, GameplayStatisticsService statisticsService)
        {
            _rewardService = rewardService;
            _statisticsService = statisticsService;
        }

        public int Wins => _statisticsService.Wins;
        public int Defeats => _statisticsService.Defeats;

        public void Apply(GameplayEndState state)
        {
            _rewardService.Apply(state);
            _statisticsService.Apply(state);
        }
    }
}
