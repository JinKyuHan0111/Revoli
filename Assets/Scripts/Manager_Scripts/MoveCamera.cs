using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target;
    public float speed;

    public GameObject cameraBounds; // 이동 가능 영역을 나타내는 2D 오브젝트를 참조하는 변수입니다.

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

        // 2D 오브젝트의 SpriteRenderer 컴포넌트를 통해 이동 가능 영역의 경계를 얻습니다.
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