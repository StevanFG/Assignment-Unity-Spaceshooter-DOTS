using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public partial struct EnemyAISystem : ISystem
{
    private EntityManager entityManager;
    private Entity playerEntity;

    private void OnUpdate(ref SystemState state)
    {
        entityManager = state.EntityManager;
        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        foreach (var (enemyComponent, transformComponent) in SystemAPI.Query<EnemyComponent, RefRW<LocalTransform>>())
        {
            float3 direction = entityManager.GetComponentData<LocalTransform>(playerEntity).Position - transformComponent.ValueRO.Position;
            float angle = math.atan2(direction.y, direction.x) + math.radians(90);
            transformComponent.ValueRW.Rotation = quaternion.Euler(new float3(0, 0, angle));

            transformComponent.ValueRW.Position += math.normalize(direction) * SystemAPI.Time.DeltaTime;
        }
    }
}