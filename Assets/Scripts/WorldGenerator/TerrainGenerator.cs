using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[Header("General Setting")]
	[SerializeField]
	private GameObject blockRef;
	[SerializeField]
	private Material material;
	[Space()]
	[SerializeField]
	private int Width = 50;
	[SerializeField]
	private int Height = 10;
	[Space()]
	[SerializeField]
	private Vector3Int ChunkSize;


	[Header("Noise Setting")]
	[SerializeField, Range(0f, 1f)]
	private float terrainSurface = 0.5f;
	[SerializeField]
	private float noiseScale = 4f;
	[SerializeField]
	private Vector2 offset;

	private bool[,,] blocks;
	private float[,,] terrainMap;

	private Vector3Int GridSize;
	private Vector3 bounds;
	private List<Chunk> chunks = new List<Chunk>();

	private void Start()
	{
		bounds = blockRef.GetComponent<MeshFilter>().sharedMesh.bounds.size;
		terrainMap = new float[Width * ChunkSize.x + 1, Height + 1, Width * ChunkSize.z + 1];
		GridSize = new Vector3Int(Width * ChunkSize.x, Height, Width * ChunkSize.z);
		blocks = new bool[Width * ChunkSize.x, Height, Width * ChunkSize.z];

		for (int x = 0; x < Width * ChunkSize.x + 1; x++)
			for (int z = 0; z < Width * ChunkSize.z + 1; z++)
				for (int y = 0; y < Height; y++) terrainMap[x, y, z] = 1f;

		InitChunks();
		PopulateTerrainMap();

		foreach (Chunk chunk in chunks)
		{
			UpdateMeshData(chunk);
		}
		CreateAllVisibleBlocks();
        Vector3 pos = new Vector3(GridSize.x * bounds.x, 0, GridSize.z * bounds.z) + transform.position;
        Vector2 sz = new Vector2(GridSize.x * bounds.x, GridSize.z * bounds.z);
        GetComponent<EnvironmentGeneration>().GenerateEnvironment(transform.position, sz);
	} //rewrite

	private void InitChunks()
	{
		for (int x = 0; x < Width; x++)
			for (int z = 0; z < Width; z++)
				chunks.Add(Chunk.ChunkFabric.Create(new Vector3Int(x, 0, z), material, transform));
	}

	private void UpdateMeshData(Chunk chunk)
	{
		float[] cube = new float[Cube.CornersAmount];
		MeshBuilder meshBuilder = new MeshBuilder();

		Vector3Int position = new Vector3Int(chunk.GetCoords().x * ChunkSize.x, GridSize.y - 1, chunk.GetCoords().z * ChunkSize.z);
		Vector3Int limit = position + ChunkSize;
		Vector3Int corner;

		for (position.x = chunk.GetCoords().x * ChunkSize.x; position.x < limit.x; position.x++)
		{
			for (position.z = chunk.GetCoords().z * ChunkSize.z; position.z < limit.z; position.z++)
			{
				for (position.y = 0; position.y < limit.y; position.y++)
				{
					for (int i = 0; i < cube.Length; i++)
					{
						corner = position + Cube.CornerTable[i];
						cube[i] = terrainMap[corner.x, corner.y, corner.z];
					}

					meshBuilder.AddVertices(MarchingCubes.MarchCube(position, bounds, cube));
				}
			}
		}
		chunk.SetMesh(meshBuilder.BuildMesh());
	}

	private void PopulateTerrainMap()
	{
		Vector3 position = Vector3.zero;
		float thisHeight;
		float point;

		for (position.x = 0; position.x < GridSize.x; position.x++)
		{
			for (position.z = 0; position.z < GridSize.z; position.z++)
			{
				for (position.y = 0; position.y < GridSize.y; position.y++)
				{
					thisHeight = (float)GridSize.y * Mathf.PerlinNoise((float)position.x * noiseScale + offset.x, (float)position.z * noiseScale + offset.y);

					if (position.y <= thisHeight - (1 - terrainSurface))
						point = 0f;
					else point = 1f;

					blocks[(int)position.x, (int)position.y, (int)position.z] = point == 0f;

					if (blocks[(int)position.x, (int)position.y, (int)position.z])
					{
						FillBlock(ref position, 0f);
					}
					else terrainMap[(int)position.x, (int)position.y, (int)position.z] = 1f;
				}
			}
		}
	}

	private void FillBlock(ref Vector3 position, float value)
	{
		Vector3 corner;
		for (int i = 0; i < 8; i++)
		{
			corner = position + Cube.CornerTable[i];
			terrainMap[(int)corner.x, (int)corner.y, (int)corner.z] = value;
		}
	}

	private void CreateAllVisibleBlocks()
	{
		Vector3 position = Vector3.zero;
		for (position.x = 0; position.x < GridSize.x; position.x++)
		{
			for (position.z = 0; position.z < GridSize.z; position.z++)
			{
				for (position.y = 0; position.y < GridSize.y; position.y++)
				{
					if (IsBlockVisible(ref position))
					{
						GetBlocksChunk(ref position).AddBlock(ref position, ref bounds);
					}
				}
			}
		}
	}

	private bool IsBlockVisible(ref Vector3 position)
	{
		if (!blocks[(int)position.x, (int)position.y, (int)position.z]) return false;

		Vector3 neighbourPosition;

		for (Faces face = 0; face < Faces.Zero; face++)
		{
			neighbourPosition = Cube.GetFaceNormal(face) + position;

			if (IsPositionInBounds(neighbourPosition))
			{
				if (!blocks[(int)neighbourPosition.x, (int)neighbourPosition.y, (int)neighbourPosition.z]) return true;
			}
			else continue;
		}
		return false;
	}

	private void DestroyBlock(ref Vector3 position)
    {
		if (blocks[(int)position.x, (int)position.y, (int)position.z] != false)
		{
			blocks[(int)position.x, (int)position.y, (int)position.z] = false;
			UpdateNeighboursVisibility(ref position);
			UpdateTerrainMapAroundBlock(ref position);
			GetBlocksChunk(ref position).DisableBlock(ref position);
		}
	}

	private void UpdateNeighboursVisibility(ref Vector3 position)
    {
		Vector3 neighbourPosition;

		for (Faces face = 0; face < Faces.Zero; face++)
		{
			neighbourPosition = Cube.GetFaceNormal(face) + position;

			if (IsPositionInBounds(neighbourPosition))
			{
				if(IsBlockVisible(ref neighbourPosition)) GetBlocksChunk(ref neighbourPosition).EnableBlock(ref neighbourPosition);
				else GetBlocksChunk(ref neighbourPosition).DisableBlock(ref neighbourPosition);
			}
			else continue;
		}
	}

	private void UpdateTerrainMapAroundBlock(ref Vector3 position)
    {
		if (blocks[(int)position.x, (int)position.y, (int)position.z])
		{
			FillBlock(ref position, 0f);
		}
        else
        {
			FillBlock(ref position, 1f);
			Vector3 neighbourPosition;
			Vector3Int neighbours = Vector3Int.zero;
			for (neighbours.x = -1; neighbours.x <= 1; neighbours.x++)
			{
				for (neighbours.y = -1; neighbours.y <= 1; neighbours.y++)
				{
					for (neighbours.z = -1; neighbours.z <= 1; neighbours.z++)
					{
						neighbourPosition = position + neighbours;
						if (IsPositionInBounds(neighbourPosition) && blocks[(int)neighbourPosition.x, (int)neighbourPosition.y, (int)neighbourPosition.z])
							FillBlock(ref neighbourPosition, 0f);
					}
				}
			}
		}
	}

    private Chunk GetBlocksChunk(ref Vector3 position)
    {
        Vector3Int coord;
        foreach (Chunk chunk in chunks)
        {
            coord = new Vector3Int((int)position.x / ChunkSize.x, 0, (int)position.z / ChunkSize.z);
            if (chunk.GetCoords() == coord)
                return chunk;
        }
        return null;
    }

	private bool IsPositionInBounds(Vector3 position)
    {
		return (position.x >= 0 && position.y >= 0 && position.z >= 0
				&& position.x <GridSize.x && position.y < GridSize.y && position.z < GridSize.z);
    }
}