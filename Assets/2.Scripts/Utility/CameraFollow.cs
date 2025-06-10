using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // 따라갈 대상(Hero)

    [SerializeField] private float smoothSpeed = 5f; // 따라가는 속도 (값이 높을수록 즉시 따라감)
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // z=-10은 기본 카메라 z위치

    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = new Vector3(target.position.x+3, 0, target.position.z) + offset;
        // 부드럽게 따라가고 싶으면 Lerp, 즉시 따라가게 하고 싶으면 아래 한 줄만 써도 됨.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}