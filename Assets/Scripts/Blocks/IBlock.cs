using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock 
{
    public bool PlaceBlock(IBlock block, SnapPoint snapPoint);
    public bool SnapBlock(IBlock block, SnapPoint snapPoint);
    public void DeleteBlock();
    public SnapPoint GetClosestSnapPoint(Vector3 face, Vector3 location);
    public SnapPoint GetActiveSnapPoint(Vector3 face);
    public Vector3 GetSize();
    public Vector3 GetPosition();
    public void SetPosition(Vector3 position);
    public void SetRotation(float angle);
    public Quaternion GetRotation();
}