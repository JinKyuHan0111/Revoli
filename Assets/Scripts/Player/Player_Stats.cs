using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MySingleton<PlayerStats>
{
    [Header("플레이어 기초 스탯")]
    // 플레이어 스탯 변수들
    public float moveSpeed = 5f; // 이동 속도
    public float jumpForce = 5f; // 점프력
    public float attackDamage = 10f; // 공격력
    public float Player_hp = 100f; //체력
}