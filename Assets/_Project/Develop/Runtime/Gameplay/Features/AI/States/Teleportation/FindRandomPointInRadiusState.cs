using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Teleportation
{
    public class FindRandomPointInRadiusState : State, IUpdatableState
    {
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<Vector3> _currentTargetPoint;
        private Transform _transform;

        public ReactiveVariable<bool> _isTargetPointFinded;

        public FindRandomPointInRadiusState(Entity entity)
        {
            _radius = entity.TeleportationRadius;
            _currentTargetPoint = entity.TeleportationTargetPoint;
            _transform = entity.Transform;

            _isTargetPointFinded = entity.IsTeleportationTargetPointFinded;
        }

        public override void Enter()
        {
            base.Enter();

            _isTargetPointFinded.Value = false;

            do
            {
                _currentTargetPoint.Value = GetRandomRadiusPosition();
            } while (_transform.position == _currentTargetPoint.Value);

            _isTargetPointFinded.Value = true;
        }

        public void Update(float deltaTime)
        {
        }

        private Vector3 GetRandomRadiusPosition()
            => new Vector3(Random.Range(-_radius.Value, _radius.Value), 0, Random.Range(-_radius.Value, _radius.Value));
    }
}
