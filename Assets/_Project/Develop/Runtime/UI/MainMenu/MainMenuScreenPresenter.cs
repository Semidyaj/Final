using Assets._Project.Develop.Runtime.Meta.Features.StatisticsService;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Core.Statistics;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _screen;

        private readonly ProjectPresentersFactory _projectPresentersFactory;

        private readonly MainMenuPresentersFactory _mainMenuPresentersFactory;

        private readonly MainMenuPopupService _popupService;

        private readonly ResetStatistics _resetService;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            MainMenuPopupService popupService,
            ResetStatistics resetService,
            MainMenuPresentersFactory mainMenuPresentersFactory)
        {
            _screen = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _popupService = popupService;
            _resetService = resetService;
            _mainMenuPresentersFactory = mainMenuPresentersFactory;
        }

        public void Initialize()
        {
            _screen.OpenLevelsMenuButtonClicked += OnOpenLevelsMenuButtonClicked;
            _screen.ResetStatsButtonClicked += OnResetStatsButtonClicked;

            CreateWallet();
            CreateStatisticsView();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _screen.OpenLevelsMenuButtonClicked -= OnOpenLevelsMenuButtonClicked;
            _screen.ResetStatsButtonClicked += OnResetStatsButtonClicked;

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);
            _childPresenters.Add(walletPresenter);
        }

        private void CreateStatisticsView()
        {
            StatisticsPresenter statisticsPresenter = _mainMenuPresentersFactory.CreateStatisticsPresenter(_screen.StatisticsView);
            _childPresenters.Add(statisticsPresenter);
        }

        private void OnOpenLevelsMenuButtonClicked() => _popupService.OpenLevelsMenuPopup();

        private void OnResetStatsButtonClicked() => _resetService.Reset();
    }
}
