/*-----------------------------------------
Creation Date: 5/5/2024 7:11:59 PM
Author: theco
Description: Stores all music and sfx used in the game.
-----------------------------------------*/

using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audi { get; set; }

    [SerializedDictionary]
    public SerializedDictionary<string, AudioClip> bgMusic;
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

    void Start()
    {
        audi = GetComponent<AudioSource>();
        audi.loop = true;
        audi.clip = bgMusic["TitleScreen"];
        audi.Play();
    }

    public void PlaySound(int soundClip, float volumeScale = 1f)
    {
        Debug.Log("Playing sound " + soundClip);
        audi.PlayOneShot(soundFX[soundClip], volumeScale);
    }
}

public static class AudioFadeOut
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}