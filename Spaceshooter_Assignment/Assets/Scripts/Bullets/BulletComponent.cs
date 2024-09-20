using Unity.Entities;

public struct BulletComponent : IComponentData
{
    public float bulletSpeed;
    public float bulletDamage;
}