using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("스킬 값")]
    private float Teleport_value = 7f;
    Vector2 moveDirection;
    float moveX = 0f; //텔레포트 방향 지정
    float moveY = 0f;

    [Header("쿨타임 관련 변수")]
    private float curTime = 0f; //쿨타임을 세는 변수
    public float coolTime = 0f; //쿨타임이 몇초인지 저장하는 변수

    [Header("받아올 것들")]
    Animator animator_Skill;

    public SpriteRenderer playerRenderer;
    public GameObject Player;
    private Player_Move player_Move;
    private SkillManager skillManager;
    void Start()
    {
        animator_Skill = GetComponent<Animator>();
        player_Move = transform.parent.GetComponent<Player_Move>();
        skillManager = GetComponent<SkillManager>();

        coolTime = skillManager.teleport_Cool; // 쿨타임 할당
    }

    void Update()
    {
        Use_Teleport();
        //Debug.Log("curTime : " + curTime); //체크 완료
        if (animator_Skill.GetBool("Teleport_End") == true && curTime >0)
        {
            curTime -= Time.deltaTime;
        }
    }
    void Use_Teleport() //텔레포트 사용시 버튼을 누를때 이벤트가 나오는 함수
    {
        if (curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
                moveX = moveDirection.x;
                moveY = moveDirection.y;
                player_Move.can_Move = false;
                //Debug.Log("E 눌림 & curTime 0이하"); //체크 완료
                animator_Skill.SetBool("Teleport", true); 
                animator_Skill.SetBool("Teleport_End", false); 

                curTime = coolTime; //curTime 에 coolTime 을 대입해서 if문 안돌게 만들고
            }
        }
    }
    void Player_hide_Opaque() // 애니메이션이 일정 위치에 도달하면 텔레포트가 사용되도록 이벤트가 나오는 함수
    {
        //Player.SetActive(false); //비활성화가 아닌 RGBA 의 A 값을 낮춰서 투명으로 만들어야 작동
        if (playerRenderer.color.a != 0f)
        {
            playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 0f); //투명화
        }
        else
        {
            playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 255f); //불투명화
        }
    }
    void Teleport_In() //텔레포트 애니메이션이 끝났을 시 작동하는 이벤트 함수
    {
        animator_Skill.SetBool("Teleport", false);
        //Player.SetActive(true);
        
        if (moveX > 0) //고정적인 값으로 텔레포트를 위해서
        {
            moveX = 1;
        }
        else
        {
            moveX = -1;
        }
        if (moveY > 0)
        {
            moveY = 1;
        }
        else
        {
            moveY = 0; // -1 바닥으로 떨어져서 일단 중단
        }
            Player.transform.position = new Vector3(Player.transform.position.x + (Teleport_value * moveX),
                Player.transform.position.y + (Teleport_value * moveY), Player.transform.position.z); //앞을 보고 있을때(Flip.x == false) X + 값으로 이동시킴
    }
    void Teleport_End()
    {
        animator_Skill.SetBool("Teleport_End", true);
        player_Move.can_Move = true;
    }
}
