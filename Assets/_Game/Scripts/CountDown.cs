using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public TMPro.TMP_Text counDownText;
    private float nextTimeUpdate;
    private int counts;
    private bool countedDown;

    public void StartCountdown(int duration)
    {
        countedDown = false;
        counts = duration;
        nextTimeUpdate = Time.time + 1f;

        counDownText.text = counts.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (countedDown) return;
        if (Time.time > nextTimeUpdate)
        {
            counts--;
            nextTimeUpdate = Time.time + 1f;
            counDownText.text = counts.ToString();
            if (counts < 1)
            {
                counDownText.text = "";
                countedDown = true;
                return;
            }
        }
    }
}