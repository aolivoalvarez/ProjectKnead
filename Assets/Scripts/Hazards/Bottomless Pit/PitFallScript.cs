using UnityEngine;

public class PitFallScript : MonoBehaviour
{
    [SerializeField, Tooltip("How much damage the player takes if they fall in.")]
    int damage = 2;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.isJumping)
            {
                gameManager.RespawnAtCheckpoint(damage);
            }
        }
    }
}
