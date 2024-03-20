using UnityEngine;

// ���׸� �̱��� Ŭ����
public class MySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    private static MySingleton<T> _instance;

    // �ܺο��� ������ �� �ִ� �̱��� �ν��Ͻ� �Ӽ�
    public static MySingleton<T> Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ����
            if (_instance == null)
            {
                // ���� Ÿ���� �ν��Ͻ� ã��
                _instance = FindObjectOfType<MySingleton<T>>();

                // ���� ���� ��� ���ο� ���ӿ�����Ʈ�� �߰��Ͽ� �ν��Ͻ� ����
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<MySingleton<T>>();
                }
            }

            // ������ �Ǵ� �̹� �����ϴ� �ν��Ͻ� ��ȯ
            return _instance;
        }
    }

    // �̱��� ��� �� ������ ���� ����
    void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϴ� ��� �ߺ� ���� ����
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ���� ������Ʈ�� �ı�
        }
    }

    // ��Ÿ �̱����� ��ɰ� �����͸� ����
}
