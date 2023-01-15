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

    public GameObject ConsolePanel;
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
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request,
            (result) =>
            {
                StartCoroutine(AppManager.Instance.PrintLog("로그인 성공"));
                myID = result.PlayFabId;
                AnimalBookManager.Instance.Setting();
                AppManager.Instance.QuitPanel(Login_Panel);
            },
            (error) =>
            { 
                StartCoroutine(AppManager.Instance.PrintLog("로그인 실패"));
                AppManager.Instance.OpenPanel(Login_Panel);
            }
        );
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = Register_EmailInput.text,
            Password = Register_PasswordInput.text,
            Username = Register_UserNameInput.text,
            DisplayName = Register_UserNameInput.text
        };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            (result) => { StartCoroutine(AppManager.Instance.PrintLog("회원가입 성공")); },
            (error) => StartCoroutine(AppManager.Instance.PrintLog("회원가입 실패")));
    }

    public void GetAnimal(string msg)   //실행 처음만 실행
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request,
            (result) =>
            {
                print(result.Data[msg].Value);
                AnimalBookManager.Instance.isFound[num] = true;
                AnimalBookManager.Instance.AnimalContentParent.GetChild(num).gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
                AnimalBookManager.Instance.CollectedAnimalCount++;
                AnimalBookManager.Instance.ShowCount();
            },
            (error) => { print("데이터 불러오기 실패"); });
    }

    #region AnimalBook DB
    public void SaveAnimal(string msg)
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { msg, DateTime.Now.ToString(("yyyy-MM-dd HH:mm")) } } };
        PlayFabClientAPI.UpdateUserData(request,
            (result) =>
            {
                StartCoroutine(AppManager.Instance.PrintLog(msg + "을(를) 등록하였습니다."));
            },
            (error) => StartCoroutine(AppManager.Instance.PrintLog(msg + "을(를) 등록 실패했습니다.")));
    }
    #endregion
}