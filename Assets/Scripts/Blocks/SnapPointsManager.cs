using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointsManager
{
    private IBlock parent;

    private Vector3Int pointsPerAxis;
    private Vector3 cornerLocation;
    private Vector3 stepSize;
    private Bounds bounds;
    private SnapPointData[] DisabledPoints;
    private SnapPoint[] snapPoints;
    
    private int pointsAmount;

    //-----------Object Controll-----------//

    public SnapPointsManager(IBlock parent, Vector3Int pointsPerAxis,Bounds bounds,SnapPointData[] DisabledPoints = null)
    {
        this.parent = parent;
        this.pointsPerAxis = pointsPerAxis;
        this.bounds = bounds;
        this.DisabledPoints = DisabledPoints;

        stepSize = GetStepSize();
        pointsAmount = CalculatePointsAmount();

        snapPoints = new SnapPoint[pointsAmount];

        CreateSnapPoints(GetPointsData());

        cornerLocation = GetCornerLocation();
    }

    //-----------Game Logic-----------//

    public SnapPoint GetClosestSnapPoint(Vector3 face, Vector3 location)
    {
        SnapPoint closest = null;

        for (int i = 0; i < snapPoints.Length; i++)
        {
            if (face == parent.GetRotation() * snapPoints[i].GetFaceNormal())
            {
                if ((closest == null || Vector3.Distance(parent.GetPosition() + parent.GetRotation() * snapPoints[i].GetLocalPosition(), location) <
                    Vector3.Distance(parent.GetPosition() + parent.GetRotation() * closest.GetLocalPosition(), location)) && snapPoints[i].isActive()) closest = snapPoints[i];
            }
        }
        return closest;
    }

    public SnapPoint GetActiveSnapPoint(Vector3 face)
    {
        return GetClosestSnapPoint(face * -1, cornerLocation);
    }

    //-----------Initialization-----------//

    private void CreateSnapPoints(SnapPointData[] pointsData)
    {
        for (int i = 0; i < pointsAmount; i++)
            snapPoints[i] = CreateSnapPoint(pointsData[i]);
    }

    private SnapPoint CreateSnapPoint(SnapPointData snapPointData)
    {
        Vector3 pointLocation = TranslatePointsLocation(snapPointData);
        return new SnapPoint(snapPointData, pointLocation);
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

    //-----------Getters/Setters-----------//

    private bool IsPointDisabled(SnapPointData position)
    {
        if (DisabledPoints != null)
        {
            foreach (SnapPointData point in DisabledPoints)
            {
                if (position.faceNormal == Cube.GetFaceNormal(point.face) && position.blockGridPosition == point.blockGridPosition)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private int CalculatePointsAmount()
    {
        return (pointsPerAxis.x * pointsPerAxis.z + pointsPerAxis.x * pointsPerAxis.y + pointsPerAxis.y * pointsPerAxis.z) * 2 - (DisabledPoints == null? 0 : DisabledPoints.Length);
    }

    private Vector3 GetStepSize()
    {
        return new Vector3(bounds.size.x / (pointsPerAxis.x * 2), bounds.size.y / (pointsPerAxis.y * 2), bounds.size.z / (pointsPerAxis.z * 2));
    }

    public Vector3 GetCornerLocation()
    {
        return new Vector3(parent.GetPosition().x - bounds.size.x / 2, parent.GetPosition().y - bounds.size.y / 2, parent.GetPosition().z - bounds.size.z / 2);
    }

    public Vector3 GetPointsPerAxis()
    {
        return pointsPerAxis;
    }

}
