/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Attached to the inner Collider2D of the Pit prefab. Damages the player and respawns them at the checkpoint.
-----------------------------------------*/

using UnityEngine;

public class PitFallScript : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.isJumping)
            {
                GameManager.instance.DisablePlayerInput();
                PlayerController.instance.transform.position = transform.position;
                PlayerController.instance.animator.SetBool("IsFallingIntoPit", true);
            }
        }
    }
}
