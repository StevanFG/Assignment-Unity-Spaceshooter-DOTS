using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float moveSpeed;
    public float shootCD;
    public Entity bulletPrefab;
}
