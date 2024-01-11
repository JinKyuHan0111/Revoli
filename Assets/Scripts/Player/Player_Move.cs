using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private float moveSpeed = 0f;

    public PlayerStats playerStats;
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    void Update()
    {
        // playerStats�� null�� �ƴ� ��쿡�� moveSpeed ���� �����ͼ� ���
        if (playerStats != null)
        {
            moveSpeed = playerStats.moveSpeed;

            // Ű���� �Է��� ���� �̵�
            float horizontal = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(horizontal, 0f, 0f) * moveSpeed * Time.deltaTime;

            // ���� ��ġ���� �̵� ���͸� ����
            transform.Translate(movement);
        }
    }

}
