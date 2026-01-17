using Assets._Project.Develop.Runtime.Gameplay.Configs;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class GameplayProcess
    {
        public event Action Won;
        public event Action Lost;

        private DIContainer _container;
        private SymbolGameplayConfig _config;

        private string _guessedString;
        private string _resultString;

        public GameplayProcess(DIContainer container)
        {
            _container = container;

            _config = _container.Resolve<SymbolGameplayConfig>();
        }

        public void Start()
        {
            _guessedString = GenerateString();

            Debug.Log($"Please repeat string \"{_guessedString}\"");
        }

        public void Update()
        {
            _resultString += GetInputKeyboardText();

            if (_resultString.Length == _guessedString.Length && IsInputCompareGuessed())
            {
                Won?.Invoke();
                return;
            }

            if (IsInputCompareGuessed() == false)
            {
                Lost?.Invoke();
                return;
            }
        }

        private string GenerateString()
        {
            string result = "";

            for (int i = 0; i < _config.SequenceLength; i++)
                result += _config.AllowedSymbols[Random.Range(0, _config.AllowedSymbols.Length)];

            return result;
        }

        private string GetInputKeyboardText()
        {
            string resultForFrame = "";

            foreach (char enteredSymbol in Input.inputString)
                resultForFrame += enteredSymbol;

            return resultForFrame;
        }

        private bool IsInputCompareGuessed()
        {
            for (int i = 0; i < _resultString.Length; i++)
                if (_resultString[i] != _guessedString[i])
                    return false;

            return true;
        }
    }
}
