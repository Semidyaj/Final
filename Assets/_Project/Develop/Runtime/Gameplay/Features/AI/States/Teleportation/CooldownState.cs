using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Teleportation
{
    public class CooldownState : State, IUpdatableState
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<bool> _isCompleted;

        public CooldownState(Entity entity)
        {
            _currentTime = entity.TeleportationCooldownCurrentTime;
            _isCompleted = entity.IsTeleportationCompleted;
        }

        public override void Enter()
        {
            base.Enter();

            _isCompleted.Value = false;

            _currentTime.Value = 0;
        }

        public void Update(float deltaTime)
        {
            _currentTime.Value += deltaTime;
        }
    }
}
