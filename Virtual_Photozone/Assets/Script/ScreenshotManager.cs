using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    //스크린샷에 필요한 오브젝트
    public GameObject blink;
    public GameObject uiPanel;
    public RectTransform ScreenPaddingPanel;
    public RectTransform upUIPanel;

    //스크린 비율
    public int currentRatio;    //현재 스크린 비율 //설정 바뀔 때만 변경
    public int screenRatio;     //스크린 비율

    // 코루틴 중복방지
    private bool isCoroutinePlaying;    

    // 파일 불러올 때 필요
    string albumName = "arTest";     // 생성될 앨범의 이름
    string fileName = "Ar_Photozone";

    [SerializeField]
    GameObject galleryPanel;    // 찍은 사진이 뜰 패널

    private static ScreenshotManager _instance = null;
    public static ScreenshotManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ScreenshotManager)) as ScreenshotManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    #region Screenshot Function
    public void Capture_Button()
    {
        // 중복방지 bool
        if (!isCoroutinePlaying)
        {
            StartCoroutine("captureScreenshot");
        }
    }

    IEnumerator captureScreenshot()
    {
        isCoroutinePlaying = true;

        //풀 스크린일 시 UI제거 필요
        if(currentRatio == 2)
        {
            uiPanel.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
        // 스크린샷 + 갤러리갱신
        Screenshot();
        yield return new WaitForEndOfFrame();

        // 블링크
        GameObject blinkObj;
        blinkObj = Instantiate(blink);
        blinkObj.transform.SetParent(uiPanel.transform.parent);    //Main UI Panel에 생성
        blinkObj.transform.localPosition = new Vector3(0, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(blinkObj);

        //사운드

        // 풀 스크린일 시 UI 복귀
        if (currentRatio == 2) 
            uiPanel.SetActive(true);  

        isCoroutinePlaying = false;
    }

    void Screenshot()
    {
        Texture2D texture = new Texture2D(Screen.width, screenRatio, TextureFormat.RGB24, false);

        if (currentRatio == 2)  //풀스크린일 때 상단 UI 계산 X
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        else
            texture.ReadPixels(new Rect(0, Screen.height - (screenRatio + upUIPanel.gameObject.GetComponent<RectTransform>().rect.height), Screen.width, screenRatio), 0, 0);

        texture.Apply();

        // 갤러리갱신
        NativeGallery.SaveImageToGallery(texture, albumName,
            fileName + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");

        Destroy(texture);
    }
    #endregion

    public void SetScreenRatio(int ratio)
    {
        currentRatio = ratio;

        switch (currentRatio)
        {
            case 0:
                screenRatio = Screen.width; // 1 : 1 비율
                break;
            case 1:
                screenRatio = (Screen.width * 4) / 3; // 3 : 4 비율
                break;
            case 2:
                screenRatio = Screen.height; // Full 비율
                break;
        }
    }
}