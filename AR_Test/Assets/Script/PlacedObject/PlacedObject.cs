using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacedObject : MonoBehaviour
{
    [SerializeField]
    private GameObject selected;

    public bool IsSelected
    {
        get => SelectedObject == this;
    }

    private static PlacedObject selectedObject;
    public static PlacedObject SelectedObject
    {
        get => selectedObject;
        set{
            //같은 오브젝트 리턴
            if (selectedObject == value)
                return;
            
            //다른 오브젝트일 경우 이전 오브젝트 비활성화
            if (selectedObject != null) 
                selectedObject.selected.SetActive(false);

            //현재 오브젝트 저장
            selectedObject = value;
            
            //선택 활성화
            if (value != null)
                value.selected.SetActive(true);
        }
    }
}
