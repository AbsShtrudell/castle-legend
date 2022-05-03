using UnityEngine;
using Zenject;

public class TerrainInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject world;
    public override void InstallBindings()
    {
        TerrainGenerator generator = world.GetComponent<TerrainGenerator>();
        Container.Bind<TerrainGenerator>().FromInstance(generator).AsSingle().NonLazy();
    }
}