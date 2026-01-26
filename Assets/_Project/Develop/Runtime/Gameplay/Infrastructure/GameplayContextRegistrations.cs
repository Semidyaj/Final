using Assets._Project.Develop.Runtime.Configs.Gameplay;
using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            container.RegisterAsSingle(container => CreateGameplayConfig(container, args.Type));
            container.RegisterAsSingle(CreateGameplayCycle);
            container.RegisterAsSingle(CreateGameplayProcess);
            container.RegisterAsSingle(CreateGameplaySceneSwitcher);
        }

        private static GameplaySceneSwitcher CreateGameplaySceneSwitcher(DIContainer c)
        {
            SceneSwitcherService sceneSwitcherService = c.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = c.Resolve<ICoroutinesPerformer>();

            return new GameplaySceneSwitcher(sceneSwitcherService, coroutinesPerformer);
        }

        private static GameplayCycle CreateGameplayCycle(DIContainer c)
        {
            GameplayProcess gameplayProcess = c.Resolve<GameplayProcess>();

            return new GameplayCycle(gameplayProcess);
        }

        private static GameplayProcess CreateGameplayProcess(DIContainer c)
        {
            ConfigsProviderService configProviderService = c.Resolve<ConfigsProviderService>();

            SymbolGameplayConfig config = c.Resolve<SymbolGameplayConfig>();

            return new GameplayProcess(config);
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
