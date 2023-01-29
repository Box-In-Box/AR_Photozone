using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;

public class PlayfabManager : MonoBehaviour
{
    public InputField EmailInput, PasswordInput;
    public InputField Register_EmailInput, Register_PasswordInput, Register_UserNameInput;
    private string myID;
    public Text RankingText;

    public Text LoginLogText;
    public GameObject Login_Panel;

    private static PlayfabManager _instance = null;
    public static PlayfabManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlayfabManager)) as PlayfabManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    public void Login()
    {
        LoginLogText.text="로그인중...";

        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request,
            (result) =>
            {
                ClearLoginLogText();
                StartCoroutine(AppManager.Instance.PrintLog("로그인 성공"));

                myID = result.PlayFabId;
                StartCoroutine(SettingAnimalBook());
                AppManager.Instance.QuitPanel(Login_Panel);
            },
            (error) =>
            { 
                AppManager.Instance.OpenPanel(Login_Panel);
                
                if (error.ErrorDetails != null && error.ErrorDetails.Count > 0)
                {
                    using (var iter = error.ErrorDetails.Keys.GetEnumerator())
                    {
                            iter.MoveNext();
                            string key = iter.Current;
                            LoginLogText.text = error.ErrorDetails[key][0];
                            Debug.Log("form playfab error code : " + ((short)error.Error));

                            //주요 오류 한글화
                            switch (error.Error) {
                                case PlayFabErrorCode.InvalidParams:
                                    if (key == "Email")
                                        LoginLogText.text="이메일을 잘못 입력하셨습니다.";
                                    if (key == "Password")
                                        LoginLogText.text="비밀번호를 잘못 입력하셨습니다.";
                                    break;
                            }
                    }
                }
                else
                {
                    LoginLogText.text = error.ErrorMessage;
                    Debug.Log("Not form playfab error code : " + ((short)error.Error));

                    //주요 오류 한글화
                    switch (error.Error) {
                        case PlayFabErrorCode.AccountNotFound:
                            LoginLogText.text="등록되지 않은 이메일 입니다.";
                            break;
                        case PlayFabErrorCode.InvalidEmailOrPassword:
                            LoginLogText.text="비밀번호를 잘못 입력하셨습니다.";
                            break;
                    }
                }
            }
        );
    }

    public void Register()
    {
        LoginLogText.text="회원가입중...";

        var request = new RegisterPlayFabUserRequest
        {
            Email = Register_EmailInput.text,
            Password = Register_PasswordInput.text,
            Username = Register_UserNameInput.text,
            DisplayName = Register_UserNameInput.text
        };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            (result) => 
            { 
                StartCoroutine(AppManager.Instance.PrintLog("회원가입 성공")); 
            },
            (error) => 
            {   
                if (error.ErrorDetails != null && error.ErrorDetails.Count > 0)
                {
                    using (var iter = error.ErrorDetails.Keys.GetEnumerator())
                    {
                            iter.MoveNext();
                            string key = iter.Current;
                            LoginLogText.text = error.ErrorDetails[key][0];
                            Debug.Log("form playfab error code : " + ((short)error.Error));

                            //주요 오류 한글화
                            switch (error.Error) {
                                case PlayFabErrorCode.EmailAddressNotAvailable:
                                    LoginLogText.text="이미 등록된 이메일 입니다.";
                                    break;
                                case PlayFabErrorCode.InvalidParams:
                                    if (key == "Email")
                                        LoginLogText.text="이메일 형식이 아닙니다.";
                                    if (key == "Password")
                                        LoginLogText.text="비밀번호는 6자리 이상으로 입력해주세요.";
                                    if (key == "Username")
                                        LoginLogText.text="닉네임은 3~20자리로 입력해주세요.";
                                    break;
                            }
                    }
                }
                else
                {
                    LoginLogText.text = error.ErrorMessage;
                    Debug.Log("Not form playfab error code : " +  ((short)error.Error));

                    //주요 오류 한글화
                    switch (error.Error) {
                        case PlayFabErrorCode.NameNotAvailable:
                            LoginLogText.text="이미 등록된 닉네임 입니다.";
                            break;
                    }
                }
            }
        );
    }

    #region AnimalBook DB
    IEnumerator SettingAnimalBook()
    {
        yield return AnimalBookManager.Instance.StartCoroutine(AnimalBookManager.Instance.Setting());
    }

    public void GetAnimal(string msg)   //실행 처음만 실행
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request,
            (result) =>
            {
                string time = result.Data[msg].Value;
                AnimalBookManager.Instance.SetAnimal(msg, time);
            },
            (error) => { print("데이터 불러오기 실패"); }); //***Debug 띄워지는거 없애야함
    }

    
    public void SaveAnimal(string msg, string time)
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { msg, time } } };
        PlayFabClientAPI.UpdateUserData(request,
            (result) =>
            {
                StartCoroutine(AppManager.Instance.PrintLog(msg + "을(를) 등록하였습니다."));
            },
            (error) => StartCoroutine(AppManager.Instance.PrintLog(msg + "을(를) 등록 실패했습니다.")));
    }
    #endregion

    #region AnimalBook Ranking
    public void SetStat(int score)
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "Score", Value = score } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => Debug.Log("Save Score : " + score), (error) => Debug.Log("Error Save Score"));
    }

    public void GetState()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                foreach (var eachStat in result.Statistics) {
                    AnimalBookManager.Instance.score = eachStat.Value;
                } 
            },
            (error) => { Debug.Log("Error Load Score"); });
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "Score", MaxResultsCount = 10, ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true } };
        PlayFabClientAPI.GetLeaderboard(request, (result) => 
        {   
            AnimalBookManager.Instance.leaderboardText.text = "";
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                var curBoard = result.Leaderboard[i];
                AnimalBookManager.Instance.leaderboardText.text += String.Format("{0}위 {1, -10} 님 {2, 2}마리", (i+1), curBoard.Profile.DisplayName, curBoard.StatValue) +"\n";
            }
        },
        (error) => Debug.Log("Failed load leaderboard"));
    }
    #endregion
    public void ClearLoginLogText()    //로그인 - 회원가입 전환시 필요
    {
        LoginLogText.text="";
    }
}