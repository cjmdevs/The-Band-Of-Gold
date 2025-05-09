using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    //add as many for audio

    //public AudioClip background;
    public AudioClip swordSlash;
    public AudioClip magicLaser;
    public AudioClip Bow;
    public AudioClip playerDeath;
    public AudioClip playerHit;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip playerDash;
    public AudioClip itemPickup;
    public AudioClip objectBroke;
    public AudioClip mino1;
    public AudioClip mino2;
    public AudioClip mino3;
    public AudioClip minoSummon;

    

    private void Start()
    {
    //musicSource.clip = background;
    //musicSource.Play();    
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


}
