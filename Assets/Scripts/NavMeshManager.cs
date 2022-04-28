using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    private List<LocalNavMesh> NavMeshList;

    private List<List<LocalNavMesh>> NavMeshGroups;

    private void Awake()
    {
        NavMeshList = new List<LocalNavMesh>();
        NavMeshGroups = new List<List<LocalNavMesh>>();
    }

    public LocalNavMesh CreateLocalNavMesh(Transform target, Vector3 size)
    {
        if (target != null)
        {
            NavMeshList.Add(target.gameObject.AddComponent<LocalNavMesh>());

            NavMeshList[NavMeshList.Count - 1].navMeshManager = this;
            NavMeshList[NavMeshList.Count - 1].m_Size = size;
            NavMeshGroups.Add(new List<LocalNavMesh> { NavMeshList[NavMeshList.Count - 1] });

            return NavMeshList[NavMeshList.Count - 1];
        }
        return null;
    }

    public void UpdateNavMesh(LocalNavMesh caller)
    {
        int groupID = GetGroupID(caller);

        if(!InGroupBounds(caller, groupID))
        {
            RemoveFromGroup(caller);

            UpdateGroupNavMesh(groupID);

            groupID = GetGroupID(caller);
        }

        for (int i = 0; i < NavMeshList.Count; i++)
        {
            if (InGroupBounds(caller, i))
            {
                MergeGroups(GetGroupID(caller), i);
            }
        }

        UpdateGroupNavMesh(GetGroupID(caller));
    }

    private int GetGroupID(LocalNavMesh navMesh)
    {
        for(int i = 0; i < NavMeshGroups.Count; i++)
        {
            for(int j = 0; j < NavMeshGroups[i].Count; j++)
            {
                if(NavMeshGroups[i][j] == navMesh) return i;
            }
        }
        return -1;
    }

    private int AddInEmptyGroup(LocalNavMesh navMesh)
    {
        for (int i = 0; i < NavMeshGroups.Count; i++)
        {
            if (NavMeshGroups[i].Count == 0)
            {
                NavMeshGroups[i].Add(navMesh);
                return i;
            }
        }
        NavMeshGroups.Add(new List<LocalNavMesh> { navMesh });
        return NavMeshGroups.Count - 1;
    }

    private void RemoveFromGroup(LocalNavMesh navMesh)
    {
        int groupID = GetGroupID(navMesh);

        if(groupID != -1)
            NavMeshGroups[groupID].Remove(navMesh);

        AddInEmptyGroup(navMesh);
    }

    private void MergeGroups(int group1, int group2)
    {
        if(group1 != -1 && group2 != -1)
        {
            List<LocalNavMesh> smaller = NavMeshGroups[group1].Count >= NavMeshGroups[group2].Count ? NavMeshGroups[group2] : NavMeshGroups[group1];
            List<LocalNavMesh> bigger = NavMeshGroups[group1].Count >= NavMeshGroups[group2].Count ? NavMeshGroups[group1] : NavMeshGroups[group2];

            for (int i = 0; i < smaller.Count; i++)
            {
                bigger.Add(smaller[i]);
            }
            smaller.Clear();
            NavMeshGroups.Remove(smaller);
        }
    }

    private bool InGroupBounds(LocalNavMesh navMesh, int groupID)
    {
        if(groupID == -1) return false;

        List<LocalNavMesh> group = new List<LocalNavMesh>();

        for(int i = 0; i < NavMeshGroups[groupID].Count; i++)
        {
            if(navMesh != NavMeshGroups[groupID][i]) group.Add(NavMeshGroups[groupID][i]);
        }

        return navMesh.InBounds(GetGroupBounds(group));
    }

    private bool InGroup(LocalNavMesh navMesh, int groupID)
    {
        if(groupID == GetGroupID(navMesh)) return true;
        return false;
    }

    private void UpdateGroupNavMesh(int groupID)
    {
        Bounds bounds = GetGroupBounds(NavMeshGroups[groupID]);

        for (int i = 0; i < NavMeshGroups[groupID].Count; i++)
        {
            NavMeshGroups[groupID][i].UpdateNavMesh(bounds, true); ;
        }
    }

    Bounds GetGroupBounds(List<LocalNavMesh> group)
    {
        Vector3[] centers = new Vector3[group.Count];

        for(int i = 0; i < group.Count; i++) centers[i] = group[i].GetBounds().center;

        Bounds bounds = new Bounds(GetCentroid(centers), Vector3.zero);

        for (int i = 0; i < group.Count; i++)
        {
            bounds.Encapsulate(group[i].GetBounds());
        }
        return bounds;
    }

    Vector3 GetCentroid(Vector3[] points)
    {
        Vector3 outp = Vector3.zero;
        for(int i = 0; i < points.Length; i++)
        {
            outp += points[i];
        }
        return outp /= points.Length;
    }
}
