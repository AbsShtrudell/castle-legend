using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public void EnableRenderers(bool e)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = e;
        }
        GetComponent<Renderer>().enabled = true; 
    }

    public void CombineMeshes()
    {
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        Renderer renderer;
        Mesh mesh;
        List<Mesh> finalMeshes = new List<Mesh>();
        List<Material> materials = new List<Material>();
        List< List<CombineInstance>> submeshes = new List<List<CombineInstance>> ();
        CombineInstance combine;

        foreach (MeshFilter filter in meshFilters)
        {
            if (filter.transform == transform) continue;
            renderer = filter.GetComponent<Renderer>();
            int i = 0;
            do
            {
                if (materials.Count == 0 || !materials[i].Equals(renderer.sharedMaterial))
                {
                    materials.Add(renderer.sharedMaterial);
                    submeshes.Add(new List<CombineInstance>());
                    combine = new CombineInstance();
                    combine.subMeshIndex = i;
                    combine.mesh = filter.sharedMesh;
                    combine.transform = filter.transform.localToWorldMatrix;

                    submeshes[i].Add(combine);
                    break;
                }
                else
                {
                    combine = new CombineInstance();
                    combine.subMeshIndex = i;
                    combine.mesh = filter.sharedMesh;
                    combine.transform = filter.transform.localToWorldMatrix;

                    submeshes[i].Add(combine);
                }
                i++;
            } while (i < materials.Count);
        }

        CombineInstance[] combineInstances = new CombineInstance[materials.Count];
        for (int i = 0; i < materials.Count; i++)
        {
            mesh = new Mesh();
            mesh.CombineMeshes(submeshes[i].ToArray());

            combineInstances[i] = new CombineInstance();
            combineInstances[i].mesh = mesh;
            combineInstances[i].subMeshIndex = 0;

            finalMeshes.Add(mesh);
        }

        mesh = new Mesh();
        mesh.CombineMeshes(combineInstances);
        mesh.Optimize();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
        transform.position = oldPos;
        transform.rotation = oldRot;
        EnableRenderers(false);
    }
}
