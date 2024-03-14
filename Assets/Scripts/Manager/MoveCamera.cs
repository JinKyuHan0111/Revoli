using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 대상, 주로 플레이어를 지정합니다.
    public float speed; // 카메라가 대상을 따라가는 속도를 결정하는 변수입니다.

    public Vector2 center; // 카메라가 이동할 수 있는 영역의 중심점을 설정합니다.
    public Vector2 size; // 카메라가 이동할 수 있는 영역의 크기를 설정합니다.
    float height; // 카메라의 세로 크기입니다.
    float width; // 카메라의 가로 크기입니다.

    void Start()
    {
        // 카메라의 가로 및 세로 크기를 계산합니다. 이는 카메라가 이동할 수 있는 영역을 제한하는 데 사용됩니다.
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    // 에디터에서만 보이는 영역을 그려주는 기능입니다. 이동 가능 영역을 시각화하기 위해 사용됩니다.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 영역의 색상을 빨간색으로 설정합니다.
        Gizmos.DrawWireCube(center, size); // 중심점(center)을 중심으로 크기(size)를 가진 와이어 프레임 큐브를 그립니다.
    }

    void LateUpdate()
    {
        // 대상(플레이어)의 위치를 향해 카메라를 부드럽게 이동시킵니다.
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        // 카메라의 x, y 위치를 제한하는 영역을 계산합니다.
        float lx = size.x * 0.5f - width; // x축의 이동 가능한 최대 범위를 계산합니다.
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x); // 카메라의 x 위치를 제한합니다.

        float ly = size.y * 0.5f - height; // y축의 이동 가능한 최대 범위를 계산합니다.
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y); // 카메라의 y 위치를 제한합니다.

        // 계산된 x, y 위치를 적용하고, z 위치는 카메라가 대상을 바라볼 수 있도록 설정합니다(-10f는 카메라를 대상 앞에 위치시키기 위한 값입니다).
        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
