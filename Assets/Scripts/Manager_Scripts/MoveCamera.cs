using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target;
    public float speed;

    public GameObject cameraBounds; // �̵� ���� ������ ��Ÿ���� 2D ������Ʈ�� �����ϴ� �����Դϴ�.

    private float height;
    private float width;

    void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        // 2D ������Ʈ�� SpriteRenderer ������Ʈ�� ���� �̵� ���� ������ ��踦 ����ϴ�.
        Bounds bounds = cameraBounds.GetComponent<SpriteRenderer>().bounds;
        float minX = bounds.min.x + width;
        float maxX = bounds.max.x - width;
        float minY = bounds.min.y + height;
        float maxY = bounds.max.y - height;

        float clampX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}