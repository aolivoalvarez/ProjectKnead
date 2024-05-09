/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Can be attached to any GameObject with a Collider2D in order to tell it what tagged trigger can be used to destroy it.
-----------------------------------------*/

using UnityEngine;

public class BreakableObjectScript : MonoBehaviour
{
    [SerializeField] string[] tagsThatDestroyThisObject;
    [SerializeField] int breakSoundIndex = -1;

    void BreakObject()
    {
        if (breakSoundIndex >= 0) AudioManager.instance.PlaySound(breakSoundIndex);
        if (GetComponent<SpawnPickup>() != null)
            GetComponent<SpawnPickup>().SpawnThisPickup();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string currentTag in tagsThatDestroyThisObject)
        {
            if (other.gameObject.CompareTag(currentTag))
            {
                BreakObject();
            }
        }
    }
}
