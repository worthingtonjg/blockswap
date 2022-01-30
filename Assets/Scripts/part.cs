using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public EnumPlayer Owner;
    public EnumPartColor Color;
    public EnumPartShape Shape;

    public GameObject Selector;

    public void ToggleSelected(bool selected)
    {
        Selector.SetActive(selected);
    }
}
