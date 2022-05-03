using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject playerPrefab;
    public override void InstallBindings()
    {
        PlayerController player = playerPrefab.GetComponent<PlayerController>();
        BlocksContainer container = playerPrefab.GetComponent<BlocksContainer>();
        Container.Bind<PlayerController>().FromInstance(player).AsSingle();
        Container.Bind<BlocksContainer>().FromInstance(container).AsSingle();
    }
}