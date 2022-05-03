using UnityEngine;
using Zenject;

public class MiniMapInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject MinimapCamera;
    public override void InstallBindings()
    {
        MiniMapManager miniMap = MinimapCamera.GetComponent<MiniMapManager>();
        Camera cam = GetComponent<Camera>();
        Container.Bind<MiniMapManager>().FromInstance(miniMap).AsSingle();
    }
}