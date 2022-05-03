using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiniMapManager : MonoBehaviour
{
    [Zenject.Inject] TerrainGenerator terrain;
    Camera cam;

    public float iconScale
    {
        get { return CalculateIconScale(); }
    }

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (cam != null)
        {
            cam.transform.position = new Vector3(terrain.GetTerrainBounds().center.x, 1000, terrain.GetTerrainBounds().center.z);
            cam.orthographicSize = terrain.GetTerrainBounds().size.x / 2;
        }
    }

    private float CalculateIconScale()
    {
        return terrain.GetTerrainBounds().size.x / 300 * 2.5f ;
    }
}
