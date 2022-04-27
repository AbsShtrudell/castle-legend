using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBlock : MonoBehaviour, IBlock
{
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Vector3Int pointsPerAxis;
    [SerializeField]
    private SnapPointData[] DisabledPoints;

    private Bounds bounds;
    private Vector3 stepSize;
    private Vector3 cornerLocation;
    private int pointsAmount;
    private SnapPoint[] snapPoints;

    void Start()
    {
        if(GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().sharedMaterial = material;

        bounds = mesh.bounds;
        stepSize = GetStepSize();
        pointsAmount = GetPointsAmount();
        cornerLocation = GetCornerLocation();

        snapPoints = new SnapPoint[pointsAmount];

        CreateSnapPoints(GetPointsData());
    }

    public bool PlaceBlock(IBlock block, SnapPoint snapPoint)
    {
        Vector3 thisPosition = GetRotation() * snapPoint.GetLocalPosition() + GetPosition();

        SnapPoint blockSnapPoint = block.GetActiveSnapPoint(GetRotation() * snapPoint.GetFaceNormal());
        if (snapPoint == null) return false;

        Vector3 blockPosition = block.GetRotation() * blockSnapPoint.GetLocalPosition() + block.GetPosition();
        block.SetPosition(block.GetPosition() + Vector3.Normalize(thisPosition - blockPosition) * Vector3.Distance(thisPosition, blockPosition));
        snapPoint.Active(false);
        return true;
    }

    public bool SnapBlock(IBlock block, SnapPoint snapPoint)
    {
        Vector3 thisPosition = GetRotation() * snapPoint.GetLocalPosition() + GetPosition();

        SnapPoint blockSnapPoint = block.GetActiveSnapPoint(GetRotation() * snapPoint.GetFaceNormal());
        if(snapPoint == null) return false;

        Vector3 blockPosition = block.GetRotation() * blockSnapPoint.GetLocalPosition() + block.GetPosition();
        block.SetPosition(Vector3.Lerp(block.GetPosition(),
                                       block.GetPosition() + Vector3.Normalize(thisPosition - blockPosition) * Vector3.Distance(blockPosition, thisPosition),
                                       Time.deltaTime * 10f));
        return true;
    }

    public SnapPoint GetClosestSnapPoint(Vector3 face, Vector3 location)
    {
        SnapPoint closest = null;

        for (int i = 0; i < snapPoints.Length; i++)
        {
            if (face == GetRotation() * snapPoints[i].GetFaceNormal())
            {
                if ((closest == null || Vector3.Distance(GetPosition() + GetRotation() * snapPoints[i].GetLocalPosition(), location) <
                    Vector3.Distance(GetPosition() + GetRotation() * closest.GetLocalPosition(), location)) && snapPoints[i].isActive()) closest = snapPoints[i];
            }
        }
        return closest;
    }

    public SnapPoint GetActiveSnapPoint(Vector3 face)
    {
        return GetClosestSnapPoint(face * -1, cornerLocation);
    }

    private void CreateSnapPoints(SnapPointData[] pointsData)
    {
        for (int i = 0; i < pointsAmount; i++)
            snapPoints[i] = CreateSnapPoint(pointsData[i]);

    }
    private SnapPoint CreateSnapPoint(SnapPointData snapPointData)
    {
        Vector3 pointLocation = TranslatePointsLocation(snapPointData);
        return new SnapPoint(snapPointData, pointLocation, this);
    }

    private Vector3 TranslatePointsLocation(SnapPointData position)
    {
        float xPos = position.faceNormal.x * bounds.size.x / 2;
        float yPos = position.faceNormal.y * bounds.size.y / 2;
        float zPos = position.faceNormal.z * bounds.size.z / 2;

        xPos += xPos == 0 ? position.blockGridPosition.x * 2 * stepSize.x + stepSize.x - bounds.size.x / 2 : 0;
        yPos += yPos == 0 ? position.blockGridPosition.y * 2 * stepSize.y + stepSize.y - bounds.size.y / 2 : 0;
        zPos += zPos == 0 ? position.blockGridPosition.z * 2 * stepSize.z + stepSize.z - bounds.size.z / 2 : 0;

        return new Vector3(xPos, yPos, zPos);
    }

    private SnapPointData[] GetPointsData()
    {
        SnapPointData[] pointsData = new SnapPointData[pointsAmount];

        int counter = 0;
        for (Faces face = 0; face < Faces.Zero; face++)
        {
            int yLimit = (int)(pointsPerAxis.y * (Cube.GetFaceNormal(face).x == 0 ? Mathf.Abs(Cube.GetFaceNormal(face).z) : Mathf.Abs(Cube.GetFaceNormal(face).x)));
            int xLimit = (int)(pointsPerAxis.x * (Cube.GetFaceNormal(face).z == 0 ? Mathf.Abs(Cube.GetFaceNormal(face).y) : Mathf.Abs(Cube.GetFaceNormal(face).z)));
            int zLimit = (int)(pointsPerAxis.z * (Cube.GetFaceNormal(face).x == 0 ? Mathf.Abs(Cube.GetFaceNormal(face).y) : Mathf.Abs(Cube.GetFaceNormal(face).x)));
            int z = 0;
            do
            {
                int x = 0;
                do
                {
                    int y = 0;
                    do
                    {
                        SnapPointData snapPoint = new SnapPointData(face, new Vector3Int(x, y, z));
                        if (!IsPointDisabled(snapPoint))
                        {
                            pointsData[counter] = snapPoint;
                            counter++;
                        }
                        y++;
                    } while (y < yLimit);
                    x++;
                } while (x < xLimit);
                z++;
            } while (z < zLimit);
        }
        return pointsData;
    }

    private bool IsPointDisabled(SnapPointData position)
    {
        foreach (SnapPointData point in DisabledPoints)
        {
            if (position.faceNormal == Cube.GetFaceNormal(point.face) && position.blockGridPosition == point.blockGridPosition)
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 GetSize()
    {
        return pointsPerAxis;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private Vector3 GetStepSize()
    {
        return new Vector3(bounds.size.x / (pointsPerAxis.x * 2), bounds.size.y / (pointsPerAxis.y * 2), bounds.size.z / (pointsPerAxis.z * 2));
    }

    private Vector3 GetCornerLocation()
    {
        return new Vector3(gameObject.transform.position.x - bounds.size.x / 2, gameObject.transform.position.y - bounds.size.y / 2, gameObject.transform.position.z - bounds.size.z / 2);
    }

    private int GetPointsAmount()
    {
        return (pointsPerAxis.x * pointsPerAxis.z + pointsPerAxis.x * pointsPerAxis.y + pointsPerAxis.y * pointsPerAxis.z) * 2 - DisabledPoints.Length;
    }
    public void SetRotation(float angle)
    {
        transform.Rotate(new Vector3(0, angle, 0));
    }
    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
}
