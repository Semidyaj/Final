using Assets._Project.Develop.Runtime.Configs.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
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

        private ReactiveVariable<string> _guessedString = new();
        private ReactiveVariable<string> _resultString = new();

        public GameMode(SymbolGameplayConfig config, InputHandler inputHandler, SequenceComparer sequenceComparer)
        {
            _config = config;
            _inputHandler = inputHandler;
            _sequenceComparer = sequenceComparer;
        }

        public IReadOnlyVariable<string> GuessedString => _guessedString;
        public IReadOnlyVariable<string> ResultString => _resultString;

        public void Start()
        {
            _guessedString.Value = GenerateString();
            
            Debug.Log($"Please repeat string \"{_guessedString.Value}\"");
        }

        public void Update()
        {
            _resultString.Value += _inputHandler.GetInputKeyboardText();

            if (_resultString.Value.Length == _guessedString.Value.Length && _sequenceComparer.IsInputCompareGuessed(_resultString, _guessedString))
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
