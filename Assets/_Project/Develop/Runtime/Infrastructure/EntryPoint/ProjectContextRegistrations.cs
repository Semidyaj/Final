using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public class ProjectContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);

            container.RegisterAsSingle<ILoadingScreen>(CreateStandardLoadingScreen);

            container.RegisterAsSingle(CreateConfigsProviderService);

            container.RegisterAsSingle(CreateResourcesAssetsLoader);

            container.RegisterAsSingle(CreateSceneLoaderService);

            container.RegisterAsSingle(CreateSceneSwitcherService);
        }

        private static SceneSwitcherService CreateSceneSwitcherService(DIContainer c)
        {
            SceneLoaderService sceneLoaderService = c.Resolve<SceneLoaderService>();

            ILoadingScreen loadingScreen = c.Resolve<ILoadingScreen>();

            return new SceneSwitcherService(sceneLoaderService, loadingScreen, c);
        }

        private static StandardLoadingScreen CreateStandardLoadingScreen(DIContainer c)
        {
            StandardLoadingScreen standardLoadingScreenPrefab = c.Resolve<ResourcesAssetsLoader>()
                .Load<StandardLoadingScreen>("Utilities/StandardLoadingScreen");

            return Object.Instantiate(standardLoadingScreenPrefab);
        }

        private static SceneLoaderService CreateSceneLoaderService(DIContainer c)
            => new SceneLoaderService();

        private static ConfigsProviderService CreateConfigsProviderService(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            ResourcesConfigLoader resourcesConfigLoader = new ResourcesConfigLoader(resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigLoader);
        }

        private static ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c) 
            => new ResourcesAssetsLoader();

        private static CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
        {
            CoroutinesPerformer coroutinesPerformerPrefab = c.Resolve<ResourcesAssetsLoader>()
                .Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Object.Instantiate(coroutinesPerformerPrefab);
        }
    }
}
