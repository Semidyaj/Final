using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Mining
{
    public class PointClickMiningService
    {
        private readonly EntitiesFactory _entitiesFactory;
        private readonly WalletService _walletService;
        private readonly MainHeroHolderService _mainHeroHolderService;

        private Entity _entity;

        private ReactiveVariable<Vector3> _raycastClickPosition;
        private ReactiveEvent _positionFoundEvent;

        private IDisposable _positionFoundDisposable;

        private ReactiveVariable<bool> _isEnoughGold;
        private ReactiveVariable<bool> _isMinePlaced;
        private ReactiveVariable<bool> _isEnabled = new();

        public PointClickMiningService(EntitiesFactory entitiesFactory, WalletService walletService, MainHeroHolderService mainHeroHolderService)
        {
            _entitiesFactory = entitiesFactory;
            _walletService = walletService;
            _mainHeroHolderService = mainHeroHolderService;

            _entity = _mainHeroHolderService.MainHero;

            _raycastClickPosition = _entity.InputMouseClickGroundPosition;
            _positionFoundEvent = _entity.InputMouseClickPositionFindedEvent;

            _isEnoughGold = _entity.MineIsEnoughGold;
            _isMinePlaced = _entity.MineIsPlaced;
        }

        public IReadOnlyVariable<bool> IsEnabled => _isEnabled;

        public void Enable()
        {
            _positionFoundDisposable = _positionFoundEvent.Subscribe(OnPositionFinded);

            _isEnabled.Value = true;
        }

        public void Disable()
        {
            _positionFoundDisposable.Dispose();

            _isEnabled.Value = false;
        }

        private void OnPositionFinded()
        {
            if (_isEnabled.Value == false)
                return;

            if (IsEnoughGold() == false)
                return;

            _walletService.Spend(CurrencyTypes.Gold, 20);

            _isMinePlaced.Value = true;

            _entitiesFactory.CreateMine(_raycastClickPosition.Value, _entity);
        }

        private bool IsEnoughGold()
        {
            if (_walletService.Enough(CurrencyTypes.Gold, 20))
            {
                _isEnoughGold.Value = true;
                return true;
            }
           
            _isEnoughGold.Value = false;
            return false;
        }
    }
}
