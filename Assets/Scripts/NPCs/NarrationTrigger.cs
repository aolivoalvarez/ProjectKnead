/*-----------------------------------------
Creation Date: 5/7/2024 4:10:14 PM
Author: theco
Description: When the player touches the attached collider, displays the stored narration.
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NarrationTrigger : MonoBehaviour
{
    [SerializeField] string[] narration;
    [SerializeField] bool disableCollider = false;
    int currentIndex;
    DialogueManager dialogueManager;

    void Start()
    {
        if (dialogueManager == null) Initialize();
    }

    void Initialize()
    {
        GetComponent<Collider2D>().isTrigger = true;
        if (disableCollider) GetComponent<Collider2D>().enabled = false;
        currentIndex = 0;
        dialogueManager = DialogueManager.instance;
    }

    public void DisplayText()
    {
        if (dialogueManager == null) Initialize();
        dialogueManager.currentNarration = this;

        if (dialogueManager.AdvanceNarration()) { }
        else if (currentIndex < narration.Length)
        {
            dialogueManager.SetNarration(narration[currentIndex]);
            currentIndex++;
        }
        else
        {
            dialogueManager.CloseNarration();
            Destroy(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisplayText();
        }
    }
}
