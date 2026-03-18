using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature.AbilitiesDroppingFeature
{
    public class AbilityDropService
    {
        private readonly AbilitiesConfigsContainer _abilitiesConfigsContainer;
        private readonly AbilitiesDroppingRulesService _abilitiesDroppingRulesService;

        public AbilityDropService(AbilitiesConfigsContainer abilitiesConfigsContainer, AbilitiesDroppingRulesService abilitiesDroppingRulesService)
        {
            _abilitiesConfigsContainer = abilitiesConfigsContainer;
            _abilitiesDroppingRulesService = abilitiesDroppingRulesService;
        }

        public List<AbilityConfig> Drop(int count, Entity entity)
        {
            List<AbilityConfig> availableAbilities = new List<AbilityConfig>(_abilitiesConfigsContainer.AbilityConfigs
                .Where(abilityOption => _abilitiesDroppingRulesService.IsAvailable(entity, abilityOption)));

            List<AbilityConfig> selectedAbilities = new();

            for (int i = 0; i < count; i++)
            {
                AbilityConfig selectedAbility = availableAbilities[UnityEngine.Random.Range(0, availableAbilities.Count)];
                selectedAbilities.Add(selectedAbility);
                availableAbilities.Remove(selectedAbility);
            }

            return selectedAbilities;
        }
    }
}
