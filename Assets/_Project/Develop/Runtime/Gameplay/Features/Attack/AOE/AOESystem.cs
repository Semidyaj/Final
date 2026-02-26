using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AOE
{
    public class AOESystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<float> _radius;

        private Buffer<Collider> _contacts;
        private Buffer<Entity> _contactsEntities;
        private LayerMask _damageMask;
        private CapsuleCollider _body;

        private Transform _transform;

        private ReactiveEvent _attackRequest;
        private ReactiveEvent _attackEvent;
        private ReactiveVariable<bool> _isAttackEnded;

        private IDisposable _attackRequestDisposable;

        private readonly CollidersRegistryService _collidersRegistryService;

        public AOESystem(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
        }

        public void OnInit(Entity entity)
        {
            _damage = entity.AOEDamage;
            _radius = entity.AOERadius;

            _contacts = entity.AOECollidersBuffer;
            _contactsEntities = entity.AOEEntitiesBuffer;
            _damageMask = entity.AOETakeDamageMask;
            _body = entity.BodyCollider;

            _transform = entity.Transform;

            _attackRequest = entity.AOEAttackRequest;
            _attackEvent = entity.AOEAttackEvent;
            _isAttackEnded = entity.IsAOEAttackEnded;

            _attackRequestDisposable = _attackRequest.Subscribe(OnDealDamage);
        }

        public void OnDispose()
        {
            _attackRequestDisposable.Dispose();
        }

        private void OnDealDamage()
        {
            AOEAttackProcess();

            _isAttackEnded.Value = true;

            _attackEvent?.Invoke();
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
