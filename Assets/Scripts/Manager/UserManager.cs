using System;
using System.Threading.Tasks;
using Leaderboard;
using Newtonsoft.Json;
using UI.Login;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class UserModel
{
    public string name;
    public int gold = 50;
    public int diamond = 0;
    public int level = 1;
    public int exp = 0;
    public int cups = 0;
}

public class ResultData
{
    public bool Win;
    public int OldExp;
    public int OldLevel;
    public int Cup;
    public int Gold;
}

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }
    public UserModel UserModel { get; private set; }
    [SerializeField] private PoliciesPanel policiesPanel;
    [SerializeField] private NameChangePanel nameChangePanel;
    public LeaderboardManager leaderboardManager;
    private bool _first;
    private const int ModelVersion = 4;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Application.runInBackground = true;
        Instance = this;
        _first = !PlayerPrefs.HasKey("model_version");
        if (_first)
        {
            policiesPanel.Show(OnAcceptPolices);
        }
        else
        {
            GetUserData();
            LoadHome();
        }
    }

    private async void OnAcceptPolices()
    {
        GetUserData();
        await Task.Delay(TimeSpan.FromSeconds(1));
        nameChangePanel.Show(LoadHome);
    }

    private void OnDestroy()
    {
        Instance = null;
        SaveUser();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveUser();
        }
    }

    private void LoadHome()
    {
        leaderboardManager = new LeaderboardManager(UserModel.name);
    }

    private void GetUserData()
    {
        if (PlayerPrefs.HasKey("model_version"))
        {
            var modelVersion = PlayerPrefs.GetInt("model_version");
            if (modelVersion == ModelVersion)
            {
                UserModel = JsonConvert.DeserializeObject<UserModel>(PlayerPrefs.GetString("user"));
                if (UserModel == null)
                {
                    Debug.LogError("Json convert fail");
                    CreateUserData();
                }
            }
            else
            {
                CreateUserData();
            }
        }
        else
        {
            CreateUserData();
        }
    }

    private void SaveUser()
    {
        if (UserModel != null)
        {
            PlayerPrefs.SetInt("model_version", ModelVersion);
            PlayerPrefs.SetString("user", JsonConvert.SerializeObject(UserModel));
            PlayerPrefs.Save();
        }
    }

    private void CreateUserData()
    {
        UserModel = new UserModel
        {
            name = $"Player{1000 + Random.Range(15, 500)}",
        };
        SaveUser();
    }


#if UNITY_EDITOR

    [MenuItem("PlayerPrefs/Clear")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Admin/LevelUP")]
    private static void AdminLevelUp()
    {
        Instance.UserModel.level++;
        var result = Instance.CalculateMatchResult(true);
        Instance.TakeReward(result);
    }


    [MenuItem("Admin/Get100000Gold")]
    private static void AdminGetGold()
    {
        Instance.UserModel.gold += 100000;
    }

#endif


    public ResultData CalculateMatchResult(bool win)
    {
        var result = new ResultData()
        {
            OldExp = UserModel.exp,
            OldLevel = UserModel.level,
            Win = win
        };

        result.Cup = win ? 10 : 5;
        result.Gold = win ? 100 : 0;

        UserModel.exp += win ? 120 : 50;
        var expThreshold = (100 * UserModel.level);
        var incrementLevel = UserModel.exp / expThreshold;
        UserModel.exp -= incrementLevel * expThreshold;
        UserModel.level += incrementLevel;

        return result;
    }

    public void TakeReward(ResultData resultData)
    {
        UserModel.cups += resultData.Cup;
        UserModel.gold += resultData.Gold;
    }
}