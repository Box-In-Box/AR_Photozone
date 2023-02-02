using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationData : MonoBehaviour
{
    [TextArea(3, 10)]
    public string locationDescription;
    [TextArea(3, 10)]
    public string mapDescription;
    
    public int lengthX;
    public int lengthY;
    public bool isSetPosition = false;
    public Vector3 position;
    float delta = 10.0f;
    float speed = 3.0f;

    void Update()
    {
        if (isSetPosition == false)
            return;

        Vector3 v = position;
        v.y += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
