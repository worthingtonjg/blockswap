using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<GameObject> CameraParts;
    public GameObject discardPile;
    public Part LastPlayedPart;
    private DiscardDetector discardPileScript;
    private float gap = 4.0f;
    private float offset = 21.62f;

    private static CameraController _instance;

    public static CameraController Instance 
    {
        get 
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CameraController>();
            }

            return _instance;
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        discardPileScript = discardPile.GetComponent<DiscardDetector>();
        var cameraPosition = Random.Range(0,16);
        SetCameraPosition(cameraPosition);
    }

    public  void PointToPart(Part part)
    {
        int cameraPosition = (int)part.Shape * 4 + (int)part.Color;
        SetCameraPosition(cameraPosition);
    }

    private void SetCameraPosition(int cameraPosition) 
    {
        LastPlayedPart = WhichPart(cameraPosition);
        transform.position = new Vector3(cameraPosition * gap - offset, transform.position.y, transform.position.z);
    }

    private Part WhichPart(int index)
    {
        var part = CameraParts[index];
        var partComponent = part.GetComponent<Part>();
        return partComponent;
    }
}
