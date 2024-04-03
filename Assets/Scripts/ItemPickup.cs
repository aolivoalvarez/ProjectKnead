using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //[SerializeField] protected Item thisItem;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerAttackScript>().hasSword = true;
            Destroy(gameObject);
        }
    }
}
