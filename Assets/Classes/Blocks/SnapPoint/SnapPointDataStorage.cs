using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SnapPointData
{
    [SerializeField]
    private Vector3 m_blockGridPosition;
    [SerializeField]
    private Axis m_axis;

    private Vector3 m_normal;

    public Vector3 normal
    {
        get { return m_normal; }
        set { m_normal = value; m_axis = GetAxis(value); }
    }
    
    public Axis axis
    { 
        get { return m_axis; } 
        set { m_axis = value; m_normal = GetNormal(value); }
    }
    public Vector3 blockGridPosition
    {
        get { return m_blockGridPosition; }
        set { m_blockGridPosition = value; }
    }

    public SnapPointData(Axis axisValue, Vector3 blockGridPositionValue)
    {
        m_blockGridPosition = blockGridPositionValue;
        m_axis = axisValue;
        m_normal = GetNormal(axisValue);
    }

    static private Vector3[] Faces = new Vector3[6] {
        Vector3.back,
        Vector3.down,
        Vector3.forward,
        Vector3.left,
        Vector3.right,
        Vector3.up
    };

    static public Vector3 GetNormal(Axis face)
    {
        return Faces[(int)face];
    }
    static public Axis GetAxis(Vector3 face)
    {
        for (int i = 0; i < Faces.Length; i++)
        {
            if (Faces[i] == face)
                return (Axis)i;
        }
        return Axis.Zero;
    }
}

public class SnapPointDataStorage : MonoBehaviour
{
    public SnapPointData pointData;
}
