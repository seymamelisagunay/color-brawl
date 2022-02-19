using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Color tutorialTargetColor;
    public GameObject tapInstruction;
    public LevelManager levelManager;
    private GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager.onLevelLoaded += LoadTutorial;
    }

    void LoadTutorial() {
         foreach (var block in levelManager.blocks)
        {
            block.visual.GetComponent<SpriteRenderer>().color = tutorialTargetColor;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (levelManager.Ended)
        {
            return;
        }
        if (levelManager.BlueScore == levelManager.BlockCount)
        {
            levelManager.Ended = true;
            levelManager.hud.SetActive(false);
            gameManager.EndLevel(1, 1);
            gameManager.matchResultPanel.gameObject.SetActive(false);
            gameManager.lobbyPanel.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
           
            tapInstruction.SetActive(false);
            levelManager.StartLevel();
        }
    }
}
