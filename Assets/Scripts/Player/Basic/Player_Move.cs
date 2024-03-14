using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float move_Speed = 0f;
    private float jumpForce = 0f;
    public bool can_Move = true; // ��ų ����� �Ǵ� �����̸� �ȵǴ� ��Ȳ�� �����ÿ� false�� ����ؼ� �������� ���ϰ� �����̸� �ֱ� ����

    private PlayerStats playerStats;
    private Player_Atk player_Atk;

    private SpriteRenderer spriteRenderer;

    private bool isJump = true; // ���� ���� ���� �ʱⰪ�� true�� �����Ͽ� ���ۺ��� ���� �����ϵ��� ��
    private int jumpCount = 0; // ���� ���� Ƚ��
    private int maxJumpCount = 2; // �ִ� ���� Ƚ��

    private bool prevMoveDirection; // ���� ������ ���� ��� ����

    [SerializeField] private string groundLayerName = "Ground"; //���� ��Ҵ��� Ȯ���ϱ� ���ؼ� �̸� ����

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
            isJump = true; // ���� ������ ���� ī��Ʈ�� �����ϰ�, �ٽ� ������ �� �ֵ��� ����
        }
    }
}
