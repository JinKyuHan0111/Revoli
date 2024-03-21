using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    BasicAttack,
    Skill_Prototype,
    Etc
}

public class Player_Atk : MonoBehaviour
{
    [Header("공격력")]
    private float Atk_damage = 0f;
    private PlayerStats playerStats;

    [Header("쿨타임 관련 변수")]
    private float curTime = 0f;
    public float coolTime = 0.5f;

    [Header("공격판정 관련 변수")]
    public Transform pos;
    [SerializeField] private Vector2 CapsuleSize;
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
    float range_posX = 0f;
    float range_posY = 0f;

    Animator anim;
    Rigidbody2D playerRb;
    Player_Move player;
    private bool doubleAttackAble = false;

    private float lastPressTime = 0f; // 마지막으로 버튼을 누른 시간

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player_Move>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && curTime <= 0 && doubleAttackAble == false)
        {
            Attack();
        }
       
        if (Input.GetKeyDown(KeyCode.W) && curTime <= 0)
        {
            //Attack(HitType.Skill_Prototype);
        }
        if (Time.time - lastPressTime <= 2f & doubleAttackAble)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //Debug.Log("더블어택 조건 달성");
                DoubleAttack();
            }
        }
        if (Time.time - lastPressTime >2f && Time.time - lastPressTime <2.05f)
        {
            Debug.Log("더블어택 시간 지나감");
            Invoke("EndAttack", 0.03f);
            doubleAttackAble = false;
        }
    }

    void HitRange_Setting(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.BasicAttack:
                CapsuleSize = new Vector2(1, 1.5f);
                range_posX = 0.18f;
                range_posY = 0f;
                break;
            case HitType.Skill_Prototype:
                CapsuleSize = new Vector2(2, 3f);
                range_posX = 0.28f;
                range_posY = 0.167f;
                break;
        }
        HitRangeCalc();
    }

    void HitRangeCalc()
    {
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
        if (player.isAttacking == false)
        {
            player.isAttacking = true;
            HitRange_Setting(HitType.BasicAttack);
            player.can_Move = false;
            Atk_damage = playerStats.attackDamage;
            Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, CapsuleSize, capsuleDirection, transform.rotation.z);
            foreach (Collider2D collider in collider2Ds)
            {
                Debug.Log("Attack1");
            }
            anim.SetTrigger("doAttack");
            curTime = coolTime;

        }
        
    }
    public void FirAttackUse()
    {
        lastPressTime = Time.time; // 발동 시간 기록
        doubleAttackAble = true;
    }


    public void EndAttack()
    {
        Debug.Log("AttackEnd");
        player.isAttacking = false;
        player.can_Move = true;
    }

    void DoubleAttack()
    {
        player.isAttacking = true;
        HitRange_Setting(HitType.BasicAttack);
        Atk_damage = playerStats.attackDamage;
        Debug.Log("Attack2");
        player.can_Move = false;
        anim.SetTrigger("doAttack2");
        curTime = coolTime;
        doubleAttackAble = false;
    }
}
