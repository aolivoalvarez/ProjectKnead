/*-----------------------------------------
Creation Date: 3/18/2024 6:01:18 PM
Author: theco
Description: For the FallingItem prefab. Stores an object and makes it look like it's falling to the ground. When it hits the ground, releases the object and destroys self.
-----------------------------------------*/

using UnityEngine;
using DG.Tweening;
using System.Collections;

public class FallingItemScript : MonoBehaviour
{
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] float timeToFall;
    public GameObject storedItem;
    public float groundLevelY;
    float offsetY;

    void OnEnable()
    {
        if (AnimationCurvesScript.instance != null && storedItem != null)
        {
            GetComponentInChildren<Collider2D>().enabled = false; // while an item is falling, it should have no collision
            offsetY = GetComponentInChildren<SpriteRenderer>().bounds.size.y * 0.5f;
            GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Entity";
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            Instantiate(shadowPrefab, new Vector2(transform.position.x, groundLevelY), Quaternion.identity, transform); // creates a child object from the shadow prefab
            StartCoroutine(FallRoutine(storedItem.transform.DOMoveY(groundLevelY + offsetY, timeToFall).SetEase(AnimationCurvesScript.instance.fallingItem)));
        }
    }

    public IEnumerator FallRoutine(Tween fallTween)
    {
        AudioManager.instance.PlaySound(14);
        GetComponentInChildren<Collider2D>().enabled = false;
        if (storedItem.GetComponent<BasicEnemyScript>() != null)
        {
            storedItem.GetComponent<BasicEnemyScript>().agent.enabled = false;
        }

        while (fallTween.IsActive())
        {
            yield return new WaitForEndOfFrame();
        }
        GetComponentInChildren<Collider2D>().enabled = true;
        storedItem.transform.SetParent(transform.parent);
        if (storedItem.GetComponent<Pickup>() != null)
            storedItem.GetComponent<Pickup>().StartDespawnRoutine();
        if (storedItem.GetComponent<BasicEnemyScript>() != null)
            storedItem.GetComponent<BasicEnemyScript>().agent.enabled = true;
        Destroy(gameObject);
    }
}
