using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public TMPro.TMP_Text redScoreText;
    public TMPro.TMP_Text blueScoreText;
    public TMPro.TMP_Text emptyAmountText;
    public TMPro.TMP_Text timeoutText;
    public float matchDuration;
    private float matchEndTime;
    private float nextTimeUpdate;
    public GameObject hud;
    public List<Block> blocks;
    private GameManager gameManager;
    public int BlockCount;
    public int RedScore;
    public int BlueScore;
    public Color defaultBlockColor;
    public bool Ended;

    public bool Waiting;
    public GameObject blueSpawn;
    public GameObject redSpawn;
    public GameObject bluePlayer;
    public GameObject redPlayer;
    public float WaitingTime;
    public float EndWaitingDuration;
    private float LevelStartTime;
    public CountDown countDown;
    public Action onLevelLoaded;
    private float EndWaitingTime;
    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        Waiting = true;
    }

    public void LoadLevel()
    {
        Waiting = true;
        hud.SetActive(true);

        bluePlayer.transform.position = blueSpawn.transform.position;
        redPlayer.transform.position = redSpawn.transform.position;

        matchEndTime = Time.time + matchDuration;
        EndWaitingTime = matchEndTime + EndWaitingDuration;
        Ended = false;
        var platforms = GameObject.FindGameObjectsWithTag("Platform");
        blocks.Clear();
        foreach (var platform in platforms)
        {
            var block = platform.GetComponent<Block>();
            Debug.Log(block.gameObject.name);
            block.visual.GetComponent<SpriteRenderer>().color = defaultBlockColor;
            block.ownerID = "";
            blocks.Add(block);
        }
        BlueScore = 0;
        RedScore = 0;
        BlockCount = blocks.Count;
        LevelStartTime = Time.time + WaitingTime;
        if (countDown)
        {
            countDown.gameObject.SetActive(true);
            countDown.StartCountdown((int)WaitingTime);
        }
        onLevelLoaded?.Invoke();

    }
    public void StartLevel()
    {
        Waiting = false;
        bluePlayer.GetComponent<Character>().EnableMovement();
        redPlayer.GetComponent<Character>().EnableMovement();
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
        BlueScore = blueScore;
        RedScore = redScore;
        redScoreText.text = RedScore.ToString();
        blueScoreText.text = BlueScore.ToString();
        emptyAmountText.text = (BlockCount - RedScore - BlueScore).ToString();
    }
    void Update()
    {
        if (Time.time > EndWaitingTime)
        {
            hud.SetActive(false);
            gameManager.EndLevel(BlueScore, RedScore);
            return;
        }
        if (Ended) return;

        if (Time.time > matchEndTime)
        {
            Ended = true;

        }
        else if (Waiting && Time.time > LevelStartTime)
        {
            StartLevel();
        }
        else if (Time.time > nextTimeUpdate && !Waiting)
        {
            if (Time.time > EndWaitingTime) return;
            timeoutText.text = ((int)(matchEndTime - Time.time)).ToString();
            nextTimeUpdate = Time.time + 1f;
        }
    }
}
