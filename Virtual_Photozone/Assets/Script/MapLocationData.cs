using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationData : MonoBehaviour
{
    [TextArea(3, 10)]
    public string locationDescription;
    
    public int lengthX;
    public int lengthY;
}
