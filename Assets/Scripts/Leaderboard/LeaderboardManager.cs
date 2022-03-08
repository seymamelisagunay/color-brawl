using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Leaderboard
{
    [Serializable]
    public class BoardUser
    {
        public string id;
        public int rank;
        public string userName;
        public int cup;
    }

    [Serializable]
    public class LeaderboardData
    {
        public List<BoardUser> users;
    }

    public class LeaderboardManager
    {
        public LeaderboardData leaderboard;
        public BoardUser selfUser;

        public LeaderboardManager(string userName)
        {
            if (PlayerPrefs.HasKey("leaderboard"))
                LoadLeaderboard();
            else
                GenerateLeaderboard(userName);
        }

        private void LoadLeaderboard()
        {
            var jsonData = PlayerPrefs.GetString("leaderboard");
            leaderboard = JsonUtility.FromJson<LeaderboardData>(jsonData);

            // Set fake cup increment
            foreach (var data in leaderboard.users)
            {
                if (data.id == "self")
                {
                    selfUser = data;
                    continue; // Because this is a fake increment cup
                }

                data.cup += 5 * Random.Range(1, 10);
            }

            OrderUserData();
            Save();
        }

        private void GenerateLeaderboard(string userName)
        {
            //Load usernames
            var nameData = Resources.Load<TextAsset>("usernames").text;
            var nameList = nameData.Split('\n');
            var chooseNames = nameList.ToList().OrderBy(x => Guid.NewGuid()).Take(29).ToList();

            // Define self user data
            selfUser = new BoardUser()
            {
                id = "self",
                cup = 0,
                rank = 0,
                userName = userName
            };

            // Create user data
            leaderboard = new LeaderboardData()
            {
                users = new List<BoardUser>()
                {
                    selfUser
                }
            };

            // Create fake data
            foreach (var name in chooseNames)
            {
                leaderboard.users.Add(new BoardUser()
                {
                    id = "fake" + Random.Range(0, 10000),
                    cup = 10 + (5 * Random.Range(1, 20)),
                    rank = 0,
                    userName = name
                });
            }

            //Prepare one user for tutorial after leaderboard show
            leaderboard.users[2].cup = 5;

            OrderUserData();
            Save();
        }

        private void Save()
        {
            var jsonData = JsonUtility.ToJson(leaderboard);
            PlayerPrefs.SetString("leaderboard", jsonData);
        }

        private void OrderUserData()
        {
            leaderboard.users = leaderboard.users.OrderByDescending(x => x.cup).ToList();
            for (var i = 0; i < leaderboard.users.Count; i++)
            {
                leaderboard.users[i].rank = i + 1;
            }
        }

        public void SetCurrentCup(int userModelCups)
        {
            selfUser.cup = userModelCups;
            OrderUserData();
            Save();
        }
    }
}