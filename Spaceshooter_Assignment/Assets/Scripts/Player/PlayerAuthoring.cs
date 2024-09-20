using UnityEngine;
using Unity.Entities;
using System;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float shootCD;
    public GameObject bulletPrefab;

    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring playerAuthoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.None);

            AddComponent(playerEntity, new PlayerComponent()
            {
                moveSpeed = playerAuthoring.moveSpeed,
                shootCD = playerAuthoring.shootCD,
                bulletPrefab = GetEntity(playerAuthoring.bulletPrefab, TransformUsageFlags.None),
            });
        }
    }
}
