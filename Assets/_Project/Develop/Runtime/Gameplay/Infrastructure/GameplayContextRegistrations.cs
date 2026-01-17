using Assets._Project.Develop.Runtime.Gameplay.Configs;
using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            container.RegisterAsSingle(container => CreateGameplayConfig(container, args.Type));
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
