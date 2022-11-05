using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    public RawImage img;
    public GameObject photoScreenPanel;


    private void Update()
    {
        //뒤로가기 누를 때 갤러리 들어가지는 버그 수정 필요
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (photoScreenPanel.activeSelf == true)
            {
                QuitGalleryPanel();
            }
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
    #endregion

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
        yield return null;
    }

    void ImageSizeSetting(RawImage img, float x, float y)
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
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        photoScreenPanel.SetActive(false);
    }
}
