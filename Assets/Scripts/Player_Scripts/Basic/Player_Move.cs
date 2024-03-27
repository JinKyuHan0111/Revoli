using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //��ũ��Ʈ
    Health_Ctrl health;
    GameManager gameManager;
    private Player_Atk player_Atk; // �÷��̾��� ������ �����ϴ� ������Ʈ�� ����
    private PlayerStats playerStats; // �÷��̾��� ������ �����ϴ� ������Ʈ�� ����

    //��������
    private Rigidbody2D playerRb; // �÷��̾��� Rigidbody2D ������Ʈ�� �����ϱ� ���� ����
    CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer; // �÷��̾��� ��������Ʈ�� �����ϱ� ���� ������Ʈ ����
    private Animator anim; //�÷��̾��� Animator ������Ʈ�� �����ϱ� ���� ����

    [Header("�̵�����")]
    private float move_Speed = 0f; // �÷��̾��� �̵� �ӵ��� �����ϴ� ����
    public bool can_Move = true; // �÷��̾ ������ �� �ִ��� ���θ� ��Ÿ���� ����. ��ų ��� ���̳� �ٸ� ��Ȳ���� �������� �����ϰ� ���� �� false�� ����
    public bool isAttacking = false; //���ݽ� ������ ���߰� �ϱ����� ����

    [Header("��������")]
    private int jumpCount = 0; // ���� ���� Ƚ���� ��Ÿ���� ����
    private float jumpForce = 0f; // �÷��̾ ������ �� ����� ���� ũ�⸦ �����ϴ� ����
    private int maxJumpCount = 2; // ���� �ִ� ���� Ƚ���� ��Ÿ���� ����
    private bool prevMoveDirection; // �÷��̾��� ���� �̵� ������ �����ϴ� ����, ��������Ʈ ������ ���� ���
    [SerializeField] private string groundLayerName = "Ground"; // ���� ��Ҵ��� Ȯ���ϱ� ���� ���̾� �̸�

    private bool isDie = false; // �÷��̾��� ����Ȯ�� ����

    [Header("�뽬����")]
    private bool isDashing = false; // �뽬 ������ �Ǵ��ϴ� ����
    private bool canDash = true;
    private float dashPower;
    private float originDashPower=12f;
    private float dashTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer trailRenderer;
    public float raycastDistance = 1f;
    public LayerMask wallLayer;

    [Header("������ ����")]
    public GameObject area;
    private BoxCollider2D areaBounds;

    private void Start()
    {
        // ���� �� ������Ʈ���� �������� �۾�
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        player_Atk = GetComponent<Player_Atk>();
        anim = GetComponent<Animator>();
        gameManager = GetComponent<GameManager>();
        areaBounds = area.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(isDashing)
        {
            return;
        }
        // �� �����Ӹ��� �÷��̾��� �����Ӱ� ���� ó��
        if (can_Move)
        {
            Horizontal_Move();
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.W) && canDash)
        {
            Debug.Log("�뽬 ����");
            StartCoroutine(Dash());
        }

        // �÷��̾� ��ġ�� ���� ���� ����
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, areaBounds.bounds.min.x, areaBounds.bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, areaBounds.bounds.min.y, areaBounds.bounds.max.y);

        transform.position = clampedPosition;
        if (transform.position != clampedPosition)
        {
            playerRb.velocity = new Vector2(0,playerRb.velocity.y);
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }


    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;
        dashPower = spriteRenderer.flipX? -originDashPower : originDashPower;
        Vector2 dashVelocity = new Vector2(transform.localScale.x * dashPower, 0);
        playerRb.velocity = dashVelocity;

        // TrailRenderer Ȱ��ȭ
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashTime);

        // TrailRenderer ��Ȱ��ȭ
        trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;

        // �뽬 ���� ������ �� �ֵ��� �÷��̾��� �ӵ��� �ʱ�ȭ
        playerRb.velocity = new Vector2(0,playerRb.velocity.y); //Vector2.zero

        yield return new WaitForSeconds(0.2f); //�ʱ�ȭ�� ���� �뽬 ����ÿ� ������ �ʱ�ȭ �Ǵ� ���� �ذ��
        isDashing = false; // �뽬 ����

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    float horizontal;
    void Horizontal_Move()
    {
        // �÷��̾��� ���� �̵��� ó���ϴ� �޼���
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;
            horizontal = Input.GetAxis("Horizontal");
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

            
            if (!isAttacking)
            {
                // ���� �̵� ó��
                playerRb.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

                if (playerRb.velocity.x > move_Speed)
                    playerRb.velocity = new Vector2(move_Speed, playerRb.velocity.y);
                else if (playerRb.velocity.x < move_Speed * (-1))
                    playerRb.velocity = new Vector2(move_Speed * (-1), playerRb.velocity.y);
            }
            //�ִϸ��̼� ó��
            if (Mathf.Abs(playerRb.velocity.x) > 1)
            { 
                anim.SetBool("isWalking", true);

                if (playerRb.velocity.y < -1.5f)
                {
                    anim.SetBool("isLanding", true);
                }
                else if(playerRb.velocity.y == 0)
                {
                    anim.SetBool("isLanding", false);
                    anim.SetBool("isJumping", false);
                }
            }
            else
            {
                anim.SetBool("isWalking", false);

                if (playerRb.velocity.y < -1.5f)
                {
                    anim.SetBool("isLanding", true);
                }
                else if (playerRb.velocity.y == 0)
                {
                    anim.SetBool("isLanding", false);
                    anim.SetBool("isJumping", false);
                    jumpCount = 0;
                }
                horizontal = 0;
            }
        }
    }
    public void OnDamaged(float dmg)
    {
        //Heath Down
        health.Take_Dmg(dmg);// �ӽ� ���� �ǰ� ������(proto-type)

        anim.SetTrigger("doDamaged");
    }

   void OnAttack()
    {

    }

    void Jump()
    {
        if (playerStats != null && Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            jumpForce = playerStats.jumpForce;

            //�ִϸ��̼� ó��
            anim.SetBool("isJumping", true);
            anim.SetBool("isLanding", false);

            if(playerRb.velocity.y < -1.5f)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isLanding", true);
            }
            else if(playerRb.velocity.y == 0)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isLanding", false);
                jumpCount = 0;
            }

            playerRb.velocity = new Vector2(playerRb.velocity.x, -0.5f); // ���� �ӵ� �ʱ�ȭ
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;

            // �ٸ� ����(��: ���� ����)���� �߷� ���� ������� ����
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName) && !anim.GetBool("isJumping"))
        {
            if (playerRb.velocity.y <= 0 && horizontal != 0)
            {
                jumpCount++;
            }
            else if (playerRb.velocity.x != 0)
            {
                jumpCount++;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾� �ݶ��̴� ��ü�� Ground�� ���˽� �������� �����ϰ� �ϴ°� ����
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName) && playerRb.velocity.y < 0)
        {
            Debug.DrawRay(playerRb.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRb.position, Vector3.down, 1.5f, LayerMask.GetMask("Ground"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1 && !isDashing)
                {
                    /*Debug.Log(rayHit.collider.name);*/
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isLanding", false);
                    jumpCount = 0;
                }
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, wallLayer))
        {

            //Debug.Log("�浹");
            playerRb.velocity= new Vector2(0,playerRb.velocity.y);
            
        }
    }

    public void OnDie()
    {
        isDie =  true;
        
        playerRb.velocity = Vector2.zero;
        capsuleCollider.enabled = false;

        anim.Play("Player_Death");

        Invoke("GameOverCall", 3);
    }
    
    void GameOverCall()
    {
        gameManager.EndGame();
    }

}
