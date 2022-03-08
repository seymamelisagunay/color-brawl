using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts
{
    public interface ILevelManager
    {
        bool Ended { get; set; }
        bool Waiting { get; set; }
        void UpdateScore();
        void LoadLevel();
    }

    public class LevelManager : MonoBehaviour, ILevelManager
    {
        public float matchDuration;
        private float matchEndTime;
        private float nextTimeUpdate;
        public List<Block> blocks;
        private GameManager gameManager;
        public int redScore;
        public int blueScore;
        public bool Ended { get; set; }
        public bool Waiting { get; set; }
        public GameObject blueSpawn;
        public GameObject redSpawn;
        public Character bluePlayer;
        public Character redPlayer;
        public float WaitingTime;
        public float EndWaitingDuration;
        private float LevelStartTime;
        private float EndWaitingTime;
        private bool EndWaited;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            Waiting = true;
        }

        public void LoadLevel()
        {
            gameObject.SetActive(true);
            Waiting = true;
            EndWaited = false;
            bluePlayer.gameObject.SetActive(true);
            redPlayer.gameObject.SetActive(true);
            bluePlayer.transform.position = blueSpawn.transform.position;
            redPlayer.transform.position = redSpawn.transform.position;

            matchEndTime = Time.time + matchDuration;
            EndWaitingTime = matchEndTime + EndWaitingDuration;
            Ended = false;
            blocks.Clear();
            var blockList = gameObject.GetComponentsInChildren<Block>();
            foreach (var block in blockList)
            {
                block.SetOwner("empty");
                blocks.Add(block);
            }

            blueScore = 0;
            redScore = 0;
            gameManager.scoreProgress.StartBlockCount(blocks.Count);
            LevelStartTime = Time.time + WaitingTime;
            if (gameManager.countDown)
            {
                gameManager.countDown.gameObject.SetActive(true);
                gameManager.countDown.StartCountdown((int) WaitingTime);
            }

            UpdateScore();
        }

        public void StartLevel()
        {
            Waiting = false;
            bluePlayer.StartLevel(this);
            redPlayer.StartLevel(this);
        }

        public void UpdateScore()
        {
            var redScore = 0;
            var blueScore = 0;
            foreach (var block in blocks)
            {
                if (block.ownerID == "red")
                {
                    redScore++;
                    continue;
                }

                if (block.ownerID == "blue")
                {
                    blueScore++;
                    continue;
                }
            }

            this.blueScore = blueScore;
            this.redScore = redScore;
            gameManager.scoreProgress.UpdateProgress(this.redScore, this.blueScore);
        }

        private void EndLevel()
        {
            gameObject.SetActive(false);
            gameManager.EndLevel(blueScore, redScore);
            bluePlayer.gameObject.SetActive(false);
            redPlayer.gameObject.SetActive(false);
            EndWaited = true;
        }

        void Update()
        {
            if (Ended && Time.time > EndWaitingTime && !EndWaited)
            {
                EndLevel();
                return;
            }

            if (Ended) return;

            if (Time.time > matchEndTime)
            {
                Ended = true;
                bluePlayer.GetComponent<Character>().Stop();
                redPlayer.GetComponent<Character>().Stop();
            }
            else if (Waiting && Time.time > LevelStartTime)
            {
                StartLevel();
            }
            else if (Time.time > nextTimeUpdate && !Waiting)
            {
                if (Time.time > EndWaitingTime) return;
                gameManager.timeOutText.text = ((int) (matchEndTime - Time.time)).ToString();
                nextTimeUpdate = Time.time + 1f;
            }
        }
    }
}