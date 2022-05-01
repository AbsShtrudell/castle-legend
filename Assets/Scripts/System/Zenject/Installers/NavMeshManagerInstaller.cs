using UnityEngine;
using Zenject;

public class NavMeshManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<NavMeshManager>().AsSingle();
    }
}