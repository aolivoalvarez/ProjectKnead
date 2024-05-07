/*-----------------------------------------
Creation Date: 5/6/2024 9:48:20 PM
Author: theco
Description: Controls displayed dialogue and text boxes.
-----------------------------------------*/

using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] GameObject dialogueSpeakerBox;
    [SerializeField] GameObject dialogueTextBox;
    [SerializeField] GameObject narrationBox;
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

    public void SetDialogue(string text, string speaker = "null")
    {
        dialogueSpeakerBox.SetActive(speaker != "null");
        dialogueTextBox.SetActive(true);
        dialogueText.pageToDisplay = 1;
        dialogueSpeaker.text = speaker;
        dialogueText.text = text;
    }

    public bool AdvanceDialogue()
    {
        if (dialogueText.pageToDisplay < dialogueText.textInfo.pageCount)
        {
            dialogueText.pageToDisplay++;
            return true;
        }
        return false;
    }

    public void CloseDialogue()
    {
        dialogueSpeakerBox.SetActive(false);
        dialogueTextBox.SetActive(false);
    }

    public void SetNarration(string text)
    {
        narration.text = text;
    }
}
