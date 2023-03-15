using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    public RawImage img;
    public GameObject photoScreenPanel;

    [SerializeField] private RectTransform _zoomTargetRt;
 
    private readonly float _ZOOM_IN_MAX = 16f;
    private readonly float _ZOOM_OUT_MAX = 1f;
    private readonly float _ZOOM_SPEED = 1.5f;
    
    private bool _isZooming = false;

    private static GalleryManager _instance = null;
    public static GalleryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GalleryManager)) as GalleryManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active GalleryManager object");
                }
            }
            return _instance;
        }
    }

    private void Update()
    {
        
        //뒤로가기 누를 때 갤러리 들어가지는 버그 수정 필요
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (photoScreenPanel.activeSelf == true) {
                QuitGalleryPanel();
            }
        }

        if (Input.touchCount == 2) {
            ZoomAndPan();
        }
        else {
            _isZooming = false;
        }
    }

    #region Gallery
    public void Gallery()
    {
        NativeGallery.GetImageFromGallery((file) => {
            FileInfo selectedImage = new FileInfo(file);

            if (selectedImage.Length > 500000000)
                return;

            if (!string.IsNullOrEmpty(file))
                StartCoroutine(LoadImage(file));
        });
        photoScreenPanel.gameObject.SetActive(true);
    }

    IEnumerator LoadImage(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath).Split('.')[0];
        string saveImagePath = Application.persistentDataPath + "/Image";

        if (Directory.Exists(saveImagePath))
            Directory.CreateDirectory(saveImagePath);

        File.WriteAllBytes(saveImagePath + fileName + ".png", fileData);

        var tempfile = File.ReadAllBytes(filePath);

        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(tempfile);

        img.texture = texture;
        img.SetNativeSize();
        ImageSizeSetting(img
            , photoScreenPanel.GetComponent<RectTransform>().rect.width
            , photoScreenPanel.GetComponent<RectTransform>().rect.height);

        img.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        img.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        img.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        _zoomTargetRt = img.rectTransform;
        yield return null;
    }

    public void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if(x / y > imgX / imgY)
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (y / imgY));
        }
        else
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (x / imgX));
        }
    }

    public void QuitGalleryPanel()
    {
        img.texture = null;
        _zoomTargetRt = null;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        photoScreenPanel.SetActive(false);
    }
    #endregion
    
    private void ZoomAndPan()
    {
        if (_isZooming == false)
        {
            _isZooming = true;
        }
 
        /* get zoomAmount */
        var prevTouchAPos = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
        var prevTouchBPos = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;
        var curTouchAPos = Input.GetTouch(0).position;
        var curTouchBPos = Input.GetTouch(1).position;
        var deltaDistance =
            Vector2.Distance(Normalize(curTouchAPos), Normalize(curTouchBPos))
            - Vector2.Distance(Normalize(prevTouchAPos), Normalize(prevTouchBPos));
        var currentScale = _zoomTargetRt.localScale.x;
        var zoomAmount = deltaDistance * currentScale * _ZOOM_SPEED; // zoomAmount == deltaScale
 
        /* clamp & zoom */
        var zoomedScale = currentScale + zoomAmount;
        if (zoomedScale < _ZOOM_OUT_MAX)
        {
            zoomedScale = _ZOOM_OUT_MAX;
            zoomAmount = 0f;
        }
        if (_ZOOM_IN_MAX < zoomedScale)
        {
            zoomedScale = _ZOOM_IN_MAX;
            zoomAmount = 0f;
        }
        _zoomTargetRt.localScale = zoomedScale * Vector3.one;
 
        /* apply offset */
        // offset is a value against movement caused by scale up & down
        var pivotPos = _zoomTargetRt.anchoredPosition;
        var fromCenterToInputPos = new Vector2(
                Input.mousePosition.x - Screen.width * 0.5f,
                Input.mousePosition.y - Screen.height * 0.5f);
        var fromPivotToInputPos = fromCenterToInputPos - pivotPos;
        var offsetX = (fromPivotToInputPos.x / zoomedScale) * zoomAmount;
        var offsetY = (fromPivotToInputPos.y / zoomedScale) * zoomAmount;
        _zoomTargetRt.anchoredPosition -= new Vector2(offsetX, offsetY);
 
        /* get moveAmount */
        var deltaPosTouchA = Input.GetTouch(0).deltaPosition;
        var deltaPosTouchB = Input.GetTouch(1).deltaPosition;
        var deltaPosTotal = (deltaPosTouchA + deltaPosTouchB) * 0.5f;
        var moveAmount = new Vector2(deltaPosTotal.x, deltaPosTotal.y);
 
        /* clamp & pan */
        var clampX = (Screen.width * zoomedScale - Screen.width) * 0.5f;
        var clampY = (Screen.height * zoomedScale - Screen.height) * 0.5f;
        var clampedPosX = Mathf.Clamp(_zoomTargetRt.localPosition.x + moveAmount.x, -clampX, clampX);
        var clampedPosY = Mathf.Clamp(_zoomTargetRt.localPosition.y + moveAmount.y, -clampY, clampY);
        _zoomTargetRt.anchoredPosition = new Vector3(clampedPosX, clampedPosY);
    }

    private Vector2 Normalize(Vector2 position)
    {
        var normlizedPos = new Vector2(
            (position.x - Screen.width * 0.5f) / (Screen.width * 0.5f),
            (position.y - Screen.height * 0.5f) / (Screen.height * 0.5f));
        return normlizedPos;
    }
}