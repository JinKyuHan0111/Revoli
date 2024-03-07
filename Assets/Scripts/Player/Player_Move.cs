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
    

    private bool prevMoveDirection; // ���� ������ ���� ��� ����

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
        // playerStats�� null�� �ƴ� ��쿡�� moveSpeed ���� �����ͼ� ���
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;

            // Ű���� �Է��� ���� �̵�
            float horizontal = Input.GetAxis("Horizontal");

            bool currentMoveDirection = horizontal > 0f ? false : (horizontal < 0f ? true : prevMoveDirection);


            if (currentMoveDirection != prevMoveDirection)
            {
                spriteRenderer.flipX = currentMoveDirection;
                prevMoveDirection = currentMoveDirection;
                // �ø��� �� pos.localPosition.x ���� �����ǵ��� ����
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

            // ���� ��ġ���� �̵� ���͸� ����
            transform.Translate(movement);
        }
    }
    void Jump()
    {
        // playerStats�� null�� �ƴ� ��쿡�� jumpForce ���� �����ͼ� ���
        if (playerStats != null)
        {
            jumpForce = playerStats.jumpForce;
            //Debug.Log("������ : "+jumpForce); //üũ �Ϸ�
            //Ű����
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
