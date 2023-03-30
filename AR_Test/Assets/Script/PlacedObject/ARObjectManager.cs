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

    public int foodCount = 1;
    public int propsCount = 0;
    public int etcCount = 0;

    public static string foodMsg = "Food_";
    public Transform foodContent;

    void Start()
    {
        raycastManager = GameObject.FindObjectOfType<ARRaycastManager>();
        placeOnPlane = GameObject.FindObjectOfType<PlaceOnPlane>();
        
        StartCoroutine(PlacedObjectSetting());
    }

    IEnumerator PlacedObjectSetting()
    {
        string objName;
        for (int i = 0; i < foodCount; i++)
        {
            objName = foodMsg + i.ToString(); //온클릭 추가에서 ++된 상태로 삽입되는 버그로 추가됨
            GameObject go = Instantiate(Resources.Load("Item_Slot"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("PlacedObjectImg/" + "Food/" + objName, typeof(Sprite)) as Sprite;
            go.GetComponent<Button>().onClick.AddListener(() => placeOnPlane.SetPlacedPrefabName(objName));
            go.GetComponent<Button>().onClick.AddListener(() => placedObjectPanel.SetActive(false));
            go.transform.SetParent(foodContent);
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
}