using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OptionsManager : MonoBehaviour
{
    public GameObject Player1Button;
    public GameObject Player2Button;
    public GameObject PlayerCountHighlight;
    public int PlayerCount;
    public GameObject GameMode1Button;
    public GameObject GameMode2Button;
    public GameObject GameModeHighlight;
    public int GameMode;

     void Start()
    {
        PlayerCount = PlayerPrefs.GetInt("PlayerCount");
        if (PlayerCount <= 0)
        {
            PlayerCount = 2;
            PlayerPrefs.SetInt("PlayerCount", 2);
        }
        HighlightPlayerCount();
        GameMode = PlayerPrefs.GetInt("GameMode");
        if (GameMode <= 0)
        {
            GameMode = 1;
            PlayerPrefs.SetInt("GameMode", 1);
        }
        HighlightGameMode();
    }

    private void Update() 
    {
        if (Input.GetKeyUp(KeyCode.Return))
        { 
            BackButtonClick();
        }
    }

    public void SetNumberOfPlayers(int num)
    {
        PlayerCount = num;
        PlayerPrefs.SetInt("PlayerCount", num);
        HighlightPlayerCount();
    }

    private void HighlightPlayerCount()
    {
        switch(PlayerCount)
        {
            case 1:
                PlayerCountHighlight.transform.position = Player1Button.transform.position;
            break;
            case 2:
                PlayerCountHighlight.transform.position = Player2Button.transform.position;
            break;
        }
    }

    public void SetGameMode(int num)
    {
        GameMode = num;
        PlayerPrefs.SetInt("GameMode", num);
        HighlightGameMode();
    }

    private void HighlightGameMode()
    {
        switch(GameMode)
        {
            case 1:
                GameModeHighlight.transform.position = GameMode1Button.transform.position;
            break;
            case 2:
                GameModeHighlight.transform.position = GameMode2Button.transform.position;
            break;
        }
    }
    public void BackButtonClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
