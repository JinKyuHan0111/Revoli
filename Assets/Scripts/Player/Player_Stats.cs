using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MySingleton<PlayerStats>
{
    // �÷��̾� ���� ������
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float jumpForce = 10f; // ������
    public int attackDamage = 10; // ���ݷ�
}