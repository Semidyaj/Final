using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Allies;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.ResultHandler;
using Assets._Project.Develop.Runtime.Gameplay.Features.RewardsService;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.States;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.StatisticsService;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _inputArgs;

        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            _inputArgs = args;

            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();
            container.RegisterAsSingle(CreateEntitiesFactory);
            container.RegisterAsSingle(CreateEntitiesLifeContext);

            container.RegisterAsSingle(CreateAlliesFactory);

            container.RegisterAsSingle(CreateCollidersRegistryService);

            container.RegisterAsSingle(CreateBrainsFactory);
            container.RegisterAsSingle(CreateAIBrainsContext);

            container.RegisterAsSingle(CreateMainHeroFactory);
            container.RegisterAsSingle(CreateEnemiesFactory);

            container.RegisterAsSingle(CreateStagesFactory);
            container.RegisterAsSingle(CreateStageProviderService);
            container.RegisterAsSingle(CreatePreperationTriggerService);

            container.RegisterAsSingle(CreateGameplayStatesFactory);
            container.RegisterAsSingle(CreateGameplayStatesContext);

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);

            container.RegisterAsSingle(CreateMainHeroHolderService).NonLazy();

            container.RegisterAsSingle(CreateTowerHolderService).NonLazy();

            container.RegisterAsSingle(CreateGameplayResultHandler).NonLazy();
        }

        private static TowerHolderService CreateTowerHolderService(DIContainer c)
            => new TowerHolderService(c.Resolve<EntitiesLifeContext>());

        private static AlliesFactory CreateAlliesFactory(DIContainer c)
            => new AlliesFactory(c);

        private static GameplayResultHandler CreateGameplayResultHandler(DIContainer c)
            => new GameplayResultHandler(
            c.Resolve<GameplayRewardsService>(),
            c.Resolve<GameplayStatisticsService>());

        private static GameplayStatesContext CreateGameplayStatesContext(DIContainer c)
            => new GameplayStatesContext(c.Resolve<GameplayStatesFactory>().CreateGameplayStateMachine(_inputArgs));

        private static GameplayStatesFactory CreateGameplayStatesFactory(DIContainer c)
            => new GameplayStatesFactory(c);

        private static MainHeroHolderService CreateMainHeroHolderService(DIContainer c)
            => new MainHeroHolderService(c.Resolve<EntitiesLifeContext>());

        private static PreperationTriggerService CreatePreperationTriggerService(DIContainer c)
            => new PreperationTriggerService(
                c.Resolve<EntitiesFactory>(),
                c.Resolve<EntitiesLifeContext>());

        private static StageProviderService CreateStageProviderService(DIContainer c)
            => new StageProviderService(
                c.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().GetBy(_inputArgs.LevelNumber),
                c.Resolve<StagesFactory>());

        private static StagesFactory CreateStagesFactory(DIContainer c)
            => new StagesFactory(c);

        private static EnemiesFactory CreateEnemiesFactory(DIContainer c)
            => new EnemiesFactory(c);

        private static MainHeroFactory CreateMainHeroFactory(DIContainer c)
            => new MainHeroFactory(c);

        private static DesktopInput CreateDesktopInput(DIContainer c)
            => new DesktopInput();

        private static AIBrainsContext CreateAIBrainsContext(DIContainer c)
            => new AIBrainsContext();

        private static BrainsFactory CreateBrainsFactory(DIContainer c)
            => new BrainsFactory(c);

        private static CollidersRegistryService CreateCollidersRegistryService(DIContainer c)
            => new CollidersRegistryService();

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer c)
            => new MonoEntitiesFactory(
                c.Resolve<ResourcesAssetsLoader>(),
                c.Resolve<EntitiesLifeContext>(),
                c.Resolve<CollidersRegistryService>());

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer c)
            => new EntitiesLifeContext();

        private static EntitiesFactory CreateEntitiesFactory(DIContainer c)
            => new EntitiesFactory(c);
    }
}
