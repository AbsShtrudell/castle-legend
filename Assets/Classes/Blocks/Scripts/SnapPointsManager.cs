using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{ Back = 0, Down, Front, Left, Right, Up, Zero };

public class SnapPointsManager : MonoBehaviour
{
    [SerializeField]
    private int xPoints;
    [SerializeField]
    private int yPoints;
    [SerializeField]
    private int zPoints;
    [SerializeField]
    private GameObject SnapPointRef;
    [SerializeField]
    private SnapPointData[] DisabledPoints;
    private Bounds bounds;

    private List<GameObject> SnapPoints = new List<GameObject>();

    void Start()
    {
        CreateSnapPoints(GetPoints());
    }

    private void OnEnable()
    {
        bounds = gameObject.GetComponent<MeshFilter>().mesh.bounds;
    }

    private SnapPointData[] GetPoints()
    {
        int pointsAmount = GetPointsAmount();
        SnapPointData[] pointsData = new SnapPointData[pointsAmount];
        int counter = 0;

        for(Axis face = 0; face <= Axis.Up; face++)
        {
            int z = 0;
            do
            {
                int x = 0;
                do
                {
                    int y = 0;
                    do
                    {
                        SnapPointData pointData = new SnapPointData(face, new Vector3(x, y, z));
                        if (!IsPointDisabled(pointData))
                        {
                            pointsData[counter] = pointData;
                        }
                        counter++;
                        y++;
                    } while (y < yPoints * (SnapPointData.GetNormal(face).x == 0 ? Mathf.Abs(SnapPointData.GetNormal(face).z) : Mathf.Abs(SnapPointData.GetNormal(face).x)));
                    x++;
                } while (x < xPoints * (SnapPointData.GetNormal(face).z == 0 ? Mathf.Abs(SnapPointData.GetNormal(face).y) : Mathf.Abs(SnapPointData.GetNormal(face).z)));
                z++;
            } while (z < zPoints * (SnapPointData.GetNormal(face).x == 0 ? Mathf.Abs(SnapPointData.GetNormal(face).y) : Mathf.Abs(SnapPointData.GetNormal(face).x)));
        }
        return pointsData;
    }

    private Vector3 TranslatePointsLocation(SnapPointData position)
    { 
        float xStep = bounds.size.x / (xPoints * 2);
        float yStep = bounds.size.y / (yPoints * 2);
        float zStep = bounds.size.z / (zPoints * 2);

        float xPos = position.normal.x * bounds.size.x / 2;
        float yPos = position.normal.y * bounds.size.y / 2;
        float zPos = position.normal.z * bounds.size.z / 2;

        xPos += xPos == 0 ? position.blockGridPosition.x * 2 * xStep + xStep - bounds.size.x / 2 : 0;
        yPos += yPos == 0 ? position.blockGridPosition.y * 2 * yStep + yStep - bounds.size.y / 2 : 0;
        zPos += zPos == 0 ? position.blockGridPosition.z * 2 * zStep + zStep - bounds.size.z / 2 : 0;

        return new Vector3(xPos, yPos, zPos);
    }

    private int GetPointsAmount()
    {
        return (xPoints * zPoints + xPoints * yPoints + yPoints * zPoints) * 2;
    }

    private void CreateSnapPoints(SnapPointData[] pointsData)
    {
        foreach (SnapPointData point in pointsData)
        {
            SnapPoints.Add(CreateSnapPoint(point));
        }
    }
    private GameObject CreateSnapPoint(SnapPointData snapPointData)
    {
        Vector3 pointLocation = TranslatePointsLocation(snapPointData);
        GameObject newSnapPoint = GameObject.Instantiate(SnapPointRef, gameObject.transform.position + pointLocation, gameObject.transform.rotation, gameObject.transform);
        newSnapPoint.GetComponent<SnapPointDataStorage>().pointData = snapPointData;

        return newSnapPoint;
    }

    private bool IsPointDisabled(SnapPointData position)
    {
        foreach(SnapPointData point in DisabledPoints)
        {
            if(position.normal == point.normal && position.blockGridPosition == point.blockGridPosition)
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 GetCornerLocation()
    {
        return new Vector3(gameObject.transform.position.x - bounds.size.x / 2, gameObject.transform.position.y - bounds.size.y / 2, gameObject.transform.position.z - bounds.size.z / 2);
    }

    public GameObject GetClosestSnapPoint(Vector3 face, Vector3 location)
    {
        GameObject closest = null;

        foreach (GameObject Snap in SnapPoints)
        {
            if(face == Snap.GetComponent<SnapPointDataStorage>().pointData.normal)
            {
                if (closest == null)
                    closest = Snap;
                else
                    if(Vector3.Distance(Snap.transform.position, location) < Vector3.Distance(closest.transform.position, location)) closest = Snap;
            }
        }
        return closest;
    }

    public void SnapBlock(GameObject SnapPoint)
    {
        transform.position = Vector3.Lerp(gameObject.transform.position, gameObject.transform.position + Vector3.Normalize(SnapPoint.transform.position - GetActiveSnapPoint(SnapPoint).transform.position) * Vector3.Distance(GetActiveSnapPoint(SnapPoint).transform.position, SnapPoint.transform.position), Time.deltaTime * 10f);
        Debug.Log("Snap");
    }
    private GameObject GetActiveSnapPoint(GameObject SnapPoint)
    {
        return GetClosestSnapPoint(SnapPoint.GetComponent<SnapPointDataStorage>().pointData.normal * -1, GetCornerLocation());
    }
}