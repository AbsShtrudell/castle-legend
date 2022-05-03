using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiniMapIcon : MonoBehaviour
{
    [Zenject.Inject]
    private MiniMapManager minimap;

    private SpriteRenderer icon;

    private void OnEnable()
    {
        icon = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        icon.transform.localScale = new Vector3(minimap.iconScale, minimap.iconScale, 0);
    }
}
