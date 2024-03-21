using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemy Stats")]
public class EnemyManager : MonoBehaviour
{
    public string EnemyName;
    public int health;
    public int attackDamage;
    public float moveSpeed;
}
