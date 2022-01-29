using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartDespawner : MonoBehaviour
{
    public EnumPlayer Owner;

    private Dictionary<EnumPlayer, PartSpawner> spawners;
    
    // Start is called before the first frame update
    void Start()
    {
        var spawnerList = GameObject.FindObjectsOfType<PartSpawner>();
        spawners = spawnerList.ToDictionary(k => k.Owner, v => v);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
    {
        var part = other.gameObject.GetComponent<Part>();
        if(part != null)
        {
            
            print($"ConveyerToMachine: {other.gameObject.name}");
            spawners[Owner].ConveyerToMachine(other.gameObject);
        }
        else
        {
            print($"Ignored: {other.gameObject.name}");
        }
    }
}
