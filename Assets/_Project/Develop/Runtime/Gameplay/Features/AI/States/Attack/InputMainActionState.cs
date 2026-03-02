using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Attack
{
    public class InputMainActionState : State, IUpdatableState
    {
        private ReactiveEvent _findPointToActionRequest;

        private ReactiveVariable<bool> _isAttackEnded;
        private ReactiveVariable<bool> _isMinePlaced;

        public InputMainActionState(Entity entity)
        {
            _findPointToActionRequest = entity.InputFindMouseClickPositionRequest;

            _isAttackEnded = entity.IsAOEAttackEnded;
            _isMinePlaced = entity.MineIsPlaced;
        }

        public override void Enter()
        {
            base.Enter();

            _isAttackEnded.Value = false;
            _isMinePlaced.Value = false;

            _findPointToActionRequest?.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}
