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
        //���ǿ� ���� ���ϴ� �߷°��� �ƴϸ� ������Ʈ ���ο� ��� �Ұ�
        //rigid.gravityScale = 10f; // �߷� Ȱ��ȭ

    }

    void Update()
    {
        
    }
    //�ǵ��
    //�ܺν�ũ��Ʈ���� �ҷ��ð��� �ƴϸ� private Ȥ�� �����ڸ� ���� ����
    //�Ʒ� �̵� �ڵ�� ��������  velocity�� �ƴ� ������ �������� vector2�� ��������Ʈ�� ������ �ٲܼ� ����
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
        // �÷��̾� �±� ã��
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
        rigid.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //�浹 ���� 
        Debug.Log("�浹 ����!");
       
    }
}