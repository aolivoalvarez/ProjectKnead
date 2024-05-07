/*-----------------------------------------
Creation Date: 5/6/2024 9:39:16 PM
Author: theco
Description: Stores dialogue for the NPC gameobject this script is attached to.
-----------------------------------------*/

using UnityEngine;

public class NpcTalkScript : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] string[] dialogue;
    int currentIndex;
    DialogueManager dialogueManager;

    void Start()
    {
        currentIndex = 0;
        dialogueManager = DialogueManager.instance;
    }

    public void Talk()
    {
        if (dialogueManager.AdvanceDialogue()) { }
        else if (currentIndex < dialogue.Length)
        {
            if (dialogue[currentIndex].StartsWith('(')) dialogueManager.SetDialogue(dialogue[currentIndex]);
            else dialogueManager.SetDialogue(dialogue[currentIndex], _name);
            currentIndex++;
        }
        else
        {
            dialogueManager.CloseDialogue();
            currentIndex = 0;
        }
    }
}
