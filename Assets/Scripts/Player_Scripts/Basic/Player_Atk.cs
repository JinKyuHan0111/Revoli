using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    BasicAttack0,
    BasicAttack1,
    Skill_Prototype,
    Etc
}

public class Player_Atk : MonoBehaviour
{
    [Header("공격력")]
    private float Atk_damage = 0f;
    private PlayerStats playerStats;

    [Header("공격판정 관련 변수")]
    public Transform pos;
    [SerializeField]private HitType[] rangeNames = 
    {
        HitType.BasicAttack0,
        HitType.BasicAttack1,
        HitType.Skill_Prototype,
        HitType.Etc
    }; //보기 편하게 이름 지정
    [Header("위 타입과 같은 순서")]
    [SerializeField] private Vector2[] rangeValues = { 
        new Vector2(0.5f, 0.5f),
        new Vector2(0.8f, 0.5f),
        new Vector2(3, 4f) 
    };
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
    Vector2[] range_pos = {
        new Vector2(0.15f, 0f),
        new Vector2(0.2f, 0f),
        new Vector2(0.15f, 0f) //나중지정
    };
    bool[] rangeBool = { false, false , false };

    Animator anim;
    Player_Move player;
    private bool doubleAttackAble = false;

    private float lastPressTime = 0f; // 마지막으로 버튼을 누른 시간

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player_Move>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)&& doubleAttackAble == false)
        {
            Attack();
        }
       
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Attack(HitType.Skill_Prototype);
        }
        if (Time.time - lastPressTime <= 2f & doubleAttackAble)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //Debug.Log("더블어택 조건 달성"); //체크 완료
                DoubleAttack();
            }
        }
        if (Time.time - lastPressTime >2f && Time.time - lastPressTime <2.05f)
        {
            //Debug.Log("더블어택 시간 지나감"); 체크완료
            Invoke("EndAttack", 0.03f);
            doubleAttackAble = false;
        }
    }


    void HitRangeCalc(HitType hitType)
    {
        if (pos.localPosition.x > 0)
        {
            pos.localPosition = new Vector3(range_pos[(int)hitType].x,range_pos[(int)hitType].y, pos.localPosition.z);
        }
        else if (pos.localPosition.x < 0)
        {
            pos.localPosition = new Vector3(-range_pos[(int)hitType].x, range_pos[(int)hitType].y, pos.localPosition.z);
        }
    }

    void Attack()
    {
        if (player.isAttacking == false)
        {
            HitRangeCalc(HitType.BasicAttack0);
            AttackBase(HitType.BasicAttack0);
            player.can_Move = false;
            anim.SetTrigger("doAttack");
        }
    }
    public void FirAttackUse()
    {
        lastPressTime = Time.time; // 발동 시간 기록
        doubleAttackAble = true;
    }


    public void EndAttack()
    {
        //Debug.Log("AttackEnd"); 체크 완료
        player.isAttacking = false;
        player.can_Move = true;
    }
    private void OnDrawGizmos() //범위 개발 내 시각화
    {
        for (int i = 0;i<rangeBool.Length; i++)
        {
            if (rangeBool[i]==true)
            {
                Gizmos.color = GetGizmosColor(rangeNames[i]);
                Gizmos.DrawWireCube(pos.position, rangeValues[i]); // 해당 공격 범위를 그립니다.
            }
        }
    }
    private Color GetGizmosColor(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.BasicAttack0:
                return Color.red;
            case HitType.BasicAttack1:
                return Color.blue;
            case HitType.Skill_Prototype:
                return Color.green;
            case HitType.Etc:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
    void DoubleAttack()
    {
        HitRangeCalc(HitType.BasicAttack0);
        player.isAttacking = true;
        AttackBase(HitType.BasicAttack1);
        player.can_Move = false;
        anim.SetTrigger("doAttack2");
        doubleAttackAble = false;
    }
    void AttackBase(HitType hitType)
    {
        player.isAttacking = true;
        Atk_damage = playerStats.attackDamage;
        Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, rangeValues[(int)hitType], capsuleDirection, transform.rotation.z);
        for (int i = 0; i < rangeNames.Length; i++) //공격범위 시각화 관련 후반부에 지워도 상관 X
        {
            if ((int)hitType == i)
            {
                rangeBool[i] = true;
            }
            else
            {
                rangeBool[i]=false;
            }
        }
        foreach (Collider2D collider in collider2Ds)
        {
            //Debug.Log("Attack");
            if(collider.tag == "Enemy")
            {
                EnemyMove Enem = collider.gameObject.GetComponent<EnemyMove>();
                if (Enem != null)
                {
                    Debug.Log("몬스터 공격");
                    Enem.OnDamage(Atk_damage);
                }
            }
        }
    }
}
