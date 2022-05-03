using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBlock : MonoBehaviour, IBlock, IPoolable<DynamicBlock>
{
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Vector3Int pointsPerAxis;
    [SerializeField]
    private SnapPointData[] DisabledPoints;

    private SnapPointsManager snapPointsManager;

    private Action<DynamicBlock> returnToPool;

    //-----------Object Controll-----------//

    void OnEnable()
    {
        if(GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if(mesh != null)
            GetComponent<MeshFilter>().sharedMesh = mesh;

        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();

        if(material != null)
            GetComponent<MeshRenderer>().sharedMaterial = material;

        snapPointsManager = new SnapPointsManager(this, pointsPerAxis, mesh.bounds, DisabledPoints);
    }

    private void OnDisable()
    {
        ReturnToPool();
    }

    public void Initialize(Action<DynamicBlock> returnAction)
    {
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        transform.position = Vector3.zero;
        returnToPool?.Invoke(this);
    }

    public void DeleteBlock()
    {
        ReturnToPool();
    }

    //-----------Run-time Logic-----------//

    public bool PlaceBlock(IBlock block, Vector3 face, Vector3 location)
    {
        if( block == null) return false;

        SnapPoint snapPoint = snapPointsManager.GetClosestSnapPoint(face, location);
        if(snapPoint == null) return false;

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
        if(blockSnapPoint == null) return false;

        Vector3 blockPosition = block.GetRotation() * blockSnapPoint.GetLocalPosition() + block.GetPosition();
        block.SetPosition(Vector3.Lerp(block.GetPosition(),
                                       block.GetPosition() + Vector3.Normalize(thisPosition - blockPosition) * Vector3.Distance(blockPosition, thisPosition),
                                       Time.deltaTime * 10f));
        return true;
    }

    //-----------getters/setters-----------//

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(float angle)
    {
        transform.Rotate(new Vector3(0, angle, 0));
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Vector3 GetSize()
    {
        return pointsPerAxis;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public SnapPointsManager GetSnapPointsManager()
    {
        return snapPointsManager;
    }
}
