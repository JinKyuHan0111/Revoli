using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    private bool isDie = false;
    private PlayerStats playerStats;
    private Player_Move playerMove;
    private Health_Ctrl health_Ctrl;
    public bool FindPlayer = false; // 플레이어 찾았는지 확인
    public bool isGround = false; // 땅에 있는지 확인
    public bool isDamage = false; // 몬스터가 데미지를 받고 있는지 확인

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rigid;


    public int nextMove = 1; // 다음 행동 변수

    Vector3 originalScale;

    [Header("적 이름")]
    public string EnemyName; //적 이름
    [Header("enemy 기초 스탯")]
    public float speed; // 이동 속도
    public float attackDamage; // 공격력
    public float Enemy_MaxHp; //최대 체력

    public float detectionRange = 3f; // 탐색 범위
    public float knockback = 1f;
    public float targetSpeed = 10f; // 일정한 속도
    public float CurrentHp;
    public Vector2 moveForce; //임시로 만들어둔 
    public float damageCooldown = 1.5f; // 플레이어 무적시간

    public bool isPlayerDamaged = false;

    private GameObject player;

    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        EnemyPool enemyPoolInstance = (EnemyPool)EnemyPool.Instance; 
        speed = enemyPoolInstance.GetEnemySpeed(EnemyName);
        Enemy_MaxHp = enemyPoolInstance.GetEnemyHealth(EnemyName);
        attackDamage = enemyPoolInstance.GetEnemyAtk(EnemyName);

        player = GameObject.FindGameObjectWithTag("Player"); //플레이어 위치

        CurrentHp = Enemy_MaxHp;
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        health_Ctrl = GetComponent<Health_Ctrl>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isDie)
        {
            
            return;
        }
        PlayerFind();
        OnDie();
        if (!isDamage && !isDie)
        {
            if (!FindPlayer)
                BasicEMove();
            else
                EMove();
        }
    }

    public void PlayerFind()
    {
        // 적 캐릭터와 플레이어 사이의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // 플레이어가 탐색 범위 내에 있는지 확인
        if (distanceToPlayer <= detectionRange)
        {
            // 탐색 범위 내에 플레이어가 있으면 플레이어를 발견했다고 간주하고 시각화
            FindPlayer = true;
        }
        else
        {
            FindPlayer = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 적 캐릭터의 위치에서 탐색 범위를 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void OnDamage(float dmg)
    {
        if (!isDamage && !isDie)
        {
            CurrentHp -= dmg;

            // 플레이어와 적 사이의 방향 벡터 계산
            Vector2 hitDirection = rigid.transform.position;

            // 넉백 효과를 주기 위해 힘의 방향을 역방향으로 설정
            Vector2 knockbackForce = -hitDirection.normalized * knockback * nextMove * -1;

            // 넉백 힘을 추가
            rigid.velocity = knockbackForce;

            // 애니메이션이 실행 중이 아니라면 실행
            animator.SetTrigger("doDmg");

            // 대미지 상태 설정
            isDamage = true;

            // 대미지 후 일정 시간이 지나면 대미지 상태를 해제하는 Coroutine 호출
            StartCoroutine(ResetDamageStateCoroutine(1f));
        }
    }

    private IEnumerator ResetDamageStateCoroutine(float delay)
    {
        // 대미지 상태를 해제하는 대기
        yield return new WaitForSeconds(delay);

        // 대미지 상태 해제
        isDamage = false;

        Debug.Log("테스트");
    }

    public void OnDie()
    {
        if (CurrentHp < 0 && !isDie)
        {

            isDie = true;

            rigid.velocity = Vector2.zero;
            animator.Play("monster1_Dead");

            StartCoroutine(Die(1f));

        }
    }

    private IEnumerator Die(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    public void IsGround()
    {
        //자기 바로 아래에 탐색
        Vector2 donw = new Vector2(rigid.position.x, rigid.position.y - 0.4f);

        //한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(donw, Vector3.down, new Color(0, 1, 0));

        //레이를 쏴서 맞은 오브젝트를 탐지 
        RaycastHit2D raycast = Physics2D.Raycast(donw, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (raycast.collider != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    public void BasicEMove()
    {
        //Move
        MaintainSpeed(speed);

        //Platform check(맵 앞이 낭떨어지면 뒤돌기 위해서 지형을 탐색)

        //자신의 한 칸 앞 지형을 탐색해야하므로 position.x + nextMove(-1,1,0이므로 적절함)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

        //한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        //레이를 쏴서 맞은 오브젝트를 탐지 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        //탐지된 오브젝트가 null : 그 앞에 지형이 없음
        if (raycast.collider == null && isGround)
        {
            //땅이 없는 경우 방향을 전환합니다.
            nextMove = nextMove * (-1);
            FlipCharacterDirection();
        }
    }

    public void EMove()
    {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0f;

            animator.SetBool("doDamaged", false);
            MaintainSpeed(speed);

            Debug.Log("1111");
            if (direction.x > 0.1f)
            {
                //오른쪽이라면 전환하지않음
                transform.localScale = originalScale;
                nextMove = 1;

            }
            else if (direction.x < -0.1f)
            {
                //왼쪽이라면 전환함
                transform.localScale = new Vector2(-originalScale.x, originalScale.y);
                nextMove = -1;
            }
        
    }

    private void MaintainSpeed(float targetSpeed)
    {
        //바닥 체크
        IsGround();
        // 현재 속도 측정
        float currentSpeed = Mathf.Abs(rigid.velocity.x);
        //애니메이션을 위한 속도 주기
        //걷는 애니메이션 추가

        // 현재 속도가 일정 속도 이하라면 추가적인 힘을 가하여 속도를 증가시킴
        if (currentSpeed < targetSpeed && isGround)
        {
            // 캐릭터의 이동 방향 설정
            float moveDirection = nextMove * speed;
            moveForce = new Vector2(moveDirection, 0.1f);

            // 힘을 가합니다.
            rigid.velocity = moveForce;
            //만약 현재 속도가 0이라면 강제적으로 이동시킵니다
            if (currentSpeed == 0f && !FindPlayer)
            {
                nextMove = nextMove * -1;
                FlipCharacterDirection();
                rigid.AddForce(moveForce * moveDirection * -1, ForceMode2D.Impulse);
            }
        }
        else
        {
            // 현재 속도가 일정 속도 이상이면 힘을 가하지 않음
            rigid.velocity = new Vector2(nextMove * targetSpeed, rigid.velocity.y);
        }
    }

    private void FlipCharacterDirection()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // x 스케일을 반전시킴
        transform.localScale = newScale;
    }


    //콜라인더 충돌 관련 로직들


    public void OnCollisionStay2D(Collision2D collision)
    {
        //플레이어 충돌 
        if (collision.gameObject.CompareTag("Player") && !isDie && !isPlayerDamaged)
        {

            Health_Ctrl healthCtrl = collision.gameObject.GetComponent<Health_Ctrl>(); //체력 참조
            Animator otherAnimator = collision.gameObject.GetComponent<Animator>(); // 애니메이션 참조
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();// 플레이어 참조
            
            if (healthCtrl != null)
            {
                // 플레이어 위치
                Vector2 hitDirection = playerRigidbody.transform.position;

                // 넉백 효과를 주기 위해 힘의 방향이니깐 몬스터 움직이는 방향으로 넉백
                Vector2 knockbackForce = -hitDirection.normalized * attackDamage * nextMove;
                playerRigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);

                // 데미지를 가하는 Take_Dmg 메서드 호출
                healthCtrl.Take_Dmg(attackDamage);
                otherAnimator.SetTrigger("doDamaged");

                isPlayerDamaged = true;
                StartCoroutine(InvincibilityCooldownCoroutine());
            }
        }
    }

    private IEnumerator InvincibilityCooldownCoroutine()
    {
        // 데미지 쿨다운 시간 동안 대기
        yield return new WaitForSeconds(damageCooldown);

        // 데미지 쿨다운 후 플레이어 피격 상태 해제
        isPlayerDamaged = false;
    }
}