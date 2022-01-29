using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    public float SpawnRate = 3f;
    public List<GameObject> PartsInMachine;

    public List<GameObject> PartsOnConveyer;

    public List<GameObject> PartsInPlay;

    public GameObject Destination;

    public EnumPlayer Owner;

    void Awake()
    {
            PartsInMachine = new List<GameObject>();
            PartsOnConveyer = new List<GameObject>();
            PartsInPlay = new List<GameObject>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnNextPartInMachine", SpawnRate, SpawnRate);
    }

    void Update()
    {
        
    }

    private void SpawnNextPartInMachine()
    {
        print($"SpawnNextPartInMachine: {PartsInMachine.Count}");
        var part = MachineToConveyer();

        if(part != null)
        {
            part.transform.position = transform.position;

            var partMoveComponent = part.GetComponent<PartMove>();
            partMoveComponent.destination = Destination;
        }
    }

    public int PartCount()
    {
        return PartsInMachine.Count + PartsOnConveyer.Count + PartsInPlay.Count;
    }

    public void AddPartToMachine(GameObject part)
    {
        print($"AddPartToMachine: {Owner}");
        part.transform.position = new Vector3(1000,1000,1000);

        var partComponent = part.GetComponent<Part>();
        partComponent.Owner = Owner;
        
        var partMoveComponent = part.GetComponent<PartMove>();
        partMoveComponent.destination = null;
        
        PartsInMachine.Insert(0, part);
        print($"PartsInMachine: {PartsInMachine.Count}");
    }

    public void TakePartFromConveyer(GameObject part)
    {
        PartsOnConveyer.Remove(part);
        PartsInPlay.Add(part);
    }

    public GameObject MachineToConveyer()
    {
        var part = PartsInMachine.LastOrDefault();

        PartsInMachine.Remove(part);
        PartsOnConveyer.Insert(0, part);

        return part;
    }

    public GameObject ConveyerToMachine()
    {
        var part = PartsOnConveyer.LastOrDefault();

        PartsOnConveyer.Remove(part);
        PartsInMachine.Insert(0, part);

        return part;
    }
}

