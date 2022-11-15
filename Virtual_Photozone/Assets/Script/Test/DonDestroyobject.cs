using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroyobject : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<DonDestroyobject>();
        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }
}
