using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Core.TestPopup
{
    public class TestPopupView : PopupViewBase
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text) => _text.text = text;
    }
}
