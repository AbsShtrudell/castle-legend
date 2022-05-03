using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField]
    private float clusterRadius;
    [SerializeField]
    private EnvironmentType[] environments;
    [SerializeField]
    private int numSamplesInCluster = 30;
    [SerializeField]
    private int numSamplesCluster = 240;
    [Inject] 
    DiContainer container;

    //-----------Environment Generation-----------//

    public void GenerateEnvironment(Vector3 location, Vector2 size)
    {
        Vector2[] clustersPositions = GenerateClustersLocations(size);
        Vector2 position;
        EnvironmentType environment;

        for (int i = 0; i < clustersPositions.Length; i++)
        {
            position = clustersPositions[i] + new Vector2(location.x, location.z);
            environment = environments[Random.Range(0, environments.Length)];
            GenerateCluster(environment, position);
        }
    }

    private void GenerateCluster(EnvironmentType env, Vector2 location)
    {
        Vector2[] objPositions = PoissonDiscSamplinng.GeneratePoints(env.objectsRadius, env.clusterSize, numSamplesInCluster);
        Vector3 objLocation;
        GameObject prefab;
        GameObject go;

        for (int i = 0; i < objPositions.Length; i++)
        {
            objLocation = new Vector3(objPositions[i].x + location.x, 500, objPositions[i].y + location.y);
            prefab = env.variants[Random.Range(0, env.variants.Length)];

            go = container.InstantiatePrefab(prefab, objLocation, Quaternion.identity, null);

            Ray ray = new Ray(objLocation, Vector3.down);
            RaycastHit hitData;
            if (!Physics.Raycast(ray, out hitData, 1000))
            {
                GameObject.Destroy(go);
            }

            go.transform.position = hitData.point;
            go.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        }
    }

    private Vector2[] GenerateClustersLocations(Vector2 size)
    {
        return PoissonDiscSamplinng.GeneratePoints(clusterRadius, size, numSamplesCluster);
    }

    //-----------Others-----------//

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
