﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class AnimalBookManager : MonoBehaviour
{
    [Header("-----Animal Book-----")]
    public int animalNumber = 33;
    public int collectedAnimalCount;
    public static string animalMsg = "Animal_";

    [Space(10f)]
    public Text animalCountText;
    public Transform animalContentParent;
    public Button loginFromAnimalBook;

    [Header("-----Animal Card-----")]
    public GameObject animalCard;
    public Text animalCardName;
    public Text collectedTime;
    public RawImage animalCardView;
    
    [Header("-----Animal Ranking-----")]
    public int score = -1;
    float refreshTime = 0.5f;
    public Text animalRankingText;
    public Button loginFromAnimalRanking;
    public Text leaderboardText;
    public Text leaderboardValueText;
    public GameObject rankImg;

    [Header("-----ETC-----")]
    public Sprite normalmgTap;
    public Sprite selectedImgTap;

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

    public IEnumerator Setting() //처음만 실행(로그인 시)
    {
        //초기값 생성 및 초기화
        isFound = new bool[animalNumber];
        getTime = new string[animalNumber];
        collectedAnimalCount = 0;
        animalCountText.text = "돌아다니며 동물을 수집해보세요!";
        animalRankingText.text = "";
        loginFromAnimalBook.gameObject.SetActive(false);
        loginFromAnimalRanking.gameObject.SetActive(false);
        rankImg.SetActive(true);
        
        Transform[] childList = animalContentParent.GetComponentsInChildren<Transform>();

        //로그인 후 다른 아이디 로그인 => 동물도감 초기화
        if(childList != null) {
            for(int i = 1; i < childList.Length; i++) { //i = 0 은 부모
                if(childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }

        for (int i = 0; i < animalNumber; i++)
        {
            PlayfabManager.Instance.GetAnimal("Animal_" + i.ToString());
            GameObject go = Instantiate(Resources.Load("Item_Slot"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("AnimalBookImg/" + animalMsg + i.ToString(), typeof(Sprite)) as Sprite;
            go.GetComponent<Image>().color = new Color(0, 0, 0);
            go.transform.SetParent(animalContentParent);
        }
        //모은 동물 개수 동기화
        int collectTemp = collectedAnimalCount;
        yield return new WaitForSeconds(refreshTime);
        while(collectTemp != collectedAnimalCount) {
            collectTemp = collectedAnimalCount;
            yield return new WaitForSeconds(refreshTime);
        }

        //랭킹, 리더보드 동기화
        PlayfabManager.Instance.GetState();
        yield return new WaitForSeconds(refreshTime);
        StartCoroutine(AnimalRank());
    }

    public void AddAnimal(string msg)   //서버, 로컬도감 등록
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false) //중복 방지
        {
            //로컬, 서버등록
            isFound[num] = true;

            string time = DateTime.Now.ToString(("yyyy-MM-dd_HH_mm"));
            getTime[num] = time;
            PlayfabManager.Instance.SaveAnimal(msg, time);

            ChangeCollectedImg(num);

            AddAnimalCount();
            StartCoroutine(AnimalRank()); //랭킹 동기화
        }
        else //이미 수집 되었을 때
        {
            AppManager.Instance.PrintConsoleText(msg + "은(는) 이미 등록되었습니다.");
        }
    }

    public void SetAnimal(string msg, string time)   //로컬 도감 등록 처음에만 실행
    {
        int num = Int32.Parse(msg.Substring(msg.IndexOf('_') + 1).Trim());

        if(isFound[num] == false)   //중복 방지
        {
            //로컬등록
            isFound[num] = true;
            getTime[num] = time;

            //도감 설정
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

    IEnumerator AnimalRank()
    {
        while (score < collectedAnimalCount) {
            PlayfabManager.Instance.SetStat(collectedAnimalCount);
            yield return new WaitForSeconds(refreshTime);
            PlayfabManager.Instance.GetState();
            yield return new WaitForSeconds(refreshTime);
        }
        PlayfabManager.Instance.GetLeaderboard();
    }

    public void RefreshAnimalRank() //Button
    {
        StartCoroutine(RefreshAnimalRankCoroutine());
    }

    IEnumerator RefreshAnimalRankCoroutine()
    {
        leaderboardText.text = "";
        leaderboardValueText.text = "";
        PlayfabManager.Instance.GetState();
        PlayfabManager.Instance.GetLeaderboard();
        yield return null;
    }

    //동물 카드 설정
    public void SetAnimalCard(int num) 
    {
        string when = getTime[num];
        string[] time = when.Split('_');

        animalCardName.text = DataManager.Instance.animalData.animalName[num];
        collectedTime.text = "수집일 : " + time[0] + " / " + time[1] + "시 " + time[2] + "분";

        Sprite sprite = Resources.Load("AnimalBookImg/" + animalMsg + num.ToString(), typeof(Sprite)) as Sprite;

        animalCardView.texture = MapManager.Instance.textureFromSprite(sprite);
        animalCardView.SetNativeSize();

        GalleryManager.Instance.ImageSizeSetting(animalCardView, 
            animalCardView.GetComponent<RectTransform>().rect.height, 
            animalCardView.GetComponent<RectTransform>().rect.width);
        
        animalCardView.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        animalCardView.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        animalCardView.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }

    public void Ranking(int rank, string displayName, int value)   //랭킹에 따른 이미지, 텍스트 조절
    {
        if(rank <= 3) {
            leaderboardText.text += String.Format("        {0, -10}", displayName) +"\n";
            leaderboardValueText.text += String.Format("{0, 3}마리", value) +"\n";
        }
        else {
            leaderboardText.text += String.Format("{0, 2}위 {1, -2}", rank, displayName) +"\n";
            leaderboardValueText.text += String.Format("{0, 3}마리", value) +"\n";
        }
    }

    public void SetNormalmgTap(GameObject go) => go.GetComponent<Image>().sprite = normalmgTap;
    public void SetselectedImgTap(GameObject go) => go.GetComponent<Image>().sprite = selectedImgTap;
}