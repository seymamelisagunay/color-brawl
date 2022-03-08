using UnityEngine;

namespace _Game.Scripts.Playground
{
    [ExecuteInEditMode]
    public class Unit : MonoBehaviour
    {
        public bool Active;
        public (int x, int y) cord;
        public GameObject activeMesh;
        public GameObject passiveMesh;
        public bool isOutside;
        private PlaygroundController _controller;
        public bool Disable;

        public void Init(PlaygroundController controller)
        {
            _controller = controller;
            var oldPosition = transform.localPosition;
            var nextX = Mathf.RoundToInt(oldPosition.x);
            var nextY = Mathf.RoundToInt(oldPosition.y);
            transform.localPosition = new Vector3(nextX, nextY);
            _controller.Map[nextX, nextY] = true;
            cord = (nextX, nextY);
            isOutside = nextX == 0 || nextY == 0 || nextX == (controller.width - 1) ||
                        nextY == (controller.height - 1);
            if (Disable)
                isOutside = true;
        }

        public void SetState()
        {
            if (isOutside)
            {
                Active = !isOutside;
            }
            else
            {
                var isMyAroundFull = _controller.Map[cord.x - 1, cord.y]
                                     && _controller.Map[cord.x + 1, cord.y]
                                     && _controller.Map[cord.x, cord.y - 1]
                                     && _controller.Map[cord.x, cord.y + 1];
                Active = !isMyAroundFull;
            }

            activeMesh.SetActive(Active);
            passiveMesh.SetActive(!Active);

            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in spriteRenderers)
            {
                renderer.sortingOrder = cord.y;
            }
        }
    }
}