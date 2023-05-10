using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacedObjectManager : MonoBehaviour
{
    public Sprite normalmgTap;
    public Sprite selectedImgTap;

    public void SetNormalmgTap(GameObject go) => go.GetComponent<Image>().sprite = normalmgTap;
    public void SetselectedImgTap(GameObject go) => go.GetComponent<Image>().sprite = selectedImgTap;

    private static ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private PlaceOnPlane placeOnPlane;
    public GameObject placedObjectPanel;

    public Button RemoveObjectBtn;


    //PlacedObject Panel
    public const byte HUMAN = 0, FOOD = 1, ETC = 2;

    public int humanCount;
    public int foodCount;
    public int etcCount;

    public static string foodMsg = "Food_";
    public static string humanMsg = "Human_";
    public static string etcMsg = "ETC_";
    public Transform[] placedObjectContent;

    /**
    Resources - PlacedObjectImg, Resources - PlacedObject 추가
    PlacedObject는 자식으로 slected이름으로 같은 프리팹 추가 후 placed Object에 자식 오브젝트 추가
    자식 오브젝트는 비활성화
    layer -> PlacedObject 자식 모두 변경
    인스펙터창에서 각 수 지정
    **/

    private static PlacedObjectManager _instance = null;
    public static PlacedObjectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlacedObjectManager)) as PlacedObjectManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active GalleryManager object");
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        raycastManager = GameObject.FindObjectOfType<ARRaycastManager>();
        placeOnPlane = GameObject.FindObjectOfType<PlaceOnPlane>();
        
        StartCoroutine(PlacedObjectSetting());
    }

    IEnumerator PlacedObjectSetting()
    {
        string objName;
        //Human setting
        for (int i = 0; i < humanCount; i++)
        {
            objName = humanMsg + i.ToString();
            GameObject go = Instantiate(Resources.Load("Item_Slot"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("PlacedObjectImg/" + "Human/" + objName, typeof(Sprite)) as Sprite;
            go.GetComponent<Button>().onClick.AddListener(() => placeOnPlane.SetPlacedPrefabName());
            go.GetComponent<Button>().onClick.AddListener(() => placedObjectPanel.SetActive(false));
            go.transform.SetParent(placedObjectContent[HUMAN]);
            go.name = objName;
        }
        yield return null;

        //Food setting
        for (int i = 0; i < foodCount; i++)
        {
            objName = foodMsg + i.ToString();
            GameObject go = Instantiate(Resources.Load("Item_Slot"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("PlacedObjectImg/" + "Food/" + objName, typeof(Sprite)) as Sprite;
            go.GetComponent<Button>().onClick.AddListener(() => placeOnPlane.SetPlacedPrefabName());
            go.GetComponent<Button>().onClick.AddListener(() => placedObjectPanel.SetActive(false));
            go.transform.SetParent(placedObjectContent[FOOD]);
            go.name = objName;
        }
        yield return null;

        //ETC setting
        for (int i = 0; i < etcCount; i++)
        {
            objName = etcMsg + i.ToString();
            GameObject go = Instantiate(Resources.Load("Item_Slot"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("PlacedObjectImg/" + "ETC/" + objName, typeof(Sprite)) as Sprite;
            go.GetComponent<Button>().onClick.AddListener(() => placeOnPlane.SetPlacedPrefabName());
            go.GetComponent<Button>().onClick.AddListener(() => placedObjectPanel.SetActive(false));
            go.transform.SetParent(placedObjectContent[ETC]);
            go.name = objName;
        }
        yield return null;
    }

    public static bool Raycast(Vector2 screenPosition, out Pose pose)
    {
        if (raycastManager.Raycast(screenPosition, hits, TrackableType.AllTypes)) {
            pose = hits[0].pose;
            return true;
        }
        else {
            pose = Pose.identity;
            return false;
        }
    }

    public static bool TryGetInputPosition(out Vector2 position)
    {
        position = Vector2.zero;

        if (Input.touchCount == 0)
            return false;
        
        position = Input.GetTouch(0).position;

        if (Input.GetTouch(0).phase != TouchPhase.Began) 
            return false;

        return true;
    }

    public void SetRemoveObjectBtn(GameObject target)
    {
        if (target == null) {
            RemoveObjectBtn.onClick.RemoveAllListeners();
            RemoveObjectBtn.gameObject.SetActive(false);
            return;
        }

        RemoveObjectBtn.onClick.RemoveAllListeners();
        RemoveObjectBtn.onClick.AddListener( ()=> Destroy(target));
        RemoveObjectBtn.onClick.AddListener( ()=> TestConsoleManager.Instance.AddConsoleLog(target.transform.gameObject.ToString() + "오브젝트 삭제"));
        RemoveObjectBtn.onClick.AddListener( ()=> RemoveObjectBtn.gameObject.SetActive(false));

        RemoveObjectBtn.gameObject.SetActive(true);
    }
}