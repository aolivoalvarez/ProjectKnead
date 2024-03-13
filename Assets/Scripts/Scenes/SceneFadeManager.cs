/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to Canvas_Fade. Handles fade in and fade out between scene changes.
-----------------------------------------*/

// SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;

    [SerializeField] Image fadeOutImage;
    [SerializeField, Range(0.1f, 10f)] float fadeOutSpeed = 5f;
    [SerializeField, Range(0.1f, 10f)] float fadeInSpeed = 5f;
    [SerializeField] Color fadeOutStartColor;
    public bool isFadingOut { get; private set; }
    public bool isFadingIn { get; private set; }

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }

    void Update()
    {
        if (isFadingOut)
        {
            if (fadeOutImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * fadeOutSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingOut = false;
            }
        }

        if (isFadingIn)
        {
            if (fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * fadeInSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        fadeOutImage.color = fadeOutStartColor;
        isFadingOut = true;
    }

    public void StartFadeIn()
    {
        if (fadeOutImage.color.a >= 1f)
        {
            fadeOutImage.color = fadeOutStartColor;
            isFadingIn = true;
        }
    }
}
