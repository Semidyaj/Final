using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Utilities.AssetsManagment;
using Assets._Project.Develop.Utilities.ConfigsManagment;
using Assets._Project.Develop.Utilities.CoroutinesManager;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop
{
    public class Test : MonoBehaviour
    {
        private DIContainer _container;

        private void Awake()
        {
            _container = new();

            _container.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);
            _container.RegisterAsSingle(CreateConfigsProviderService);
            _container.RegisterAsSingle(CreateResourcesAssetsLoader);

            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            coroutinesPerformer.StartPerform(LoadConfigs());
        }

        private ConfigsProviderService CreateConfigsProviderService(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            ResourcesConfigLoader resourcesConfigLoader = new ResourcesConfigLoader(resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigLoader);
        }

        private ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c) => new ResourcesAssetsLoader();

        private CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
        {
            CoroutinesPerformer coroutinesPerformerPrefab = c.Resolve<ResourcesAssetsLoader>()
                .Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Instantiate(coroutinesPerformerPrefab);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConfigsProviderService configsProviderService = _container.Resolve<ConfigsProviderService>();

                TestConfig config = configsProviderService.GetConfig<TestConfig>();
                Debug.Log(config.Damage);
            }
        }

        private IEnumerator LoadConfigs()
        {
            ConfigsProviderService configsProviderService = _container.Resolve<ConfigsProviderService>();

            Debug.Log("StartLoadConfig");

            yield return configsProviderService.LoadAsync();

            Debug.Log("EndLoadConfig");
        }
    }
}