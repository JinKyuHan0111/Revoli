using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType{ //공격 형식에 대한 Enum
    BasicAttack,
    Skill_Prototype,
    Etc //Etc 없애고 바꿔서 추가하면 됨
}
public class Player_Atk : MonoBehaviour
{
    [Header("공격력")]
    private float Atk_damage = 0f; //playerStats 에서 공격력을 불러와서 저장할 함수
    private PlayerStats playerStats;

    [Header("쿨타임 관련 변수")]
    private float curTime = 0f;
    public float coolTime = 0.5f;

    [Header("공격판정 관련 변수")]
    public Transform pos;
    [SerializeField] private Vector2 CapsuleSize;
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
    float range_posX = 0f; //법위의 위치값
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
            case HitType.BasicAttack: //basicAttack 의 공격법위 
            {
                HitRangeCalc(1, 2);
                break;
            }
            case HitType.Skill_Prototype: //Skill_Prototype 의 공격법위 
            {
                HitRangeCalc(2, 4);
                break;
            }
        }
    }
    void HitRangeCalc(float rangeX,float rangeY) //HitRangeCalc(범위 값 x,범위 값 y)
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
                        //Debug.Log("Z버튼 누름 & curTime <=0 작동 완료"); //체크 완료
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
                //Debug.Log("Z버튼 누름 & curTime <=0 작동 완료"); //체크 완료
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
                //Debug.Log("Z버튼 누름 & curTime <=0 작동 완료"); //체크 완료
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
    // 아래 함수는 콜라이더의 윤곽선을 보여주기 위한 함수
    // 사용한 이후 주석 처리 예정
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
        /*//Debug.Log("Z버튼 누름 & curTime <=0 작동 완료"); //체크 완료
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
