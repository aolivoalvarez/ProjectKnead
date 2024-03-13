/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Can be attached to any GameObject with a Collider2D in order to tell it what tagged trigger can be used to destroy it.
-----------------------------------------*/

using UnityEngine;

public class BreakableObjectScript : MonoBehaviour
{
    [SerializeField] string tagThatDestroysThisObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tagThatDestroysThisObject))
        {
            Destroy(gameObject);
        }
    }
}
