using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;
using NUnit.Framework;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public float spawnCD = 1;
    public List<EnemySO> enemiesSO;
    public Vector2 cameraSize;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent() { spawnCD = authoring.spawnCD, cameraSize = authoring.cameraSize });

            List<EnemyData> enemyData = new List<EnemyData>();

            foreach (EnemySO enemy in authoring.enemiesSO)
            {
                enemyData.Add(new EnemyData { health = enemy.health, moveSpeed = enemy.moveSpeed, prefab = GetEntity(enemy.prefab, TransformUsageFlags.None) });
            }

            AddComponentObject(enemySpawnerAuthoring, new EnemyDataContainer { enemiesList = enemyData });
        }
    } 
}
