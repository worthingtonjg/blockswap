using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PartSpawner : MonoBehaviour
{
    public float SpawnRate = 3f;
    public List<GameObject> PartsInMachine;

    public TMP_Text PartCount;
    public TMP_Text Message;

    public List<GameObject> PartsOnConveyer;

    public List<GameObject> PartsInPlay;

    public GameObject Destination;

    public EnumPlayer Owner;

    private EnumGameMode GameMode;

    void Awake()
    {
        PartsInMachine = new List<GameObject>();
        PartsOnConveyer = new List<GameObject>();
        PartsInPlay = new List<GameObject>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnNextPartInMachine", 0f, SpawnRate);
        GameMode = (EnumGameMode)PlayerPrefs.GetInt("GameMode");
    }

    void Update()
    {
        
    }

    private void SpawnNextPartInMachine()
    {
        //print($"SpawnNextPartInMachine: {PartsInMachine.Count}");
        var part = MachineToConveyer();

        if(part != null)
        {
            part.transform.position = transform.position;

            var partMoveComponent = part.GetComponent<PartMove>();
            partMoveComponent.destination = Destination;
        }
    }

    public int CalcPartCount()
    {
        var count = (PartsInMachine.Count + PartsOnConveyer.Count + PartsInPlay.Count);
        
        if (GameMode == EnumGameMode.ClearShapes)
        {
            PartCount.text = count.ToString();
        }

        return count;
    }

    public IEnumerator ShowMessage(string message, int penalty = 0)
    {
        Message.text = message;
        yield return new WaitForSeconds(3f);

        for(int i=0; i<penalty; i++)
        {
            var part = PartsManager.Instance.TakePart();
            AddPartToMachine(part);
        }

        Message.text = string.Empty;
    }

    public void AddPartFromButton(GameObject part)
    {
        part.transform.position = new Vector3(1000,1000,1000);

        var partComponent = part.GetComponent<Part>();
        partComponent.Owner = Owner;
        
        var partMoveComponent = part.GetComponent<PartMove>();
        partMoveComponent.destination = null;

        PartsInPlay.Add(part);

        CalcPartCount();
    }

    public void AddPartToMachine(GameObject part, string message = null, int? penalty = null)
    {
        part.transform.SetParent(null);
        part.transform.position = new Vector3(1000,1000,1000);

        var partComponent = part.GetComponent<Part>();
        partComponent.Owner = Owner;
        
        var partMoveComponent = part.GetComponent<PartMove>();
        partMoveComponent.destination = null;
        
        PartsInPlay.Remove(part);
        PartsInMachine.Insert(0, part);
        //print($"PartsInMachine: {PartsInMachine.Count}");

        if(!string.IsNullOrEmpty(message))
        {
            StartCoroutine(ShowMessage(message, penalty ?? 0));
        }

        CalcPartCount();
    }

    public void TakePartFromConveyer(GameObject part)
    {
        PartsOnConveyer.Remove(part);
        PartsInPlay.Add(part);

        CalcPartCount();
    }

    public GameObject MachineToConveyer()
    {
        var part = PartsInMachine.LastOrDefault();
        if(part == null) return null;

        part.GetComponent<Part>().CanSelect = true;
        
        PartsInMachine.Remove(part);
        PartsOnConveyer.Insert(0, part);

        CalcPartCount();

        return part;
    }

    public GameObject ConveyerToMachine(GameObject part)
    {
        part.transform.position = new Vector3(1000,1000,1000);

        var partMoveComponent = part.GetComponent<PartMove>();
        partMoveComponent.destination = null;

        PartsOnConveyer.Remove(part);
        PartsInMachine.Insert(0, part);

        CalcPartCount();

        return part;
    }

    public void RemovePartFromPlay(GameObject part)
    {
        part.transform.position = new Vector3(1000,1000,1000);

        PartsInPlay.Remove(part);

        CalcPartCount();
    }
}

