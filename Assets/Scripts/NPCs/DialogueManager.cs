/*-----------------------------------------
Creation Date: 5/6/2024 9:48:20 PM
Author: theco
Description: Controls displayed dialogue and text boxes.
-----------------------------------------*/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] GameObject dialogueSpeakerBox;
    [SerializeField] GameObject dialogueTextBox;
    [SerializeField] Button dialogueButton;
    public GameObject narrationBox;
    [SerializeField] Button narrationButton;
    public NarrationTrigger currentNarration { get; set; }
    TextMeshProUGUI dialogueSpeaker;
    TextMeshProUGUI dialogueText;
    TextMeshProUGUI narration;

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
        dialogueSpeaker = dialogueSpeakerBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText = dialogueTextBox.GetComponentInChildren<TextMeshProUGUI>();
        narration = narrationBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueSpeakerBox.SetActive(false);
        dialogueTextBox.SetActive(false);
        narrationBox.SetActive(false);
    }

    void Update()
    {
        if (dialogueTextBox.activeSelf || narrationBox.activeSelf)
        {
            GameManager.instance.DisablePlayerInput();
            PlayerController.instance.pInput.Player.Interact.Enable();
        }
    }

    public void SetDialogue(string text, string speaker = "null")
    {
        if (!dialogueSpeakerBox.activeSelf) AudioManager.instance.PlaySound(40);
        else AudioManager.instance.PlaySound(41);
        dialogueSpeakerBox.SetActive(speaker != "null");
        dialogueTextBox.SetActive(true);
        dialogueButton.Select();
        dialogueText.pageToDisplay = 1;
        dialogueSpeaker.text = speaker;
        text = text.Replace("\\n", "\n");
        dialogueText.text = text;
    }
    public bool AdvanceDialogue()
    {
        if (dialogueText.pageToDisplay < dialogueText.textInfo.pageCount)
        {
            AudioManager.instance.PlaySound(41);
            dialogueText.pageToDisplay++;
            return true;
        }
        return false;
    }
    public void CloseDialogue()
    {
        AudioManager.instance.PlaySound(41);
        dialogueSpeakerBox.SetActive(false);
        dialogueTextBox.SetActive(false);
        GameManager.instance.EnablePlayerInput();
    }

    public void SetNarration(string text)
    {
        StartCoroutine(PauseAfterFadeIn());
        if (!narrationBox.activeSelf) AudioManager.instance.PlaySound(40);
        else AudioManager.instance.PlaySound(41);
        narrationBox.SetActive(true);
        narrationButton.Select();
        narration.pageToDisplay = 1;
        text = text.Replace("\\n", "\n");
        narration.text = text;
    }
    public bool AdvanceNarration()
    {
        if (narration.pageToDisplay < narration.textInfo.pageCount)
        {
            AudioManager.instance.PlaySound(41);
            narration.pageToDisplay++;
            return true;
        }
        return false;
    }
    public void CloseNarration()
    {
        AudioManager.instance.PlaySound(41);
        Time.timeScale = 1;
        narrationBox.SetActive(false);
        GameManager.instance.EnablePlayerInput();
    }
    
    public void AdvCurrentNarration()
    {
        AudioManager.instance.PlaySound(41);
        currentNarration.DisplayText();
    }

    IEnumerator PauseAfterFadeIn()
    {
        while (SceneFadeManager.instance.isFadingIn)
        {
            yield return null;
        }
        Time.timeScale = 0;
    }
}
