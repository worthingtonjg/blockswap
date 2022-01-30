using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject discardPile;
    public Part LastPlayedPart;
    private DiscardDetector discardPileScript;
    private float gap = 4.0f;
    private float offset = 21.62f;

    // Start is called before the first frame update
    void Start()
    {
        discardPileScript = discardPile.GetComponent<DiscardDetector>();
        var cameraPosition = Random.Range(0,16);
        SetCameraPosition(cameraPosition);
    }

    Part WhichPart(int index)
    {
        int shape = (int)(index / 4);
        int color = (int)(index % 4);
        Part newPart = new Part();
        newPart.Owner = 0;
        newPart.Shape =  (EnumPartShape)shape;
        newPart.Color = (EnumPartColor)color;
        return newPart;
    }

    public  void PointToPart(Part part)
    {
        int cameraPosition = (int)part.Shape * 4 + (int)part.Color;
        SetCameraPosition(cameraPosition);
    }

    void SetCameraPosition(int cameraPosition) 
    {
        LastPlayedPart = WhichPart(cameraPosition);
        Debug.Log("LastPlayedPart is " + LastPlayedPart.Color + " " + LastPlayedPart.Shape);
        //discardPileScript.LastPlayedPart(LastPlayedPart);
        transform.position = new Vector3(cameraPosition * gap - offset, transform.position.y, transform.position.z);
    }
}
