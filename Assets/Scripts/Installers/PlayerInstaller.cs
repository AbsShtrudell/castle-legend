using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _playerPrefab;
    public override void InstallBindings()
    {
        GameObject playerInstance = Container.InstantiatePrefab(_playerPrefab);
    }
}