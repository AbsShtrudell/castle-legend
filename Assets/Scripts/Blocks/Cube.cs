using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cube
{
	public enum Faces
	{ Back = 0, Bottom, Front, Left, Right, Head, Zero };

	private static Vector3[] faces = new Vector3[6] {
		Vector3.back,
		Vector3.down,
		Vector3.forward,
		Vector3.left,
		Vector3.right,
		Vector3.up
	};

	public static readonly int CornersAmount = 8;

	public static readonly Vector3Int[] CornerTable = new Vector3Int[8] {

		new Vector3Int(0, 0, 0),
		new Vector3Int(1, 0, 0),
		new Vector3Int(1, 1, 0),
		new Vector3Int(0, 1, 0),
		new Vector3Int(0, 0, 1),
		new Vector3Int(1, 0, 1),
		new Vector3Int(1, 1, 1),
		new Vector3Int(0, 1, 1)

	};

	public static readonly int[,] EdgeIndexes = new int[12, 2] {

		{0, 1}, {1, 2}, {3, 2}, {0, 3}, {4, 5}, {5, 6}, {7, 6}, {4, 7}, {0, 4}, {1, 5}, {2, 6}, {3, 7}
	};

	static public Vector3 GetFaceNormal(Faces face)
	{
		return faces[(int)face];
	}

	static public Faces GetFace(Vector3 face)
	{
		for (int i = 0; i < faces.Length; i++)
		{
			if (faces[i] == face)
				return (Faces)i;
		}
		return Faces.Zero;
	}

}
