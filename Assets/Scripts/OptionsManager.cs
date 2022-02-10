using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject Player1Button;
    public GameObject Player2Button;
    public GameObject PlayerCountHighlight;
    public int PlayerCount;
    public GameObject GameMode1Button;
    public GameObject GameMode2Button;
    public GameObject GameModeHighlight;
    public int GameMode;
    public Toggle SoundToggle;
    public bool SoundOption;
    public Toggle MusicToggle;
    public bool MusicOption;

     void Start()
    {        
        PlayerCount = PlayerPrefs.GetInt("PlayerCount");
        if (PlayerCount != 1) // not set or set to 2 or greater
        {
            PlayerCount = 2;
            PlayerPrefs.SetInt("PlayerCount", 2);
        }
        HighlightPlayerCount();
        GameMode = PlayerPrefs.GetInt("GameMode"); 
        if (GameMode <= 0) // not set or set to 0
        {
            GameMode = 0;
            PlayerPrefs.SetInt("GameMode", (int)EnumGameMode.ClearShapes);
        } else { // set to 1 or greater
            GameMode = 1;
            PlayerPrefs.SetInt("GameMode", (int)EnumGameMode.TimedGame);
        }
        HighlightGameMode();

        if (PlayerPrefs.HasKey("SoundOption"))
        {
            SoundOption = PlayerPrefs.GetInt("SoundOption") == 1;
        }
        else
        {
            SoundOption = true;
            PlayerPrefs.SetInt("SoundOption", 1);
        }
        SoundToggle.isOn = SoundOption;    

        AudioSource audio = MainCamera.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MusicOption"))
        {
            audio.UnPause();
            MusicOption = PlayerPrefs.GetInt("MusicOption") == 1;
        }
        else
        {
            audio.Pause();
            MusicOption = true;
            PlayerPrefs.SetInt("MusicOption", 1);
        }
        MusicToggle.isOn = MusicOption;
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
        if (num <= 1)
        {
            GameMode = (int)EnumGameMode.ClearShapes;
        } else {
            GameMode = (int)EnumGameMode.TimedGame;
        }
        PlayerPrefs.SetInt("GameMode", GameMode);
        HighlightGameMode();
    }

    private void HighlightGameMode()
    {
        switch(GameMode)
        {
            case 0: // Clear Shapes
                GameModeHighlight.transform.position = GameMode1Button.transform.position;
            break;
            case 1: // Timed Game
                GameModeHighlight.transform.position = GameMode2Button.transform.position;
            break;
            default:
                print("Unknown Game Mode value: {GameMode}");
            break;
        }
    }

    public void SetSoundOptions()
    {
        if (SoundToggle.isOn)
        {
            SoundOption = true;
            PlayerPrefs.SetInt("SoundOption", 1);
        }
        else
        {
            SoundOption = false;
            PlayerPrefs.SetInt("SoundOption", 0);
        }
    }

    public void SetMusicOptions()
    {
        AudioSource audio = MainCamera.GetComponent<AudioSource>();        
        if (MusicToggle.isOn)
        {
            audio.UnPause();
            MusicOption = true;
            PlayerPrefs.SetInt("MusicOption", 1);
        }
        else
        {
            audio.Pause();
            MusicOption = false;
            PlayerPrefs.SetInt("MusicOption", 0);
        }
    }

    public void BackButtonClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
