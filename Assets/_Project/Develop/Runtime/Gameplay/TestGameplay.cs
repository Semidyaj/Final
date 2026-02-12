using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;

        private Entity _ghost;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            _ghost = _entitiesFactory.CreateGhostEntity(Vector3.zero);
            _entitiesFactory.CreateGhostEntity(Vector3.zero + Vector3.forward * 5);

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                _ghost.TakeDamageRequest.Invoke(50);

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            _ghost.MoveDirection.Value = input;
            _ghost.RotationDirection.Value = input;
        }
    }
}
