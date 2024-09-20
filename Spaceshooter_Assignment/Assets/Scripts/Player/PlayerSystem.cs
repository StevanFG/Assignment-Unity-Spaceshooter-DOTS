using Unity.Transforms;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct PlayerSystem : ISystem
{ 
    private Entity playerEntity;
    private Entity inputEntity;
    private EntityManager entityManager;
    private PlayerComponent playerComponent;
    private InputComponent inputComponent;

    public void OnUpdate(ref SystemState state)
    {
        entityManager = state.EntityManager;
        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        inputComponent = entityManager.GetComponentData<InputComponent>(inputEntity);

        Move(ref state);
        Shoot(ref state);
    }

    private void Move(ref SystemState state)
    {
        LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);

        playerTransform.Position += new float3(inputComponent.movement * playerComponent.moveSpeed * SystemAPI.Time.DeltaTime, 0);

        Vector2 dir = (Vector2)inputComponent.mousePos - (Vector2)Camera.main.WorldToScreenPoint(playerTransform.Position);
        float angle = math.degrees(math.atan2(dir.y, dir.x)) + -90f;
        playerTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        entityManager.SetComponentData(playerEntity, playerTransform);
    }

    private float nextShootTime;
    private void Shoot(ref SystemState state)
    {
        if (inputComponent.leftClick && nextShootTime < SystemAPI.Time.ElapsedTime)
        {
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);

            Entity bulletEntity = entityManager.Instantiate(playerComponent.bulletPrefab);

            ECB.AddComponent(bulletEntity, new BulletComponent { bulletSpeed = 10, bulletDamage = 20 });

            LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(bulletEntity);
            bulletTransform.Rotation = entityManager.GetComponentData<LocalTransform>(playerEntity).Rotation;
            LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);
            bulletTransform.Position = playerTransform.Position + playerTransform.Right() * 0.10f + playerTransform.Up() * 0.70f; 
            ECB.SetComponent(bulletEntity, bulletTransform);

            ECB.Playback(entityManager);

            nextShootTime = (float)SystemAPI.Time.ElapsedTime + playerComponent.shootCD;
        }
    }
}
