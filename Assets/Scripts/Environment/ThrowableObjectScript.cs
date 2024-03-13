/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: For thrown objects, keeps track of what they should damage and how much damage they should deal.
-----------------------------------------*/

using UnityEngine;

public class ThrowableObjectScript : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;
    public bool isLifted { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isLifted && GetComponent<Rigidbody2D>() != null)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<HenchmanScript>().TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
