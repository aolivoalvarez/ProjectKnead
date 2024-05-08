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
                AudioManager.instance.PlaySound(14);
                GameManager.instance.DisablePlayerInput();
                player.EndPlayerCoroutines();
                player.transform.position = transform.position;
                player.rigidBody.velocity = Vector3.zero;
                other.gameObject.GetComponent<Collider2D>().enabled = false;
                player.animator.SetBool("IsFallingIntoPit", true);
            }
        }
        else if (other.gameObject.GetComponent<HenchmanScript>() != null)
        {
            AudioManager.instance.PlaySound(14);
            Destroy(other.gameObject.GetComponent<HenchmanScript>().agent);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.DOScale(0f, 1f);
            Destroy(other.gameObject, 1f);
        }
        else if (other.gameObject.GetComponent<BasicEnemyScript>() != null)
        {
            AudioManager.instance.PlaySound(14);
            Destroy(other.gameObject.GetComponent<BasicEnemyScript>().agent);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.DOScale(0f, 1f);
            Destroy(other.gameObject, 1f);
        }
        else if (other.gameObject.GetComponent<PushObjectScript>() != null)
        {
            AudioManager.instance.PlaySound(14);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.DOScale(0f, 1f);
            Destroy(other.gameObject, 1f);
        }
    }
}
