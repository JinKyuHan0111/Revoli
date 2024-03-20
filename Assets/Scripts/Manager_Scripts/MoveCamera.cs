using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target; // ī�޶� ���� ���, �ַ� �÷��̾ �����մϴ�.
    public float speed; // ī�޶� ����� ���󰡴� �ӵ��� �����ϴ� �����Դϴ�.

    public Vector2 center; // ī�޶� �̵��� �� �ִ� ������ �߽����� �����մϴ�.
    public Vector2 size; // ī�޶� �̵��� �� �ִ� ������ ũ�⸦ �����մϴ�.
    float height; // ī�޶��� ���� ũ���Դϴ�.
    float width; // ī�޶��� ���� ũ���Դϴ�.

    void Start()
    {
        // ī�޶��� ���� �� ���� ũ�⸦ ����մϴ�. �̴� ī�޶� �̵��� �� �ִ� ������ �����ϴ� �� ���˴ϴ�.
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    // �����Ϳ����� ���̴� ������ �׷��ִ� ����Դϴ�. �̵� ���� ������ �ð�ȭ�ϱ� ���� ���˴ϴ�.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // ������ ������ ���������� �����մϴ�.
        Gizmos.DrawWireCube(center, size); // �߽���(center)�� �߽����� ũ��(size)�� ���� ���̾� ������ ť�긦 �׸��ϴ�.
    }

    void LateUpdate()
    {
        // ���(�÷��̾�)�� ��ġ�� ���� ī�޶� �ε巴�� �̵���ŵ�ϴ�.
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        // ī�޶��� x, y ��ġ�� �����ϴ� ������ ����մϴ�.
        float lx = size.x * 0.5f - width; // x���� �̵� ������ �ִ� ������ ����մϴ�.
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x); // ī�޶��� x ��ġ�� �����մϴ�.

        float ly = size.y * 0.5f - height; // y���� �̵� ������ �ִ� ������ ����մϴ�.
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y); // ī�޶��� y ��ġ�� �����մϴ�.

        // ���� x, y ��ġ�� �����ϰ�, z ��ġ�� ī�޶� ����� �ٶ� �� �ֵ��� �����մϴ�(-10f�� ī�޶� ��� �տ� ��ġ��Ű�� ���� ���Դϴ�).
        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
