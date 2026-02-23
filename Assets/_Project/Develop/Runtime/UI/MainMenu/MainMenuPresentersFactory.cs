using Assets._Project.Develop.Runtime.Infrastructure.DI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPresentersFactory
    {
        private readonly DIContainer _container;

        public MainMenuPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public MainMenuScreenPresenter CreateMainMenuScreen(MainMenuScreenView view)
            => new MainMenuScreenPresenter(
                view, 
                _container.Resolve<ProjectPresentersFactory>(), 
                _container.Resolve<MainMenuPopupService>(), 
                //_container.Resolve<ResetStatistics>(),
                _container.Resolve<MainMenuPresentersFactory>());
    }
}
