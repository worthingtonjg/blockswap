using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PartsManager : MonoBehaviour
{
    public bool GameStarted = false;
    public GameObject CountDownCanvas;
    public TMP_Text CountDown;
    public TMP_Text TimerText;
    public TMP_Text P1Score;
    public TMP_Text P2Score;
    public int PlayerCount;
    public EnumGameMode GameMode;
    public float Timer;

    public GameObject GameOverCanvas;
    public GameObject RedWinsImage;
    public GameObject BlueWinsImage;
    public GameObject TieImage;

    public List<GameObject> PartPrefabs;
    public List<GameObject> AvailableParts;
    public List<GameObject> UsedParts;

    public int CountOfEach = 5;
    public int StartingCount = 15;
    
    private Dictionary<EnumPlayer, PartSpawner> spawners;

    private static PartsManager _instance;
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
    private void Start()
    {
        InitializeParts();
        InitializeSpawners();
        InitializeGame();
        StartCoroutine(CountDownToStart());
    }

    private void Update() 
    {
        if (GameStarted && GameMode == EnumGameMode.TimedGame && Timer >= 0)
        {
            // Display the game timer
            Timer -= Time.deltaTime;
            TimerText.text = ((int)Timer).ToString();
            if (Timer < 0) // Times up
            {
                WhoWon();
            }
        }    
    }

    private IEnumerator CountDownToStart()
    {
        if(CountDownCanvas != null)
        {
            CountDown.text = "Starting in 3 ... ";
            yield return new WaitForSeconds(.3f);
            SoundEffectsManager.Instance.PlayBeep1();
            yield return new WaitForSeconds(1f);
            SoundEffectsManager.Instance.PlayBeep1();
            CountDown.text = "Starting in 2 ... ";
            yield return new WaitForSeconds(1f);
            SoundEffectsManager.Instance.PlayBeep1();
            CountDown.text = "Starting in 1 ... ";
            SoundEffectsManager.Instance.PlayBeep1();
            yield return new WaitForSeconds(1f);
            CountDown.text = "Play!";
            SoundEffectsManager.Instance.PlayBeep2();
            yield return new WaitForSeconds(1f);        
            CountDownCanvas.SetActive(false);
            GameStarted = true;
        }
    }

    private void InitializeGame()
    {
        int gm = PlayerPrefs.GetInt("GameMode");
        if (gm <= 0) // not set or set to 0
        {
            GameMode = EnumGameMode.ClearShapes;
        } else { // set to 1 or greater
            GameMode = EnumGameMode.TimedGame;
        }
        PlayerCount = PlayerPrefs.GetInt("PlayerCount");
        PlayerPrefs.SetInt("GameMode", (int)GameMode);
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "GameScene")
        {
            if(GameMode == EnumGameMode.TimedGame)
            {
                P1Score.text = "0";
                P2Score.text = "0";
                Timer = 60f;
                TimerText.text = Timer.ToString();
            }
            else
            {
                P1Score.text = StartingCount.ToString();
                P2Score.text = StartingCount.ToString();
                TimerText.text = "";
            }    
        }
    }

    private void InitializeParts()
    {
        // Initialize Parts
        AvailableParts = new List<GameObject>();
        UsedParts = new List<GameObject>();

        foreach (var prefab in PartPrefabs)
        {
            for (int i = 0; i < CountOfEach; i++)
            {
                var instance = GameObject.Instantiate(prefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
                AvailableParts.Add(instance);
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
        if(AvailableParts.Count == 0)
        {
            AvailableParts = UsedParts.ToList();
            UsedParts = new List<GameObject>();
        }

        if(AvailableParts.Count == 0) return null;

        int partIndex = Random.Range(0, AvailableParts.Count);
        var part = AvailableParts[partIndex];
        AvailableParts.Remove(part);

        return part;
    } 

    public void ProcessPart(GameObject part, bool match)
    {
        UsedParts.Add(part);

        var partComponent = part.GetComponent<Part>();
        if(match)
        {
            //print("matched");
            spawners[partComponent.Owner].RemovePartFromPlay(part);
            SoundEffectsManager.Instance.PlayGoodMatch();
            if (GameMode == EnumGameMode.TimedGame)
            {
                if (partComponent.Owner == EnumPlayer.P1)
                {
                    int score = int.Parse(P1Score.text) + 1;
                    P1Score.text = score.ToString();
                }
                else
                {
                    int score = int.Parse(P2Score.text) + 1;
                    P2Score.text = score.ToString();
                }
            }
        }
        else
        {
            //print("not matched");
            spawners[partComponent.Owner].AddPartToMachine(part, "Invalid Part Penalty: +2 Parts", 2);
            SoundEffectsManager.Instance.PlayBadMatch();
        }

        if(GameMode == EnumGameMode.ClearShapes && spawners[partComponent.Owner].CalcPartCount() == 0)
        {
            ShowGameOver(partComponent.Owner);
        }
    }

    private void ShowGameOver(EnumPlayer winner)
    {
        GameStarted = false;
        GameOverCanvas.SetActive(true);
        if(winner == EnumPlayer.P1 || PlayerCount == 1)
        {
            BlueWinsImage.SetActive(true);
        }
        else if (winner == EnumPlayer.P2)
        {
            RedWinsImage.SetActive(true);
        }
        else
        {
            TieImage.SetActive(true);
        }
        SoundEffectsManager.Instance.PlayGameOver();
    }

    private void WhoWon() 
    {
        // Check score
        if (int.Parse(P2Score.text) > int.Parse(P1Score.text))
        {
            ShowGameOver(EnumPlayer.P2); // Player 2 wins
        }
        else if (int.Parse(P1Score.text) > int.Parse(P2Score.text))
        {
            ShowGameOver(EnumPlayer.P1); // Player 1 wins
        }
        else
        {
            ShowGameOver(EnumPlayer.Unassigned); // it's a tie
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
