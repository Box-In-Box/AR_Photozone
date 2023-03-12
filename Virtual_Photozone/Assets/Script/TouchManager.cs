using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    private ARRaycastManager raycastMgr;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool Touched = false;

    private GameObject touchObj;

    private float moveSpeed = 0.5f;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;

    [SerializeField] 
    public Camera arCamera;

    private static TouchManager _instance = null;
    public static TouchManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TouchManager)) as TouchManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active TouchManager object");
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        raycastMgr = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray;
            RaycastHit hitobj;

            ray = arCamera.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out hitobj) && Touched == false)
            {
                if (hitobj.collider.tag.Equals("Structure"))
                {
                    touchObj = hitobj.collider.gameObject;
                    StructureTouch(touchObj);
                    Touched = true;
                }
                else if (hitobj.collider.tag.Equals("Animal"))
                {
                    touchObj = hitobj.collider.gameObject;
                    FindAnimalTouch(touchObj);
                    return;
                }
                else if (hitobj.collider.tag.Equals("Description"))
                {
                    //아마 구조물 옆 별 이미지로 해당 위치 설명이 들어갈 듯
                    touchObj = hitobj.collider.gameObject;
                    DescriptionTouch(touchObj);
                    return;
                }
            }
            prePos = touch.position - touch.deltaPosition;
        }
        
        if (touch.phase == TouchPhase.Ended)
        {
            Touched = false;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            nowPos = touch.position - touch.deltaPosition;
            movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * moveSpeed;
            float temp = movePos.y; //좌우 앞 뒤이동만
            movePos.y = movePos.z;
            movePos.z = temp;
            touchObj.transform.Translate(movePos);
            prePos = touch.position - touch.deltaPosition;
        }
    }
    void StructureTouch(GameObject touchObj)
    {
        AppManager.Instance.PrintConsoleText(touchObj.gameObject.name + " 구조물 발견");
    }

    void FindAnimalTouch(GameObject touchObj)
    {
        AnimalBookManager.Instance.AddAnimal(touchObj.gameObject.GetComponent<ObjectData>().ID_Name);
    }

    void DescriptionTouch(GameObject touchObj)
    {   
        AppManager.Instance.PrintConsoleText(touchObj.gameObject.name + " 설명");
    }
}