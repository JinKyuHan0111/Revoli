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
        // playerStats가 null이 아닌 경우에만 moveSpeed 값을 가져와서 사용
        if (playerStats != null)
        {
            moveSpeed = playerStats.moveSpeed;

            // 키보드 입력을 통한 이동
            float horizontal = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(horizontal, 0f, 0f) * moveSpeed * Time.deltaTime;

            // 현재 위치에서 이동 벡터를 더함
            transform.Translate(movement);
        }
    }

}
