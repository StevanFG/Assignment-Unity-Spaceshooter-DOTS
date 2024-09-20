using Unity.Mathematics;
using UnityEngine.InputSystem;
using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private Controls controls;
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton<InputComponent>(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }

        controls = new Controls();
        controls.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 moveVector = controls.ActionMap.Movement.ReadValue<Vector2>();
        Vector2 mousePosition = controls.ActionMap.MousePos.ReadValue<Vector2>();
        bool isLeftClick = controls.ActionMap.Shooting.ReadValue<float>() == 1 ? true : false;

        SystemAPI.SetSingleton(new InputComponent { mousePos = mousePosition, movement = moveVector, leftClick = isLeftClick });
    }
}
