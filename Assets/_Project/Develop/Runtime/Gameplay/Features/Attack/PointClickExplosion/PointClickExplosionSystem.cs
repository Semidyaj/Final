using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AOE;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.PointClickExplosion
{
    public class PointClickExplosionSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<Vector3> _raycastClickPosition;
        private ReactiveEvent _positionFoundEvent;

        private ReactiveVariable<float> _damage;
        private ReactiveVariable<float> _radius;
        private LayerMask _damageMask;

        private ReactiveEvent<AOEInfoStruct> _explosionEvent;

        private IDisposable _positionFoundDisposable;

        public void OnInit(Entity entity)
        {
            _raycastClickPosition = entity.InputMouseClickGroundPosition;
            _positionFoundEvent = entity.InputMouseClickPositionFindedEvent;

            _damage = entity.AOEDamage;
            _radius = entity.AOERadius;
            _damageMask = entity.AOETakeDamageMask;

            _explosionEvent = entity.AOEAttackRequest;

            _positionFoundDisposable = _positionFoundEvent.Subscribe(OnPositionFinded);
        }

        public void OnDispose()
        {
            _positionFoundDisposable.Dispose();
        }

        private void OnPositionFinded()
        {
            AOEInfoStruct explosionInfo = new AOEInfoStruct(_damage.Value, _radius.Value, _raycastClickPosition.Value, _damageMask);

            _explosionEvent?.Invoke(explosionInfo);
        }
    }
}
