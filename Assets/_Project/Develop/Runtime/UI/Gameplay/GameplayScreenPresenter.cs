using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.Experience;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _screen;
        private readonly GameplayPresentersFactory _presentersFactory;

        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;

        private readonly List<IPresenter> _childPresenters = new();

        public GameplayScreenPresenter(GameplayScreenView screen, GameplayPresentersFactory presentersFactory)
        {
            _screen = screen;
            _presentersFactory = presentersFactory;
        }

        public void Initialize()
        {
            CreateStageNumber();
            CreateEntitiesHealthDisplay();
            CreateExpirienceView();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void LateUpdate()
        {
            _entitiesHealthDisplayPresenter.LateUpdate();
        }

        public void Dispose()
        {
            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateStageNumber()
        {
            StagePresenter stagePresenter = _presentersFactory.CreateStagePresenter(_screen.StageNumberView);

            _childPresenters.Add(stagePresenter);
        }

        private void CreateEntitiesHealthDisplay()
        {
            _entitiesHealthDisplayPresenter = _presentersFactory.CreateEntitiesHealthDisplayPresenter(_screen.EntitiesHealthDisplay);

            _childPresenters.Add(_entitiesHealthDisplayPresenter);
        }

        private void CreateExpirienceView()
        {
            MainHeroExperiencePresenter experiencePresenter = _presentersFactory.CreateMainHeroExperiencePresenter(_screen.ExperienceBarView);

            _childPresenters.Add(experiencePresenter);
        }
    }
}
