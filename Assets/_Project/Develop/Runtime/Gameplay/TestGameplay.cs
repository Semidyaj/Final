using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        private MainHeroFactory _mainHeroFactory;
        private EnemiesFactory _enemiesFactory;

        private Entity _hero;
        private Entity _ghostAI;

        [SerializeField] private GhostConfig _ghostConfig;
        [SerializeField] private HeroConfig _heroConfig;
        [SerializeField] private HomeworkHeroConfig _homeworkHeroConfig;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();

            _mainHeroFactory = _container.Resolve<MainHeroFactory>();
            _enemiesFactory = _container.Resolve<EnemiesFactory>();
        }

        public void Run()
        {
            _hero = _mainHeroFactory.Create(Vector3.zero);

            //_hero = _mainHeroFactory.CreateHomeworkHero(Vector3.zero);

            _enemiesFactory.Create(Vector3.zero + Vector3.forward * 5, _ghostConfig);
            _enemiesFactory.Create(Vector3.zero - Vector3.forward * 5, _ghostConfig);
            _enemiesFactory.Create(Vector3.zero + Vector3.right * 5, _ghostConfig);
            _enemiesFactory.Create(Vector3.zero - Vector3.right * 5, _ghostConfig);

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;
        }
    }
}
