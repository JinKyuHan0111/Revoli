using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MySingleton<PlayerStats>
{
    // 플레이어 스탯 변수들
    public float moveSpeed = 5f; // 이동 속도
    public float jumpForce = 10f; // 점프력
    public int attackDamage = 10; // 공격력
}