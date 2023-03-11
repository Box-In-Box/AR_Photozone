using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestConsoleManager : MonoBehaviour
{
    public GameObject ConsolePanel;
    public GameObject DebugPanel;
    public Transform StructureObjectToggle;
    public Transform AnimalObjectToggle;
    public Transform LogTextOgject;
    public GameObject ObjectTogglePrfab;
    public GameObject TextLogPrfab;
    public bool isConsolePanelActive = true; //콘솔패널 활성화 여부 -> 스크린샷매니저에서 이용

    public string myLog;
 	Queue myLogQueue = new Queue ();

    public int testDeactivationRadius;
    public Transform testStructurePanel;
    public GameObject[] testStructureObject;
    public GameObject sdfsd;

    private static TestConsoleManager _instance = null;
    public static TestConsoleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TestConsoleManager)) as TestConsoleManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active TestConsoleManager object");
                }
            }
            return _instance;
        }
    }

    private void Awake() 
    {
        SettingTestObject();
    }

    void Start()
    {
        UI_Position_Setting();
        StartCoroutine(ShowTestStructureView());
        SettingDebugPanel();
        StartCoroutine(GpsObjectLog());
    }

    public void SettingTestObject()
    {
        int testStructureCount = testStructurePanel.childCount;

        if(testStructureCount != 0)
            testStructureObject = new GameObject[testStructureCount];

        for(int i = 0; i < testStructureCount; i++) {
            testStructureObject[i] = testStructurePanel.GetChild(i).gameObject;
        }
    }

    IEnumerator ShowTestStructureView()
    {
        yield return new WaitForSeconds(2f);
        while(true) {
            int objectDistance;

            for(int i = 0; i < testStructureObject.Length; i++) {
                objectDistance = (int)testStructureObject[i].GetComponent<ARLocation.PlaceAtLocation>().RawGpsDistance;
                /*if(i == 0) StartCoroutine(AppManager.Instance.PrintLog(objectDistance.ToString()));*/

                if(objectDistance > testDeactivationRadius) {
                    if(testStructureObject[i].transform.GetChild(0).gameObject.activeSelf == true)
                        testStructureObject[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else {
                    if(testStructureObject[i].transform.GetChild(0).gameObject.activeSelf == false)
                        testStructureObject[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //테스트 버튼 위치 조절
    void UI_Position_Setting() 
    {
        int count = ConsolePanel.transform.GetChild(0).childCount;
        int maxRow = 8;
        Vector3 position = ConsolePanel.transform.localPosition;
        position.x = 0;
        position.y = 0;

        for(int i = 0; i < count; i++) {
            if (i >= maxRow)
                break;

            float childPositionY = ConsolePanel.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().rect.height;

            position.y += childPositionY;
        }
        ConsolePanel.transform.localPosition = position;

        ConsolePanel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        ConsolePanel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        ConsolePanel.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);

        // width - login Btn - left spacing - right spacing
        DebugPanel.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width - 250 - 30 - 30);
        DebugPanel.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, position.y + ((count-1) * 30));
    }

    //랜덤 동물 추가
    public void Random_Add_Animal() 
    {
        int num = Random.Range(0, AnimalBookManager.Instance.animalNumber);

        AnimalBookManager.Instance.AddAnimal(AnimalBookManager.animalMsg + num.ToString());
    }

    //모든 동물 추가
    public void Add_All_Animal() 
    {
        for(int i = 0; i < AnimalBookManager.Instance.animalNumber; i++) {
            AnimalBookManager.Instance.AddAnimal(AnimalBookManager.animalMsg + i.ToString());
        }
    }

    public void OpenClosePanel(GameObject gameObject) => gameObject.SetActive(gameObject.activeSelf ? false : true);

    public void SettingDebugPanel()
    {
        for(int i = 0; i < MapManager.Instance.structureObject.Length; i++) {
            GameObject obj = Instantiate(ObjectTogglePrfab) as GameObject;
            obj.transform.SetParent(StructureObjectToggle.transform, false);
        }

        for(int i = 0; i < StructureObjectToggle.childCount; i++) {
            StructureObjectToggle.GetChild(i).GetChild(1).GetComponent<Text>().text = MapManager.Instance.structureObject[i].gameObject.GetComponent<LocationDescription>().locationDescription;
        }

        for(int i = 0; i < MapManager.Instance.animalObject.Length; i++) {
            GameObject obj = Instantiate(ObjectTogglePrfab) as GameObject;
            obj.transform.SetParent(AnimalObjectToggle.transform, false);
        }

        for(int i = 0; i < AnimalObjectToggle.childCount; i++) {
            AnimalObjectToggle.GetChild(i).GetChild(1).GetComponent<Text>().text = MapManager.Instance.animalObject[i].gameObject.GetComponent<LocationDescription>().locationDescription;
        }
    }

    public void AddConsoleLog(string msg)
    {
        GameObject obj = Instantiate(TextLogPrfab) as GameObject;
        obj.transform.SetParent(LogTextOgject.transform, false);
        obj.GetComponent<Text>().text = msg;
    }

    IEnumerator GpsObjectLog()
    {
        while(true) {
            for(int i = 0; i < StructureObjectToggle.childCount; i++) {
                StructureObjectToggle.GetChild(i).GetComponent<Toggle>().isOn = MapManager.Instance.structureObject[i].transform.GetChild(0).gameObject.activeSelf ? true : false;
                StructureObjectToggle.GetChild(i).GetChild(2).GetComponent<Text>().text = MapManager.Instance.structureObjectDistance[i].ToString();
            }
            
            for(int i = 0; i < AnimalObjectToggle.childCount; i++) {
                AnimalObjectToggle.GetChild(i).GetComponent<Toggle>().isOn = MapManager.Instance.animalObject[i].transform.GetChild(0).gameObject.activeSelf ? true : false;
                AnimalObjectToggle.GetChild(i).GetChild(2).GetComponent<Text>().text = MapManager.Instance.animalObjectDistance[i].ToString();
            }
            yield return null;
        }
    }
}
