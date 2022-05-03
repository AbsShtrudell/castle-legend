using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TerrainGenerator))]
[RequireComponent(typeof(EnvironmentGenerator))]
public class World : MonoBehaviour
{
    [Zenject.Inject]
    private PlayerController player;
    [Zenject.Inject]
    private DiContainer container;

    private TerrainGenerator terrGen;
    private EnvironmentGenerator envGen;

    public GameObject pawnref;

    private void Start()
    {
        envGen = GetComponent<EnvironmentGenerator>();
        terrGen = GetComponent<TerrainGenerator>();

        terrGen.GenerateTerrain();
        envGen.GenerateEnvironment(Vector3.zero , new Vector2(terrGen.GetTerrainBounds().size.x, terrGen.GetTerrainBounds().size.z));

        player.transform.position = new Vector3(terrGen.GetTerrainBounds().center.x / 2.5f, 0, terrGen.GetTerrainBounds().center.z / 2.5f);

        SpawnPawns();
    }

    private void SpawnPawns()
    {
        Vector2[] locations = PoissonDiscSamplinng.GeneratePoints(5, new Vector2(20, 20), 30);

        for (int i = 0; i < locations.Length; i++)
        {
            Vector3 location = new Vector3(locations[i].x + terrGen.GetTerrainBounds().center.x, 500, locations[i].y + terrGen.GetTerrainBounds().center.z);

            GameObject go = container.InstantiatePrefab(pawnref, location, Quaternion.identity, null);

            Ray ray = new Ray(location, Vector3.down);
            RaycastHit hitData;
            if (!Physics.Raycast(ray, out hitData, 1000))
            {
                GameObject.Destroy(go);
            }

            go.transform.position = hitData.point;
            go.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        }
    }
}
