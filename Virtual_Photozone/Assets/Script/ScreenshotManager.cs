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

    bool isCoroutinePlaying;    // 코루틴 중복방지

    // 파일 불러올 때 필요
    string albumName = "arTest";     // 생성될 앨범의 이름
    string fileName = "Ar_Photozone";

    [SerializeField]
    GameObject galleryPanel;    // 찍은 사진이 뜰 패널
    #endregion

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

        // UI 제거 //풀 스크린일 시 UI제거 필요
        uiPanel.SetActive(false);
        yield return new WaitForEndOfFrame();

        // 스크린샷 + 갤러리갱신
        Screenshot();
        yield return new WaitForEndOfFrame();

        // 블링크 + 사운드
        Blink();
        Sound();
        yield return new WaitForSecondsRealtime(0.1f);
        DestroyBlink();

        // UI 복귀
        uiPanel.SetActive(true);
        isCoroutinePlaying = false;
    }
    #endregion

    void Blink()
    {
        b = Instantiate(blink);
        b.transform.SetParent(transform);
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
        int screenRatio = (Screen.width * 4) / 3;   // 3 : 4 비율 //AppManager ScreenPadding과 연관

        // 스크린샷
        Texture2D texture = new Texture2D(Screen.width, screenRatio, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, Screen.height - (screenRatio + upUIPanel.gameObject.GetComponent<RectTransform>().rect.height), Screen.width, screenRatio), 0, 0);
        texture.Apply();

        // 갤러리갱신
        NativeGallery.SaveImageToGallery(texture, albumName,
            fileName + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");

        Destroy(texture);
    }
    #endregion
}