using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;

        private Entity _entity1;
        private Entity _entity2;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            _entity1 = _entitiesFactory.CreateTestRigidbodyEntity(new Vector3(-2, 0, 0));
            _entity2 = _entitiesFactory.CreateTestCharacterControllerEntity(new Vector3(2, 0, 0));

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            _entity1.MoveDirection.Value = input;
            _entity1.RotateDirection.Value = input;

            _entity2.MoveDirection.Value = input;
            _entity2.RotateDirection.Value = input;
        }
    }
}
