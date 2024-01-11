using UnityEngine;

// 제네릭 싱글톤 클래스
public class MySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static MySingleton<T> _instance;

    // 외부에서 접근할 수 있는 싱글톤 인스턴스 속성
    public static MySingleton<T> Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (_instance == null)
            {
                // 현재 타입의 인스턴스 찾기
                _instance = FindObjectOfType<MySingleton<T>>();

                // 씬에 없을 경우 새로운 게임오브젝트에 추가하여 인스턴스 생성
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<MySingleton<T>>();
                }
            }

            // 생성된 또는 이미 존재하는 인스턴스 반환
            return _instance;
        }
    }

    // 싱글톤 기능 및 데이터 등을 구현
    void Awake()
    {
        // 인스턴스가 이미 존재하는 경우 중복 생성 방지
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
        else
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 현재 오브젝트는 파괴
        }
    }

    // 기타 싱글톤의 기능과 데이터를 구현
}
