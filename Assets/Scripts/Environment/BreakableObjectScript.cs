/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Can be attached to any GameObject with a Collider2D in order to tell it what tagged trigger can be used to destroy it.
-----------------------------------------*/

using UnityEngine;

public class BreakableObjectScript : MonoBehaviour
{
    [SerializeField] string[] tagsThatDestroyThisObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string currentTag in tagsThatDestroyThisObject)
        {
            if (other.gameObject.CompareTag(currentTag))
            {
                Destroy(gameObject);
            }
        }
    }
}
