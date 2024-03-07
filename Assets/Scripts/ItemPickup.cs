using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerAttackScript>().hasSword = true;
            Destroy(gameObject);
        }
    }
}
