using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MySingleton<EnemyPool>
{
    // �� ������ ���� �����ϱ� ���� ��ųʸ�
    private Dictionary<string, float> enemy_Health_Dictionary = new Dictionary<string, float>(); //�� ü�� ��ųʸ�
    private Dictionary<string, float> enemy_Atk_Dictionary = new Dictionary<string, float>(); //�� ���ݷ� ��ųʸ�
    private Dictionary<string, float> enemy_Speed_Dictionary = new Dictionary<string, float>(); //�� ���ǵ� ��ųʸ�
    private void Awake()
    {
        // === Prototype0===
        enemy_Health_Dictionary.Add("Prototype0", 10f);
        enemy_Atk_Dictionary.Add("Prototype0", 10f);
        enemy_Speed_Dictionary.Add("Prototype0", 3f);
        // ex) === Prototype1=== 
    }
    public float GetEnemyHealth(string enemyType)
    {
        if (enemy_Health_Dictionary.ContainsKey(enemyType))
        {
            return enemy_Health_Dictionary[enemyType];
        }
        else
        {
            Debug.LogError("���� ��ü�Դϴ�");
            return 0f;
        }
    }

    public float GetEnemyAtk(string enemyType)
    {
        if (enemy_Atk_Dictionary.ContainsKey(enemyType))
        {
            return enemy_Atk_Dictionary[enemyType];
        }
        else
        {
            Debug.LogError("���� ��ü�Դϴ�");
            return 0f;
        }
    }

    public float GetEnemySpeed(string enemyType)
    {
        if (enemy_Speed_Dictionary.ContainsKey(enemyType))
        {
            return enemy_Speed_Dictionary[enemyType];
        }
        else
        {
            Debug.LogError("���� ��ü�Դϴ�");
            return 0f;
        }
    }
}
