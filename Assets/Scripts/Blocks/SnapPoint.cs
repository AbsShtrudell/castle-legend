using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint
{
    private SnapPointData pointData;
    private Vector3 localPosition;
    private bool active = true;

    public SnapPoint(SnapPointData pointdata, Vector3 localposition)
    {
        pointData = pointdata;
        localPosition = localposition;
    }
    public SnapPoint(Faces face, Vector3Int blockGridPosition, Vector3 localposition)
    {
        pointData = new SnapPointData(face, blockGridPosition);
        localPosition = localposition;
    }

    //-----------getters/setters-----------//

    public Vector3 GetFaceNormal()
    {
        return pointData.faceNormal;
    }

    public Vector3 GetLocalPosition()
    {
        return localPosition;
    }

    public Vector3Int GetBlockGridPosition()
    {
        return pointData.blockGridPosition;
    }

    public bool isActive() => active;

    public void Active(bool state) => active = state;
}

[System.Serializable]
public struct SnapPointData
{
    [SerializeField]
    public Vector3Int blockGridPosition;
    [SerializeField]
    public Faces face;
    [HideInInspector]
    public Vector3 faceNormal
    {
        get { return Cube.GetFaceNormal(face); }
        set { _faceNormal = value; face = Cube.GetFace(value); }
    }

    private Vector3 _faceNormal;

    public SnapPointData(Faces faceValue, Vector3Int blockGridPositionValue)
    {
        blockGridPosition = blockGridPositionValue;
        _faceNormal = Cube.GetFaceNormal(faceValue);

        face = faceValue;
        faceNormal = _faceNormal;
    }
}
