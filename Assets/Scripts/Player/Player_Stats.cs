using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MySingleton<PlayerStats>
{
    [Header("�÷��̾� ���� ����")]
    // �÷��̾� ���� ������
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float jumpForce = 5f; // ������
    public float attackDamage = 10f; // ���ݷ�
    public float Player_hp = 100f; //ü��
}