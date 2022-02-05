using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDetector : MonoBehaviour
{
    public GameObject partsManager;
    public GameObject cameraController;
    private PartsManager partsManagerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        partsManagerScript = partsManager.GetComponent<PartsManager>();
    }

    void OnTriggerEnter(Collider partCollided) 
    {
        if (partsManagerScript.GameStarted)
        {
            var part = partCollided.gameObject.GetComponent<Part>();
            if (part == null) return;

            var lastPlayedPart = CameraController.Instance.LastPlayedPart;

            bool match = part.Color == lastPlayedPart.Color || part.Shape == lastPlayedPart.Shape;

            if (match) 
            {
                CameraController.Instance.PointToPart(part);
            }
            
            partsManagerScript.ProcessPart(partCollided.gameObject, match);
        }
    }
}
