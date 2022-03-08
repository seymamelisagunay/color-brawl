using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts
{
    public class Block : MonoBehaviour
    {
        public string ownerID;
        private SpriteRenderer visualRenderer;
        public GameObject renderer;

        private void Awake()
        {
            visualRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetOwner(string ownerID)
        {
            this.ownerID = ownerID;
            visualRenderer.sprite = Resources.Load<Sprite>($"Blocks/{ownerID}");
            renderer.transform.localScale = Vector3.one * 0.5f;
            renderer.transform.DOScale(1, 0.1f);
        }
    }
}