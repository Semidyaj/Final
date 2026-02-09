using Assets._Project.Develop.Runtime.Gameplay.Common;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class RigidbodyMovementSystem : IUpdatableSystem, IInitializableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _moveSpeed;

        private ReactiveVariable<Vector3> _rotateDirection;
        private ReactiveVariable<float> _rotateSpeed;

        private Rigidbody _rigidbody;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _moveSpeed = entity.MoveSpeed;

            _rotateDirection = entity.RotateDirection;
            _rotateSpeed = entity.RotateSpeed;

            _rigidbody = entity.Rigidbody;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 velocity = _moveDirection.Value.normalized * _moveSpeed.Value;

            _rigidbody.velocity = velocity;

            Quaternion targetRotation = Quaternion.LookRotation(_rotateDirection.Value.normalized, Vector3.up);

            float step = _rotateSpeed.Value * deltaTime;

            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, step));
        }
    }
}
