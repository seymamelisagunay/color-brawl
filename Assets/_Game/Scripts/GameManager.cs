using System.Collections.Generic;
using _ColorBrawl.Scripts;
using TMPro;
using UnityEngine;

namespace _Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public MatchResult matchResultPanel;
        public GameObject lobbyPanel;
        public TMP_Text timeOutText;
        public List<LevelManager> levels;
        public TutorialManager tutorialManager;
        public ScoreProgress scoreProgress;

        public int CurrentLevelIndex => PlayerPrefs.GetInt("currentLevelIndex", -1);

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            StartLevel();
        }

        public void StartLevel()
        {
            tutorialManager.gameObject.SetActive(false);
            foreach (var level in levels)
            {
                level.gameObject.SetActive(false);
            }

            lobbyPanel.SetActive(false);
            matchResultPanel.gameObject.SetActive(false);
            var currentLevel = CurrentLevelIndex == -1 ? (ILevelManager) tutorialManager : levels[CurrentLevelIndex];
            currentLevel.LoadLevel();
        }

        public void EndLevel(int blueScore, int redScore)
        {
            matchResultPanel.gameObject.SetActive(true);
            matchResultPanel.Fill(blueScore, redScore);
            var nextLevelIndex = CurrentLevelIndex + 1;
            nextLevelIndex %= levels.Count;
            PlayerPrefs.SetInt("currentLevelIndex", nextLevelIndex);
        }
    }
}