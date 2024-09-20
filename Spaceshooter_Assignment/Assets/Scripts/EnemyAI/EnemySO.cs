using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="SO")]
public class EnemySO : ScriptableObject
{
    public GameObject prefab;
    public float health;
    public float moveSpeed;
}
