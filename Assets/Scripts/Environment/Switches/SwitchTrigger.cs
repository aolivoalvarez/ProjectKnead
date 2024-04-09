/*-----------------------------------------
Creation Date: 4/9/2024 1:17:20 PM
Author: theco
Description: Attached to color switches.
-----------------------------------------*/

using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class SwitchTrigger : MonoBehaviour
{
    [SerializeField] Sprite sprite_red;
    [SerializeField] Sprite sprite_blue;
    Dungeon.SwitchColor thisColor;
    DungeonManager dm;

    void Start()
    {
        dm = DungeonManager.instance;
    }

    void Update()
    {
        if (dm.currentDungeon >= 0 && thisColor != dm.dungeons[dm.currentDungeon].currentColor)
        {
            thisColor = dm.dungeons[dm.currentDungeon].currentColor;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        switch (thisColor)
        {
            case Dungeon.SwitchColor.Red:
                GetComponentInChildren<SpriteRenderer>().sprite = sprite_red;
                break;
            case Dungeon.SwitchColor.Blue:
            default:
                GetComponentInChildren<SpriteRenderer>().sprite = sprite_blue;
                break;
        }
    }

    void ChangeColor()
    {
        switch (thisColor)
        {
            case Dungeon.SwitchColor.Red:
                thisColor = Dungeon.SwitchColor.Blue;
                break;
            case Dungeon.SwitchColor.Blue:
            default:
                thisColor = Dungeon.SwitchColor.Red;
                break;
        }
        dm.dungeons[dm.currentDungeon].currentColor = thisColor;
        UpdateSprite();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerSword"))
        {
            ChangeColor();
        }
    }
}
