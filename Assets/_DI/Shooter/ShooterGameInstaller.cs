using EndlessRoad;
using EndlessRoad.Shooter;
using UnityEngine;
using Zenject;

public class ShooterGameInstaller : MonoInstaller
{
    [SerializeField] private ObjectForPooling _poolingObjects;
    [SerializeField] private InitializingWeapons _initializingWeapons;
    [SerializeField] private PlayerControl _player;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ObjectPool>()
            .AsSingle()
            .WithArguments(_poolingObjects.Pool)
            .NonLazy();

        Container.BindInterfacesAndSelfTo<ImpactService>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<EventBus>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<WeaponHolder>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<EnemiesController>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<InitializingWeapons>()
            .FromScriptableObject(_initializingWeapons)
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<GameObserver>()
            .AsSingle()
            .WithArguments(_player)
            .Lazy();
    }
}