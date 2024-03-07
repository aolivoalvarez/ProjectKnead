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
