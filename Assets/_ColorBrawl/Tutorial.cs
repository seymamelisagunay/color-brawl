using System.Collections;
using System.Collections.Generic;
using _ColorBrawl;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Color tutorialTargetColor;
    public TMPro.TMP_Text tapInstruction;
    public LevelManager levelManager;
    private GameManager gameManager;
    private float endWaitingTime;
    public bool endWaiting;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager.onLevelLoaded += LoadTutorial;
    }

    void LoadTutorial() {
        endWaiting = false;
         foreach (var block in levelManager.blocks)
        {
            block.visual.gameObject.SetActive(false);
            block.redSprite.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (levelManager.BlueScore == levelManager.BlockCount)
        {
            if(endWaiting && Time.time > endWaitingTime) {
                levelManager.EndLevel();
                gameManager.matchResultPanel.gameObject.SetActive(false);
                gameManager.lobbyPanel.gameObject.SetActive(true);
                return;
            }
            if(!endWaiting) {
                tapInstruction.text = "COMPLETED!";
                endWaitingTime = Time.time + levelManager.EndWaitingDuration;
                endWaiting = true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (levelManager.Ended)
            {
                return;
            }
            tapInstruction.text = "TURN RED TO BLUE";
            levelManager.StartLevel();
        }
       
    }
}
