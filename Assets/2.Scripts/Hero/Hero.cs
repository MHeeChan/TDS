using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    [Header("자동 발사 설정")]
    [SerializeField] private float detectInterval = 1f;       // 자동 발사 주기
    [SerializeField] private float detectRadius = 10f;        // 적 탐지 반경
    [SerializeField] private LayerMask enemyLayer;            // 적 레이어
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform gunTransform;
    private float lastDetectTime = 0f;
    private List<Transform> nearbyEnemies = new List<Transform>();
    bool clicked = false;
    Vector3 dir;
    void Start()
    {
        if (mainCamera == null)
            Debug.LogError("Main Camera가 설정되어 있지 않습니다!");
    }

    void Update()
    {
        
        // 1) 마우스 클릭이 있으면 클릭 위치로 즉시 발사
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            // z는 항상 카메라~월드 평면의 거리 (Hero가 z=0이면)
            mousePos.z = Mathf.Abs(mainCamera.transform.position.z);

            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(mousePos);

            Vector3 dirG = (worldPoint - gunTransform.position).normalized;
            float angle = Mathf.Atan2(dirG.y, dirG.x) * Mathf.Rad2Deg;
            gunTransform.rotation = Quaternion.Euler(0, 0, angle - 45f);

            Vector3 clickDir = (worldPoint - transform.position).normalized;
            dir = clickDir;
            clicked = true;
        }

        // 2) 주기마다 자동 발사 (가장 가까운 적 방향으로)
        if (Time.time - lastDetectTime >= detectInterval)
        {
            lastDetectTime = Time.time;
            UpdateNearbyEnemies();

            Transform nearest = GetNearestEnemy();
            Vector3 autoDir;
            if (nearest != null)
                autoDir = (nearest.position - transform.position).normalized;
            else
                autoDir = transform.up;  // 적 없으면 위 방향(기본)으로 발사
            //FireAllWeapons(autoDir);
            
            if (!clicked)
            {
                // 총을 autoDir 방향으로 회전
                Vector3 dirG = autoDir; // gunTransform.position 대신 transform.position을 써도 무방
                float angle = Mathf.Atan2(dirG.y, dirG.x) * Mathf.Rad2Deg;
                gunTransform.rotation = Quaternion.Euler(0, 0, angle- 45f);

                FireAllWeapons(autoDir);
            }
            else
            {
                FireAllWeapons(dir);
            }
        }
        
    }

    // 현재 nearbyEnemies를 갱신
    private void UpdateNearbyEnemies()
    {
        nearbyEnemies.Clear();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, enemyLayer);
        foreach (var c in hits)
            nearbyEnemies.Add(c.transform);
    }

    // nearestEnemy를 반환
    private Transform GetNearestEnemy()
    {
        Transform best = null;
        float minDist = float.MaxValue;
        foreach (var e in nearbyEnemies)
        {
            float d = Vector2.Distance(transform.position, e.position);
            if (d < minDist)
            {
                minDist = d;
                best = e;
            }
        }
        return best;
    }

    // weaponSystem.Weapons에 등록된 모든 무기를 direction으로 발사
    private void FireAllWeapons(Vector3 direction)
    {
        GameObject go = ObjectPoolManager.instance.GetGo("Bullet", direction);
        go.transform.position = transform.position;
    }
}
