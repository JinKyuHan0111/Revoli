using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector2 CapsuleSize;
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;


    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        Atack();
    }
    void Atack()
    {
        if (curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Debug.Log("Z버튼 누름 & curTime <=0 작동 완료"); //체크 완료
                Collider2D[] collider2Ds = Physics2D.OverlapCapsuleAll(pos.position, CapsuleSize, capsuleDirection, transform.rotation.z);
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                }
                //animator.setTrigger("atk");
                curTime = coolTime; curTime = 0;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }
    // 아래 함수는 콜라이더의 윤곽선을 보여주기 위한 함수
    // 사용한 이후 주석 처리 예정
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, CapsuleSize);

    }
}
