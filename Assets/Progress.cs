using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public Slider blueSlider;
    public Slider redSlider;

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
