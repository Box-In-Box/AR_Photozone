using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AnimalBookManager : MonoBehaviour
{
    private int AnimalCount = 33;
    private int CollectedAnimalCount = 0;

    public GameObject AnimalPrefab;
    public Text AnimalCountText;
    private Transform AnimalContentParent;

    private bool[] isFound;
    private bool[] isFoundTest;
    private bool[] isFoundTestAll;

    void Start()
    {
        AnimalContentParent = GameObject.Find("AnimalBookContent").transform;
        isFound = new bool[AnimalCount];

        // TEST ARRAY --
        isFoundTest = new bool[33] { true, false, false, false, false, false, false, false, false, false, 
            true, true, true, true, false, false, false, false, true, true, 
            true, true, true, true, true, false, false, false, false, false, 
            false, false, false};

        isFoundTestAll = new bool[33];
        isFoundTestAll = Enumerable.Repeat(true, 33).ToArray();
        // -- TEST ARRAY

        AnimalCountText.GetComponent<Text>().text = "수집한 동물의 수 : " + CollectedAnimalCount.ToString() + " / " + AnimalCount.ToString();
        SetAnimalBook();
    }

    void SetAnimalBook()
    {
        for (int i = 0; i < AnimalCount; i++) {
            GameObject go = Instantiate(AnimalPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<Image>().sprite = Resources.Load("AnimalBookImg/"+"Animal_"+ (i+1).ToString(), typeof(Sprite)) as Sprite;

            if (isFoundTestAll[i] == false) // TEST BOOL
                go.GetComponent<Image>().color = new Color(0, 0, 0);

            go.transform.SetParent(AnimalContentParent);
        }
    }


    void FindAnimal()   //도감 등록
    {

    }
}
