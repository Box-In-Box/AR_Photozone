using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnPlane : MonoBehaviour
{
    public string placedPrefabName;
    private Vector2 touchPosition;

    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private LayerMask placedObjectLayerMask;
    private Ray ray;
    private RaycastHit hit;

    void Update()
    {
        //화면 터치하지 않을 때 리턴
        if (!PlacedObjectManager.TryGetInputPosition(out touchPosition))
            return;

        //오브젝트 선택
        ray = arCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placedObjectLayerMask)) {
            PlacedObject.SelectedObject = hit.transform.GetComponentInChildren<PlacedObject>();
            return;
        }

        //오브젝트 선택이 아닐 경우 취소
        PlacedObject.SelectedObject = null;

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
    }
}
