using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager
{
    private List<LocalNavMesh> NavMeshList = new List<LocalNavMesh>();
    private List<NavMeshGroup> NavMeshGroups = new List<NavMeshGroup>();

    public void AddLocalNavMesh(Transform target, Vector3 size)
    {
        if (target != null)
        {
            NavMeshList.Add(LocalNavMesh.Factory.Create(target, size, this));
            NavMeshGroups.Add(new NavMeshGroup(new List<LocalNavMesh>() { NavMeshList[NavMeshList.Count - 1] }));
        }
    }

    public void UpdateNavMesh(LocalNavMesh caller)
    {
        int groupID = GetGroupID(caller);
        int updateOldGroup = -1;

        if (NavMeshGroups[groupID].GetMembers().Count != 1)
        {
            if (!NavMeshGroups[groupID].InGroupBounds(caller))
            {
                NavMeshGroups[groupID].RemoveFromGroup(caller);

                updateOldGroup = groupID;
                groupID = AddInEmptyGroup(caller);
            }
        }

        for (int i = 0; i < NavMeshGroups.Count; i++)
        {
            if (groupID != i)
            {
                if (NavMeshGroups[i].InGroupBounds(caller))
                {
                    NavMeshGroups.Remove(NavMeshGroup.MergeGroups(NavMeshGroups[GetGroupID(caller)], NavMeshGroups[i]));
                    groupID = GetGroupID(caller);
                }
            }
        }

        NavMeshGroups[groupID].UpdateGroupNavMesh();

        if(updateOldGroup != -1)
        {
            if (NavMeshGroups[groupID].GetMembers().Count != 0)
                NavMeshGroups[updateOldGroup].UpdateGroupNavMesh();
            else NavMeshGroups.Remove(NavMeshGroups[groupID]);
        }
    }

    private int AddInEmptyGroup(LocalNavMesh navMesh)
    {
        NavMeshGroups.Add(new NavMeshGroup(new List<LocalNavMesh>() { navMesh }));
        return NavMeshGroups.Count - 1;
    }

    private int GetGroupID(LocalNavMesh navMesh)
    {
        for (int i = 0; i < NavMeshGroups.Count; i++)
            if (NavMeshGroups[i].InGroup(navMesh)) return i;

        return -1;
    }

    private class NavMeshGroup
    {
        private List<LocalNavMesh> members;
        private NavMeshDataInstance instance;
        private NavMeshData navMeshData;

        public NavMeshGroup(List<LocalNavMesh> groupMembers)
        {
            if (groupMembers != null)
            {
                members = groupMembers;
            }
            else members = new List<LocalNavMesh>();
            navMeshData = new NavMeshData();
            instance = NavMesh.AddNavMeshData(navMeshData);
        }

        ~NavMeshGroup()
        {
            Destroy();
        }

        public void Destroy()
        {
            members.Clear();
            instance.Remove();
        }

        public void UpdateGroupNavMesh()
        {
            Bounds bounds = GetGroupBounds(this);

            for (int i = 0; i < members.Count; i++)
            {
                NavMeshUpdater.UpdateNavMesh(bounds, navMeshData, true); ;
            }
        }

        public bool InGroup(LocalNavMesh navMesh)
        {
            if (members.Contains(navMesh)) return true;
            else return false;
        }

        public bool RemoveFromGroup(LocalNavMesh navMesh)
        {
            if (members.Contains(navMesh))
            {
                members.Remove(navMesh);
                return true;
            }
            return false;
        }

        public void AddInGroup(LocalNavMesh navMesh)
        {
            if(!members.Contains(navMesh))
            {
                members.Add(navMesh);
            }
        }

        public static NavMeshGroup MergeGroups(NavMeshGroup group1, NavMeshGroup group2)
        {
            if (group1.members != null && group2.members != null)
            {
                NavMeshGroup smallerGroup = group1.members.Count >= group2.members.Count ? group2 : group1;
                NavMeshGroup biggerGroup =  group1.members.Count >= group2.members.Count ? group1 : group2;

                for (int i = 0; i < smallerGroup.members.Count; i++)
                {
                    biggerGroup.members.Add(smallerGroup.members[i]);
                }
                smallerGroup.Destroy();
                return smallerGroup;
            }
            return null;
        }

        public bool InGroupBounds(LocalNavMesh navMesh)
        {
            if (members.Count != 0)
            {
                NavMeshGroup group = new NavMeshGroup(null);

                for (int i = 0; i < members.Count; i++)
                {
                    if (navMesh != members[i]) group.AddInGroup(members[i]);
                }

                return navMesh.InBounds(GetGroupBounds(group));
            }
            return false;
        }

        public static Bounds GetGroupBounds(NavMeshGroup group)
        {
            Bounds[] boundsArray = new Bounds[group.members.Count];
            Vector3[] centers = new Vector3[group.members.Count];
            Bounds bounds = new Bounds();

            for (int i = 0; i < group.members.Count; i++)
            {
                boundsArray[i] = group.members[i].GetBounds();
                centers[i] = boundsArray[i].center;
            }

            bounds.center = GetGroupCentroid(centers);

            for (int i = 0; i < group.members.Count; i++)
            {
                bounds.Encapsulate(boundsArray[i]);
            }
            return bounds;
        }

        public static Vector3 GetGroupCentroid(Vector3[] points)
        {
            Vector3 outp = Vector3.zero;
            for (int i = 0; i < points.Length; i++)
            {
                outp += points[i];
            }
            return outp /= points.Length;
        }

        public List<LocalNavMesh> GetMembers()
        {
            return members;
        }
    }
}
