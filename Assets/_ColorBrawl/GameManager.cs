using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MatchResult matchResultPanel;
    public GameObject lobbyPanel;

    public List<LevelManager> levels;
    public int currentLevelIndex => PlayerPrefs.GetInt("currentLevelIndex", 0);

    void Awake() {
        Application.targetFrameRate = 60;
    }
    void Start() {
        StartLevel();
    }
    public void StartLevel()
    {
        foreach(var level in levels) {
            level.gameObject.SetActive(false);
        }
        lobbyPanel.SetActive(false);
        matchResultPanel.gameObject.SetActive(false);
        var currentLevel = levels[currentLevelIndex];
        currentLevel.gameObject.SetActive(true);
        currentLevel.LoadLevel();
       
    }
    public void EndLevel(int BlueScore, int RedScore) {
        matchResultPanel.gameObject.SetActive(true);
        matchResultPanel.Fill(BlueScore, RedScore);
        var nextLevelIndex = currentLevelIndex + 1;
        nextLevelIndex = nextLevelIndex % levels.Count;
        if(nextLevelIndex == 0) {
            nextLevelIndex++;
        }
        PlayerPrefs.SetInt("currentLevelIndex", nextLevelIndex);
    }
}
