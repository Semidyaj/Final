using Assets._Project.Develop.Runtime.Configs.Gameplay;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class GameMode
    {
        public event Action Won;
        public event Action Lost;
        
        private readonly SymbolGameplayConfig _config;

        private InputHandler _inputHandler;
        private SequenceComparer _sequenceComparer;

        private string _guessedString;
        private string _resultString;

        public GameMode(SymbolGameplayConfig config, InputHandler inputHandler, SequenceComparer sequenceComparer)
        {
            _config = config;
            _inputHandler = inputHandler;
            _sequenceComparer = sequenceComparer;
        }

        public void Start()
        {
            _guessedString = GenerateString();

            Debug.Log($"Please repeat string \"{_guessedString}\"");
        }

        public void Update()
        {
            _resultString += _inputHandler.GetInputKeyboardText();

            if (_resultString.Length == _guessedString.Length && _sequenceComparer.IsInputCompareGuessed(_resultString, _guessedString))
                Won?.Invoke();

            if (_sequenceComparer.IsInputCompareGuessed(_resultString, _guessedString) == false)
                Lost?.Invoke();
        }

        private string GenerateString()
        {
            string result = "";

            for (int i = 0; i < _config.SequenceLength; i++)
                result += _config.AllowedSymbols[Random.Range(0, _config.AllowedSymbols.Length)];

            return result;
        }
    }
}
