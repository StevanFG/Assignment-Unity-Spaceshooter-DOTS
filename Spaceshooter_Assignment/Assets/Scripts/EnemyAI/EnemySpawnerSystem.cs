using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
using Unity.Transforms;
using System;


public partial class EnemySpawnerSystem : SystemBase
{
    private EnemySpawnerComponent enemySpawnerComponent;
    private EnemyDataContainer enemyDataContainerComponent;
    private Entity enemySpawnerEntity;
    private float nextSpawnTime;
    private Random random;

    protected override void OnCreate()
    {
        random = Random.CreateFromIndex((uint)enemySpawnerComponent.GetHashCode());
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out enemySpawnerEntity))
        {
            return;
        }

        enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(enemySpawnerEntity);
        enemyDataContainerComponent = EntityManager.GetComponentObject<EnemyDataContainer>(enemySpawnerEntity);

        if (SystemAPI.Time.ElapsedTime > nextSpawnTime)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        List<EnemyData> list = enemyDataContainerComponent.enemiesList;

        if (list == null || list.Count == 0)
        {
            Debug.LogError("Enemy list is empty or not initialized!");
            return;
        }

        int index = UnityEngine.Random.Range(0, list.Count);

        Entity newEnemyEntity = EntityManager.Instantiate(list[index].prefab);
        EntityManager.SetComponentData(newEnemyEntity, new LocalTransform { Position = GetPositionOutsideOfCameraRange(), Rotation = quaternion.identity, Scale = 1 });

        EntityManager.AddComponentData(newEnemyEntity, new EnemyComponent { currentHealth = enemyDataContainerComponent.enemiesList[index].health });
        nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + enemySpawnerComponent.spawnCD;
    }

    private float3 GetPositionOutsideOfCameraRange()
    {
        float3 position = new float3(random.NextFloat2(-enemySpawnerComponent.cameraSize * 2, enemySpawnerComponent.cameraSize * 2), 0);

        while (position.x < enemySpawnerComponent.cameraSize.x && position.x > -enemySpawnerComponent.cameraSize.x
            && position.y < enemySpawnerComponent.cameraSize.y && position.y > -enemySpawnerComponent.cameraSize.y)
        {
            position = new float3(random.NextFloat2(-enemySpawnerComponent.cameraSize * 2, enemySpawnerComponent.cameraSize * 2), 0);
        }

        position += new float3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

        return position;
    }
}
