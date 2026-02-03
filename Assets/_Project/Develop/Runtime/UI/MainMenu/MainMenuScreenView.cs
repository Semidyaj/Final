using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenTestPopupButtonClicked;

        [SerializeField] private Button _openTestPopupButton;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }

        private void OnEnable()
        {
            _openTestPopupButton.onClick.AddListener(OnOpenTestPopupButtonClicked);
        }

        private void OnDisable()
        {
            _openTestPopupButton.onClick.RemoveListener(OnOpenTestPopupButtonClicked);
        }

        private void OnOpenTestPopupButtonClicked() => OpenTestPopupButtonClicked?.Invoke();
    }
}
