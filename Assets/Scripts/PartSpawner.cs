using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    public List<GameObject> PartsInMachine;

    public List<GameObject> PartsOnConveyer;

    public List<GameObject> PartsInPlay;

    public EnumPlayer Owner;

    // Start is called before the first frame update
    void Start()
    {
        PartsInMachine = new List<GameObject>();
        PartsOnConveyer = new List<GameObject>();
        PartsInPlay = new List<GameObject>();
    }

    public int PartCount()
    {
        return PartsInMachine.Count + PartsOnConveyer.Count + PartsInPlay.Count;
    }

    public void AddPartToMachine(GameObject part)
    {
        var partComponent = part.GetComponent<Part>();
        partComponent.Owner = Owner;

        PartsInMachine.Insert(0, part);
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

