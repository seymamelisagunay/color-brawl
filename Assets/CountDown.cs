using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CountDown : MonoBehaviour
{
    public GameObject hud;
    public TMPro.TMP_Text counDownText;
    private float nextTimeUpdate;
    private int counts;
    public void StartCountdown(int duration)
    {
        counts = duration;
        nextTimeUpdate = Time.time + 1f;

        counDownText.text = counts.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeUpdate)
        {
         
            counts--;
            nextTimeUpdate = Time.time + 1f;
            counDownText.text = counts.ToString();
            if (counts < 1)
            {
                hud.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
