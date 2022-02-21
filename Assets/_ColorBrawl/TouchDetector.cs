using System;
using UnityEngine;

namespace _ColorBrawl
{
    public class TouchDetector : MonoBehaviour
    {
        // Start is called before the first frame update
        public Character character;
        public int CollisionCount;
        public LevelManager levelManager;
        public Action OnFirstTouch { get; set; }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (CollisionCount == 0)
            {
                OnFirstTouch?.Invoke();
            }

            CollisionCount++;
            if (col.gameObject.tag == "Platform")
            {
                col.gameObject.GetComponent<Block>().SetOwner(character);
                levelManager.UpdateScore();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            CollisionCount--;
        }

        public bool Touching()
        {
            return CollisionCount > 0;
        }
    }
}