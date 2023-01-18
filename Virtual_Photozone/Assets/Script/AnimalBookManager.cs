using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class AnimalBookManager : MonoBehaviour
{
    public static int AnimalNumber = 33;
    public static string AnimalMsg = "Animal_";
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

    public void Setting() //처음만 실행(로그인 시)
    {
        isFound = new bool[AnimalNumber];

        CollectedAnimalCount = 0;
        for (int i = 0; i < AnimalNumber; i++)
        {
            PlayfabManager.Instance.GetAnimal("Animal_" + i.ToString());
            GameObject go = Instantiate(AnimalPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("AnimalBookImg/" + AnimalMsg + i.ToString(), typeof(Sprite)) as Sprite;
            go.GetComponent<Image>().color = new Color(0, 0, 0);
            go.transform.SetParent(AnimalContentParent);
        }
    }

    public void AddAnimal(string msg)   //서버, 로컬도감 등록
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false)   //중복 방지
        {
            //로컬, 서버등록
            isFound[num] = true;
            PlayfabManager.Instance.SaveAnimal(msg);

            ChangeCollectedImg(num);

            AddAnimalCount();
        }
        else    //이미 수집 되었을 때
        {
            StartCoroutine(AppManager.Instance.PrintLog(msg + "은(는) 이미 등록되었습니다."));
        }
    }

    public void SetAnimal(string msg)   //로컬 도감 등록
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false)   //중복 방지
        {
            //로컬등록
            isFound[num] = true;

            ChangeCollectedImg(num);

            AddAnimalCount();
        }
    }

    //수집된 동물 이미지 수정(그림자 > 원본)
    public void ChangeCollectedImg(int num)
    {
        GameObject go = AnimalContentParent.GetChild(num).gameObject;
        go.GetComponent<Image>().color = new Color(255, 255, 255);
    }

    //수집된 동물 카운트 증가 + 텍스트 변경
    public void AddAnimalCount() 
    {
        CollectedAnimalCount++;
        AnimalCountText.GetComponent<Text>().text = "수집한 동물의 수 : " + CollectedAnimalCount.ToString() + " / " + AnimalNumber.ToString();
    }
}
