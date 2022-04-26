using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGeneration : MonoBehaviour
{
    [SerializeField]
    private float clusterRadius;
    [SerializeField]
    private EnvironmentType[] environments;

    public void GenerateEnvironment(Vector3 center, Vector2 size)
    {
        Vector2[] clustersPositions = GenerateClustersLocations(size);

        for (int i = 0; i < clustersPositions.Length; i++)
        {
            GenerateCluster(environments[Random.Range(0, environments.Length)], clustersPositions[i] + new Vector2(center.x, center.z));
        }
    }

    private Vector2[] GenerateClustersLocations(Vector2 size)
    {
        return PoissonDiscSamplinng.GeneratePoints(clusterRadius, size, 160);
    }

    private void GenerateCluster(EnvironmentType env, Vector2 center)
    {
        Vector2[] objPositions = PoissonDiscSamplinng.GeneratePoints(env.objectsRadius, env.clusterSize, 30);
        for (int i = 0; i < objPositions.Length; i++)
        {
            Vector3 objLocation = new Vector3(objPositions[i].x + center.x, 500, objPositions[i].y + center.y);
            GameObject go = Instantiate(env.variants[Random.Range(0, env.variants.Length)], objLocation, Quaternion.identity);

            Ray ray = new Ray(objLocation, Vector3.down);
            RaycastHit hitData;
            if (!Physics.Raycast(ray, out hitData, 1000))
            {
                GameObject.Destroy(go);
            }
            Vector3 sz = go.GetComponent<MeshFilter>().sharedMesh.bounds.size;
            go.transform.position = hitData.point;
        }
    }

    [System.Serializable]
    public struct EnvironmentType
    {
        [SerializeField]
        public Vector2 clusterSize;
        [SerializeField]
        public float objectsRadius;
        [SerializeField]
        public GameObject[] variants;
    }
}
