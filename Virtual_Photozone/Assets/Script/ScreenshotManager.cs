using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    #region data Fields
    public GameObject blink;
    private GameObject b;
    public GameObject uiPanel;
    public RectTransform ScreenPaddingPanel;
    public RectTransform upUIPanel;

    public int currentRatio;    //현재 스크린 비율 //사진 찍을 때 마다 AppManager 변수 잦은 호출로 같은 변수 따로 선언
    public int screenRatio;     //스크린 비율

    bool isCoroutinePlaying;    // 코루틴 중복방지

    // 파일 불러올 때 필요
    string albumName = "arTest";     // 생성될 앨범의 이름
    string fileName = "Ar_Photozone";

    [SerializeField]
    GameObject galleryPanel;    // 찍은 사진이 뜰 패널
    #endregion

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
    #endregion

    #region Capture Coroutine
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

        // 블링크 + 사운드
        Blink();
        Sound();
        yield return new WaitForSecondsRealtime(0.1f);
        DestroyBlink();

        // 풀 스크린일 시 UI 복귀
        if (currentRatio == 2)
        {
            uiPanel.SetActive(true);  
        }

        isCoroutinePlaying = false;
    }
    #endregion

    void Blink()
    {
        b = Instantiate(blink);
        b.transform.SetParent(uiPanel.transform.parent);    //Main UI Panel에 생성
        b.transform.localPosition = new Vector3(0, 0, 0);
        b.transform.localScale = new Vector3(10, 10, 10);
    }
    void Sound()
    {
       
    }

    void DestroyBlink()
    {
        if(b != null)
            Destroy(b);
    }

    #region Screenshot And GalleryUpdate
    void Screenshot()
    {
        // 스크린샷 **수정 필요 살짝 내려서 찍힘
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