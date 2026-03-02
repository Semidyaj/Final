using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelectors;
using Assets._Project.Develop.Runtime.Infrastructure.DI;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly StateMachineFactory _stateMachineFactory;
        private readonly AIBrainsContext _brainsContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;

            _stateMachineFactory = _container.Resolve<StateMachineFactory>();

            _brainsContext = _container.Resolve<AIBrainsContext>();
        }

        // MAIN HERO
        public StateMachineBrain CreateMainInputHeroBrain(Entity entity)
        {
            AIStateMachine stateMachine = _stateMachineFactory.CreateMainHeroMoveAndAttackStateMachine(entity);
            //AIStateMachine stateMachine = _stateMachineFactory.CreateMainInputHeroStateMachine(entity);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateMainHeroBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine rootStateMachine = _stateMachineFactory.CreateMainHeroStateMachine(entity, targetSelector);
            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        // ALLIES
        public StateMachineBrain CreateTowerBrain(Entity entity)
        {
            EmptyState emptyState = new EmptyState();

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(emptyState);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        // ENEMIES
        public StateMachineBrain CreateBarbarianBrain(Entity entity)
        {
            AIStateMachine stateMachine = _stateMachineFactory.CreateMovementToPointStateMachine(entity);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AIStateMachine stateMachine = _stateMachineFactory.CreateRandomMovementStateMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        // TELEPORTATION
        public StateMachineBrain CreateTeleportationToLeastHealthTargetBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine stateMachine = _stateMachineFactory.CreateRandomTeleportationStateMachine(entity,  targetSelector);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateRandomTeleportationBrain(Entity entity)
        {
            AIStateMachine stateMachine = _stateMachineFactory.CreateRandomTeleportationStateMachine(entity);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }
    }
}
