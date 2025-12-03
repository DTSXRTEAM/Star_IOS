using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void Change(object par)
    {
        ColorWrapper c = par as ColorWrapper;
        gameObject.GetComponent<Renderer>().material.color = c.Color;
    }
}
