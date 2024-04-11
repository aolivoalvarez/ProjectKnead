/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Attached to the inner Collider2D of the Pit prefab. Kills enemies, or damages the player and respawns them at the checkpoint.
-----------------------------------------*/

using DG.Tweening;
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
        else if (other.gameObject.GetComponent<HenchmanScript>() != null)
        {
            Destroy(other.gameObject.GetComponent<HenchmanScript>().agent);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.DOScale(0f, 1f);
            Destroy(other.gameObject, 1f);
        }
        else if (other.gameObject.GetComponent<PushObjectScript>() != null)
        {
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.DOScale(0f, 1f);
            Destroy(other.gameObject, 1f);
        }
    }
}
