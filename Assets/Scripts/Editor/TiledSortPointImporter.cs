/*-----------------------------------------
Creation Date: 3/19/2024 3:47:22 PM
Author: theco
Description: Sets the sprite sort point of tiles on import.
-----------------------------------------*/

using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[AutoCustomTmxImporter()]
public class TiledSortPointImporter : CustomTmxImporter
{
    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        var map = args.ImportedSuperMap;
        var walls = map.GetComponentsInChildren<SuperCustomProperties>().Where(o => o.TryGetCustomProperty("spriteSortPointX", out CustomProperty temp)
                                                                               && o.TryGetCustomProperty("spriteSortPointY", out temp));
        foreach (var w in walls)
        {
            CustomProperty temp;
            w.TryGetCustomProperty("spriteSortPointX", out temp);
            float spriteSortPointX = temp.GetValueAsFloat();
            w.TryGetCustomProperty("spriteSortPointY", out temp);
            float spriteSortPointY = temp.GetValueAsFloat();

            Sprite[] sprites = new Sprite[w.GetComponent<Tilemap>().GetUsedSpritesCount()];
            w.GetComponent<Tilemap>().GetUsedSpritesNonAlloc(sprites);

            foreach (var s in sprites)
            {
                Debug.Log(s.pivot);
            }
        }
    }
}