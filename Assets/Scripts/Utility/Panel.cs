using System;
using UnityEngine;

namespace Game.UI.Profile
{
    public interface IPanel
    {
        bool Visible { get; }
        void Show();
        void Hide();
    }

    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Animator))]
    public class Panel : MonoBehaviour, IPanel
    {
        private bool _open;
        private static readonly int OpenAnim = Animator.StringToHash("open");
        private static readonly int CloseAnim = Animator.StringToHash("close");
        private CanvasGroup _canvasGroup;
        private Animator _animator;
        private CanvasGroup CanvasGroup => _canvasGroup ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>());
        private Animator Animator => _animator ? _animator : (_animator = GetComponent<Animator>());

        public bool Visible => _open;

        public void Show()
        {
            if (_open) return;
            Animator.enabled = true;
            Animator.SetTrigger(OpenAnim);
            _open = true;
        }

        public void Hide()
        {
            if (_open)
            {
                _open = false;
                Animator.SetTrigger(CloseAnim);
            }
            else
            {
                CanvasGroup.alpha = 0;
                CanvasGroup.blocksRaycasts = false;
            }
        }
    }
}