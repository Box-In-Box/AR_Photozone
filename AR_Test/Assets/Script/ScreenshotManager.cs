using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    [Header("-----Screenshot Object-----")]
    public GameObject blink;
    public GameObject uiPanel;
    public RectTransform ScreenPaddingPanel;
    public RectTransform upUIPanel;

    [Header("-----Screen Ratio-----")]
    public int screenRatio;     //찍을 때마다 appManager에서 가져오는것은 비효율
    public int screenWidth;     //스크린 비율

    [Header("-----Shutter Sound-----")]
    public AudioSource audioSource;
    public int shutterSound;    //셔터음
    public AudioClip[] shutterSoundList;

    [Header("-----Etc-----")]
    public bool ismirror;
    private bool isCoroutinePlaying;    
    string albumName = "arTest";     // 생성될 앨범의 이름
    string fileName = "Ar_Photozone";
    public GameObject TransparentUI;
    public GameObject ResetARUI;

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
                    Debug.LogError("There's no active ScreenshotManager object");
                }
            }
            return _instance;
        }
    }

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

        //풀 스크린일 시 UI 제거 필요
        if(screenRatio == 2)
        {
            uiPanel.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
        //콘솔 패널 UI 제거
        if(TestConsoleManager.Instance.isConsolePanelActive == true)
        {
            TestConsoleManager.Instance.ConsolePanel.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
        //클린UI 버튼 제거
        TransparentUI.SetActive(false);
        ResetARUI.SetActive(false);
        yield return new WaitForEndOfFrame();

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

        //셔터음
        ShutterSound();

        //풀 스크린일 시 UI 복귀
        if (screenRatio == 2) 
            uiPanel.SetActive(true);
        //콘솔 패널 UI 복귀
        if (TestConsoleManager.Instance.isConsolePanelActive == true)
            TestConsoleManager.Instance.ConsolePanel.SetActive(true);
        //클린UI 버튼 복귀
        TransparentUI.SetActive(true);
        ResetARUI.SetActive(true);

        isCoroutinePlaying = false;
    }

    #region Screenshot
    void Screenshot()
    {
        Texture2D texture = new Texture2D(Screen.width, screenWidth, TextureFormat.RGB24, false);

        if (screenRatio == 2)  //풀스크린일 때 상단 UI 계산 X
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        else
            texture.ReadPixels(new Rect(0, Screen.height - (screenWidth + upUIPanel.gameObject.GetComponent<RectTransform>().rect.height), Screen.width, screenWidth), 0, 0);

        texture.Apply();

        if(ismirror) //미러모드
            texture = FlipTexture(texture);

        // 갤러리갱신
        NativeGallery.SaveImageToGallery(texture, albumName,
            fileName + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");

        Destroy(texture);
    }

    Texture2D FlipTexture(Texture2D texture)
    {
        Texture2D flipped = new Texture2D(texture.width,texture.height);
         
            int textureWidth = texture.width;
            int textureHeight = texture.height;
         
           for(int i=0;i<textureWidth;i++){
              for(int j=0;j<textureHeight;j++)
                  flipped.SetPixel(textureWidth-i-1, j, texture.GetPixel(i,j));
           }
            flipped.Apply();

            return flipped;
    }
    #endregion

    #region Shutter Sound
    void ShutterSound() 
    {
        audioSource.Play();
    }

    public void SetShutterSound(int sound) 
    {
        shutterSound = sound;
        audioSource.clip = shutterSoundList[shutterSound];
    }
    #endregion

    #region Screen Ratio
    public void SetScreenRatio(int ratio)
    {
        screenRatio = ratio;

        switch (screenRatio)
        {
            case 0:
                screenWidth = Screen.width; // 1 : 1 비율
                break;
            case 1:
                screenWidth = (Screen.width * 4) / 3; // 3 : 4 비율
                break;
            case 2:
                screenWidth = Screen.height; // Full 비율
                break;
        }
    }
    #endregion

    public void SetMirrorMode(bool mirror)
    {
        ismirror = mirror;
    }
}