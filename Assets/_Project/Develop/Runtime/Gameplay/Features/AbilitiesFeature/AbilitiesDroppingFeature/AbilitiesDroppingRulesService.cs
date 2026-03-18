using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature.AbilitiesDroppingFeature
{
    public class AbilitiesDroppingRulesService
    {
        public bool IsAvailable(Entity entity, AbilityConfig config)
        {
            switch (config)
            {
                case StatChangeAbilityConfig statChangeAbilityConfig:
                    return entity.TryGetModifiesStats(out var modifiedStats) && modifiedStats.ContainsKey(statChangeAbilityConfig.StatType);
            }

            return true;
        }
    }
}
