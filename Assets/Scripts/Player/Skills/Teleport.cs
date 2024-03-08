using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("��ų ��")]
    private float Teleport_value = 5f;

    [Header("��Ÿ�� ���� ����")]
    private float curTime = 0f; //��Ÿ���� ���� ����
    public float coolTime = 0f; //��Ÿ���� �������� �����ϴ� ����

    [Header("�޾ƿ� �͵�")]
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

        coolTime = skillManager.teleport_Cool; // ��Ÿ�� �Ҵ�
    }

    void Update()
    {
        Use_Teleport();
        //Debug.Log("curTime : " + curTime); //üũ �Ϸ�
        if (animator_Skill.GetBool("Teleport_End") == true && curTime >0)
        {
            curTime -= Time.deltaTime;
        }
    }
    void Use_Teleport() //�ڷ���Ʈ ���� ��ư�� ������ �̺�Ʈ�� ������ �Լ�
    {
        if (curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                player_Move.can_Move = false;
                //Debug.Log("E ���� & curTime 0����"); //üũ �Ϸ�
                animator_Skill.SetBool("Teleport", true); 
                animator_Skill.SetBool("Teleport_End", false); 
                curTime = coolTime; //curTime �� coolTime �� �����ؼ� if�� �ȵ��� �����
            }
        }
    }
    void Player_hide_Opaque() // �ִϸ��̼��� ���� ��ġ�� �����ϸ� �ڷ���Ʈ�� ���ǵ��� �̺�Ʈ�� ������ �Լ�
    {
        //Player.SetActive(false); //��Ȱ��ȭ�� �ƴ� RGBA �� A ���� ���缭 �������� ������ �۵�
        if (playerRenderer.color.a != 0f)
        {
            playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 0f); //����ȭ
        }
        else
        {
            playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 255f); //������ȭ
        }
    }
    void Teleport_In() //�ڷ���Ʈ �ִϸ��̼��� ������ �� �۵��ϴ� �̺�Ʈ �Լ�
    {
        animator_Skill.SetBool("Teleport", false);
        //Player.SetActive(true);
        if (playerRenderer.flipX == false)
        {
            Player.transform.position = new Vector3(Player.transform.position.x + Teleport_value,
                Player.transform.position.y, Player.transform.position.z); //���� ���� ������(Flip.x == false) X + ������ �̵���Ŵ
        }
        else if (playerRenderer.flipX == true)
        {
            Player.transform.position = new Vector3(Player.transform.position.x - Teleport_value, Player.
                transform.position.y, Player.transform.position.z); //�ڸ� ���� ������(Flip.x == true) X�� - ������ �̵���Ŵ
        }
    }
    void Teleport_End()
    {
        animator_Skill.SetBool("Teleport_End", true);
        player_Move.can_Move = true;
    }
}
