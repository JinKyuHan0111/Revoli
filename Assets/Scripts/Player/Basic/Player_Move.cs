using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody2D playerRb; // �÷��̾��� Rigidbody2D ������Ʈ�� �����ϱ� ���� ����
    private float move_Speed = 0f; // �÷��̾��� �̵� �ӵ��� �����ϴ� ����
    private float jumpForce = 0f; // �÷��̾ ������ �� ����� ���� ũ�⸦ �����ϴ� ����
    public bool can_Move = true; // �÷��̾ ������ �� �ִ��� ���θ� ��Ÿ���� ����. ��ų ��� ���̳� �ٸ� ��Ȳ���� �������� �����ϰ� ���� �� false�� ����

    private PlayerStats playerStats; // �÷��̾��� ������ �����ϴ� ������Ʈ�� ����
    private Player_Atk player_Atk; // �÷��̾��� ������ �����ϴ� ������Ʈ�� ����

    private SpriteRenderer spriteRenderer; // �÷��̾��� ��������Ʈ�� �����ϱ� ���� ������Ʈ ����

    private bool isJump = true; // ���� ���� ������ ���������� ��Ÿ���� ����, �ʱⰪ�� true�� �����Ͽ� ���ۺ��� ���� ����
    private int jumpCount = 0; // ���� ���� Ƚ���� ��Ÿ���� ����
    private int maxJumpCount = 2; // ���� �ִ� ���� Ƚ���� ��Ÿ���� ����

    private bool prevMoveDirection; // �÷��̾��� ���� �̵� ������ �����ϴ� ����, ��������Ʈ ������ ���� ���

    [SerializeField] private string groundLayerName = "Ground"; // ���� ��Ҵ��� Ȯ���ϱ� ���� ���̾� �̸�

    private void Start()
    {
        // ���� �� ������Ʈ���� �������� �۾�
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        player_Atk = GetComponent<Player_Atk>();
    }

    void Update()
    {
        // �� �����Ӹ��� �÷��̾��� �����Ӱ� ���� ó��
        if (can_Move)
        {
            Horizontal_Move();
            Jump();
        }
    }

    void Horizontal_Move()
    {
        // �÷��̾��� ���� �̵��� ó���ϴ� �޼���
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;
            float horizontal = Input.GetAxis("Horizontal");
            bool currentMoveDirection = horizontal > 0f ? false : (horizontal < 0f ? true : prevMoveDirection);

            // ��������Ʈ ���� ��ȯ ����
            if (currentMoveDirection != prevMoveDirection)
            {
                spriteRenderer.flipX = currentMoveDirection;
                prevMoveDirection = currentMoveDirection;
                // ���� ��ġ�� �̵� ���⿡ ���� ����
                if (spriteRenderer.flipX)
                {
                    player_Atk.pos.localPosition = new Vector3(-Mathf.Abs(player_Atk.pos.localPosition.x), player_Atk.pos.localPosition.y, player_Atk.pos.localPosition.z);
                }
                else
                {
                    player_Atk.pos.localPosition = new Vector3(Mathf.Abs(player_Atk.pos.localPosition.x), player_Atk.pos.localPosition.y, player_Atk.pos.localPosition.z);
                }
            }

            // ���� �̵� ó��
            Vector3 movement = new Vector3(horizontal, 0f, 0f) * move_Speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
   void Jump()
    {
        if (playerStats != null && Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            jumpForce = playerStats.jumpForce;

            // �� ��° ���� ���� �� �߷� ����
            if (jumpCount == 1) // �̹� �� �� ������ ���¿��� �� ��° ������ �� ��
            {
                playerRb.gravityScale += 0.5f; // �߷°� ����
            }
            playerRb.velocity = new Vector2(playerRb.velocity.x, -0.5f); // ���� �ӵ� �ʱ�ȭ
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;

            // �ٸ� ����(��: ���� ����)���� �߷� ���� ������� ����
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            jumpCount = 0;
            isJump = true;
            playerRb.gravityScale = 4; // �߷� ���� ������� ����
        }
    }
}
