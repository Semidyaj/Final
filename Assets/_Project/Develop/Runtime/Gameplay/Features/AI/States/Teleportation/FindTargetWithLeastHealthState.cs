using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelectors;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Teleportation
{
    public class FindTargetWithLeastHealthState : State, IUpdatableState
    {
        private ITargetSelector _targetSelector;
        private EntitiesLifeContext _entitiesLifeContext;
        private ReactiveVariable<Entity> _currentTarget;
        private ReactiveVariable<Vector3> _currentTargetPosition;

        public FindTargetWithLeastHealthState(ITargetSelector targetSelector, EntitiesLifeContext entitiesLifeContext, Entity entity)
        {
            _targetSelector = targetSelector;
            _entitiesLifeContext = entitiesLifeContext;

            _currentTarget = entity.CurrentTarget;
            _currentTargetPosition = entity.TeleportationTargetPoint;
        }

        public void Update(float deltaTime)
        {
            _currentTarget.Value = _targetSelector.SelectTargetFrom(_entitiesLifeContext.Entities);

            if (_currentTarget.Value != null)
                _currentTargetPosition.Value = _currentTarget.Value.Transform.position - Vector3.back * 2;
        }
    }
}
