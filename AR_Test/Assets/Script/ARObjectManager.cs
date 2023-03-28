using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARObjectManager : MonoBehaviour
{
    public Sprite normalmgTap;
    public Sprite selectedImgTap;

    public void SetNormalmgTap(GameObject go) => go.GetComponent<Image>().sprite = normalmgTap;
    public void SetselectedImgTap(GameObject go) => go.GetComponent<Image>().sprite = selectedImgTap;
}
