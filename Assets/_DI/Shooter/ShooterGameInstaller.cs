using EndlessRoad;
using UnityEngine;
using Zenject;

public class ShooterGameInstaller : MonoInstaller
{
    //TODO: перенести иницилазилацию в старт игры
    [SerializeField] private ObjectForPooling _poolingObjects;
    [SerializeField] private InitializingWeapons _initializingWeapons;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<ObjectPool>()
            .AsSingle()
            .WithArguments(_poolingObjects.Pool)
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<ImpactService>()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<EventBus>()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<WeaponHolder>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<EnemiesController>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<InitializingWeapons>()
            .FromScriptableObject(_initializingWeapons)
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<GameObserver>()
            .AsSingle()
            .Lazy();

        Container
            .BindInterfacesAndSelfTo<WeaponFactory>()
            .AsSingle();
    }
}