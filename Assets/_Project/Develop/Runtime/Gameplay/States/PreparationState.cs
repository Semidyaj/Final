using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Mining;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.PointClickExplosion;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PreparationState : State, IUpdatableState
    {
        private readonly PreperationTriggerService _triggerService;

        private readonly PointClickMiningService _miningService;
        private readonly PointClickExplosionService _explosionService;

        public PreparationState(
            PointClickExplosionService explosionService,
            PointClickMiningService miningService, 
            PreperationTriggerService triggerService)
        {
            _triggerService = triggerService;
            _miningService = miningService;
            _explosionService = explosionService;
        }

        public override void Enter()
        {
            base.Enter();

            _miningService?.Enable();
            _explosionService?.Disable();

            Vector3 nextStageTriggerPosition = Vector3.zero + Vector3.forward * 2;
            _triggerService.Create(nextStageTriggerPosition);
        }

        public void Update(float deltaTime)
        {
            _triggerService.Update(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();

            _miningService?.Disable();
            _explosionService?.Enable();

            _triggerService.Cleanup();
        }
    }
}
