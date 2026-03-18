using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature.Abilities;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature
{
    public class AbilitiesFactory
    {
        private DIContainer _container;

        public AbilitiesFactory(DIContainer container)
        {
            _container = container;
        }

        public Ability CreateAbilityFor(Entity entity, AbilityConfig config)
        {
            switch (config)
            {
                case StatChangeAbilityConfig changeAbilityConfig:
                    return new StatChangeAbility(entity, changeAbilityConfig);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
