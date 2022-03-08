using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardItem : MonoBehaviour
    {
        [SerializeField] private Transform rankIconPoint;
        [SerializeField] private Transform profileIconParent;
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text cupText;
        private GameObject _rankIcon;
        public BoardUser Data { get; private set; }

        public void Init(BoardUser data)
        {
            Data = data;

            if (data.id != "self")
            {
                Instantiate(Resources.Load<GameObject>($"Leaderboard/Characters/{Random.Range(1, 6)}"),
                    profileIconParent);
            }

            UpdateView();
        }

        public void UpdateView()
        {
            nameText.SetText(Data.userName);
            cupText.SetText(Data.cup.ToString());


            if (_rankIcon != null) // clear old rank icon
                Destroy(_rankIcon);

            if (Data.rank < 4) // in first 3
                _rankIcon = Instantiate(Resources.Load<GameObject>($"Leaderboard/RankIcon/{Data.rank}"),
                    rankIconPoint);
            else
                rankText.SetText(Data.rank.ToString());
        }
    }
}