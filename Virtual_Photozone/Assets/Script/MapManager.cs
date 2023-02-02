using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
    public Sprite mapSprite;
    public RawImage mapView;
    public Transform MapLocation;

    [Header("-----GpsObject-----")]
    public Transform structurePanel;
    public Transform animalPanel;
    public bool isShowStructureDistance;
    public int structureIndex = -1;
    public int distance;

    [Header("-----MapCard-----")]
    public Text mapCardName;
    public Text locationName;
    public Text distanceText;

    [Space(10f)]
    public GameObject[] structureObject;
    public GameObject[] animalObject;

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
    private void Awake() {
        SettingGpsObject(); //Gps 오브젝트 Get 컴포넌트 
    }

    private void Start() {
        SettingMap();   //맵 해상도 변경
        SetMapLocationButton(); //변경된 해상도에 따라 포토존 위치 버튼 설정
    }

    private void Update() {
        if(isShowStructureDistance == false)
            return;
        
        ShowStructureDistance();
    }

    #region Map Image setting
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

    public Texture2D textureFromSprite(Sprite sprite)
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

            //for button Moving  (position -> isSetPosition 순서)
            mapLocationButton.GetComponent<MapLocationData>().position = vector;
            mapLocationButton.GetComponent<MapLocationData>().isSetPosition = true;
        }
    }
    #endregion

    #region Gps Object Setting
    public void SettingGpsObject()
    {
        int structureCount = structurePanel.childCount;
        int animalCount = animalPanel.childCount;

        if(structureCount != 0)
            structureObject = new GameObject[structureCount];

        if(animalCount != 0)
            animalObject = new GameObject[animalCount];

        for(int i = 0; i < structureCount; i++) {
            structureObject[i] = structurePanel.GetChild(i).gameObject;
        }

        for(int i = 0; i < animalCount; i++) {
            animalObject[i] = animalPanel.GetChild(i).gameObject;
        }
    }

    public void SetStructureInfo(int index)
    {
        structureIndex = index;
        GameObject go = EventSystem.current.currentSelectedGameObject;
        this.mapCardName.text = go.GetComponent<MapLocationData>().mapDescription;
        this.locationName.text = go.GetComponent<MapLocationData>().locationDescription;
    }

    public void SetStructureDistance()
    {
        isShowStructureDistance = true;
    }

    public void ShowStructureDistance()
    {
        if (structureIndex != -1)
            distance = (int)structureObject[structureIndex].GetComponent<ARLocation.PlaceAtLocation>().RawGpsDistance; //RawGpsDistance와 비교 필요

        if (distance < 10)
            distanceText.text = "포토존이\n 근처에 있습니다.";
        else
            distanceText.text = distance + "m\n 떨어져있습니다.";
    }

    public void OffStructureDistance()
    {
        isShowStructureDistance = false;
        distanceText.text = "";
        AppManager.Instance.ConsoleText.text = "";
    }
    #endregion
}