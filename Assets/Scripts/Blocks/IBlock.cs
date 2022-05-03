using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock 
{
    public bool PlaceBlock(IBlock block, Vector3 face, Vector3 location);
    public bool SnapBlock(IBlock block, Vector3 face, Vector3 location);
    public void DeleteBlock();
    public void SetPosition(Vector3 position);
    public void SetRotation(float angle);
    public SnapPointsManager GetSnapPointsManager();
    public Quaternion GetRotation();
    public Vector3 GetPosition();
    public Vector3 GetSize();
}