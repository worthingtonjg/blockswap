using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject discardPile;
    public int CameraPosition;
    private DiscardDetector discardPileScript;
    private float gap = 4.0f;
    private float offset = 21.62f;
    private Part lastPlayedPart;

    // Start is called before the first frame update
    void Start()
    {
        discardPileScript = discardPile.GetComponent<DiscardDetector>();
        CameraPosition = Random.Range(0,16);
        SetCameraPosition();
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
        CameraPosition = (int)part.Shape * 4 + (int)part.Color;
        SetCameraPosition();
    }

    void SetCameraPosition() 
    {
        transform.position = new Vector3(CameraPosition * gap - offset, transform.position.y, transform.position.z);
        lastPlayedPart = WhichPart(CameraPosition);
        discardPileScript.LastPlayedPart(lastPlayedPart);
    }
}
