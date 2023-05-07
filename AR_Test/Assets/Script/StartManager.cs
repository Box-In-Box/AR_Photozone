using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject starUI;
    public GameObject loggingIn;
    public GameObject skipLogin;

    [Header("-----Login-----")]
    public GameObject loginPanel;
    public Transform loginUI;
    public GameObject loginOutSide;
    Vector3 LoginPanelTransform;

    [Header("-----ConsoleText-----")]
    public Transform ConsoleTextUI;
    Vector3 ComsoleTextTransform;
    public GameObject upUIPanel;

    private static StartManager _instance = null;
    public static StartManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(StartManager)) as StartManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active StartManager object");
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        starUI.SetActive(true);
        loginPanel.SetActive(true);
        
        SettingLoginPosition();
        SettingConsolePosition();
    }

    public void SettingLoginPosition()
    {   
        //로그인 아웃사이드 해제
        loginOutSide.SetActive(false);
        //원래 위치 저장
        LoginPanelTransform = loginUI.position;
        //시작 로그인 위치 설정
        loginUI.position += new Vector3(0, -400, 0);
    }

    public void SettingConsolePosition()
    {
        //원래 위치로 (3 : 4)
        Vector3 appComsolePosition = ConsoleTextUI.transform.position;
        appComsolePosition.y = (int)(Screen.height - (((Screen.width * 4) / 3) + upUIPanel.gameObject.GetComponent<RectTransform>().rect.height));
        ConsoleTextUI.transform.position = appComsolePosition;

        //원래 위치 저장
        ComsoleTextTransform = appComsolePosition;

        //시작 콘솔 위치 설정
        Vector3 startposition = new Vector3(appComsolePosition.x, 100, appComsolePosition.z);
        ConsoleTextUI.transform.position = startposition;
    }

    [ContextMenu("SettingOriginalPosition")]
    public void SettingOriginalPosition()
    {   
        starUI.SetActive(false);
        loginPanel.SetActive(false);

        //원래대로
        if (LoginPanelTransform != null) {
            loginUI.position = LoginPanelTransform;
            loginOutSide.SetActive(true);
        }   

        if (ComsoleTextTransform != null)
            ConsoleTextUI.position = ComsoleTextTransform;
    }
}
