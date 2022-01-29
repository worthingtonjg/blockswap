using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMaker : MonoBehaviour
{
    public int StartCount = 7;
    public List<GameObject> PartPrefabs;

    public Queue<GameObject> VisibleParts;

    public Queue<GameObject> HiddenParts;

    // Start is called before the first frame update
    void Start()
    {
        if(VisibleParts.Count > 0)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

