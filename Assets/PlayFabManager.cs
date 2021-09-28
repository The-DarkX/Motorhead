using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.Events;

public class PlayFabManager : MonoBehaviour
{
    [Header("Windows")]
    public GameObject nameWindow;
    public GameObject leaderboardWindow;

    [Header("Display name window")]
    //public GameObject nameError;
    public TMP_InputField nameInput;

    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;

    string username;

    public static PlayFabManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /*

    void Start()
    {
        //Login();
    }

    void Login() 
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result) 
    {
        Debug.Log("Success");
        username = null;

        if (result.InfoResultPayload.PlayerProfile != null)
            username = result.InfoResultPayload.PlayerProfile.DisplayName;
    }
    */
    public void DisplayLeaderboard() 
    {
        leaderboardWindow.SetActive(true);

        GetLeaderboard();
    }

    public void CheckUsername() 
    {
        print(username);
        nameWindow.SetActive(true);

        /*
        if (username == null)
        {
            nameWindow.SetActive(true);
        }
        else
        {
            nameWindow.SetActive(false);
            loader.LoadScene(newSceneIndex);
        }*/
    }

    public void SubmitNameButton(int newSceneIndex) 
    {
        if (nameInput.text == null) return;

        var registerRequest = new RegisterPlayFabUserRequest
        {
            Username = nameInput.name
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterUser, OnError);

        /*
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        loader.LoadScene(newSceneIndex);
        */
    }

    void OnRegisterUser(RegisterPlayFabUserResult result) 
    {
        //Login();
        nameWindow.SetActive(false);
        LevelLoader.instance.LoadScene(1);
    }

    /*
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result) 
    {
        nameWindow.SetActive(false);
    }*/

    void OnError(PlayFabError error)
    {
        print(error.ErrorMessage);

    }

    public void SendLeaderboard(int score) 
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "CompetitionScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result) 
    {
        Debug.Log("Successful Leaderboard Sent");
    }

    public void GetLeaderboard() 
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "CompetitionScore",
            StartPosition = 0,
            MaxResultsCount = 10
,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result) 
    {
        if (rowsParent.childCount > 0)
        {
            foreach (Transform item in rowsParent)
            {
                Destroy(item.gameObject);
            }
        }

        foreach (var item in result.Leaderboard) 
        {
            Debug.Log(string.Format("PLACE: {0} | ID: {1} | VALUE: {2}", item.Position, item.DisplayName, item.StatValue));

            GameObject newItem = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newItem.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }
    }











    /*
    public void RegisterButton() 
    {
        if (passwordInput.text.Length < 6) 
        {
            messageText.text = "Password Too Short";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result) 
    {
        messageText.text = "Successfully Registered!";
    }

    public void LoginButton() 
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result) 
    {
        messageText.text = "Logged In!";
    }

    public void ResetPasswordButton() 
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "66513"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result) 
    {
        messageText.text = "Password Recovery Email Sent!";
    }
    */
}
