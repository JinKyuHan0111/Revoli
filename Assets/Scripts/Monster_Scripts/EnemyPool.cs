using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EnemyPool : MySingleton<EnemyPool>
{
    private static EnemyPool instance;

    // 각 유형의 적을 관리하기 위한 딕셔너리
    private Dictionary<string, float> enemy_Health_Dictionary = new Dictionary<string, float>(); //적 체력 딕셔너리
    private Dictionary<string, float> enemy_Atk_Dictionary = new Dictionary<string, float>(); //적 공격력 딕셔너리
    private Dictionary<string, float> enemy_Speed_Dictionary = new Dictionary<string, float>(); //적 스피드 딕셔너리

    private void Start() //딕셔너리 사전화 작업구간
    {
        // === Prototype0===
        enemy_Health_Dictionary.Add("Prototype0", 10f);
        enemy_Atk_Dictionary.Add("Prototype0", 10f);
        enemy_Speed_Dictionary.Add("Prototype0", 10f);
        // 적의 체력을 가져오는 예시
        //float enemyHealth = EnemyPool.GetEnemyHealth("Prototype0");
        //Debug.Log("Prototype0의 체력: " + enemyHealth);

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
            Debug.LogError("없는 개체입니다");
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
            Debug.LogError("없는 개체입니다");
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
            Debug.LogError("없는 개체입니다");
            return 0f;
        }
    }

}
