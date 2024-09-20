using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;

public partial struct BulletSystem : ISystem
{
    private EntityQuery bulletQuery;
    private EntityQuery enemyQuery;

    public void OnCreate(ref SystemState state)
    {
        bulletQuery = state.EntityManager.CreateEntityQuery(typeof(BulletComponent), typeof(LocalTransform));
        enemyQuery = state.EntityManager.CreateEntityQuery(typeof(EnemyComponent), typeof(LocalTransform));
    }

    private void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> bullets = bulletQuery.ToEntityArray(Allocator.Temp);
        NativeArray<Entity> enemies = enemyQuery.ToEntityArray(Allocator.Temp);

        foreach (Entity bullet in bullets)
        {
            if (entityManager.HasComponent<BulletComponent>(bullet))
            {
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(bullet);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(bullet);

                bulletTransform.Position += bulletComponent.bulletSpeed * SystemAPI.Time.DeltaTime * bulletTransform.Up();
                entityManager.SetComponentData(bullet, bulletTransform);

                Debug.Log($"Bullet Damage: {bulletComponent.bulletDamage}"); 

                for (int i = 0; i < enemies.Length; i++)
                {
                    Entity enemy = enemies[i];

                    if (entityManager.HasComponent<EnemyComponent>(enemy))
                    {
                        LocalTransform enemyTransform = entityManager.GetComponentData<LocalTransform>(enemy);

                        if (CheckCollision(bulletTransform.Position, enemyTransform.Position))
                        {
                            Debug.Log("Enemy hit");

                            EnemyComponent enemyComponent = entityManager.GetComponentData<EnemyComponent>(enemy);
                            Debug.Log($"Enemy Health Before: {enemyComponent.currentHealth}");

                            enemyComponent.currentHealth -= bulletComponent.bulletDamage;

                            Debug.Log($"Enemy Health After: {enemyComponent.currentHealth}");

                            entityManager.SetComponentData(enemy, enemyComponent);

                            if (enemyComponent.currentHealth <= 0)
                            {
                                entityManager.DestroyEntity(enemy);
                            }

                            entityManager.DestroyEntity(bullet);
                            break;
                        }
                    }
                }
            }
        }

        bullets.Dispose();
        enemies.Dispose();
    }
    private bool CheckCollision(float3 bulletPos, float3 enemyPos)
    {
        float threshold = 0.5f; 
        return math.distance(bulletPos, enemyPos) < threshold;
    }
}