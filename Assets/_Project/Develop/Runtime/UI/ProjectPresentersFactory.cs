using Assets._Project.Develop.Runtime.Configs.Meta.Wallet;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Meta.Features.StatisticsService;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Core.LevelsMenuPopup;
using Assets._Project.Develop.Runtime.UI.Core.Statistics;
using Assets._Project.Develop.Runtime.UI.Core.TestPopup;
using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.UI
{
    public class ProjectPresentersFactory
    {
        private readonly DIContainer _container;

        public ProjectPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        //public StatisticsPresenter CreateStatisticsPresenter(IconTextListView listView)
        //    => new StatisticsPresenter(_container.Resolve<ProjectPresentersFactory>(), _container.Resolve<ViewsFactory>(), listView);

        //public DefeatStatsPresenter CreateDefeatStatsPresenter(IconTextView view)
        //    => new DefeatStatsPresenter(view, _container.Resolve<GameplayStatisticsService>(), _container.Resolve<ResetStatistics>());

        //public WinStatsPresenter CreateWinStatsPresenter(IconTextView view)
        //    => new WinStatsPresenter(view, _container.Resolve<GameplayStatisticsService>(), _container.Resolve<ResetStatistics>());

        public LevelsMenuPopupPresenter CreateLevelsMenuPopupPresenter(LevelsMenuPopupView view)
            => new LevelsMenuPopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<ConfigsProviderService>(),
                this,
                _container.Resolve<ViewsFactory>(),
                view);

        public LevelTilePresenter CreateLevelTilePresenter(int levelNumber, LevelTileView view)
            => new LevelTilePresenter(
                _container.Resolve<LevelsProgressionService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                levelNumber,
                view);

        public TestPopupPresenter CreateTestPopupPresenter(TestPopupView view)
            => new TestPopupPresenter(view, _container.Resolve<ICoroutinesPerformer>());

        public WalletPresenter CreateWalletPresenter(IconTextListView listView)
        {
            return new WalletPresenter(
                _container.Resolve<WalletService>(),
                this,
                _container.Resolve<ViewsFactory>(),
                listView);
        }

        public CurrencyPresenter CreateCurrencyPresenter(
            IconTextView view,
            IReadOnlyVariable<int> currency,
            CurrencyTypes currencyType)
        {
            return new CurrencyPresenter(
                currency,
                currencyType,
                _container.Resolve<ConfigsProviderService>().GetConfig<CurrencyIconsConfig>(),
                view);
        }
    }
}
