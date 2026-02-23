//using Assets._Project.Develop.Runtime.Gameplay.Features;
//using Assets._Project.Develop.Runtime.UI.Core;
//using Assets._Project.Develop.Runtime.Utilities.Reactive;
//using System;

//namespace Assets._Project.Develop.Runtime.UI.Gameplay
//{
//    public class GameplayScreenPresenter : IPresenter
//    {
//        private readonly GameplayScreenView _screen;

//        private readonly GameMode _gameMode;

//        private readonly IReadOnlyVariable<string> _guessedString;
//        private readonly IReadOnlyVariable<string> _resultString;

//        private IDisposable _disposableGuessed;
//        private IDisposable _disposableInput;

//        public GameplayScreenPresenter(GameplayScreenView screen, GameMode gameMode)
//        {
//            _screen = screen;
//            _gameMode = gameMode;

//            _guessedString = _gameMode.GuessedString;
//            _resultString = _gameMode.ResultString;
//        }

//        public void Initialize()
//        {
//            _disposableGuessed = _guessedString.Subscribe(OnTextGenerated);
//            _disposableInput = _resultString.Subscribe(OnInputTextChanged);
//        }

//        public void Dispose()
//        {
//            _disposableGuessed.Dispose();
//            _disposableInput.Dispose();
//        }

//        private void OnInputTextChanged(string oldText, string newText) => UpdateInputValue(newText);
//        private void OnTextGenerated(string oldText, string newText) => UpdateGuessedValue(newText);

//        private void UpdateGuessedValue(string value) => _screen.SetGeneratedText(value);
//        private void UpdateInputValue(string value) => _screen.SetInputText(value);
//    }
//}
