using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                //Debug.Log("Z��ư ���� & curTime <=0 �۵� �Ϸ�"); //üũ �Ϸ�
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
    // �Ʒ� �Լ��� �ݶ��̴��� �������� �����ֱ� ���� �Լ�
    // ����� ���� �ּ� ó�� ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, CapsuleSize);

    }
}
