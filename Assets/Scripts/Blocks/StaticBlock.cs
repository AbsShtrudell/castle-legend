using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StaticBlock : IBlock
{
    private Vector3 coord;
    private BoxCollider collider;
    private SnapPointsManager snapPointsManager;
    public event Action<StaticBlock> deleteBlock;

    //-----------Object Controll-----------//

    public void SetUp(BoxCollider collider, Vector3Int pointsPerAxis, Vector3 coord, Action<StaticBlock> deleteAction)
    {
        this.collider = collider;
        this.coord = coord;
        deleteBlock += deleteAction;

        snapPointsManager = new SnapPointsManager(this, pointsPerAxis, collider.bounds);
    }

    public void Disable()
    {
        collider.enabled = false;
    }

    public void Enable()
    {
        collider.enabled = true;
    }

    public void DeleteBlock()
    {
        deleteBlock?.Invoke(this);
    }

    //-----------Run-time Logic-----------//

    public bool PlaceBlock(IBlock block, Vector3 face, Vector3 location)
    {
        if (block == null) return false;

        SnapPoint snapPoint = snapPointsManager.GetClosestSnapPoint(face, location);
        if (snapPoint == null) return false;

        Vector3 thisPosition = GetRotation() * snapPoint.GetLocalPosition() + GetPosition();

        SnapPoint blockSnapPoint = block.GetSnapPointsManager().GetActiveSnapPoint(GetRotation() * snapPoint.GetFaceNormal());
        if (blockSnapPoint == null) return false;

        Vector3 blockPosition = block.GetRotation() * blockSnapPoint.GetLocalPosition() + block.GetPosition();
        block.SetPosition(block.GetPosition() + Vector3.Normalize(thisPosition - blockPosition) * Vector3.Distance(thisPosition, blockPosition));

        return true;
    }

    public bool SnapBlock(IBlock block, Vector3 face, Vector3 location)
    {
        if (block == null) return false;

        SnapPoint snapPoint = snapPointsManager.GetClosestSnapPoint(face, location);
        if (snapPoint == null) return false;

        Vector3 thisPosition = GetRotation() * snapPoint.GetLocalPosition() + GetPosition();

        SnapPoint blockSnapPoint = block.GetSnapPointsManager().GetActiveSnapPoint(GetRotation() * snapPoint.GetFaceNormal());
        if (blockSnapPoint == null) return false;

        Vector3 blockPosition = block.GetRotation() * blockSnapPoint.GetLocalPosition() + block.GetPosition();
        block.SetPosition(Vector3.Lerp(block.GetPosition(),
                                       block.GetPosition() + Vector3.Normalize(thisPosition - blockPosition) * Vector3.Distance(blockPosition, thisPosition),
                                       Time.deltaTime * 10f));
        return true;
    }

    //-----------getters/setters-----------//

    public void SetPosition(Vector3 position)
    {
        collider.transform.position = GetPosition();
    }

    public void SetRotation(float angle)
    {
        collider.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public SnapPointsManager GetSnapPointsManager()
    {
        return snapPointsManager;
    }

    public BoxCollider GetCollider()
    {
        return collider;
    }

    public Quaternion GetRotation()
    {
        return collider.transform.rotation;
    }

    public Vector3 GetPosition()
    {
        return collider.center;
    }

    public Vector3 GetCoord()
    {
        return coord;
    }

    public Vector3 GetSize()
    {
        return snapPointsManager.GetPointsPerAxis();
    }
}
