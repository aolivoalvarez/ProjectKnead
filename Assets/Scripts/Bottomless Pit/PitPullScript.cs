using UnityEngine;

public class PitPullScript : MonoBehaviour
{
    [SerializeField, Tooltip("How quickly the pit pulls entities in.")]
    float pullSpeed = 1f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.isJumping)
            {
                Vector3 distance = transform.position - other.gameObject.transform.position;
                Vector3 pullDirection = distance.normalized;
                player.roughPosition += new Vector2(pullDirection.x * pullSpeed, pullDirection.y * pullSpeed);
            }
        }
    }
}
