using System;
using UnityEngine;

namespace UI.Login
{
    public class PoliciesPanel : MonoBehaviour
    {
        private Action _callback;
        public CanvasGroup canvasGroup;

        public void Show(Action callback)
        {
            _callback = callback;
            Show();
        }

        public void Show()
        {
            var acceptPolicies = PlayerPrefs.GetInt("policies", 0) == 1;
            if (acceptPolicies)
            {
                _callback.Invoke();
                return;
            }

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void ButtonAccept()
        {
            PlayerPrefs.SetInt("policies", 1);
            PlayerPrefs.Save();
            Hide();
            _callback.Invoke();
        }

        public void ButtonShowTerms()
        {
            Application.OpenURL("https://www.gnarlygamestudio.com/assets/images/Terms%20Of%20Service.pdf");
            Application.OpenURL("https://www.gnarlygamestudio.com/assets/images/Privacy%20Policy.pdf");
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}