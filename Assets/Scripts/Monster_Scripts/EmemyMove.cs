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
    public bool FindPlayer = false; // �÷��̾� ã�Ҵ��� Ȯ��
    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rigid;


    public int nextMove = 1; // ���� �ൿ ����

    Vector3 originalScale;

    [Header("enemy ���� ����")]
    public float speed = 5f; // �̵� �ӵ�
    public float attackDamage = 10f; // ���ݷ�
    public float Enemy_MaxHp = 100f; //�ִ� ü��



    public float targetSpeed = 10f; // ������ �ӵ�
    public float CurrentHp;
    public Vector2 moveForce; //�ӽ÷� ������ ����
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

        //Platform check(�� ���� ���������� �ڵ��� ���ؼ� ������ Ž��)

        //�ڽ��� �� ĭ �� ������ Ž���ؾ��ϹǷ� position.x + nextMove(-1,1,0�̹Ƿ� ������)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.7f, rigid.position.y);

        //��ĭ �� �κоƷ� ������ ray�� ��
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        //���̸� ���� ���� ������Ʈ�� Ž�� 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        //Ž���� ������Ʈ�� null : �� �տ� ������ ����
        if (raycast.collider == null)
        {
            // ���� �����ϰų� ���� ���� ��� ������ ��ȯ�մϴ�.
            nextMove = nextMove * (-1);
            FlipCharacterDirection();
        }
    }

    public void EMove()
    {
        if (targetObj != null && targetObj.tag == "Player")
        {
            Debug.Log("�߰���!");
            FindPlayer = true;
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = 0f;

            MaintainSpeed(speed);


            if (direction.x > 0.1f)
            {
                //�������̶�� ��ȯ��������
                transform.localScale = originalScale;
                nextMove = 1;

            }
            else if (direction.x < -0.1f)
            {
                //�����̶�� ��ȯ��
                transform.localScale = new Vector2(-originalScale.x, originalScale.y);
                nextMove = -1;
            }
        }
    }

    private void MaintainSpeed(float targetSpeed)
    {
        // ���� �ӵ� ����
        float currentSpeed = Mathf.Abs(rigid.velocity.x);
        // ���� �ӵ��� ���� �ӵ� ���϶�� �߰����� ���� ���Ͽ� �ӵ��� ������Ŵ
        if (currentSpeed < targetSpeed)
        {
            // ĳ������ �̵� ���� ����
            float moveDirection = nextMove * speed;
            moveForce = new Vector2(moveDirection, 0.1f);

            // ���� ���մϴ�.
            rigid.velocity = moveForce;
            //���� ���� �ӵ��� 0�̶�� ���������� �̵���ŵ�ϴ�
            if(currentSpeed == 0f && !FindPlayer)
            {
                nextMove = nextMove * -1;
                FlipCharacterDirection();
                rigid.AddForce(moveForce * moveDirection * -1, ForceMode2D.Impulse);
            }
        }
        else
        {
            // ���� �ӵ��� ���� �ӵ� �̻��̸� ���� ������ ����
            rigid.velocity = new Vector2(nextMove * targetSpeed, rigid.velocity.y);
        }
    }

    private void FlipCharacterDirection()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // x �������� ������Ŵ
        transform.localScale = newScale;
    }


    //�ݶ��δ� �浹 ���� ������

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾� �±� ã��
        if (collision.gameObject.CompareTag("Player"))
        {
            targetObj = collision.gameObject;
            target = targetObj.transform;
            targetPos = targetObj.transform.position;

            FindPlayer = true;
            Debug.Log("�÷��̾� ����");
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigid.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
            FindPlayer = false;
            Debug.Log("�÷��̾� Ż��");
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //�浹 ���� 
        Debug.Log("�浹 ����!");
        //�÷��̾� �浹 
        if (collision.gameObject.CompareTag("Player"))
        {

            Health_Ctrl healthCtrl = collision.gameObject.GetComponent<Health_Ctrl>();
            if (healthCtrl != null)
            {
                // �������� ���ϴ� Take_Dmg �޼��� ȣ��
                healthCtrl.Take_Dmg(attackDamage);
            }
        }
        //���� ��������
        /*else if (collision.gameObject.CompareTag("Ground") && !FindPlayer)
        {
            nextMove = nextMove * (-1);
            FlipCharacterDirection();
        }*/
    }
}