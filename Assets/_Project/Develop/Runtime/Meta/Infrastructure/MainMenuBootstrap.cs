using Assets._Project.Develop.Runtime.Gameplay.Features;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManager;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private GameModeChooseService _gameModeChooseService;

        private WalletService _walletService;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _gameModeChooseService = _container.Resolve<GameModeChooseService>();

            _gameModeChooseService.GameModeChosen += OnModeChosen;

            _walletService = _container.Resolve<WalletService>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Select gameplay mode: 1 - numbers, 2 - letters");
        }

        private void Update()
        {
            _gameModeChooseService?.Update();

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _walletService.Add(CurrencyTypes.Gold, 10);
                Debug.Log("Gold wallet: " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (_walletService.Enough(CurrencyTypes.Gold, 10))
                {
                    _walletService.Spend(CurrencyTypes.Gold, 10);
                    Debug.Log("Gold wallet: " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
                }
            }
        }

        private void OnModeChosen(GameplayTypes type)
        {
            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(type)));
        }

        private void OnDestroy()
        {
            _gameModeChooseService.GameModeChosen -= OnModeChosen;
        }
    }
}
