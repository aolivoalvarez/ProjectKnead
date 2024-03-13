/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Attached to the outer Collider2D of the Pit prefab. Pulls the player towards the center of the pit.
-----------------------------------------*/

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
                player.rigidBody.AddForce(new Vector2(pullDirection.x * pullSpeed, pullDirection.y * pullSpeed)); // pulls the player towards the pit
                player.moveSpeedMult = 0.5f; // slows the player to half speed
                player.canJump = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.moveSpeedMult = 1f;
                player.canJump = true;
            }
        }
    }
}
