using Assets._Project.Develop.Runtime.Configs.Gameplay;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Gameplay.Features.ResultHandler;
using Assets._Project.Develop.Runtime.Gameplay.Features.RewardsService;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.StatisticsService;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            container.RegisterAsSingle(container => CreateGameplayConfig(container, args.Type));

            container.RegisterAsSingle(CreateGameplayCycle);

            container.RegisterAsSingle(CreateGameMode);

            container.RegisterAsSingle(CreateGameplaySceneSwitcher);

            container.RegisterAsSingle(CreateGameplayResultHandler).NonLazy();

            container.RegisterAsSingle(CreateGameplayUIRoot).NonLazy();

            container.RegisterAsSingle(CreateGameplayPresentersFactory);

            container.RegisterAsSingle(CreateGameplayScreenPresenter).NonLazy();

            container.RegisterAsSingle(CreateEntitiesFactory);

            container.RegisterAsSingle(CreateEntitiesLifeContext);

            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();
        }

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer c)
            => new MonoEntitiesFactory(c.Resolve<ResourcesAssetsLoader>(), c.Resolve<EntitiesLifeContext>());

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer c)
            => new EntitiesLifeContext();

        private static EntitiesFactory CreateEntitiesFactory(DIContainer c)
            => new EntitiesFactory(c);

        private static GameplayScreenPresenter CreateGameplayScreenPresenter(DIContainer c)
        {
            GameplayUIRoot uiRoot = c.Resolve<GameplayUIRoot>();

            GameplayScreenView view = c
                .Resolve<ViewsFactory>()
                .Create<GameplayScreenView>(ViewsIDs.GameplayScreen, uiRoot.HUDLayer);

            GameplayScreenPresenter presenter = c
                .Resolve<GameplayPresentersFactory>()
                .CreateGameplayScreen(view);

            return presenter;
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer c)
            => new GameplayPresentersFactory(c);

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer c)
        {
            GameplayUIRoot gameplayUIRootPrefab = c.Resolve<ResourcesAssetsLoader>()
                .Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot");

            return Object.Instantiate(gameplayUIRootPrefab);
        }

        private static GameplayResultHandler CreateGameplayResultHandler(DIContainer c)
            => new GameplayResultHandler(
                c.Resolve<GameplayRewardsService>(),
                c.Resolve<GameplayStatisticsService>());

        private static GameplaySceneSwitcher CreateGameplaySceneSwitcher(DIContainer c)
        {
            SceneSwitcherService sceneSwitcherService = c.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = c.Resolve<ICoroutinesPerformer>();

            return new GameplaySceneSwitcher(sceneSwitcherService, coroutinesPerformer);
        }

        private static GameplayCycle CreateGameplayCycle(DIContainer c)
        {
            GameMode gameplayProcess = c.Resolve<GameMode>();

            return new GameplayCycle(gameplayProcess);
        }

        private static GameMode CreateGameMode(DIContainer c)
        {
            ConfigsProviderService configProviderService = c.Resolve<ConfigsProviderService>();

            SymbolGameplayConfig config = c.Resolve<SymbolGameplayConfig>();

            InputHandler inputHandler = new InputHandler();
            SequenceComparer sequenceComparer = new SequenceComparer();

            return new GameMode(config, inputHandler, sequenceComparer);
        }

        private static SymbolGameplayConfig CreateGameplayConfig(DIContainer c, GameplayTypes type)
        {
            ConfigsProviderService configProviderService = c.Resolve<ConfigsProviderService>();

            SymbolGameplayConfig gameplayConfig;

            switch (type)
            {
                case GameplayTypes.Numbers:
                    gameplayConfig = configProviderService.GetConfig<NumbersGameplayConfig>();
                    break;

                case GameplayTypes.Letters:
                    gameplayConfig = configProviderService.GetConfig<LettersGameplayConfig>();
                    break;

                default:
                    throw new ArgumentException($"Not valid {nameof(type)}");
            }

            return gameplayConfig;
        }
    }
}
