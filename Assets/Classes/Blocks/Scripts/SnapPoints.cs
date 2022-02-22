using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoints : MonoBehaviour
{
    public int xPoints;
    public int yPoints;
    public int zPoints;
    public GameObject SnapPointRef;
    public Vector3[] DisabledPoints;

    void Start()
    {
        CreateSnapPoints(GetPoints());
    }

    private Vector3[] GetPoints()
    {
        int pointsAmount = GetPointsAmount();
        Vector3[] pointsLocations = new Vector3[pointsAmount];

        int counter = 0;
        //front and back
        {
            int z = -1;
            for(int x = 0; x < xPoints; x++)
            {
                for(int y = 0; y < yPoints; y++)
                {
                    if (isPointDisabled(x, y, z)) continue;
                    pointsLocations[counter] = TranslatePointsRelativeLocation(x, y, z);
                    counter ++;
                }
            }
            z = zPoints + 1;
            for (int x = 0; x < xPoints; x++)
            {
                for (int y = 0; y < yPoints; y++)
                {
                    if (isPointDisabled(x, y, z)) continue;
                    pointsLocations[counter] = TranslatePointsRelativeLocation(x, y, z);
                    counter++;
                }
            }
        }
        //left and right
        {
            int x = -1;
            for (int z = 0; z < zPoints; z++)
            {
                for (int y = 0; y < yPoints; y++)
                {
                    if (isPointDisabled(x, y, z)) continue;
                    pointsLocations[counter] = TranslatePointsRelativeLocation(x, y, z);
                    counter++;
                }
            }
            x = xPoints + 1;
            for (int z = 0; z < zPoints; z++)
            {
                for (int y = 0; y < yPoints; y++)
                {
                    if (isPointDisabled(x, y, z)) continue;
                    pointsLocations[counter] = TranslatePointsRelativeLocation(x, y, z);
                    counter++;
                }
            }
        }
        //top and bottom
        {
            int y = -1;
            for (int z = 0; z < zPoints; z++)
            {
                for (int x = 0; x < xPoints; x++)
                {
                    if (isPointDisabled(x, y, z)) continue;
                    pointsLocations[counter] = TranslatePointsRelativeLocation(x, y, z);
                    counter++;
                }
            }
            y = yPoints + 1;
            for (int z = 0; z < zPoints; z++)
            {
                for (int x = 0; x < xPoints; x++)
                {
                    if (isPointDisabled(x, y, z)) continue;
                    pointsLocations[counter] = TranslatePointsRelativeLocation(x, y, z);
                    counter++;
                }
            }
        }
        return pointsLocations;
    }

    private bool isPointDisabled(int x,int y,int z)
    {
        Vector3 relativePointLocation = new Vector3(x, y, z);
        foreach (Vector3 disabledPoint in DisabledPoints)
        {
            if (disabledPoint == relativePointLocation) return true;
        }
        return false;
    }

    private Vector3 TranslatePointsRelativeLocation(int x, int y, int z)
    {
        Bounds bounds = gameObject.GetComponent<MeshFilter>().mesh.bounds;

        float xStep = bounds.size.x / (xPoints * 2);
        float yStep = bounds.size.y / (yPoints * 2);
        float zStep = bounds.size.z / (zPoints * 2);

        float xPos = x * 2 * xStep + xStep - bounds.size.x / 2;
        float yPos = y * 2 * yStep + yStep - bounds.size.y / 2;
        float zPos = z * 2 * zStep + zStep - bounds.size.z / 2;

        Vector3 output;

        if (x == -1 || x == xPoints+1)
        {
            xPos = x/Mathf.Abs(x) * bounds.size.x/2 ;
            output = new Vector3(xPos, yPos, zPos);
            return output;
        }
        if (y == -1 || y == yPoints + 1)
        {
            yPos = y / Mathf.Abs(y) * bounds.size.y/2;
            output = new Vector3(xPos, yPos, zPos);
            return output;
        }
        if (z == -1 || z == zPoints + 1)
        {
            zPos = z / Mathf.Abs(z) * bounds.size.z/2;
            output = new Vector3(xPos, yPos, zPos);
            return output;
        }

        output = new Vector3(xPos, yPos, zPos);
        return output;
    }

    private int GetPointsAmount()
    {
        return (xPoints * zPoints + xPoints * yPoints + yPoints * zPoints) * 2;  
    }

    void CreateSnapPoints(Vector3[] pointsLocations)
    {
        foreach (Vector3 pointLocation in pointsLocations) 
        {
            Debug.Log(pointLocation);
            GameObject.Instantiate(SnapPointRef, gameObject.transform.position + pointLocation, gameObject.transform.rotation, gameObject.transform);
        }
    }

    void Update()
    {
        
    }
}