using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class AnimalBookManager : MonoBehaviour
{
    [Header("-----Animal Book-----")]
    public int animalNumber = 33;
    public int collectedAnimalCount = 0;
    public static string animalMsg = "Animal_";

    public GameObject animalPrefab;
    public Text animalCountText;
    public Transform animalContentParent;
    public GameObject animalCard;
    public Text animalCardName;
    public Text collectedTime;
    public Button loginFromAnimalBook;

    [Header("-----Animal Ranking-----")]
    public Text animalRankingText;
    public Button loginFromAnimalRanking;

    [Space(10f)]
    public bool[] isFound;
    public string[] getTime;

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
        isFound = new bool[animalNumber];
        getTime = new string[animalNumber];
        collectedAnimalCount = 0;
        animalCountText.text = "돌아다니며 동물을 수집해보세요!";
        animalRankingText.text = "";
        loginFromAnimalBook.gameObject.SetActive(false);
        loginFromAnimalRanking.gameObject.SetActive(false);

        for (int i = 0; i < animalNumber; i++)
        {
            PlayfabManager.Instance.GetAnimal("Animal_" + i.ToString());
            GameObject go = Instantiate(animalPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("AnimalBookImg/" + animalMsg + i.ToString(), typeof(Sprite)) as Sprite;
            go.GetComponent<Image>().color = new Color(0, 0, 0);
            go.transform.SetParent(animalContentParent);
        }
    }

    public void AddAnimal(string msg)   //서버, 로컬도감 등록 ***로그인 후 실행되도록 수정 필요***
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false)   //중복 방지
        {
            //로컬, 서버등록
            isFound[num] = true;

            string time = DateTime.Now.ToString(("yyyy-MM-dd_HH_mm"));
            getTime[num] = time;
            PlayfabManager.Instance.SaveAnimal(msg, time);

            ChangeCollectedImg(num);

            AddAnimalCount();
        }
        else    //이미 수집 되었을 때
        {
            StartCoroutine(AppManager.Instance.PrintLog(msg + "은(는) 이미 등록되었습니다."));
        }
    }

    public void SetAnimal(string msg, string time)   //로컬 도감 등록
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false)   //중복 방지
        {
            //로컬등록
            isFound[num] = true;
            getTime[num] = time;

            ChangeCollectedImg(num);
            AddAnimalCount();
        }
    }

    //수집된 동물 이미지 수정(그림자 > 원본)
    public void ChangeCollectedImg(int num)
    {
        GameObject go = animalContentParent.GetChild(num).gameObject;
        go.GetComponent<Image>().color = new Color(255, 255, 255);

        //수집된 동물은 동물카드 열람 가능
        go.GetComponent<Button>().onClick.AddListener( ()=> AnimalBookManager.Instance.SetAnimalCard(num) );
        go.GetComponent<Button>().onClick.AddListener( ()=> AppManager.Instance.OpenPanel(animalCard) );
    }

    //수집된 동물 카운트 증가 + 텍스트 변경
    public void AddAnimalCount() 
    {
        collectedAnimalCount++;
        animalCountText.GetComponent<Text>().text = "수집한 동물의 수 : " + collectedAnimalCount.ToString() + " / " + animalNumber.ToString();
    }

    public void SetAnimalCard(int num) 
    {
        string when = getTime[num];
        string[] time = when.Split('_');

        animalCardName.text = CardData.Instance.GetAnimalName(num);
        collectedTime.text = "수집일 : " + time[0] + " / " + time[1] + "시 " + time[2] + "분";
    }
}
