using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class ScoreProgress : MonoBehaviour
    {
        public Slider blueSlider;
        public Slider redSlider;
        [SerializeField] private TMP_Text selfNameText;
        [SerializeField] private TMP_Text enemyNameText;

        public void SetNames(string self, string enemy)
        {
            selfNameText.SetText(self);
            enemyNameText.SetText(enemy);
        }

        public void StartBlockCount(int bCount)
        {
            blueSlider.maxValue = bCount;
            redSlider.maxValue = bCount;
        }

        public void UpdateProgress(int red, int blue)
        {
            redSlider.value = red;
            blueSlider.value = blue;
        }
    }
}