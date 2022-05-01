using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private Vector3Int coord;
    private Vector3 blockSize;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private LODGroup lodGroup;
    private Material material;
    private List<MeshRenderer> LODS = new List<MeshRenderer>();
    private List<StaticBlock> blocksMap = new List<StaticBlock>();

    private void SetUp(Material mat, Vector3 blockSize)
    {
        GameObject go = new GameObject($"Chunk ({coord.x}, {coord.y}, {coord.z}) mesh");
        go.transform.parent = transform.parent.transform;

        meshFilter = go.GetComponent<MeshFilter>();
        meshRenderer = go.GetComponent<MeshRenderer>();
        meshCollider = go.GetComponent<MeshCollider>();

        if (meshFilter == null)
        {
            meshFilter = go.gameObject.AddComponent<MeshFilter>();
        }

        if (meshRenderer == null)
        {
            meshRenderer = go.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        if (meshCollider == null)
        {
            meshCollider = go.gameObject.AddComponent<MeshCollider>();
        }

        gameObject.AddComponent<StaticBlockQueue>();
        go.AddComponent<NavMeshSourceTag>();

        meshCollider.enabled = false;
        meshCollider.enabled = true;

        material = mat;
        meshRenderer.material = material;

        this.blockSize = blockSize;

        go.gameObject.layer = LayerMask.NameToLayer("Terrain");
        gameObject.layer = LayerMask.NameToLayer("StaticBlock");
    }

    public void SetMesh(Mesh mesh)
    {
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }

    public void AddBlock(ref Vector3 position)
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = blockSize;
        collider.center = new Vector3(position.x * collider.size.x + collider.size.x / 2, position.y * collider.size.y, position.z * collider.size.z + collider.size.z / 2);

        StaticBlock block = new StaticBlock();
        block.SetUp(collider, new Vector3Int(1, 1, 1), position);

        blocksMap.Add(block);
    }

    public void RemoveBlock(StaticBlock block)
    {
        transform.parent.GetComponent<TerrainGenerator>().DestroyBlock(block.GetCoord()); 
        //blocksMap.Remove(block);
    }

    public StaticBlock GetBlock(BoxCollider collider)
    {
        foreach (var block in blocksMap)
        {
            if (block.GetCollider() == collider)
            {
                return block;
            }
        }
        return null;
    }

    public void EnableBlock(ref Vector3 position)
    {
        foreach (var block in blocksMap)
        {
            if (block.GetCoord() == position)
            {
                block.Enable();
                return;
            }
        }
        AddBlock(ref position);
    }

    public bool DisableBlock(Vector3 position)
    {
        foreach (var block in blocksMap)
        {
            if (block.GetCoord() == position)
            {
                block.Disable();
                return true;
            }
        }
        return false;
    }

    public static Vector3 CalculateBlockPosition(BoxCollider collider)
    {

        return new Vector3((collider.center.x - collider.size.x / 2)/ collider.size.x, collider.center.y / collider.size.y, (collider.center.z - collider.size.z / 2) / collider.size.z);
    }

    public Vector3Int GetCoords()
    {
        return coord;
    }

    public static class Fabric
    {
        public static Chunk Create(Vector3Int coord, Material mat, Transform parent, Vector3 blockSize)
        {
            GameObject chunk = new GameObject($"Chunk ({coord.x}, {coord.y}, {coord.z})");
            chunk.transform.parent = parent;
            Chunk newChunk = chunk.AddComponent<Chunk>();

            newChunk.coord = coord;
            newChunk.SetUp(mat, blockSize);

            return newChunk;
        }
    }
}
