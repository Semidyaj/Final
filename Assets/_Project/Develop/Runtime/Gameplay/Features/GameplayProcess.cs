using Assets._Project.Develop.Runtime.Configs.Gameplay;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class GameplayProcess
    {
        public event Action Won;
        public event Action Lost;
        
        private readonly SymbolGameplayConfig _config;

        private string _guessedString;
        private string _resultString;

        public GameplayProcess(SymbolGameplayConfig config)
        {
            _config = config;
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
                Won?.Invoke();

            if (IsInputCompareGuessed() == false)
                Lost?.Invoke();
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
