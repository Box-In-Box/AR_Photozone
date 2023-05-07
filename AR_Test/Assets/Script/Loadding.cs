using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadding : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -300 * Time.deltaTime));
    }
}
