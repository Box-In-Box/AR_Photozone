using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaceOnPlane : MonoBehaviour
{
    private ARPlaneManager planeManager;
    public string placedPrefabName;
    private Vector2 touchPosition;

    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private LayerMask placedObjectLayerMask;
    [SerializeField]
    private LayerMask removeLayerMask;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        planeManager.enabled = false;
    }

    void Update()
    {
        //화면 터치하지 않을 때 리턴
        if (!PlacedObjectManager.TryGetInputPosition(out touchPosition))
            return;

        //오브젝트 선택, 생성 중일 때는 선택 불가
        ray = arCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placedObjectLayerMask) && !PlacedObjectManager.Instance.placedObjectPanel.activeSelf) {
            PlacedObject.SelectedObject = hit.transform.GetComponent<PlacedObject>();
            
            //선택 오브젝트는 선택 불가 부모만 선택 후 모두 삭제
            if (hit.transform.gameObject.name != "slected")
                PlacedObjectManager.Instance.SetRemoveObjectBtn(hit.transform.gameObject);
            else if (hit.transform.gameObject.name == "slected") 
                PlacedObjectManager.Instance.SetRemoveObjectBtn(null);
            return;
        }
        
        //오브젝트 생성
        if (placedPrefabName != "" && PlacedObjectManager.Raycast(touchPosition, out Pose hitPose)) {
            string prefabLocation = "PlacedObject/";

            if (placedPrefabName.Contains("Food"))
                prefabLocation += "Food/";
            else if (placedPrefabName.Contains("Props"))
                prefabLocation += "Props/";
            else if (placedPrefabName.Contains("ETC"))
                prefabLocation += "ETC/";
                
            Instantiate(Resources.Load(prefabLocation + placedPrefabName), hitPose.position, hitPose.rotation);
            placedPrefabName = "";
            planeManager.enabled = false;
            SetAllPlanesActive(false);
        } 
    }

    public void SetPlacedPrefabName(string name)
    {
        StartCoroutine(Inputdelay(name));
    }

    //터치됨과 동시에 생성 방지
    IEnumerator Inputdelay(string name)
    {
        yield return new WaitForSeconds(0.3f);
        placedPrefabName = name;
        planeManager.enabled = true;
        SetAllPlanesActive(true);
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in planeManager.trackables) {
            plane.gameObject.SetActive(value);
        }
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touchPos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    } 
}
