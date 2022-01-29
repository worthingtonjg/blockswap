using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMove : MonoBehaviour
{
    public GameObject destination;

    public float scrollSpeed = -0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var offset = Time.deltaTime * scrollSpeed;
        var destPos = new Vector3(destination.transform.position.x, transform.position.y, destination.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, destPos, offset);
    }
}
