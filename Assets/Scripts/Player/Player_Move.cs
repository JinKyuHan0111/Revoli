using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player_Move : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float move_Speed = 0f;
    private float jumpForce = 0f;

    private PlayerStats playerStats;
    private Player_Atk player_Atk;

    private SpriteRenderer spriteRenderer;
    private bool isJump;
    

    private bool prevMoveDirection; // 이전 움직임 방향 기억 변수

    [SerializeField] private string groundLayerName = "Ground";
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerStats = GetComponent<PlayerStats>();
        player_Atk = GetComponent<Player_Atk>();
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

            bool currentMoveDirection = horizontal > 0f ? false : (horizontal < 0f ? true : prevMoveDirection);


            if (currentMoveDirection != prevMoveDirection)
            {
                spriteRenderer.flipX = currentMoveDirection;
                prevMoveDirection = currentMoveDirection;
                // 플립될 때 pos.localPosition.x 값도 반전되도록 조정
                if (spriteRenderer.flipX)
                {
                    player_Atk.pos.localPosition = new Vector3(-Mathf.Abs(player_Atk.pos.localPosition.x), player_Atk.pos.localPosition.y, player_Atk.pos.localPosition.z);
                }
                else
                {
                    player_Atk.pos.localPosition = new Vector3(Mathf.Abs(player_Atk.pos.localPosition.x), player_Atk.pos.localPosition.y, player_Atk.pos.localPosition.z);
                }
            }
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
            //Debug.Log("점프력 : "+jumpForce); //체크 완료
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
