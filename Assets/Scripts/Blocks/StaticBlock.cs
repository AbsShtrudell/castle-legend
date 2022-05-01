using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticBlock : IBlock
{
    private Vector3 coord;
    private Vector3Int pointsPerAxis;
    private SnapPoint[] snapPoints;
    private BoxCollider collider;

    private Vector3 stepSize;
    private Vector3 cornerLocation;
    private int pointsAmount;

    public void SetUp(BoxCollider collider, Vector3Int pointsPerAxis, Vector3 coord)
    {
        this.collider = collider;
        this.pointsPerAxis = pointsPerAxis;
        this.coord = coord;

        pointsAmount = GetPointsAmount();
        snapPoints = new SnapPoint[pointsAmount];

        stepSize = GetStepSize();
        cornerLocation = GetCornerLocation();

        CreateSnapPoints(GetPointsData());
    }

    public void Disable()
    {
        collider.enabled = false;
    }

    public void Destroy()
    {
        GameObject.Destroy(collider);
    }

    public void Enable()
    {
        collider.enabled = true;
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
        if (snapPoint == null) return false;

        Vector3 blockPosition = block.GetRotation() * blockSnapPoint.GetLocalPosition() + block.GetPosition();
        block.SetPosition(Vector3.Lerp(block.GetPosition(),
                                       block.GetPosition() + Vector3.Normalize(thisPosition - blockPosition) * Vector3.Distance(blockPosition, thisPosition),
                                       Time.deltaTime * 10f));
        return true;
    }

    public void DeleteBlock()
    {
        Disable();
        collider.GetComponent<Chunk>().RemoveBlock(this);
    }

    public SnapPoint GetClosestSnapPoint(Vector3 face, Vector3 location)
    {
        SnapPoint closest = null;

        for (int i = 0; i < snapPoints.Length; i++)
        {
            if (face == GetRotation() * snapPoints[i].GetFaceNormal())
            {
                if ((closest == null || Vector3.Distance(GetPosition() + GetRotation() * snapPoints[i].GetLocalPosition(), location) <
                    Vector3.Distance(GetPosition() + GetRotation() * closest.GetLocalPosition(), location) && snapPoints[i].isActive())) closest = snapPoints[i];
            }
        }
        return closest;
    }

    public SnapPoint GetActiveSnapPoint(Vector3 face)
    {
        return GetClosestSnapPoint(face * -1, GetRotation() * cornerLocation);
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
        float xPos = position.faceNormal.x * collider.size.x / 2;
        float yPos = position.faceNormal.y * collider.size.y / 2;
        float zPos = position.faceNormal.z * collider.size.z / 2;

        xPos += xPos == 0 ? position.blockGridPosition.x * 2 * stepSize.x + stepSize.x - collider.size.x / 2 : 0;
        yPos += yPos == 0 ? position.blockGridPosition.y * 2 * stepSize.y + stepSize.y - collider.size.y / 2 : 0;
        zPos += zPos == 0 ? position.blockGridPosition.z * 2 * stepSize.z + stepSize.z - collider.size.z / 2 : 0;

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
                        pointsData[counter] = new SnapPointData(face, new Vector3Int(x, y, z));
                        counter++;
                        y++;
                    } while (y < yLimit);
                    x++;
                } while (x < xLimit);
                z++;
            } while (z < zLimit);
        }
        return pointsData;
    }

    public Vector3 GetSize()
    {
        return pointsPerAxis;
    }

    private Vector3 GetStepSize()
    {
        return new Vector3(collider.size.x / (pointsPerAxis.x * 2), collider.size.y / (pointsPerAxis.y * 2), collider.size.z / (pointsPerAxis.z * 2));
    }

    private Vector3 GetCornerLocation()
    {
        return new Vector3(collider.transform.position.x - collider.size.x / 2, collider.transform.position.y - collider.size.y / 2, collider.transform.position.z - collider.size.z / 2);
    }

    public Vector3 GetPosition()
    {
        return collider.center;
    }

    private int GetPointsAmount()
    {
        return (pointsPerAxis.x * pointsPerAxis.z + pointsPerAxis.x * pointsPerAxis.y + pointsPerAxis.y * pointsPerAxis.z) * 2;
    }

    public void SetPosition(Vector3 position)
    {
        collider.transform.position = GetPosition();
    }

    public void SetRotation(float angle)
    {
        collider.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public Quaternion GetRotation()
    {
        return collider.transform.rotation;
    }

    public BoxCollider GetCollider()
    {
        return collider;
    }
    public Vector3 GetCoord()
    {
        return coord;
    }
}
