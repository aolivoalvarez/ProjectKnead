/*-----------------------------------------
Creation Date: 5/5/2024 7:11:59 PM
Author: theco
Description: Stores all music and sfx used in the game.
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    AudioSource _audi;

    public AudioClip[] bgMusic;
    public AudioClip[] soundFX;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }

    private void Start()
    {
        _audi = GetComponent<AudioSource>();
    }

    public void PlaySound(int soundClip)
    {
        _audi.PlayOneShot(soundFX[soundClip]);
    }
}