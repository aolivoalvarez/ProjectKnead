using UnityEngine;

public class LiftHitboxScript : MonoBehaviour
{
    public GameObject objectToLift;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LiftableObject") && other.GetComponent<Rigidbody2D>() == null) // can't lift an object that has already been thrown
        {
            objectToLift = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        objectToLift = null;
    }
}
