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

    [SerializeField] 
    public Camera arCamera;

    public GameObject ConsolePanel;

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
                    Debug.LogError("There's no active ManagerClass object");
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

            if (Physics.Raycast(ray, out hitobj))
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
                    Touched = true;
                    return;
                }
                else if (hitobj.collider.tag.Equals("Description"))
                {
                    //아마 구조물 옆 별 이미지로 해당 위치 설명이 들어갈 듯
                    touchObj = hitobj.collider.gameObject;
                    DescriptionTouch(touchObj);
                    Touched = true;
                    return;
                }
            }
        }
        
        if (touch.phase == TouchPhase.Ended)
        {
            Touched = false;
        }
        /* 구조물 위치 변경 안됨, 수정 필요 */
        if (raycastMgr.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            if (Touched)
            {
                touchObj.transform.position = hits[0].pose.position;
            }
        }
    }
    void StructureTouch(GameObject touchObj)
    {
        StartCoroutine(PrintText(touchObj.gameObject.name + " 구조물을 발견"));
        //구조물 은 아마 위치 변경 가능하게?
    }

    void FindAnimalTouch(GameObject touchObj)
    {
        StartCoroutine(PrintText(touchObj.gameObject.name + " 동물을 발견"));
        //animal 도감 추가
    }

    void DescriptionTouch(GameObject touchObj)
    {
        StartCoroutine(PrintText(touchObj.gameObject.name + " 설명"));
        //해당 위치나 건물 설명
    }

    IEnumerator PrintText(string msg)
    {
        ConsolePanel.SetActive(true);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        yield return new WaitForSecondsRealtime(2f);
        ConsolePanel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        ConsolePanel.SetActive(false);
    }
}