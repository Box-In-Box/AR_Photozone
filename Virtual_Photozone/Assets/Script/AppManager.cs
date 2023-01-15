using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    //콘솔 텍스트
    public GameObject ConsolePanel;

    //스크린 비율 패딩 오브젝트
    public RectTransform Down_Screen_Padding_Panel;
    public RectTransform Up_Screen_Padding_Panel;
    public RectTransform upUIPanel; //비율 상단 기준점

    //스크린 비율
    public int currentRatio;    //현재 스크린 비율
    public int screenRatio;     //스크린 비율

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
        SetRatio(currentRatio);
    }

    public void AppSetting()
    {
        //앱 또는 서버에 저장 된 값 가져오기
        //화면비율
        //사운드
        //도감
        //로그인은 도감 드갈 때
    }

    #region common
    public void OpenPanel(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void QuitPanel(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Setting Panel
    public void ScreenRatio()
    {
        Color ColorA = Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color;

        switch (currentRatio)
        {
            case 0:
                screenRatio = Screen.width; // 1 : 1 비율
                ColorA.a = 1f;
                Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                Down_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                break;
            case 1:
                screenRatio = (Screen.width * 4) / 3; // 3 : 4 비율
                ColorA.a = 1f;
                Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                Down_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                break;
            case 2:
                screenRatio = (Screen.width * 4) / 3; // Full 비율
                ColorA.a = 0.1f;
                Up_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                Down_Screen_Padding_Panel.gameObject.GetComponent<Image>().color = ColorA;
                break;
        }

        Down_Screen_Padding_Panel.sizeDelta = new Vector2(0, Screen.height - (screenRatio + upUIPanel.gameObject.GetComponent<RectTransform>().rect.height));
    }

    public void SetRatio(int ratio)
    {
        currentRatio = ratio;
        ScreenRatio();
        ScreenshotManager.Instance.SetScreenRatio(ratio);
    }

    public void OtherPanelActiveFalse(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void SetRatioText_1_1(Text text) => text.text = "1 : 1";
    public void SetRatioText_3_4(Text text) => text.text = "3 : 4";
    public void SetRatioText_Full(Text text) => text.text = "Full";

    public void SetSoundText_1(Text text) => text.text = "Sound1";
    public void SetSoundText_2(Text text) => text.text = "Sound2";
    public void SetSoundText_3(Text text) => text.text = "Sound3";
    #endregion

    #region Ranking Panel
    public void SetLoginText(Text text) => text.text = "로그인";
    public void SetRegistText(Text text) => text.text = "회원가입";
    #endregion

    public IEnumerator PrintLog(string msg)
    {
        ConsolePanel.SetActive(true);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        yield return new WaitForSecondsRealtime(1.5f);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        ConsolePanel.SetActive(false);
    }
}