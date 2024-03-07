using UnityEngine;

public class ThrowableObjectScript : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;
    public bool isLifted { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isLifted)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<HenchmanScript>().TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
