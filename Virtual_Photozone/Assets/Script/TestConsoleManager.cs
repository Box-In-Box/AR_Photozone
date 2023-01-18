﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConsoleManager : MonoBehaviour
{
    public GameObject ConsolePanel;
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
        int num = Random.Range(0, AnimalBookManager.AnimalNumber);

        AnimalBookManager.Instance.AddAnimal(AnimalBookManager.AnimalMsg + num.ToString());
    }
}