using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelectors;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        private Entity _hero;
        private Entity _ghostAI;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
        }

        public void Run()
        {
            _hero = _entitiesFactory.CreateHeroEntity(Vector3.zero);
            _hero.AddCurrentTarget();
            _brainsFactory.CreateMainHeroBrain(_hero, new NearestDamagableTargetSelector(_hero));

            _ghostAI = _entitiesFactory.CreateGhostEntity(Vector3.zero + Vector3.forward * 5);

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                _hero.TakeDamageRequest.Invoke(50);

            if (Input.GetKeyDown(KeyCode.R))
                _hero.StartAttackRequest.Invoke();

            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    _hero.TeleportationRequest.Invoke();
            //    Debug.Log($"Current energy {_hero.CurrentEnergy.Value}");
            //}

            if (Input.GetKeyDown(KeyCode.I))
                _brainsFactory.CreateGhostBrain(_ghostAI);
        }
    }
}
