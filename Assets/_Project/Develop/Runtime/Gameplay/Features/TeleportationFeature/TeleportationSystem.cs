using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportationFeature
{
    public class TeleportationSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<bool> _inCooldown;

        private ReactiveEvent _request;
        private ReactiveEvent _event;

        private ReactiveVariable<float> _radius;

        private ReactiveVariable<float> _energyCost;
        private ReactiveVariable<float> _currentEnergy;

        private Transform _transform;

        private ICompositeCondition _canUseTeleport;

        private IDisposable _requestDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.TeleportationCooldownCurrentTime;
            _initialTime = entity.TeleportationCooldownInitialTime;
            _inCooldown = entity.InTeleportationCooldown;

            _request = entity.TeleportationRequest;
            _event = entity.TeleportationEvent;

            _radius = entity.TeleportationRadius;

            _energyCost = entity.TeleportationEnergyCost;
            _currentEnergy = entity.CurrentEnergy;

            _transform = entity.Transform;

            _canUseTeleport = entity.CanUseTeleport;

            _requestDisposable = _request.Subscribe(OnTeleportationRequest);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inCooldown.Value == false)
                return;

            _currentTime.Value -= deltaTime;

            if (CooldownIsOver())
                _inCooldown.Value = false;
        }

        public void OnDispose()
        {
            _requestDisposable.Dispose();
        }

        private void OnTeleportationRequest()
        {
            if (_canUseTeleport.Evaluate())
            {
                _transform.position = GetRandomRadiusPosition();

                _event.Invoke();
                _currentEnergy.Value -= _energyCost.Value;

                _currentTime.Value = _initialTime.Value;
                _inCooldown.Value = true;
            }
        }

        private Vector3 GetRandomRadiusPosition()
            => new Vector3(Random.Range(-_radius.Value, _radius.Value), 0, Random.Range(-_radius.Value, _radius.Value));

        private bool CooldownIsOver() => _currentTime.Value <= 0;
    }
}
