using Assets._Project.Develop.Runtime.Configs.Meta.Economy;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.StatisticsService;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.RegisterAsSingle(CreateGameModeChooseService);

            container.RegisterAsSingle(CreateStatisticsView);

            container.RegisterAsSingle(CreateResetStatistics);
        }

        private static ResetStatistics CreateResetStatistics(DIContainer c)
            => new ResetStatistics(c.Resolve<WalletService>(), c.Resolve<GameplayStatisticsService>(), c.Resolve<GameplayEconomyConfig>());

        private static StatisticsView CreateStatisticsView(DIContainer c)
            => new StatisticsView(c.Resolve<GameplayStatisticsService>(), c.Resolve<WalletService>());

        private static GameModeChooseService CreateGameModeChooseService(DIContainer c)
            => new GameModeChooseService();
    }
}
