using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int CameraPosition;
    private float gap = 4.0f;
    private float offset = 21.62f;

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

    void PointToPart(Part part)
    {
        CameraPosition = (int)part.Shape * 4 + (int)part.Color;
        SetCameraPosition();
    }

    void SetCameraPosition() 
    {
        transform.position = new Vector3(CameraPosition * gap - offset, transform.position.y, transform.position.z);
    }
}
