using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Attack
{
    public class InputMiningState : State, IUpdatableState
    {
        private ReactiveEvent _findPointToMiningRequest;

        private ReactiveVariable<bool> _isMinePlaced;

        public InputMiningState(Entity entity)
        {
            _findPointToMiningRequest = entity.InputFindMouseClickPositionRequest;

            _isMinePlaced = entity.MineIsPlaced;
        }

        public override void Enter()
        {
            base.Enter();

            _isMinePlaced.Value = false;

            Debug.Log("Enter mining");

            _findPointToMiningRequest?.Invoke();
        }

        public void Update(float deltaTime)
        {
        }

        public override void Exit()
        {
            base.Exit();

            Debug.Log("Exit mining");
        }
    }
}
