using UnityEngine;

public class DamagingZone : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == PlayerController.instance.gameObject)
        {
            PlayerController.instance.DecreaseHealth(damageAmount);
        }
    }
}
