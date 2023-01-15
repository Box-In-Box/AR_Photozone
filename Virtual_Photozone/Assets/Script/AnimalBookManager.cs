using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class AnimalBookManager : MonoBehaviour
{
    public static int AnimalCount = 33;
    public int CollectedAnimalCount = 0;

    public GameObject AnimalPrefab;
    public Text AnimalCountText;
    public Transform AnimalContentParent;

    public bool[] isFound;

    private static AnimalBookManager _instance = null;
    public static AnimalBookManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(AnimalBookManager)) as AnimalBookManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        isFound = new bool[AnimalCount];
    }

    public void Setting() //처음만 실행
    {
        CollectedAnimalCount = 0; //여기서 오류 발생
        for (int i = 0; i < AnimalCount; i++)
        {
            PlayfabManager.Instance.GetAnimal("Animal_" + i.ToString());
            GameObject go = Instantiate(AnimalPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("AnimalBookImg/" + "Animal_" + i.ToString(), typeof(Sprite)) as Sprite;
            go.GetComponent<Image>().color = new Color(0, 0, 0);
            go.transform.SetParent(AnimalContentParent);
        }
    }

    public void AddAnimal(string msg)   //도감 등록
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false)   //중복 방지
        {
            isFound[num] = true;
            PlayfabManager.Instance.SaveAnimal(msg);

            GameObject go = AnimalContentParent.GetChild(num).gameObject;
            go.GetComponent<Image>().color = new Color(255, 255, 255);

            CollectedAnimalCount++;
            ShowCount();
        }
        else
        {
            StartCoroutine(AppManager.Instance.PrintLog(msg + "은(는) 이미 등록되었습니다."));
        }
    }

    public void ShowCount()
    {
        AnimalCountText.GetComponent<Text>().text = "수집한 동물의 수 : " + CollectedAnimalCount.ToString() + " / " + AnimalCount.ToString();
    }
}
