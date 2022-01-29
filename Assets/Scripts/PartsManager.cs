using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public List<GameObject> PartPrefabs;

    public List<GameObject> AvialableParts;

    public List<GameObject> UsedParts;

    public int CountOfEach = 5;

    public int StartingCount = 5;

    private Dictionary<EnumPlayer, PartSpawner> spawners;

    private static PartsManager _instance;

    public static PartsManager Instance 
    {
        get 
        {
            if(_instance)
            {
                _instance = GameObject.FindObjectOfType<PartsManager>();
            }

            return _instance;
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        var spawnerList = GameObject.FindObjectsOfType<PartSpawner>();
        spawners = spawnerList.ToDictionary(k => k.Owner, v => v);

        InitializeParts();
    }

    private void InitializeParts()
    {
        // Initialize Parts
        AvialableParts = new List<GameObject>();
        UsedParts = new List<GameObject>();

        foreach (var prefab in PartPrefabs)
        {
            for (int i = 0; i < CountOfEach; i++)
            {
                var instance = GameObject.Instantiate(prefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
                AvialableParts.Add(instance);
            }
        }
    }

    public GameObject TakePart()
    {
        if(AvialableParts.Count == 0)
        {
            AvialableParts = UsedParts.ToList();
            UsedParts = new List<GameObject>();
        }

        if(AvialableParts.Count == 0) return null;

        int partIndex = Random.Range(0, AvialableParts.Count);
        var part = AvialableParts[partIndex];
        AvialableParts.Remove(part);

        return part;
    } 

    public void ProcessPart(GameObject part, bool match)
    {
        UsedParts.Add(part);

        var partComponent = part.GetComponent<Part>();
        if(match)
        {
               
        }
    }
}