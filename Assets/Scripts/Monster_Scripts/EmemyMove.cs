using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Transform target;
    private GameObject targetObj;
    private Vector2 targetPos;
    public float speed = 5f;
    private PlayerStats playerStats;
    private Health_Ctrl health_Ctrl;
    Rigidbody2D rigid;
    
    
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        health_Ctrl = GetComponent<Health_Ctrl>();
        rigid.gravityScale = 10f; // 중력 활성화
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EMove()
    {
        if (targetObj != null && targetObj.tag == "Player")
        {
            //Debug.Log("11");
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = 0f;
            rigid.MovePosition(rigid.position + direction * speed * Time.deltaTime);
            if (rigid.velocity.x > 0.1f)
            {
                transform.localScale = new Vector3(-1, 1, 1); // 오른쪽을 보고 있을 때 스프라이트를 뒤집음
            }
            else if (rigid.velocity.x < -0.1f)
            {
                transform.localScale = new Vector3(1, 1, 1); // 왼쪽을 보고 있을 때 스프라이트를 뒤집지 않음
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그 찾기
        if (collision.gameObject.CompareTag("Player"))
        {
            targetObj = collision.gameObject;
            target = targetObj.transform;
            targetPos = targetObj.transform.position;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            EMove();
    }
    public void ontriggerexit2d()
    {
        rigid.velocity = Vector2.zero; // 속도 초기화
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌 시작 
        Debug.Log("충돌 시작!");
       
    }
}