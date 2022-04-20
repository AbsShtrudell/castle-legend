using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private int Whidth;
    [SerializeField]
    private int Depth;
    [SerializeField]
    private int Height;
    [SerializeField]
    private GameObject BlockRef;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 SpawnLocation = Vector3.zero;
        Bounds bounds = BlockRef.GetComponent<MeshFilter>().sharedMesh.bounds;
        for (int k = 0; k < Height; k++)
        {
            for (int i = 0; i < Whidth; i++)
            {
                for (int j = 0; j < Depth; j++)
                {
                    GameObject.Instantiate(BlockRef, SpawnLocation, new Quaternion());
                    SpawnLocation.x += bounds.size.x;
                }
                SpawnLocation.z += bounds.size.z;
                SpawnLocation.x = 0;
            }
            SpawnLocation.z = 0;
            SpawnLocation.x = 0;
            SpawnLocation.y += bounds.size.y;
        }
    }
}
