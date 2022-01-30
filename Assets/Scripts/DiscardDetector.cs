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
        var part = partCollided.gameObject.GetComponent<Part>();
        if (part == null) { return;}

        var lastPlayedPart = CameraController.Instance.LastPlayedPart;
        print($"***: {lastPlayedPart}");

        bool match = false;
        Debug.Log("COLLISION!!!!!!!");
        if (part != null) {Debug.Log("Owner of shape is " + part.Owner);}
        if (part != null && lastPlayedPart != null && (part.Color == lastPlayedPart.Color || part.Shape == lastPlayedPart.Shape)) 
        {
            Debug.Log("WE HAVE A MATCH");
            match = true;
            CameraController.Instance.PointToPart(part);
        }
        else if (part != null && lastPlayedPart != null)
        {
            Debug.Log("Looking for " + lastPlayedPart.Color + " " + lastPlayedPart.Shape +
            ", but found " + part.Color + " " + part.Shape);
        }
        else
        {
            if (part == null) {Debug.Log("part is null");}
            if (lastPlayedPart == null) {Debug.Log("lastPlayedPart is null");}
        }
        partsManagerScript.ProcessPart(partCollided.gameObject, match);
    }
}
