using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelectors;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MainHero
{
    public class MainHeroFactory
    {
        private readonly DIContainer _container;

        private readonly EntitiesFactory _entitiesFactory;
        private readonly BrainsFactory _brainsFactory;
        private readonly ConfigsProviderService _configProviderService;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public MainHeroFactory(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
            _configProviderService = _container.Resolve<ConfigsProviderService>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
        }

        public Entity Create(Vector3 position)
        {
            HeroConfig config = _configProviderService.GetConfig<HeroConfig>();

            Entity entity = _entitiesFactory.CreateHeroTowerDefenderEntity(position, config);
            //Entity entity = _entitiesFactory.CreateHeroEntity(position, config);

            entity
                .AddIsMainHero()
                .AddTeam(new ReactiveVariable<Teams>(Teams.MainHero));

            //entity.AddCurrentTarget();
            //_brainsFactory.CreateMainHeroBrain(entity, new NearestDamagableTargetSelector(entity));
            _brainsFactory.CreateMainInputHeroBrain(entity);

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateHomeworkHero(Vector3 position)
        {
            HomeworkHeroConfig config = _configProviderService.GetConfig<HomeworkHeroConfig>();

            Entity entity = _entitiesFactory.CreateHomeworkHero(position, config);

            entity
                .AddIsMainHero()
                .AddTeam(new ReactiveVariable<Teams>(Teams.MainHero));

            _brainsFactory.CreateRandomTeleportationBrain(entity);
            //_brainsFactory.CreateTeleportationToLeastHealthTargetBrain(entity, new FindEnemyWithLeastHealthSelector(entity));

            _entitiesLifeContext.Add(entity);

            return entity;
        }
    }
}
