using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Teleportation
{
    public class TeleportationProcessState : State, IUpdatableState
    {
        private ReactiveEvent _completed;
        private ReactiveVariable<bool> _isCompleted;

        private ReactiveVariable<Vector3> _currentTargetPoint;
        private Transform _transform;

        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _teleportationEnergyCost;

        public TeleportationProcessState(Entity entity)
        {
            _completed = entity.TeleportationEvent;
            _isCompleted = entity.IsTeleportationCompleted;

            _currentTargetPoint = entity.TeleportationTargetPoint;
            _transform = entity.Transform;

            _currentEnergy = entity.CurrentEnergy;
            _maxEnergy = entity.MaxEnergy;
            _teleportationEnergyCost = entity.TeleportationEnergyCost;
        }

        public override void Enter()
        {
            base.Enter();

            _transform.position = _currentTargetPoint.Value;
            _currentEnergy.Value -= _teleportationEnergyCost.Value;

            Debug.Log($"Energy: {_currentEnergy.Value}/{_maxEnergy.Value}");

            _completed.Invoke();
            _isCompleted.Value = true;
        }

        public void Update(float deltaTime)
        {
        }
    }
}
