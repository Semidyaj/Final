using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Attack
{
    public class InputAttackTriggerState : State, IUpdatableState
    {
        private ReactiveEvent _findPointToAttackRequest;

        private ReactiveVariable<bool> _isAttackEnded;

        public InputAttackTriggerState(Entity entity)
        {
            _findPointToAttackRequest = entity.InputFindMouseClickPositionRequest;

            _isAttackEnded = entity.IsAOEAttackEnded;
        }

        public override void Enter()
        {
            base.Enter();

            _isAttackEnded.Value = false;

            _findPointToAttackRequest?.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}
