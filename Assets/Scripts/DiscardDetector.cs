using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider partCollided) 
    {
        var part = partCollided.gameObject.GetComponent<Part>();
        Debug.Log("collided with " + part);
    }
}
