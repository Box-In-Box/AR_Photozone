using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

public class Test : MonoBehaviour
{
    #region data Fields
    string fileLocation = "mnt/sdcard/DCIM/AR_Photozone/";  //������ġ

    bool onCapture = false;
    #endregion

    #region Screenshot Function
    public void CaptureBtn()
    {
        if (onCapture == false)
        {
            StartCoroutine("CRSaveScreenshot");
        }
    }
    #endregion

    #region Capture Coroutine
    IEnumerator CRSaveScreenshot()
    {
        onCapture = true;

        yield return new WaitForEndOfFrame();

        #region Android Permission Setting
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
            {
                //���̾�α׸� ���� ������ �÷������� ����߾���. �� �ڵ�� �ּ� ó����.
                //AGAlertDialog.ShowMessageDialog("���� �ʿ�", "��ũ������ �����ϱ� ���� ����� ������ �ʿ��մϴ�.",
                //"Ok", () => OpenAppSetting(),
                //"No!", () => AGUIMisc.ShowToast("����� ��û ������"));

                // ������ Ȯ�� �˾��� ����� �������� OpenAppSetting()�� �ٷ� ȣ����.
                OpenAppSetting();

                onCapture = false;
                yield break;
            }
        }
        #endregion

        //Main Capture
        string filename = Application.productName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string finalLOC = fileLocation + filename;

        if (!Directory.Exists(fileLocation))
        {
            Directory.CreateDirectory(fileLocation);
        }

        byte[] imageByte; //��ũ������ Byte�� ����.Texture2D use 
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();

        imageByte = tex.EncodeToPNG();
        DestroyImmediate(tex);

        File.WriteAllBytes(finalLOC, imageByte);

        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + finalLOC) });
        objActivity.Call("sendBroadcast", objIntent);

        //������ ���� �̸�����
    }
    #endregion

    #region Android App Setting
    private void OpenAppSetting()
    {
        try
        {
#if UNITY_ANDROID
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string packageName = currentActivityObject.Call<string>("getPackageName");

                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    #endregion
}
