using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Transform target;
    private GameObject targetObj;
    private Vector2 targetPos;
    public float speed;
    private PlayerStats playerStats;
    private Health_Ctrl health_Ctrl;
    Rigidbody2D rigid;
    
    EnemyManager enemyManager;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        health_Ctrl = GetComponent<Health_Ctrl>();
        enemyManager = GetComponent<EnemyManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = enemyManager.moveSpeed;
        //조건에 따라서 변하는 중력값이 아니면 오브젝트 내부에 적어서 할것
        //rigid.gravityScale = 10f; // 중력 활성화

    }

    void Update()
    {
        
    }
    //피드백
    //외부스크립트에서 불러올것이 아니면 private 혹은 접근자를 쓰지 말것
    //아래 이동 코드는 물리력인  velocity가 아닌 방향을 지정해준 vector2가 스프라이트의 방향을 바꿀수 있음
    void EMove()
    {
        if (targetObj != null && targetObj.tag == "Player")
        {
            //Debug.Log("11");
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = 0f;
            rigid.MovePosition(rigid.position + direction * speed * Time.deltaTime);
            //Debug.Log(rigid.velocity); //= 0,0,0 
            if (direction.x > 0.1f)
            {
                spriteRenderer.flipX =false;
            }
            else if (direction.x < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그 찾기
        if (collision.gameObject.CompareTag("Player"))
        {
            targetObj = collision.gameObject;
            target = targetObj.transform;
            targetPos = targetObj.transform.position;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            EMove();
    }
    void ontriggerexit2d()
    {
        rigid.velocity = Vector2.zero; // 속도 초기화
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌 시작 
        Debug.Log("충돌 시작!");
       
    }
}