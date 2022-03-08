using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts
{
    public class TutorialManager : MonoBehaviour, ILevelManager
    {
        public TMPro.TMP_Text tapInstruction;
        private GameManager gameManager;
        private float endWaitingTime;
        public bool endWaiting;
        public Character bluePlayer;
        public List<Block> blocks;
        public Transform blueSpawnPoint;
        public GameObject scoreBoardPanel;
        public bool Ended { get; set; }
        public int BlueScore { get; set; }
        public bool Waiting { get; set; }
        public bool EndWaited { get; set; }
        public float EndWaitingDuration => 2;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            if (BlueScore == blocks.Count)
            {
                if (endWaiting && Time.time > endWaitingTime)
                {
                    EndLevel();
                    gameManager.matchResultPanel.gameObject.SetActive(false);
                    gameManager.lobbyPanel.Show();
                    return;
                }

                if (!endWaiting)
                {
                    tapInstruction.text = "COMPLETED!";
                    endWaitingTime = Time.time + EndWaitingDuration;
                    endWaiting = true;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (Ended)
                {
                    return;
                }

                tapInstruction.text = "TURN RED TO BLUE";
                StartLevel();
            }
        }

        public void EndLevel()
        {
            gameObject.SetActive(false);
            gameManager.EndLevel(BlueScore, 0);
            EndWaited = true;
            scoreBoardPanel.SetActive(true);
            bluePlayer.gameObject.SetActive(false);
        }

        public void StartLevel()
        {
            Waiting = false;
            bluePlayer.StartLevel(this);
        }

        public void UpdateScore()
        {
            var blueScore = blocks.Count(block => block.ownerID == "blue");
            BlueScore = blueScore;
            // FindObjectOfType<ScoreProgress>().UpdateProgress(RedScore, BlueScore);
        }

        public void LoadLevel()
        {
            gameObject.SetActive(true);
            Waiting = true;
            EndWaited = false;

            bluePlayer.transform.position = blueSpawnPoint.transform.position;
            bluePlayer.gameObject.SetActive(true);
            Ended = false;
            blocks.Clear();
            var blockList = gameObject.GetComponentsInChildren<Block>();
            foreach (var block in blockList)
            {
                block.SetOwner("red");
                blocks.Add(block);
            }

            BlueScore = 0;
            UpdateScore();
            scoreBoardPanel.SetActive(false);
        }
    }
}