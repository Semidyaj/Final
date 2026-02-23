using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Teleportation;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelectors;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly AIBrainsContext _brainsContext;
        private readonly IInputService _inputService;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;

            _timerServiceFactory = _container.Resolve<TimerServiceFactory>();
            _brainsContext = _container.Resolve<AIBrainsContext>();
            _inputService = _container.Resolve<IInputService>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();

        }

        public StateMachineBrain CreateTeleportationToLeastHealthTargetBrain(Entity entity, ITargetSelector targetSelector)
        {
            CooldownState cooldownState = new CooldownState(entity);
            FindTargetWithLeastHealthState findTargetState = new FindTargetWithLeastHealthState(targetSelector, _entitiesLifeContext, entity);
            TeleportationProcessState teleportationState = new TeleportationProcessState(entity);

            ICompositeCondition fromCooldownToFindTargetPointCondition = new CompositeCondition()
                .Add(new FuncCondition(() => entity.TeleportationCooldownCurrentTime.Value >= entity.TeleportationCooldownInitialTime.Value))
                .Add(new FuncCondition(() => entity.CurrentEnergy.Value >= entity.TeleportationEnergyCost.Value))
                .Add(new FuncCondition(() => entity.CurrentEnergy.Value >= entity.TeleportationEnergyCost.Value * 0.4f));

            FuncCondition targetPointFound = new FuncCondition(() => entity.CurrentTarget.Value != null);
            FuncCondition teleportEnded = new FuncCondition(() => entity.IsTeleportationCompleted.Value);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(cooldownState);
            stateMachine.AddState(findTargetState);
            stateMachine.AddState(teleportationState);

            stateMachine.AddTransition(cooldownState, findTargetState, fromCooldownToFindTargetPointCondition);
            stateMachine.AddTransition(findTargetState, teleportationState, targetPointFound);
            stateMachine.AddTransition(teleportationState, cooldownState, teleportEnded);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateRandomTeleportationBrain(Entity entity)
        {
            CooldownState cooldownState = new CooldownState(entity);
            FindRandomPointInRadiusState findPointState = new FindRandomPointInRadiusState(entity);
            TeleportationProcessState teleportationState = new TeleportationProcessState(entity);

            ICompositeCondition fromCooldownToFindTargetPointCondition = new CompositeCondition()
                .Add(new FuncCondition(() => entity.TeleportationCooldownCurrentTime.Value >= entity.TeleportationCooldownInitialTime.Value))
                .Add(new FuncCondition(() => entity.CurrentEnergy.Value >= entity.TeleportationEnergyCost.Value));

            FuncCondition targetPointFound = new FuncCondition(() => entity.IsTeleportationTargetPointFinded.Value);
            FuncCondition teleportEnded = new FuncCondition(() => entity.IsTeleportationCompleted.Value);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(cooldownState);
            stateMachine.AddState(findPointState);
            stateMachine.AddState(teleportationState);

            stateMachine.AddTransition(cooldownState, findPointState, fromCooldownToFindTargetPointCondition);
            stateMachine.AddTransition(findPointState, teleportationState, targetPointFound);
            stateMachine.AddTransition(teleportationState, cooldownState, teleportEnded);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateRandomTeleportationHeroBrain(Entity entity)
        {
            CooldownState cooldownState = new CooldownState(entity);
            AIStateMachine teleportationState = CreateRandomTeleportationStateMachine(entity);

            ICompositeCondition fromCooldownToTeleportationCondition = new CompositeCondition()
                .Add(new FuncCondition(() => entity.TeleportationCooldownCurrentTime.Value >= entity.TeleportationCooldownInitialTime.Value))
                .Add(new FuncCondition(() => entity.CurrentEnergy.Value >= entity.TeleportationEnergyCost.Value));

            FuncCondition fromTeleportationToCooldownCondition = new FuncCondition(() 
                => entity.IsTeleportationCompleted.Value);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(cooldownState);
            stateMachine.AddState(teleportationState);

            stateMachine.AddTransition(cooldownState, teleportationState, fromCooldownToTeleportationCondition);
            stateMachine.AddTransition(teleportationState, cooldownState, fromTeleportationToCooldownCondition);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public AIStateMachine CreateRandomTeleportationStateMachine(Entity entity)
        {
            FindRandomPointInRadiusState findPointState = new FindRandomPointInRadiusState(entity);
            TeleportationProcessState teleportationState = new TeleportationProcessState(entity);

            FuncCondition targetPointFound = new FuncCondition(() => entity.IsTeleportationTargetPointFinded.Value);
            //FuncCondition teleportEnded = new FuncCondition(() => entity.IsTeleportationCompleted.Value);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(findPointState);
            stateMachine.AddState(teleportationState);

            stateMachine.AddTransition(findPointState, teleportationState, targetPointFound);
            //stateMachine.AddTransition(teleportationState, findPointState, teleportEnded);

            return stateMachine;
        }

        public StateMachineBrain CreateMainInputHeroBrain(Entity entity)
        {
            AIStateMachine combatState = CreateInputAttackStateMachine(entity);
            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            FuncCondition fromMovementToCombatCondition = new FuncCondition(() => _inputService.Direction == Vector3.zero);
            FuncCondition fromCombatToMovementCondition = new FuncCondition(() => _inputService.Direction != Vector3.zero);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(combatState);
            stateMachine.AddState(movementState);

            stateMachine.AddTransition(movementState, combatState, fromMovementToCombatCondition);
            stateMachine.AddTransition(combatState, movementState, fromCombatToMovementCondition);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public AIStateMachine CreateInputAttackStateMachine(Entity entity)
        {
            PlayerInputRotationState rotationState = new PlayerInputRotationState(entity, _inputService);
            AttackTriggerState attackState = new AttackTriggerState(entity);

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(entity.CanStartAttack)
                .Add(new FuncCondition(() => _inputService.LeftMouseButtonClicked));

            FuncCondition fromAttackToRotateCondition = new FuncCondition(() => entity.InAttackProcess.Value == false);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(rotationState);
            stateMachine.AddState(attackState);

            stateMachine.AddTransition(rotationState, attackState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackState, rotationState, fromAttackToRotateCondition);

            return stateMachine;
        }


        public StateMachineBrain CreateMainHeroBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);
            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromMovementToCombatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null))
                .Add(new FuncCondition(() => _inputService.Direction == Vector3.zero));

            ICompositeCondition fromCombatToMovementStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => currentTarget.Value == null))
                .Add(new FuncCondition(() => _inputService.Direction != Vector3.zero));

            AIStateMachine behaviour = new AIStateMachine();

            behaviour.AddState(movementState);
            behaviour.AddState(combatState);

            behaviour.AddTransition(movementState, combatState, fromMovementToCombatStateCondition);
            behaviour.AddTransition(combatState, movementState, fromCombatToMovementStateCondition);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            AIParallelState parallelState = new AIParallelState(findTargetState, behaviour);

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AIStateMachine stateMachine = CreateRandomMovementStateMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateRandomMovementStateMachine(Entity entity)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            RandomMovementState randomMovementState = new RandomMovementState(entity, 0.5f);
            EmptyState emptyState = new EmptyState();

            TimerService movementTimer = _timerServiceFactory.Create(2f);
            disposables.Add(movementTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(movementTimer.Restart));

            TimerService idleTimer = _timerServiceFactory.Create(3f);
            disposables.Add(idleTimer);
            disposables.Add(emptyState.Entered.Subscribe(idleTimer.Restart));

            FuncCondition movementTimerEndedCondition = new FuncCondition(() => movementTimer.IsOver);
            FuncCondition idleTimerEndedCondition = new FuncCondition(() => idleTimer.IsOver);

            AIStateMachine aIStateMachine = new AIStateMachine(disposables);

            aIStateMachine.AddState(randomMovementState);
            aIStateMachine.AddState(emptyState);

            aIStateMachine.AddTransition(randomMovementState, emptyState, movementTimerEndedCondition);
            aIStateMachine.AddTransition(emptyState, randomMovementState, idleTimerEndedCondition);

            return aIStateMachine;
        }

        private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);
            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;
            Transform transform = entity.Transform;
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    Entity target = currentTarget.Value;

                    if (target == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Transform.position - transform.position));

                    return angleToTarget < 1f;
                }));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICondition fromAttackToRotateStateCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateStateCondition);

            return stateMachine;
        }
    }
}
