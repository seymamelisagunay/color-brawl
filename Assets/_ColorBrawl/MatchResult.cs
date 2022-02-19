using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchResult : MonoBehaviour
{
    public TMPro.TMP_Text blueScoreText;
    public TMPro.TMP_Text redScoreText;

    public TMPro.TMP_Text titleText;

    public void Fill(int blue, int red)
    {
     blueScoreText.text = blue.ToString(); 
     redScoreText.text = red.ToString();
     if(red > blue) {
         titleText.text = "RED WINS!";
     } else if(blue > red ) {
         titleText.text = "BLUE WINS!";
     } else {
         titleText.text = "DRAW";
     }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
