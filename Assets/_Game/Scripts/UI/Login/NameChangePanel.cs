using System;
using TMPro;
using UnityEngine;

namespace UI.Login
{
    public class NameChangePanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        public CanvasGroup canvasGroup;
        private Action _callback;

        public void Show(Action callback)
        {
            _callback = callback;
            Show();
        }

        public void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            nameInputField.text = UserManager.Instance.UserModel.name;
        }

        public void ButtonAccept()
        {
            UserManager.Instance.UserModel.name = nameInputField.text;
            Hide();
            _callback.Invoke();
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}