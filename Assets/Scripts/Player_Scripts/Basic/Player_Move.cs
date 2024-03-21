using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody2D playerRb; // 플레이어의 Rigidbody2D 컴포넌트를 참조하기 위한 변수
    private float move_Speed = 0f; // 플레이어의 이동 속도를 저장하는 변수
    private float jumpForce = 0f; // 플레이어가 점프할 때 사용할 힘의 크기를 저장하는 변수
    public bool can_Move = true; // 플레이어가 움직일 수 있는지 여부를 나타내는 변수. 스킬 사용 중이나 다른 상황에서 움직임을 제한하고 싶을 때 false로 설정
    GameManager gameManager;
    private Animator anim; //플레이어의 Animator 컴포넌트를 참조하기 위한 변수
    private PlayerStats playerStats; // 플레이어의 스탯을 관리하는 컴포넌트의 참조
    private Player_Atk player_Atk; // 플레이어의 공격을 관리하는 컴포넌트의 참조
    CapsuleCollider2D capsuleCollider;
    Health_Ctrl health;
    public bool isAttacking = false; //공격시 움직임 멈추게 하기위한 변수

    private SpriteRenderer spriteRenderer; // 플레이어의 스프라이트를 관리하기 위한 컴포넌트 참조
    private int Look;//플레이어가 어느 위치를 바라보는지 알기 위한 변수
    private bool isJump = true; // 현재 점프 가능한 상태인지를 나타내는 변수, 초기값은 true로 설정하여 시작부터 점프 가능
    private int jumpCount = 0; // 현재 점프 횟수를 나타내는 변수
    private int maxJumpCount = 2; // 허용된 최대 점프 횟수를 나타내는 변수
    private bool isDie = false; // 플레이어의 죽음확인 여부
    private bool prevMoveDirection; // 플레이어의 이전 이동 방향을 저장하는 변수, 스프라이트 반전을 위해 사용

    [SerializeField] private string groundLayerName = "Ground"; // 땅에 닿았는지 확인하기 위한 레이어 이름

    private bool isDashing = false; // 대쉬 중인지 판단하는 변수
    private float dashCooldown = 2.0f; // 대쉬 쿨타임
    private float dashTimeLeft; // 대쉬 쿨타임을 측정하기 위한 타이머
    private float dashDistance = 5f; // 대쉬 거리


    private void Start()
    {
        // 시작 시 컴포넌트들을 가져오는 작업
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        player_Atk = GetComponent<Player_Atk>();
        anim = GetComponent<Animator>();
        gameManager = GetComponent<GameManager>();
        dashTimeLeft = dashCooldown; // 대쉬 쿨타임 초기화
    }

    void Update()
    {
        // 매 프레임마다 플레이어의 움직임과 점프 처리
        if (can_Move)
        {
            Horizontal_Move();
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.W) && dashTimeLeft <= 0 && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
        }
    }
    //대쉬기능
    IEnumerator Dash()
    {
        float originalGravity = playerRb.gravityScale; // 원래 중력값 저장
        isDashing = true; // 대쉬 상태로 변경
        playerRb.gravityScale = 0; // 대쉬 중 중력 영향 제거
        Vector2 dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right; // 대쉬 방향 결정

        float dashSpeed = 300f; // 대쉬 속도값 조절해서 대쉬 거리 조절
        playerRb.velocity = new Vector2(dashDirection.x * dashSpeed, playerRb.velocity.y); // 수평 방향 대쉬 속도 설정 및 현재 수직 속도 유지

        yield return new WaitForSeconds(0.15f); // 대쉬 지속 시간, 필요에 따라 조정

        playerRb.gravityScale = originalGravity; // 대쉬가 끝나면 즉시 중력값 복원
        isDashing = false; // 대쉬 상태 해제
        dashTimeLeft = dashCooldown; // 대쉬 쿨타임 리셋
    }
    void Horizontal_Move()
    {
        // 플레이어의 수평 이동을 처리하는 메서드
        if (playerStats != null)
        {
            move_Speed = playerStats.moveSpeed;
            float horizontal = Input.GetAxis("Horizontal");
            bool currentMoveDirection = horizontal > 0f ? false : (horizontal < 0f ? true : prevMoveDirection);

            // 스프라이트 방향 전환 로직
            if (currentMoveDirection != prevMoveDirection)
            {
                spriteRenderer.flipX = currentMoveDirection;
                prevMoveDirection = currentMoveDirection;
                // 공격 위치도 이동 방향에 따라 조정
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
                // 실제 이동 처리
                playerRb.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

                if (playerRb.velocity.x > move_Speed)
                    playerRb.velocity = new Vector2(move_Speed, playerRb.velocity.y);
                else if (playerRb.velocity.x < move_Speed * (-1))
                    playerRb.velocity = new Vector2(move_Speed * (-1), playerRb.velocity.y);
            }
            //애니메이션 처리
            if (Mathf.Abs(playerRb.velocity.x) > 3)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    }
        void OnDamaged()
    {
        //Heath Down
        health.Take_Dmg(5f);// 임시 몬스터 피격 데미지(proto-type)

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

            //애니메이션 처리
            anim.SetBool("isJumping", true);

            playerRb.velocity = new Vector2(playerRb.velocity.x, -0.5f); // 수직 속도 초기화
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;

            // 다른 조건(예: 땅에 닿음)에서 중력 값을 원래대로 복원
        }

        if(playerRb.velocity.y == 0) {
            anim.SetBool("isJumping", false);
            jumpCount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어 콜라이더 전체가 Ground에 접촉시 더블점프 가능하게 하는것 방지
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName) && playerRb.velocity.y < 0)
        {
            isJump = true;
            Debug.DrawRay(playerRb.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRb.position, Vector3.down, 1.5f, LayerMask.GetMask("Ground"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1)
                    /*Debug.Log(rayHit.collider.name);*/
                    anim.SetBool("isJumping", false);
                    jumpCount = 0;
            }
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
