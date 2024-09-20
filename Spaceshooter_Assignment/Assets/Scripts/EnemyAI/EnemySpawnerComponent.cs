using Unity.Mathematics;
using Unity.Entities;

public struct EnemySpawnerComponent : IComponentData
{
    public float spawnCD;
    public float2 cameraSize;
}
