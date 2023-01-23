using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConsoleManager : MonoBehaviour
{
    public GameObject ConsolePanel;
    public bool isConsolePanelActive = true; //콘솔패널 활성화 여부 -> 스크린샷매니저에서 이용

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
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        UI_Position_Setting();
    }

    //테스트 버튼 위치 조절
    void UI_Position_Setting() 
    {
        int count = ConsolePanel.transform.childCount;
        int maxRow = 8;
        Vector3 position = ConsolePanel.transform.localPosition;
        position.x = 0;
        position.y = 0;

        for(int i = 0; i < count; i++) {
            if (i >= maxRow)
                break;

            float childPositionY = ConsolePanel.transform.GetChild(i).GetComponent<RectTransform>().rect.height;

            position.y += childPositionY;
        }
        ConsolePanel.transform.localPosition = position;

        ConsolePanel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        ConsolePanel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        ConsolePanel.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
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
}
