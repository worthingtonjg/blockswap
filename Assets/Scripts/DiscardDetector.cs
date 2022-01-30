using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDetector : MonoBehaviour
{
    public GameObject partsManager;
    public GameObject cameraController;
    private PartsManager partsManagerScript;
    private CameraController cameraControllerScript;
    public Part lastPlayedPart;
    
    // Start is called before the first frame update
    void Start()
    {
        partsManagerScript = partsManager.GetComponent<PartsManager>();
        cameraControllerScript = cameraController.GetComponent<CameraController>();
    }

    void SetLastPlayedPart()
    {
        lastPlayedPart = cameraControllerScript.LastPlayedPart;
    }

    public void LastPlayedPart(Part part)
    {
        lastPlayedPart = part;
    }

    void OnTriggerEnter(Collider partCollided) 
    {

        bool match = false;
        var part = partCollided.gameObject.GetComponent<Part>();
        if (part == null) { return;}
        if (lastPlayedPart == null)
        {
            Debug.Log("lastPlayedPart is null, but let's try to fix it.");
            SetLastPlayedPart();
            if (lastPlayedPart == null)
            {
                Debug.Log("well, at least we tried. :(");
            }
        }
        Debug.Log("COLLISION!!!!!!!");
        if (part != null) {Debug.Log("Owner of shape is " + part.Owner);}
        if (part != null && lastPlayedPart != null && (part.Color == lastPlayedPart.Color || part.Shape == lastPlayedPart.Shape)) 
        {
            Debug.Log("WE HAVE A MATCH");
            match = true;
            cameraControllerScript.PointToPart(part);
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
