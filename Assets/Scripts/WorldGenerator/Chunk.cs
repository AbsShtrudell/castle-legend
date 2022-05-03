using System;
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

    private Material material;
    private List<StaticBlock> blocksMap = new List<StaticBlock>();

    private event Action<Vector3> destroyBlock;
    private event Action<Vector3> addBlock;

    //-----------Object Controll-----------//

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

    public static class ChunkFabric
    {
        public static Chunk Create(Vector3Int coord, Material mat, Transform parent, Vector3 blockSize, Action<Vector3> destroyBlockAction, Action<Vector3> addBlockAction)
        {
            GameObject chunk = new GameObject($"Chunk ({coord.x}, {coord.y}, {coord.z})");
            chunk.transform.parent = parent;
            Chunk newChunk = chunk.AddComponent<Chunk>();

            newChunk.coord = coord;
            newChunk.destroyBlock += destroyBlockAction;
            newChunk.addBlock += addBlockAction;
            newChunk.SetUp(mat, blockSize);

            return newChunk;
        }
    }

    //-----------Blocks Controll-----------//

    public void AddBlock(Vector3 position)
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();

        collider.size = blockSize;
        collider.center = new Vector3(position.x * collider.size.x + collider.size.x / 2, position.y * collider.size.y, position.z * collider.size.z + collider.size.z / 2);

        StaticBlock block = new StaticBlock();
        block.SetUp(collider, new Vector3Int(1, 1, 1), position, DestroyBlock);

        blocksMap.Add(block);

        addBlock?.Invoke(position);
    }

    public void AddBlock(Collider collider, Vector3 normal)
    {
        StaticBlock block = GetBlock((BoxCollider)collider);
        StaticBlock newBlock;
        if (block != null)
        {
            newBlock = GetBlock(block.GetCoord() + normal);
            if (newBlock == null)
            {
                AddBlock(block.GetCoord() + normal);
            }
        }
    }

    public void DestroyBlock(StaticBlock block)
    {
        destroyBlock?.Invoke(block.GetCoord());
        block.Disable();
    }

    public void EnableBlock(Vector3 position)
    {
        foreach (StaticBlock block in blocksMap)
        {
            if (block.GetCoord() == position)
            {
                block.Enable();
                return;
            }
        }
        AddBlock(position);
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

    //-----------getters/setters-----------//

    public void SetMesh(Mesh mesh)
    {
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshCollider.enabled = false;
        meshCollider.enabled = true;
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

    public StaticBlock GetBlock(Vector3 position)
    {
        foreach (var block in blocksMap)
        {
            if (block.GetCoord() == position)
            {
                return block;
            }
        }
        return null;
    }

    public Vector3Int GetCoords()
    {
        return coord;
    }
}
