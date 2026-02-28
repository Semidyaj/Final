using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.PointClickExplosion
{
    public class PointClickExplosionSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<Vector3> _raycastClickPosition;
        private ReactiveEvent _positionFoundEvent;

        private ReactiveEvent<Vector3> _explosionEvent;

        private IDisposable _positionFoundDisposable;

        public void OnInit(Entity entity)
        {
            _raycastClickPosition = entity.InputMouseClickGroundPosition;
            _positionFoundEvent = entity.InputMouseClickPositionFindedEvent;

            _explosionEvent = entity.AOEAttackRequest;

            _positionFoundDisposable = _positionFoundEvent.Subscribe(OnPositionFinded);
        }

        public void OnDispose()
        {
            _positionFoundDisposable.Dispose();
        }

        private void OnPositionFinded()
        {
            _explosionEvent?.Invoke(_raycastClickPosition.Value);
        }
    }
}
