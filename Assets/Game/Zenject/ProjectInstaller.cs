using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ProjectInstaller : MonoInstaller
{

    //[SerializeField] private PlayerInput input;

    public override void InstallBindings()
    {
        //Container.Bind<PlayerInput>().FromInstance(input).AsSingle().NonLazy();

    }
}