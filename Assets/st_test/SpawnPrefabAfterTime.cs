using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabAfterTime : MonoBehaviour
{
    public GameObject prefabToSpawn; // 생성할 프리팹
    public Transform spawnPosition; // 배치할 위치
    public float targetTime = 5f; // 특정 시간 (초)

    private float timer = 0f; // 타이머 변수

    private void Update()
    {
        timer += Time.deltaTime; // 경과한 시간을 타이머에 더합니다.

        if (timer >= targetTime) // 특정 시간을 초과하면
        {
            SpawnPrefab(); // 프리팹을 생성하는 함수를 호출합니다.
            enabled = false; // 타이머 업데이트를 중지합니다.
        }
    }

    private void SpawnPrefab()
    {
        GameObject prefabInstance = Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        // 프리팹을 생성하고, spawnPosition의 위치에 배치합니다.

        // 추가로 필요한 작업이 있다면 이곳에 작성합니다.
    }
}