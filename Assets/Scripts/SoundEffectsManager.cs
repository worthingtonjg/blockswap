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

    public void PlayBeep1() => Sounds.PlayOneShot(Beep1);

    public void PlayBeep2() => Sounds.PlayOneShot(Beep2);

    public void PlayPickup() => Sounds.PlayOneShot(PickupPiece);

    public void PlayDrop() => Sounds.PlayOneShot(DropPiece);

    public void PlayGoodMatch() => Sounds.PlayOneShot(GoodMatch);

    public void PlayBadMatch() => Sounds.PlayOneShot(BadMatch);

    public void PlayGameOver() => Sounds.PlayOneShot(GameOver);

    void Start()
    {
        Sounds = GetComponent<AudioSource>();
    }
}
