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
    public bool isGround = false; // ���� �ִ��� Ȯ��
    public bool isDamage = false; // ���Ͱ� �������� �ް� �ִ��� Ȯ��

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rigid;


    public int nextMove = 1; // ���� �ൿ ����

    Vector3 originalScale;

    [Header("�� �̸�")]
    public string EnemyName; //�� �̸�
    [Header("enemy ���� ����")]
    public float speed; // �̵� �ӵ�
    public float attackDamage; // ���ݷ�
    public float Enemy_MaxHp; //�ִ� ü��

    public float detectionRange = 10f; // Ž�� ����

    public float targetSpeed = 10f; // ������ �ӵ�
    public float CurrentHp;
    public Vector2 moveForce; //�ӽ÷� ������ 

    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        EnemyPool enemyPoolInstance = (EnemyPool)EnemyPool.Instance;
        speed = enemyPoolInstance.GetEnemySpeed(EnemyName);
        Enemy_MaxHp = enemyPoolInstance.GetEnemyHealth(EnemyName);
        attackDamage = enemyPoolInstance.GetEnemyAtk(EnemyName);

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
        OnDie();
        if (!isDamage && !isDie)
        {
            if (!FindPlayer)
                BasicEMove();
            else
                EMove();
        }
    }

    public void OnDamage(float dmg)
    {
        if (!isDamage && !isDie)
        {
            CurrentHp -= dmg;

            // �÷��̾�� �� ������ ���� ���� ���
            Vector2 hitDirection = rigid.transform.position;

            // �˹� ȿ���� �ֱ� ���� ���� ������ ���������� ����
            Vector2 knockbackForce = -hitDirection.normalized * dmg * nextMove * -1;

            // �˹� ���� �߰�
            rigid.velocity = knockbackForce;

            // �ִϸ��̼��� ���� ���� �ƴ϶�� ����
            animator.SetTrigger("doDmg");

            // ����� ���� ����
            isDamage = true;

            // ����� �� ���� �ð��� ������ ����� ���¸� �����ϴ� Coroutine ȣ��
            StartCoroutine(ResetDamageStateCoroutine(1f));
        }
    }

    private IEnumerator ResetDamageStateCoroutine(float delay)
    {
        // ����� ���¸� �����ϴ� ���
        yield return new WaitForSeconds(delay);

        // ����� ���� ����
        isDamage = false;

        Debug.Log("�׽�Ʈ");
    }

    public void OnDie()
    {
        if (CurrentHp < 0 && !isDie)
        {
            isDie = true;

            rigid.velocity = Vector2.zero;
            animator.Play("monster1_Dead");
        }
    }

    public void IsGround()
    {
        //�ڱ� �ٷ� �Ʒ��� Ž��
        Vector2 donw = new Vector2(rigid.position.x, rigid.position.y - 0.4f);

        //��ĭ �� �κоƷ� ������ ray�� ��
        Debug.DrawRay(donw, Vector3.down, new Color(0, 1, 0));

        //���̸� ���� ���� ������Ʈ�� Ž�� 
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

        //Platform check(�� ���� ���������� �ڵ��� ���ؼ� ������ Ž��)

        //�ڽ��� �� ĭ �� ������ Ž���ؾ��ϹǷ� position.x + nextMove(-1,1,0�̹Ƿ� ������)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

        //��ĭ �� �κоƷ� ������ ray�� ��
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        //���̸� ���� ���� ������Ʈ�� Ž�� 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        //Ž���� ������Ʈ�� null : �� �տ� ������ ����
        if (raycast.collider == null && isGround)
        {
            //���� ���� ��� ������ ��ȯ�մϴ�.
            nextMove = nextMove * (-1);
            FlipCharacterDirection();
        }
    }

    public void EMove()
    {
        if (targetObj != null && targetObj.tag == "Player")
        {

            FindPlayer = true;
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = 0f;

            animator.SetBool("doDamaged", false);
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
        //�ٴ� üũ
        IsGround();
        // ���� �ӵ� ����
        float currentSpeed = Mathf.Abs(rigid.velocity.x);
        //�ִϸ��̼��� ���� �ӵ� �ֱ�
        //�ȴ� �ִϸ��̼� �߰�

        // ���� �ӵ��� ���� �ӵ� ���϶�� �߰����� ���� ���Ͽ� �ӵ��� ������Ŵ
        if (currentSpeed < targetSpeed && isGround)
        {
            // ĳ������ �̵� ���� ����
            float moveDirection = nextMove * speed;
            moveForce = new Vector2(moveDirection, 0.1f);

            // ���� ���մϴ�.
            rigid.velocity = moveForce;
            //���� ���� �ӵ��� 0�̶�� ���������� �̵���ŵ�ϴ�
            if (currentSpeed == 0f && !FindPlayer)
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
        if (collision.gameObject.CompareTag("Player") && !isDie)
        {

            Health_Ctrl healthCtrl = collision.gameObject.GetComponent<Health_Ctrl>(); //ü�� ����
            Animator otherAnimator = collision.gameObject.GetComponent<Animator>(); // �ִϸ��̼� ����
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();// �÷��̾� ����
            if (healthCtrl != null)
            {
                // �÷��̾� ��ġ
                Vector2 hitDirection = playerRigidbody.transform.position;

                // �˹� ȿ���� �ֱ� ���� ���� �����̴ϱ� ���� �����̴� �������� �˹�
                Vector2 knockbackForce = -hitDirection.normalized * attackDamage * nextMove;
                playerRigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);

                // �������� ���ϴ� Take_Dmg �޼��� ȣ��
                healthCtrl.Take_Dmg(attackDamage);
                otherAnimator.SetTrigger("doDamaged");
            }
        }
    }
}