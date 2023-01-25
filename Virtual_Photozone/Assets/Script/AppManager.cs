using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [Header("-----Console Text-----")]
    public GameObject ConsolePanel;

    [Header("-----Screen Ratio Padding Object-----")]
    public RectTransform Down_Screen_Padding_Panel;
    public RectTransform Up_Screen_Padding_Panel;
    public RectTransform upUIPanel; //비율 상단 기준점

    [Header("-----Screen Ratio-----")]
    public int screenRatio;     //스크린 비율
    public int screenHeight;     //스크린 높이
    public Text screenRatioText;

    [Header("-----Shutter Sound-----")]
    public int shutterSound;    //셔터음 사운드
    public Text shutterSoundText;

    private static AppManager _instance = null;
    public static AppManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(AppManager)) as AppManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        AppSetting();
    }

    public void AppSetting()
    {
        //Data Load
        DataManager.Instance.LoadSettingData();

        //Data Setting
        ScreenRatio(DataManager.Instance.data.screenRatio);
        ShutterSound(DataManager.Instance.data.shutterSound);

        //ScrrenshotManager Setting
        ScreenshotManager.Instance.SetScreenRatio(screenRatio);
        ScreenshotManager.Instance.SetShutterSound(shutterSound);
    }

    //종료시 자동저장
    private void OnApplicationQuit() 
    {
        DataManager.Instance.SavaSettingData();
    }

    #region Panel stting
    public void OpenPanel(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void QuitPanel(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void OtherPanelActiveFalse(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Setting Screen
    public void SetRatio(int ratio) //screen ratio 변경 (중복 save 방지)
    {
        ScreenRatio(ratio);
        ScreenshotManager.Instance.SetScreenRatio(ratio);
        DataManager.Instance.data.screenRatio = ratio;
        DataManager.Instance.SavaSettingData();
    }

    public void ScreenRatio(int ratio)  //screen ratio 설정
    {
        screenRatio = ratio;
        Color ColorA = Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color;

        switch (ratio)
        {
            case 0:
                screenHeight = Screen.width; // 1 : 1 비율
                ColorA.a = 1f;
                Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                Down_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                screenRatioText.text = "1 : 1";
                break;
            case 1:
                screenHeight = (Screen.width * 4) / 3; // 3 : 4 비율
                ColorA.a = 1f;
                Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                Down_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                screenRatioText.text = "3 : 4";
                break;
            case 2:
                screenHeight = (Screen.width * 4) / 3; // Full 비율
                ColorA.a = 0.1f;
                Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                Down_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                screenRatioText.text = "FULL";
                break;
        }
        Down_Screen_Padding_Panel.sizeDelta = new Vector2(0, Screen.height - (screenHeight + upUIPanel.gameObject.GetComponent<RectTransform>().rect.height));
    }
    #endregion

    #region Setting Shutter Sound
    public void SetSound(int sound) //shutter sound 변경 (중복 save 방지)
    {
        ShutterSound(sound);
        DataManager.Instance.data.shutterSound = shutterSound;
        DataManager.Instance.SavaSettingData();
        ScreenshotManager.Instance.SetShutterSound(sound);
    }

    public void ShutterSound(int sound) //shutter sound 설정
    {
        shutterSound = sound;

        switch(shutterSound)
        {
            case 0:
                shutterSoundText.text = "Sound1";
                break;
            case 1:
                shutterSoundText.text = "Sound2";
                break;
            case 2:
                shutterSoundText.text = "Sound3";
                break;
        }
    }
    #endregion

    #region Ranking Panel
    public void SetLoginText(Text text) => text.text = "로그인";
    public void SetRegistText(Text text) => text.text = "회원가입";
    #endregion

    #region Print Log
    public IEnumerator PrintLog(string msg)
    {
        ConsolePanel.SetActive(true);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        yield return new WaitForSecondsRealtime(1.5f);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        ConsolePanel.SetActive(false);
    }
    #endregion
}