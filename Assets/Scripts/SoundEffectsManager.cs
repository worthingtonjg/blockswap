using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    private AudioSource Sounds;
    public AudioClip Beep1;
    public AudioClip Beep2;

    public AudioClip PickupPiece;
    public AudioClip DropPiece;
    public AudioClip GoodMatch;
    public AudioClip BadMatch;
    public AudioClip GameOver;
    public Camera MainCamera;
    public bool PlaySounds;
    public bool PlayMusic;

    private static SoundEffectsManager _instance;

    public static SoundEffectsManager Instance 
    {
        get 
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundEffectsManager>();
            }

            return _instance;
        }
    
    }

    public void PlayBeep1() { if (PlaySounds) { Sounds.PlayOneShot(Beep1); } }

    public void PlayBeep2() { if (PlaySounds) { Sounds.PlayOneShot(Beep2); } }

    public void PlayPickup() { if (PlaySounds) { Sounds.PlayOneShot(PickupPiece); } }

    public void PlayDrop() { if (PlaySounds) { Sounds.PlayOneShot(DropPiece); } }

    public void PlayGoodMatch() {  if (PlaySounds) { Sounds.PlayOneShot(GoodMatch); } }

    public void PlayBadMatch() { if (PlaySounds) { Sounds.PlayOneShot(BadMatch); } }

    public void PlayGameOver() { if (PlaySounds) { Sounds.PlayOneShot(GameOver); } }

    void Start()
    {
        Sounds = GetComponent<AudioSource>();
        AudioSource audio = MainCamera.GetComponent<AudioSource>();
        PlaySounds = true;
        PlayMusic = true;
        if (PlayerPrefs.HasKey("SoundOption"))
        {
            if (PlayerPrefs.GetInt("SoundOption") == 0)
            {
                PlaySounds = false;
            }
            else
            {
                PlaySounds = true;
            }
        }
        if (PlayerPrefs.HasKey("MusicOption"))
        {
            if (PlayerPrefs.GetInt("MusicOption") == 0)
            {
                //audio.enabled = false;
                audio.Pause();
                PlayMusic = false;
            }
            else
            {
                audio.UnPause();
                PlayMusic = true;
            }
        }
    }
}
