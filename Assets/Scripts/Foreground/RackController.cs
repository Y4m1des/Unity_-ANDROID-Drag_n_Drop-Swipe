using System.Collections.Generic;
using UnityEngine;

public class RackController : MonoBehaviour
{
    #region PARAMS
    [field: SerializeField]
    private LayerMask layerMaskItem;
    [field: SerializeField]
    private LayerMask layerMaskDefault;
    [field: SerializeField]
    private const string itemLayerName = "Item";
    [field: SerializeField]
    private const string foregroundLayerName = "Foreground";
    #endregion

    private List<SpriteRenderer> objsRenders;

    private void Awake()
    {
        objsRenders = new List<SpriteRenderer>();
    }
    
    //places/removes an object on/from rack
    private void ChangeSpriteRenderer(Collision2D collision, string layerName, LayerMask layerMask, bool remove = false)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.excludeLayers = layerMask;

        SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = layerName;

        if(!remove)
        {
            //if game object places on rack:
            //add game object to the list of lying objects on rack
            objsRenders.Add(spriteRenderer);
            //and get appropriate index from list as layer order
            spriteRenderer.sortingOrder = objsRenders.FindIndex(sr => sr == spriteRenderer);
        }
        else
        {
            //if game object removes from rack:
            //remove game object from  the list of lying objects on rack
            objsRenders.Remove(spriteRenderer);
            //and set default layer order
            spriteRenderer.sortingOrder = 0;

            //changes layer order for remaning objects on the rack
            foreach (var sr in objsRenders)
            {
                sr.sortingOrder = objsRenders.FindIndex(sr => sr == spriteRenderer);
            }
        }
    }

    //calls [PLACE ON RACK] "ChangeSpriteRenderer"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeSpriteRenderer(collision, foregroundLayerName, layerMaskItem);
    }

    //calls [REMOVE FROM RACK] "ChangeSpriteRenderer"
    private void OnCollisionExit2D(Collision2D collision)
    {
        ChangeSpriteRenderer(collision, itemLayerName, layerMaskDefault, true);
    }
}
