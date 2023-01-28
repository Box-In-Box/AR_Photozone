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

    [Header("-----Mirror-----")]
    public bool isMirror;
    public GameObject mirrorOff;
    public GameObject mirrorOn;

    [Header("-----AutoLogin-----")]
    public bool isAutoLogin;
    public  Image autoLoginButtonImage;
    public Sprite autoLoginOff;
    public Sprite autoLoginOn;

    [Header("-----TransparentUI-----")]
    public bool isTransparentUI;    //로컬저장 x
    public RectTransform TransparentUIButton;
    public  Image transparentUIImage;
    public Sprite transparentUIDown;
    public Sprite transparentUIUp;

    [Space(10f)]
    public RectTransform Up_Panel;
    public RectTransform Down_Panel;
    public bool isrunnigCoroutine;

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
        MirrorMode(DataManager.Instance.data.isMirror);
        AutoLogin(DataManager.Instance.data.isAutoLogin);

        //ScrrenshotManager Setting
        ScreenshotManager.Instance.SetScreenRatio(screenRatio);
        ScreenshotManager.Instance.SetShutterSound(shutterSound);
        ScreenshotManager.Instance.SetMirrorMode(isMirror);
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

    #region MirrorMode
    public void MirrorMode(bool mirror) //ismirror 설정
    {    
        isMirror = mirror;
        mirrorOn.gameObject.SetActive(mirror);
        mirrorOff.gameObject.SetActive(!mirror);
    }
    public void SetMirrorMode() //mirror switch
    {
        MirrorSwitch();
        ScreenshotManager.Instance.SetMirrorMode(isMirror);
        DataManager.Instance.data.isMirror = isMirror;
        DataManager.Instance.SavaSettingData();
    }

    public void MirrorSwitch()
    {
        if (mirrorOff.gameObject.activeSelf == true)
            isMirror = true;
        else
            isMirror = false;

        mirrorOn.gameObject.SetActive(isMirror);
        mirrorOff.gameObject.SetActive(!isMirror);
    }
    #endregion

    #region AutoLogin
    public void AutoLogin(bool autoLogin) //isAutoLogin 설정
    {    
        isAutoLogin = autoLogin;
        if(autoLogin == true)
            autoLoginButtonImage.GetComponent<Image>().sprite = autoLoginOn;
        else
            autoLoginButtonImage.GetComponent<Image>().sprite = autoLoginOff;
    }

    public void SetAutoLogin() //mirror switch
    {
        AutoLoginSwitch();
        DataManager.Instance.data.isAutoLogin = isAutoLogin;
        DataManager.Instance.SavaSettingData();
    }

    public void AutoLoginSwitch()
    {
        if (isAutoLogin == true) {
            isAutoLogin = false;
            autoLoginButtonImage.GetComponent<Image>().sprite = autoLoginOff;
        }
        else {
            isAutoLogin = true;
            autoLoginButtonImage.GetComponent<Image>().sprite = autoLoginOn;
        }
    }
    #endregion

    #region TransparentUI
    public void SetTransparentUI()
    {
        if (isTransparentUI == false && isrunnigCoroutine == false) {
            isTransparentUI = true;
            isrunnigCoroutine = true;
            DownPanel();
        }
        if (isTransparentUI == true && isrunnigCoroutine == false) {
            isTransparentUI = false;
            isrunnigCoroutine = true;
            UpPanel();
        }
    }

    public void TransparentUSwitch() //애니메이션 끝날 때로 수정 필요
    {
        if (isTransparentUI == true)
            transparentUIImage.GetComponent<Image>().sprite = transparentUIUp;
        else
            transparentUIImage.GetComponent<Image>().sprite = transparentUIDown;
    }

    public void DownPanel() //최적화 필요
    {   
        Transform[] targetUI = {Down_Panel, Down_Screen_Padding_Panel, Up_Panel, Up_Screen_Padding_Panel, TransparentUIButton};

        Vector3[] current = {Down_Panel.position, Down_Screen_Padding_Panel.position, Up_Panel.position, Up_Screen_Padding_Panel.position, TransparentUIButton.position};
        
        Vector3[] target = new Vector3[targetUI.Length];

        target[0] = Down_Panel.position;
        target[0].y -= Down_Panel.GetComponent<RectTransform>().rect.height;

        target[1] = Down_Screen_Padding_Panel.position;
        target[1].y -= Down_Screen_Padding_Panel.GetComponent<RectTransform>().rect.height;

        target[2] = Up_Panel.position;
        target[2].y += Up_Panel.GetComponent<RectTransform>().rect.height;

        target[3] = Up_Screen_Padding_Panel.position;
        target[3].y += Up_Screen_Padding_Panel.GetComponent<RectTransform>().rect.height;

        target[4] = TransparentUIButton.position;
        target[4].y -= Down_Screen_Padding_Panel.GetComponent<RectTransform>().rect.height;

        StartCoroutine(WaitLerpCoroutine(targetUI, current, target, 1.0f));
    }

    public void UpPanel() //최적화 필요
    {   
        Transform[] targetUI = {Down_Panel, Down_Screen_Padding_Panel, Up_Panel, Up_Screen_Padding_Panel, TransparentUIButton};

        Vector3[] current = {Down_Panel.position, Down_Screen_Padding_Panel.position, Up_Panel.position, Up_Screen_Padding_Panel.position, TransparentUIButton.position};
        
        Vector3[] target = new Vector3[targetUI.Length];

        target[0] = Down_Panel.position;
        target[0].y += Down_Panel.GetComponent<RectTransform>().rect.height;

        target[1] = Down_Screen_Padding_Panel.position;
        target[1].y += Down_Screen_Padding_Panel.GetComponent<RectTransform>().rect.height;

        target[2] = Up_Panel.position;
        target[2].y -= Up_Panel.GetComponent<RectTransform>().rect.height;

        target[3] = Up_Screen_Padding_Panel.position;
        target[3].y -= Up_Screen_Padding_Panel.GetComponent<RectTransform>().rect.height;

        target[4] = TransparentUIButton.position;
        target[4].y += Down_Screen_Padding_Panel.GetComponent<RectTransform>().rect.height;

        StartCoroutine(WaitLerpCoroutine(targetUI, current, target, 1.0f));
    }

    IEnumerator WaitLerpCoroutine(Transform[] targetUI, Vector3[] current, Vector3[] target, float time)
    {
        yield return StartCoroutine(lerpCoroutine(targetUI, current, target, 1.0f));
        isrunnigCoroutine = false;
        TransparentUSwitch();
    }

    IEnumerator lerpCoroutine(Transform[] targetUI, Vector3[] current, Vector3[] target, float time)
    {
        float elapsedTime = 0.0f;

        for(int i = 0; i < targetUI.Length; i++) {
            targetUI[i].transform.position = current[i];
        }
        
        while(elapsedTime < time)
        {
            elapsedTime += (Time.deltaTime);

            for(int i = 0; i < targetUI.Length; i++) 
                targetUI[i].transform.position = Vector3.Lerp(current[i], target[i], elapsedTime / time);
            yield return null;
        }
        for(int i = 0; i < targetUI.Length; i++) 
                targetUI[i].transform.position = target[i];
        yield return null;
    }
    #endregion
}