using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float move_Speed = 0f;
    private float jumpForce = 0f;
    public bool can_Move = true; // 스킬 사용중 또는 움직이면 안되는 상황이 있을시에 false를 사용해서 움직이지 못하게 딜레이를 넣기 위함

    private PlayerStats playerStats;
    private Player_Atk player_Atk;

    private SpriteRenderer spriteRenderer;

    private bool isJump = true; // 점프 가능 여부 초기값을 true로 설정하여 시작부터 점프 가능하도록 함
    private int jumpCount = 0; // 현재 점프 횟수
    private int maxJumpCount = 2; // 최대 점프 횟수

    private bool prevMoveDirection; // 이전 움직임 방향 기억 변수

    [SerializeField] private string groundLayerName = "Ground"; //땅에 닿았는지 확인하기 위해서 이름 저장

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        player_Atk = GetComponent<Player_Atk>();
    }

    void Update()
    {
        if (can_Move)
        {
            Horizontal_Move();
            Jump();
        }
    }

    void Horizontal_Move()
    {
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;
            float horizontal = Input.GetAxis("Horizontal");
            bool currentMoveDirection = horizontal > 0f ? false : (horizontal < 0f ? true : prevMoveDirection);

            if (currentMoveDirection != prevMoveDirection)
            {
                spriteRenderer.flipX = currentMoveDirection;
                prevMoveDirection = currentMoveDirection;
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
            transform.Translate(movement);
        }
    }

    void Jump()
    {
        if (playerStats != null && Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            jumpForce = playerStats.jumpForce;
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            jumpCount = 0;
            isJump = true; // 땅에 닿으면 점프 카운트를 리셋하고, 다시 점프할 수 있도록 설정
        }
    }
}
