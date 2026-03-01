using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Mining
{
    public class PointClickMiningSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly EntitiesFactory _entitiesFactory;
        private readonly WalletService _walletService;

        private Entity _entity;

        private ReactiveVariable<Vector3> _raycastClickPosition;
        private ReactiveEvent _positionFoundEvent;

        private IDisposable _positionFoundDisposable;

        public PointClickMiningSystem(EntitiesFactory entitiesFactory, WalletService walletService)
        {
            _entitiesFactory = entitiesFactory;
            _walletService = walletService;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _raycastClickPosition = entity.InputMouseClickGroundPosition;
            _positionFoundEvent = entity.InputMouseClickPositionFindedEvent;

            _positionFoundDisposable = _positionFoundEvent.Subscribe(OnPositionFinded);
        }

        public void OnDispose()
        {
            _positionFoundDisposable.Dispose();
        }

        private void OnPositionFinded()
        {
            _walletService.Spend(CurrencyTypes.Gold, 20);

            _entity.MineIsPlaced.Value = true;

            _entity.InputIsPositionFound.Value = false;

            _entitiesFactory.CreateMine(_raycastClickPosition.Value, _entity);
        }
    }
}
