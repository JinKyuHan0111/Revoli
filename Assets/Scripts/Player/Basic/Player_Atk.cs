using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType{ //���� ���Ŀ� ���� Enum
    BasicAttack,
    Skill_Prototype,
    Etc //Etc ���ְ� �ٲ㼭 �߰��ϸ� ��
}
public class Player_Atk : MonoBehaviour
{
    [Header("���ݷ�")]
    private float Atk_damage = 0f; //playerStats ���� ���ݷ��� �ҷ��ͼ� ������ �Լ�
    private PlayerStats playerStats;

    [Header("��Ÿ�� ���� ����")]
    private float curTime = 0f;
    public float coolTime = 0.5f;

    [Header("�������� ���� ����")]
    public Transform pos;
    [SerializeField] private Vector2 CapsuleSize;
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
    float range_posX = 0f; //������ ��ġ��
    float range_posY = 0f;

    Animator anim;
    Rigidbody2D playerRb;
    Player_Move player;
    private bool doubleAttackAble = false;
    int AttackCount = 0;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player_Move>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)& curTime <= 0)
        {
            Attack();
        }
    }
    void HitRange_Setting(HitType hitType)
    {
        switch(hitType) {
            case HitType.BasicAttack: //basicAttack �� ���ݹ��� 
            {
                HitRangeCalc(1, 2);
                break;
            }
            case HitType.Skill_Prototype: //Skill_Prototype �� ���ݹ��� 
            {
                HitRangeCalc(2, 4);
                break;
            }
        }
    }
    void HitRangeCalc(float rangeX,float rangeY) //HitRangeCalc(���� �� x,���� �� y)
    {
        range_posX = 0.5f + rangeX / 2;
        range_posY = -0.4f + ((rangeY - 2) / 2);
        CapsuleSize = new Vector2(rangeX, rangeY);
        if (pos.localPosition.x > 0)
        {
            pos.localPosition = new Vector3(range_posX, range_posY, pos.localPosition.z);
        }
        else
        {
            pos.localPosition = new Vector3(-range_posX, range_posY, pos.localPosition.z);
        }
    }
    void Attack()
    {
        AttackCount++;

            switch (AttackCount)
            {
                case 1:
                    {
                        player.isAttacking = true;

                        HitRange_Setting(HitType.BasicAttack);

                        Atk_damage = playerStats.attackDamage;
                        //Debug.Log("Z��ư ���� & curTime <=0 �۵� �Ϸ�"); //üũ �Ϸ�
                        Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, CapsuleSize, capsuleDirection, transform.rotation.z);
                        foreach (Collider2D collider in collider2Ds)
                        {
                            Debug.Log("Attack1");
                        }

                        playerRb.velocity = Vector2.zero;

                        anim.SetTrigger("doAttack");

                        curTime = coolTime;

                        Invoke("EndAttack", 1);

                    }
                    break;

                case 2:

                    Invoke("DoubleAttack", 0.9f);

                    break;                   
            }
            /*if (AttackCount == 1)
            {
                player.isAttacking = true;

                HitRange_Setting(HitType.BasicAttack);

                Atk_damage = playerStats.attackDamage;
                //Debug.Log("Z��ư ���� & curTime <=0 �۵� �Ϸ�"); //üũ �Ϸ�
                Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, CapsuleSize, capsuleDirection, transform.rotation.z);
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log();
                }
                Debug.Log("Attack1");
                
                playerRb.velocity = Vector2.zero;
                
                anim.SetTrigger("doAttack");

                curTime = coolTime;

                Invoke("EndAttack", 0.5f);

            }
            if (AttackCount == 2)
            {
                Invoke("DoubleAttack", 0.3f);

            }*/
                if (Input.GetKeyDown(KeyCode.W))
            {
                HitRange_Setting(HitType.Skill_Prototype);

                Atk_damage = playerStats.attackDamage;
                //Debug.Log("Z��ư ���� & curTime <=0 �۵� �Ϸ�"); //üũ �Ϸ�
                /*Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, CapsuleSize, capsuleDirection, transform.rotation.z);
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                }*/
                //animator.setTrigger("atk");
                curTime = coolTime;
            }
        
        /*else
        {
            curTime -= Time.deltaTime;
        }*/
    }
    // �Ʒ� �Լ��� �ݶ��̴��� �������� �����ֱ� ���� �Լ�
    // ����� ���� �ּ� ó�� ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, CapsuleSize);

    }

    void EndAttack()
    {
        player.isAttacking = false;
        AttackCount = 0;
    }

    void DoubleAttack()
    {
        player.isAttacking = true;

        HitRange_Setting(HitType.BasicAttack);

        Atk_damage = playerStats.attackDamage;
        /*//Debug.Log("Z��ư ���� & curTime <=0 �۵� �Ϸ�"); //üũ �Ϸ�
        Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, CapsuleSize, capsuleDirection, transform.rotation.z);
        foreach (Collider2D collider in collider2Ds)
        {
            Debug.Log(collider.tag);
        }*/
        Debug.Log("Attack2");

        playerRb.velocity = Vector2.zero;

        anim.SetTrigger("doAttack2");

        curTime = coolTime;

        Invoke("EndAttack", 0.6f);
    }
}
