using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Sprite mapSprite;
    public RawImage mapView;
    public Transform MapLocation;

    private static MapManager _instance = null;
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(MapManager)) as MapManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    private void Start() {
        SettingMap();   //맵 해상도 변경
        SetMapLocationButton(); //변경된 해상도에 따라 포토존 위치 버튼 설정
    }

    void SettingMap()   //해상도에 따른 Map비율
    {
        mapView.texture = textureFromSprite(mapSprite);
        mapView.SetNativeSize();

        GalleryManager.Instance.ImageSizeSetting(mapView, 
            mapView.GetComponent<RectTransform>().rect.height, 
            mapView.GetComponent<RectTransform>().rect.width);
        
        mapView.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        mapView.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        mapView.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }    

    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if(sprite.rect.width != sprite.texture.width){
            Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
                                                        (int)sprite.textureRect.y, 
                                                        (int)sprite.textureRect.width, 
                                                        (int)sprite.textureRect.height );
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        } else
            return sprite.texture;
    }

    public void SetMapLocationButton()  //해상도 따라 버튼 위치 설정
    {
        int MapLengthX = (int)mapView.GetComponent<RectTransform>().rect.width;
        int MapLengthY = (int)mapView.GetComponent<RectTransform>().rect.height;

        int lengthX;
        int lengthY;

        GameObject mapLocationButton = null;

        for(int i =0; i < MapLocation.childCount; i++) {
            mapLocationButton = MapLocation.GetChild(i).gameObject;

            lengthX = mapLocationButton.GetComponent<MapLocationData>().lengthX;
            lengthY = mapLocationButton.GetComponent<MapLocationData>().lengthY;

            int movex = (int)(lengthX * MapLengthX / mapSprite.bounds.size.x);
            int movey = (int)(lengthY * MapLengthY / mapSprite.bounds.size.y);

            Vector3 vector = mapLocationButton.transform.position;
            vector.x += movex - mapLocationButton.GetComponent<RectTransform>().rect.width / 2;
            vector.y -= movey - mapLocationButton.GetComponent<RectTransform>().rect.height;

            mapLocationButton.transform.position = vector;
        }
    }
}
