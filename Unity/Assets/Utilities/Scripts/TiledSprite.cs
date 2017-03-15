using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TiledSprite : MonoBehaviour 
{

	#region Fields

    public bool TileOnStart = true;
    public bool ApplyParentScale = false;
    private SpriteRenderer spriteRenderer;

	#endregion
			

	#region Behaviours

    private void Start()
    {
        if (TileOnStart) CreateTiles();
    }

    #endregion


    #region Public Methods

    public void CreateTiles()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;

        Vector2 scale = (ApplyParentScale) ? transform.localScale : transform.lossyScale;
        scale = scale.Snap(.5f);
        Vector2 localSize = new Vector2(1f / scale.x, 1f / scale.y);
        Quaternion rotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        float gridWidth = (sprite.bounds.size.x / scale.x);
        float gridHeight = (sprite.bounds.size.y / scale.y);

        string name = gameObject.name + "(Tile)";

        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < scale.y; j++)
            {
                GameObject tile = new GameObject(name);
                SpriteRenderer tileRenderer = tile.AddComponent<SpriteRenderer>();
                tileRenderer.sprite = sprite;
                tileRenderer.color = spriteRenderer.color;
                tileRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
                tileRenderer.sortingOrder = spriteRenderer.sortingOrder;

                tile.transform.parent = transform;
                tile.transform.localPosition = new Vector2(i * gridWidth, j * gridHeight);
                if (ApplyParentScale) tile.transform.localScale = localSize;
            }
        }
        transform.rotation = rotation;

        Destroy(this);
        Destroy(spriteRenderer);
    }

    #endregion

}
