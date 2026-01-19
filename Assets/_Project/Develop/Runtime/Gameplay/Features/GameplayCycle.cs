using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class GameplayCycle : IDisposable
    {
        public event Action<GameplayEndState> GameEnd;

        private GameplayProcess _gameplayProcess;

        private GameplayEndState _endState;

        public GameplayCycle(GameplayProcess gameplayProcess)
        {
            _gameplayProcess = gameplayProcess;
        }

        public void Start()
        {
            _gameplayProcess.Won += OnWon;
            _gameplayProcess.Lost += OnLost;

            _endState = GameplayEndState.None;

            _gameplayProcess.Start();
        }

        public void Update()
        {
            if (_endState == GameplayEndState.None)
                _gameplayProcess?.Update();

            if (_endState != GameplayEndState.None)
                OnGameEnded();
        }

        private void OnWon()
        {
            Debug.Log("Congratulations! You won");
            Debug.Log("Press Space to go to menu");

            _endState = GameplayEndState.Victory;
        }

        private void OnLost()
        {
            Debug.Log("Oops! You lost");
            Debug.Log("Press Space to try again");

            _endState = GameplayEndState.Defeat;
        }

        private void OnGameEnded()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GameEnd?.Invoke(_endState);
        }

        public void Dispose()
        {
            _gameplayProcess.Won -= OnWon;
            _gameplayProcess.Lost -= OnLost;
        }
    }
}
