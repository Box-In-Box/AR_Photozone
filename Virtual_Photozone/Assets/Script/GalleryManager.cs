using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    public RawImage img;
    public GameObject photoScreen;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (photoScreen.activeSelf == true)
            {
                QuitGallery();
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
        photoScreen.gameObject.SetActive(true);
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
        ImageSizeSetting(img, Screen.width, Screen.height);

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

    public void QuitGallery()
    {
        photoScreen.gameObject.SetActive(false);
    }
}
