using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint
{
    private SnapPointData pointData;
    private Vector3 localPosition;
    private bool active = true;
    private IBlock parent;

    public SnapPoint(SnapPointData pointdata, Vector3 localposition, IBlock parent)
    {
        pointData = pointdata;
        localPosition = localposition;
        this.parent = parent;
    }
    public SnapPoint(Faces face, Vector3Int blockGridPosition, Vector3 localposition, IBlock parent)
    {
        pointData = new SnapPointData(face, blockGridPosition);
        localPosition = localposition;
        this.parent = parent;
    }

    public Vector3 GetFaceNormal()
    {
        return pointData.faceNormal;
    }
    public Vector3Int GetBlockGridPosition()
    {
        return pointData.blockGridPosition;
    }
    public Vector3 GetLocalPosition()
    {
        return localPosition;
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

    private Vector3 _faceNormal;

    [HideInInspector]
    public Vector3 faceNormal
    {
        get { return Cube.GetFaceNormal(face); }
        set { _faceNormal = value; face = Cube.GetFace(value); }
    }

    public SnapPointData(Faces faceValue, Vector3Int blockGridPositionValue)
    {
        blockGridPosition = blockGridPositionValue;
        _faceNormal = Cube.GetFaceNormal(faceValue);

        face = faceValue;
        faceNormal = _faceNormal;
    }
}
