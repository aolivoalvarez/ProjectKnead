/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: For thrown objects, keeps track of what they should damage and how much damage they should deal.
-----------------------------------------*/

using UnityEngine;

public class ThrowableObjectScript : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;
    public bool isThrown { get; set; }
    GameObject graphic;

    void Start()
    {
        graphic = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    void Update()
    {
        if (isThrown && graphic.transform.localPosition.y <= 0.5f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isThrown && !other.isTrigger)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<HenchmanScript>().TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}
