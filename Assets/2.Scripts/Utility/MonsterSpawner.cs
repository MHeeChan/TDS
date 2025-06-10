using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private string poolKey = "ZombieMelee";  // 오브젝트풀 키값
    //[SerializeField] private Vector3 spawnPosition;       // 스폰 위치
    [SerializeField] private GameObject truck;       // 스폰 위치
    [SerializeField] private float spawnInterval = 0.5f;    // 5초 간격

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            GameObject monster = ObjectPoolManager.instance.GetGo(poolKey);
            if (monster != null)
            {
                monster.transform.position = truck.transform.position + new Vector3(30,0,0);
                monster.transform.rotation = Quaternion.identity;
                monster.SetActive(true); // 혹시 Pool에서 꺼낼 때 자동 활성화가 안 되면
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}