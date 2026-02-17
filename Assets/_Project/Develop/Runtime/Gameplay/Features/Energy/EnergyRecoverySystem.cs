using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergyRecoverySystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _currentEnergy;

        private ReactiveVariable<float> _period;
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _percent;

        private ICompositeCondition _canRecoverEnergy;

        public void OnInit(Entity entity)
        {
            _maxEnergy = entity.MaxEnergy;
            _currentEnergy = entity.CurrentEnergy;

            _period = entity.EnergyRecoveryTimePeriod;
            _currentTime = entity.EnergyRecoveryCurrentTime;
            _percent = entity.PercentToRecovery;

            _canRecoverEnergy = entity.CanRecoverEnergy;

            _currentTime.Value = _period.Value;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canRecoverEnergy.Evaluate() == false)
                return;

            _currentTime.Value -= deltaTime;

            if(_currentTime.Value <= 0)
            {
                _currentEnergy.Value = MathF.Min(_currentEnergy.Value + _maxEnergy.Value * _percent.Value, _maxEnergy.Value);
                _currentTime.Value = _period.Value;

                Debug.Log($"Energy recovered. Current value is {_currentEnergy.Value}");
            }
        }
    }
}
