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
    public Transform structureSubPanel;
    public Transform animalPanel;
    [Tooltip("포토존 구조물 활성화/비활성화 거리")]
    public int structureDeactivationRadius;
    [Tooltip("동물 구조물 활성화/비활성화 거리")]
    public int animalDeactivationRadius;
    
    [Header("-----MapCard-----")]
    public bool isShowStructureDistance;
    public int structureIndex = -1;
    public int mapDistance;
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
                    Debug.LogError("There's no active MapManager object");
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
        StartCoroutine(ShowStructureView()); //거리별 포토존 오브젝트 활성화/비활성화
        StartCoroutine(ShowAnimalView()); //거리별 동물 오브젝트 활성화/비활성화
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
        int structureSubCount = structureSubPanel.childCount;
        int animalCount = animalPanel.childCount;

        if(structureCount != 0)
            structureObject = new GameObject[structureCount + structureSubCount];

        if(animalCount != 0)
            animalObject = new GameObject[animalCount];

        for(int i = 0; i < structureCount; i++) {
            structureObject[i] = structurePanel.GetChild(i).gameObject;
        }

        for(int i = 0; i < structureSubCount; i++) {    //Sub Structure 포함
            structureObject[structureCount + i] = structureSubPanel.GetChild(i).gameObject;
        }

        for(int i = 0; i < animalCount; i++) {
            animalObject[i] = animalPanel.GetChild(i).gameObject;
        }
    }

    public void SetStructureInfo(int index)
    {
        isShowStructureDistance = true;
        structureIndex = index;
        GameObject go = EventSystem.current.currentSelectedGameObject;
        this.mapCardName.text = go.GetComponent<MapLocationData>().mapDescription;
        this.locationName.text = go.GetComponent<MapLocationData>().locationDescription;
        StartCoroutine(ShowDistanceCoroutine());
    }

    IEnumerator ShowDistanceCoroutine()
    {   
        while(isShowStructureDistance) {
            ShowStructureDistance();
            yield return new WaitForSeconds(1f);
        }
    }

    public void ShowStructureDistance()
    {
        if (structureIndex != -1)
            mapDistance = (int)structureObject[structureIndex].GetComponent<ARLocation.PlaceAtLocation>().RawGpsDistance; //RawGpsDistance와 비교 필요

        if (mapDistance < structureDeactivationRadius)
            distanceText.text = "포토존이\n 근처에 있습니다.";
        else
            distanceText.text = mapDistance + "m\n 떨어져있습니다.";
    }

    public void OffStructureDistance()
    {
        isShowStructureDistance = false;
        distanceText.text = "";
    }

    IEnumerator ShowStructureView()
    {
        while(true) {
            int objectDistance;

            for(int i = 0; i < structureObject.Length; i++) {   //포토존 오브젝트
                objectDistance = (int)structureObject[i].GetComponent<ARLocation.PlaceAtLocation>().RawGpsDistance;
                
                if(objectDistance > structureDeactivationRadius) {
                    if(structureObject[i].transform.GetChild(0).gameObject.activeSelf == true)
                        structureObject[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else {
                    if(structureObject[i].transform.GetChild(0).gameObject.activeSelf == false)
                        structureObject[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ShowAnimalView()
    {
        while(true) {
            int objectDistance;

            for(int i = 0; i < animalObject.Length; i++) {  //동물 오브젝트
                objectDistance = (int)animalObject[i].GetComponent<ARLocation.PlaceAtLocation>().RawGpsDistance;
                
                if(objectDistance > animalDeactivationRadius) {
                    if(animalObject[i].transform.GetChild(0).gameObject.activeSelf == true)
                        animalObject[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else {
                    if(animalObject[i].transform.GetChild(0).gameObject.activeSelf == false)
                        animalObject[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion
}