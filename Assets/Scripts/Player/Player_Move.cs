using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float move_Speed = 0f;
    private float jumpForce = 0f;

    private PlayerStats playerStats;
    
    private bool isJump;

    [SerializeField] private string groundLayerName = "Ground";
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }
    void Update()
    {
        Horizontal_Move(); 
        Jump();
    }
    void Horizontal_Move()
    {
        // playerStats가 null이 아닌 경우에만 moveSpeed 값을 가져와서 사용
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;

            // 키보드 입력을 통한 이동
            float horizontal = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(horizontal, 0f, 0f) * move_Speed * Time.deltaTime;

            // 현재 위치에서 이동 벡터를 더함
            transform.Translate(movement);
        }
    }
    void Jump()
    {
        // playerStats가 null이 아닌 경우에만 jumpForce 값을 가져와서 사용
        if (playerStats != null)
        {
            jumpForce = playerStats.jumpForce;
            Debug.Log("점프력 : "+jumpForce);
            //키보드
            if (Input.GetKeyDown(KeyCode.Space) && isJump)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJump = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isJump = true;
        }
    }

}
