using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class LineInfoController : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Animator animator;

        public void Open(string message, float lifeTime)
        {
            messageText.text = message;
            animator.SetTrigger("open");
            transform.SetAsLastSibling();
            Invoke(nameof(Close), lifeTime);
        }

        private void Close()
        {
            animator.SetTrigger("close");
        }

        public void OnClosed()
        {
            Destroy(gameObject);
        }
    }
}