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
        // playerStats�� null�� �ƴ� ��쿡�� moveSpeed ���� �����ͼ� ���
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;

            // Ű���� �Է��� ���� �̵�
            float horizontal = Input.GetAxis("Horizontal");

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
            Debug.Log("������ : "+jumpForce);
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
