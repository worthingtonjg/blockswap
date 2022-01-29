using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int CameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        CameraPosition = Random.Range(0,16);
        SetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PointToPart(part part)
    {
        CameraPosition = (int)part.Shape * 4 + (int)part.Color;
        SetCameraPosition();
    }

    void SetCameraPosition() 
    {
        transform.position = new Vector3(CameraPosition * 2.0f -21.62f, transform.position.y, transform.position.z);
    }
}
