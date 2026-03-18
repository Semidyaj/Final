using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using System;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup
{
    public class SelectableAbilityPresenter : IPresenter
    {
        public event Action<SelectableAbilityPresenter> Selected;

        private AbilitiesFactory _abilitiesFactory;
        private Entity _entity;

        public SelectableAbilityPresenter(
            AbilityConfig abilityConfig,
            SelectableAbilityView view,
            AbilitiesFactory abilitiesFactory,
            Entity entity)
        {
            AbilityConfig = abilityConfig;
            View = view;
            _abilitiesFactory = abilitiesFactory;
            _entity = entity;
        }

        public AbilityConfig AbilityConfig { get; }
        public SelectableAbilityView View { get; }

        public void Initialize()
        {
            View.SetName(AbilityConfig.Name);
            View.SetDescription(AbilityConfig.Description);
            View.SetTabletText("NEW");

            View.Icon.SetIcon(AbilityConfig.Icon);
            View.Icon.HideLevel();

            View.Clicked += OnViewClicked;
        }

        public void Dispose()
        {
            View.Clicked -= OnViewClicked;
        }

        public void Provide()
        {
            Ability ability = _abilitiesFactory.CreateAbilityFor(_entity, AbilityConfig);
            _entity.Abilities.Add(ability);
        }

        private void OnViewClicked() => Selected?.Invoke(this);
    }
}
