using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI _generatedText;
        [SerializeField] private TextMeshProUGUI _inputText;

        public void SetGeneratedText(string text) => _generatedText.text = text;
        public void SetInputText(string text) => _inputText.text = text;
    }
}
