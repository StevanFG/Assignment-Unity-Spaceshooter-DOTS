using System.Collections.Generic;
using Unity.Entities;

public class EnemyDataContainer : IComponentData
{
    public List<EnemyData> enemiesList;
}

public struct EnemyData
{
    public Entity prefab;
    public float health;
    public float moveSpeed;
}
