using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EnemyPool : MySingleton<EnemyPool>
{
    private static EnemyPool instance;

    // �� ������ ���� �����ϱ� ���� ��ųʸ�
    private Dictionary<string, float> enemy_Health_Dictionary = new Dictionary<string, float>(); //�� ü�� ��ųʸ�
    private Dictionary<string, float> enemy_Atk_Dictionary = new Dictionary<string, float>(); //�� ���ݷ� ��ųʸ�
    private Dictionary<string, float> enemy_Speed_Dictionary = new Dictionary<string, float>(); //�� ���ǵ� ��ųʸ�

    private void Start() //��ųʸ� ����ȭ �۾�����
    {
        // === Prototype0===
        enemy_Health_Dictionary.Add("Prototype0", 10f);
        enemy_Atk_Dictionary.Add("Prototype0", 10f);
        enemy_Speed_Dictionary.Add("Prototype0", 10f);
        // ���� ü���� �������� ����
        //float enemyHealth = EnemyPool.GetEnemyHealth("Prototype0");
        //Debug.Log("Prototype0�� ü��: " + enemyHealth);

        // ex) === Prototype1=== 
    }

    public static float GetEnemyHealth(string enemyType)
    {
        if (instance.enemy_Health_Dictionary.ContainsKey(enemyType))
        {
            return instance.enemy_Health_Dictionary[enemyType];
        }
        else
        {
            Debug.LogError("���� ��ü�Դϴ�");
            return 0f;
        }
    }

    public static float GetEnemyAtk(string enemyType)
    {
        if (instance.enemy_Atk_Dictionary.ContainsKey(enemyType))
        {
            return instance.enemy_Atk_Dictionary[enemyType];
        }
        else
        {
            Debug.LogError("���� ��ü�Դϴ�");
            return 0f;
        }
    }

    public static float GetEnemySpeed(string enemyType)
    {
        if (instance.enemy_Speed_Dictionary.ContainsKey(enemyType))
        {
            return instance.enemy_Speed_Dictionary[enemyType];
        }
        else
        {
            Debug.LogError("���� ��ü�Դϴ�");
            return 0f;
        }
    }

}
