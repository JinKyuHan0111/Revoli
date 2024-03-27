using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
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

    public float detectionRange = 3f; // Ž�� ����
    public float knockback = 1f;
    public float targetSpeed = 10f; // ������ �ӵ�
    public float CurrentHp;
    public Vector2 moveForce; //�ӽ÷� ������ 
    public float damageCooldown = 1.5f; // �÷��̾� �����ð�

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

        player = GameObject.FindGameObjectWithTag("Player"); //�÷��̾� ��ġ

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
        // �� ĳ���Ϳ� �÷��̾� ������ �Ÿ� ���
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾ Ž�� ���� ���� �ִ��� Ȯ��
        if (distanceToPlayer <= detectionRange)
        {
            // Ž�� ���� ���� �÷��̾ ������ �÷��̾ �߰��ߴٰ� �����ϰ� �ð�ȭ
            FindPlayer = true;
        }
        else
        {
            FindPlayer = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �� ĳ������ ��ġ���� Ž�� ������ �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void OnDamage(float dmg)
    {
        if (!isDamage && !isDie)
        {
            CurrentHp -= dmg;

            // �÷��̾�� �� ������ ���� ���� ���
            Vector2 hitDirection = rigid.transform.position;

            // �˹� ȿ���� �ֱ� ���� ���� ������ ���������� ����
            Vector2 knockbackForce = -hitDirection.normalized * knockback * nextMove * -1;

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
            Vector2 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0f;

            animator.SetBool("doDamaged", false);
            MaintainSpeed(speed);

            Debug.Log("1111");
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


    public void OnCollisionStay2D(Collision2D collision)
    {
        //�÷��̾� �浹 
        if (collision.gameObject.CompareTag("Player") && !isDie && !isPlayerDamaged)
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

                isPlayerDamaged = true;
                StartCoroutine(InvincibilityCooldownCoroutine());
            }
        }
    }

    private IEnumerator InvincibilityCooldownCoroutine()
    {
        // ������ ��ٿ� �ð� ���� ���
        yield return new WaitForSeconds(damageCooldown);

        // ������ ��ٿ� �� �÷��̾� �ǰ� ���� ����
        isPlayerDamaged = false;
    }
}