using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AOE
{
    public class AOESystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<float> _delay;
        private ReactiveVariable<float> _currentTime;

        private Buffer<Collider> _contacts;
        private Buffer<Entity> _contactsEntities;
        private LayerMask _damageMask;
        private CapsuleCollider _body;

        private Transform _transform;

        private ReactiveEvent _teleportationEndEvent;

        private bool _isTeleportationEnded;

        private IDisposable _teleportationEndEventDisposable;

        private readonly CollidersRegistryService _collidersRegistryService;

        public AOESystem(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
        }

        public void OnInit(Entity entity)
        {
            _damage = entity.AOEDamage;
            _radius = entity.AOERadius;
            _delay = entity.AOEDelayBeforeTakeDamage;
            _currentTime = entity.AOEDelayCurrentTimer;

            _contacts = entity.AOECollidersBuffer;
            _contactsEntities = entity.AOEEntitiesBuffer;
            _damageMask = entity.AOETakeDamageMask;
            _body = entity.BodyCollider;

            _transform = entity.Transform;

            _teleportationEndEvent = entity.TeleportationEvent;

            _teleportationEndEventDisposable = _teleportationEndEvent.Subscribe(OnTeleportationEnded);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isTeleportationEnded == false)
                return;

            _currentTime.Value -= deltaTime;

            if (_currentTime.Value <= 0)
            {
                AOEAttackProcess();

                _isTeleportationEnded = false;
            }
        }

        public void OnDispose()
        {
            _teleportationEndEventDisposable.Dispose();
        }

        private void OnTeleportationEnded()
        {
            _currentTime.Value = _delay.Value;
            _isTeleportationEnded = true;
        }

        private void AOEAttackProcess()
        {
            GetAOEAttackContacts();

            for (int i = 0; i < _contactsEntities.Count; i++)
                if (_contactsEntities.Items[i].HasComponent<TakeDamageRequest>())
                    _contactsEntities.Items[i].TakeDamageRequest.Invoke(_damage.Value);
        }

        private void GetAOEAttackContacts()
        {
            _contacts.Count = Physics.OverlapSphereNonAlloc(
                _transform.position,
                _radius.Value,
                _contacts.Items,
                _damageMask,
                QueryTriggerInteraction.Ignore);

            RemoveSelfFromContacts();

            _contactsEntities.Count = 0;

            for (int i = 0; i < _contacts.Count; i++)
            {
                Collider collider = _contacts.Items[i];

                Entity contactEntity = _collidersRegistryService.GetBy(collider);

                if (contactEntity != null)
                {
                    _contactsEntities.Items[_contactsEntities.Count] = contactEntity;
                    _contactsEntities.Count++;
                }
            }
        }

        private void RemoveSelfFromContacts()
        {
            int indexToRemove = -1;

            for (int i = 0; i < _contacts.Count; i++)
            {
                if (_contacts.Items[i] == _body)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                for (int i = indexToRemove; i < _contacts.Count - 1; i++)
                {
                    _contacts.Items[i] = _contacts.Items[i + 1];
                }

                _contacts.Count--;
            }
        }
    }
}
