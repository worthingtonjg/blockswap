using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDetector : MonoBehaviour
{
    public GameObject partsManager;
    private PartsManager partsManagerScript;
    private Part lastPlayedPart;
    
    // Start is called before the first frame update
    void Start()
    {
        partsManagerScript = partsManager.GetComponent<PartsManager>();
    }

    public void LastPlayedPart(Part part)
    {
        lastPlayedPart = part;
    }

    void OnTriggerEnter(Collider partCollided) 
    {
        bool match = false;
        var part = partCollided.gameObject.GetComponent<Part>();
        Debug.Log("COLLISION!!!!!!!");
        if (part != null) {Debug.Log("Owner of shape is " + part.Owner);}
        if (part != null && lastPlayedPart != null && (part.Color == lastPlayedPart.Color || part.Shape == lastPlayedPart.Shape)) 
        {
            Debug.Log("WE HAVE A MATCH");
            match = true;
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
