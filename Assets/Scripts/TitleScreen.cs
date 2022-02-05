using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreen : MonoBehaviour
{
    public Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = MainCamera.GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("MusicOption"))
        {
            if (PlayerPrefs.GetInt("MusicOption") == 0)
            {
                audio.Pause();
            }
            else
            {
                audio.UnPause();
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        { 
            LoadGameScene();
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
