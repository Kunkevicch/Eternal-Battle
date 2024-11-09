using EndlessRoad;
using UnityEngine;
using Zenject;

public class ShooterGameInstaller : MonoInstaller
{
    [SerializeField] private ObjectForPooling _poolingObjects;

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
    }
}