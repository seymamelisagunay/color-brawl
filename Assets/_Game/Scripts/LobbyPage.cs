using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class LobbyPage : MonoBehaviour
    {
        private string[] _nameList;
        private int _nameIndex;
        private Repeater _repeater;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private Button startButton;
        private string enemyName;
        [SerializeField] private ScoreProgress scoreProgress;

        public GameManager gameManager;

        private void Start()
        {
            _nameList = UserManager.Instance.leaderboardManager.nameList;
            _nameIndex = 0;
            _repeater = new Repeater();
        }

        public void Show()
        {
            startButton.interactable = true;
            enabled = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_repeater.Set(0.1f))
            {
                enemyName = _nameList[_nameIndex];
                enemyNameText.SetText(enemyName);
                _nameIndex = (_nameIndex + 1) % _nameList.Length;
            }
        }

        public async void ButtonStart()
        {
            enabled = false;
            startButton.interactable = false;
            await Task.Delay(TimeSpan.FromSeconds(1));
            scoreProgress.SetNames("Self", enemyName);
            gameObject.SetActive(false);
            gameManager.StartLevel();
        }

        public void ButtonPlayAgain()
        {
            enemyName = _nameList[Random.Range(0, _nameList.Length)];
            scoreProgress.SetNames("Self", enemyName);
            gameManager.StartLevel();
        }
    }
}