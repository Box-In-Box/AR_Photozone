using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    public InputField EmailInput, PasswordInput;
    public InputField Register_EmailInput, Register_PasswordInput, Register_UserNameInput;

    public GameObject ConsolePanel;

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Email = Register_EmailInput.text, 
            Password = Register_PasswordInput.text, Username = Register_UserNameInput.text };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnLoginSuccess(LoginResult result) => StartCoroutine(PrintText("로그인 성공"));

    void OnLoginFailure(PlayFabError error) => StartCoroutine(PrintText("로그인 실패"));

    void OnRegisterSuccess(RegisterPlayFabUserResult result) => StartCoroutine(PrintText("회원가입 성공"));

    void OnRegisterFailure(PlayFabError error) => StartCoroutine(PrintText("회원가입 실패"));

    IEnumerator PrintText(string msg)
    {
        ConsolePanel.SetActive(true);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        yield return new WaitForSecondsRealtime(2f);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        ConsolePanel.SetActive(false);
    }
}