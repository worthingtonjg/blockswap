using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PartsManager : MonoBehaviour
{
    public bool GameStarted = false;
    public GameObject CountDownCanvas;
    public TMP_Text CountDown;
    
    public List<GameObject> PartPrefabs;

    public List<GameObject> AvialableParts;

    public List<GameObject> UsedParts;

    public AudioClip beep1;
    public AudioClip beep2;

    public int CountOfEach = 5;

    public int StartingCount = 5;

    private Dictionary<EnumPlayer, PartSpawner> spawners;

    private static PartsManager _instance;

    private AudioSource beeps;

    public static PartsManager Instance 
    {
        get 
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PartsManager>();
            }

            return _instance;
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        beeps = FindObjectOfType<AudioSource>();
        InitializeParts();
        InitializeSpawners();
        StartCoroutine(CountDownToStart());
    }

    private IEnumerator CountDownToStart()
    {
        if(CountDownCanvas != null)
        {
            CountDown.text = "Starting in 3 ... ";
            yield return new WaitForSeconds(.3f);
            beeps.PlayOneShot(beep1);
            yield return new WaitForSeconds(1f);
            beeps.PlayOneShot(beep1);
            CountDown.text = "Starting in 2 ... ";
            yield return new WaitForSeconds(1f);
            beeps.PlayOneShot(beep1);
            CountDown.text = "Starting in 1 ... ";
            beeps.PlayOneShot(beep1);
            yield return new WaitForSeconds(1f);
            CountDown.text = "Play!";
            beeps.PlayOneShot(beep2);
            yield return new WaitForSeconds(1f);        
            CountDownCanvas.SetActive(false);
            GameStarted = true;
        }
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

    private void InitializeSpawners()
    {
        var spawnerList = GameObject.FindObjectsOfType<PartSpawner>();
        spawners = spawnerList.ToDictionary(k => k.Owner, v => v);

        //print($"InitializeSpawners: {spawners.Count}");

        foreach(var spawner in spawnerList)
        {
            for(int i = 0; i < StartingCount; i++)
            {
                var part = TakePart();
                spawner.AddPartToMachine(part);
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
            print("matched");
            spawners[partComponent.Owner].RemovePartFromPlay(part);
        }
        else
        {
            print("not matched");
            spawners[partComponent.Owner].AddPartToMachine(part, "Invalid Part Penalty: +2 Parts", 2);
        }
    }
}
