using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public string[] animalName = {
        "흰 파카",
        "검은 파카",
        "곰",
        "판다 곰",
        "흰 고양이",
        "얼룩 고양이",
        "병아리",
        "닭",
        "젖소",
        "사슴",

        "댕댕이",
        "얼룩 댕댕이",
        "오리",
        "천둥 오리",
        "코끼리",
        "보라 코끼리",
        "기린",
        "염소",
        "얼룩 염소",
        "흑염소",

        "하마",
        "말",
        "백마",
        "라이온 킹",
        "라이온 퀸",
        "돼지",
        "흰 토끼",
        "갈색 토끼",
        "코뿔소",
        "핑양",

        "흰양",
        "흑양",
        "얼룩말",
    };

    private static CardData _instance = null;
    public static CardData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(CardData)) as CardData;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    public string GetAnimalName(int num) 
    {
        return animalName[num];
    }
}
