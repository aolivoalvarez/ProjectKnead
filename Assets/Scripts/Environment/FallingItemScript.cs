/*-----------------------------------------
Creation Date: 3/18/2024 6:01:18 PM
Author: theco
Description: For the FallingItem prefab. Stores an object and makes it look like it's falling to the ground. When it hits the ground, releases the object and destroys self.
-----------------------------------------*/

using UnityEngine;
using DG.Tweening;

public class FallingItemScript : MonoBehaviour
{
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] float timeToFall;
    public GameObject storedItem { get; set; }
    public float groundLevelY { get; set; }
    Vector3[] bezierPathWaypoints;
    Tween fallTween;

    void Start()
    {
        GetComponentInChildren<Collider2D>().enabled = false; // while an item is falling, it should have no collision
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Entity";
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        Instantiate(shadowPrefab, new Vector2(transform.position.x, groundLevelY), Quaternion.identity, transform); // creates a child object from the shadow prefab
        CreateBezierPath();
        fallTween = storedItem.transform.DOPath(bezierPathWaypoints, timeToFall, PathType.CubicBezier); // start the fall tween
    }

    void Update()
    {
        if (!fallTween.IsActive()) //storedItem.transform.position.y <= groundLevelY + 0.5f)
        {
            GetComponentInChildren<Collider2D>().enabled = true;
            storedItem.transform.SetParent(null);
            if (storedItem.GetComponent<Pickup>() != null)
                storedItem.GetComponent<Pickup>().StartDespawnRoutine();
            Destroy(gameObject);
        }
    }

    // hard to explain. go to https://www.desmos.com/calculator/bxotkycz67 to see it in action
    void CreateBezierPath()
    {
        bezierPathWaypoints = new[] {
            new Vector3(transform.position.x, groundLevelY + 0.5f, transform.position.z), // waypoint 0
            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0.9f), transform.position.z), // control point A
            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0.8f), transform.position.z), // control point B

            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0.05f), transform.position.z), // waypoint 1
            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0.2f), transform.position.z),
            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0.25f), transform.position.z),

            new Vector3(transform.position.x, groundLevelY + 0.5f, transform.position.z), // waypoint 2
            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0f), transform.position.z),
            new Vector3(transform.position.x, groundLevelY + 0.5f + ((transform.position.y - groundLevelY) * 0f), transform.position.z)
        };
    }
}
