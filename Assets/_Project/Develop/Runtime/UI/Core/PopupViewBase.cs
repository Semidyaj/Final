using System;
using UnityEngine;
using DG.Tweening;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public class PopupViewBase : MonoBehaviour, IShowableView
    {
        public event Action CloseRequest;

        [SerializeField] private CanvasGroup _mainGroup;

        private void Awake()
        {
            _mainGroup.alpha = 0;
        }

        public void OnCloseButtonClicked() => CloseRequest?.Invoke();

        public void Show()
        {
            OnPreShow();

            _mainGroup.DOFade(1, 1);

            OnPostShow();
        }

        public void Hide()
        {
            OnPreHide();

            _mainGroup.alpha = 0;

            OnPostHide();
        }

        protected virtual void OnPreShow() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostHide() { }
    }
}
