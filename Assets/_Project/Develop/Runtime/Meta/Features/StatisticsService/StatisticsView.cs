using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.StatisticsService
{
    public class StatisticsView
    {
        private GameplayStatisticsService _statisticsService;
        private WalletService _walletService;

        public StatisticsView(GameplayStatisticsService statisticsService, WalletService walletService)
        {
            _statisticsService = statisticsService;
            _walletService = walletService;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
                Show();
        }

        public void Show()
            => Debug.Log($"Gold - {_walletService.GetCurrency(CurrencyTypes.Gold).Value}" +
                $", wins - {_statisticsService.Wins}, defeats - {_statisticsService.Defeats}");
    }
}
