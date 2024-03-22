using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    private Transform target;
    private GameObject targetObj;
    private Vector2 targetPos;
    private bool isDie = false;
    private PlayerStats playerStats;
    private Player_Move playerMove;
    private Health_Ctrl health_Ctrl;
    public bool FindPlayer = false; // 플레이어 찾았는지 확인
    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rigid;


    public int nextMove = 1; // 다음 행동 변수

    Vector3 originalScale;

    [Header("enemy 기초 스탯")]
    public float speed = 5f; // 이동 속도
    public float attackDamage = 10f; // 공격력
    public float Enemy_MaxHp = 100f; //최대 체력



    public float targetSpeed = 10f; // 일정한 속도
    public float CurrentHp;
    public Vector2 moveForce; //임시로 만들어둔 파일
    public

    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = Enemy_MaxHp;
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        health_Ctrl = GetComponent<Health_Ctrl>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindPlayer)
            BasicEMove();
        else
            EMove();
    }

    public void OnDemage(float dmg)
    {
        CurrentHp -= dmg;
    }

    public void OnDie()
    {
        if (CurrentHp < 0)
        {
            isDie = true;

            rigid.velocity = Vector2.zero;
            animator.Play("monster1_Dead");
        }
    }
    public void BasicEMove()
    {
        //Move
        MaintainSpeed(speed);

        //Platform check(맵 앞이 낭떨어지면 뒤돌기 위해서 지형을 탐색)

        //자신의 한 칸 앞 지형을 탐색해야하므로 position.x + nextMove(-1,1,0이므로 적절함)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.7f, rigid.position.y);

        //한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        //레이를 쏴서 맞은 오브젝트를 탐지 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        //탐지된 오브젝트가 null : 그 앞에 지형이 없음
        if (raycast.collider == null)
        {
            // 벽을 감지하거나 땅이 없는 경우 방향을 전환합니다.
            nextMove = nextMove * (-1);
            FlipCharacterDirection();
        }
    }

    public void EMove()
    {
        if (targetObj != null && targetObj.tag == "Player")
        {
            Debug.Log("추격중!");
            FindPlayer = true;
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = 0f;

            MaintainSpeed(speed);


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
    }

    private void MaintainSpeed(float targetSpeed)
    {
        // 현재 속도 측정
        float currentSpeed = Mathf.Abs(rigid.velocity.x);
        // 현재 속도가 일정 속도 이하라면 추가적인 힘을 가하여 속도를 증가시킴
        if (currentSpeed < targetSpeed)
        {
            // 캐릭터의 이동 방향 설정
            float moveDirection = nextMove * speed;
            moveForce = new Vector2(moveDirection, 0.1f);

            // 힘을 가합니다.
            rigid.velocity = moveForce;
            //만약 현재 속도가 0이라면 강제적으로 이동시킵니다
            if(currentSpeed == 0f && !FindPlayer)
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그 찾기
        if (collision.gameObject.CompareTag("Player"))
        {
            targetObj = collision.gameObject;
            target = targetObj.transform;
            targetPos = targetObj.transform.position;

            FindPlayer = true;
            Debug.Log("플레이어 진입");
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigid.velocity = Vector2.zero; // 속도 초기화
            FindPlayer = false;
            Debug.Log("플레이어 탈출");
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌 시작 
        Debug.Log("충돌 시작!");
        //플레이어 충돌 
        if (collision.gameObject.CompareTag("Player"))
        {

            Health_Ctrl healthCtrl = collision.gameObject.GetComponent<Health_Ctrl>();
            if (healthCtrl != null)
            {
                // 데미지를 가하는 Take_Dmg 메서드 호출
                healthCtrl.Take_Dmg(attackDamage);
            }
        }
        //벽과 만났을때
        /*else if (collision.gameObject.CompareTag("Ground") && !FindPlayer)
        {
            nextMove = nextMove * (-1);
            FlipCharacterDirection();
        }*/
    }
}