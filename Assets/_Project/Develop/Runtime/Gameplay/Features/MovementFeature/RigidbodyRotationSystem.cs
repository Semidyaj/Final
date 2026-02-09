using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class RigidbodyRotationSystem : IUpdatableSystem, IInitializableSystem
    {
        private ReactiveVariable<Vector3> _rotateDirection;
        private ReactiveVariable<float> _rotateSpeed;

        private Rigidbody _rigidbody;

        private ICompositeCondition _canRotate;

        public void OnInit(Entity entity)
        {
            _rotateDirection = entity.RotationDirection;
            _rotateSpeed = entity.RotationSpeed;

            _rigidbody = entity.Rigidbody;

            _canRotate = entity.CanRotate;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canRotate.Evaluate() == false)
                return;

            if (_rotateDirection.Value == Vector3.zero)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(_rotateDirection.Value.normalized, Vector3.up);

            float step = _rotateSpeed.Value * deltaTime;

            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, step));
        }
    }
}
