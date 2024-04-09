/*-----------------------------------------
Creation Date: 4/9/2024 2:00:27 PM
Author: theco
Description: Attached to color switch blocks.
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SwitchBlock : MonoBehaviour
{
    [SerializeField] Dungeon.SwitchColor thisColor;
    [SerializeField] Sprite sprite_on;
    [SerializeField] Sprite sprite_off;
    DungeonManager dm;

    void Start()
    {
        dm = DungeonManager.instance;
    }

    void Update()
    {
        if (dm.currentDungeon >= 0)
        {
            UpdateState();
        }
    }

    void UpdateState()
    {
        if (thisColor != dm.dungeons[dm.currentDungeon].currentColor)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprite_off;
            GetComponentInChildren<SpriteRenderer>().sortingLayerName = "AboveGround";
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprite_on;
            GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Entity";
            GetComponent<Collider2D>().enabled = true;
        }
    }
}
