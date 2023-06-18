using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
public GameObject[] prefabs; // 스폰될 프리팹들의 배열
public Transform spawnPoint; // 스폰 위치
    public float minSpawnDelay; // 최소 스폰 딜레이
    public float maxSpawnDelay; // 최대 스폰 딜레이
public Quaternion desiredRotation;

    private float nextSpawnTime; // 다음 스폰 시간

private void Start()
{
    // 첫 번째 스폰 시간 설정
    nextSpawnTime = Time.time + GetRandomSpawnDelay();
}

private void Update()
{
    // 현재 시간이 다음 스폰 시간을 넘어갈 경우 스폰 실행
    if (Time.time >= nextSpawnTime)
    {
        SpawnPrefab();

        // 다음 스폰 시간 설정
        nextSpawnTime = Time.time + GetRandomSpawnDelay();
    }
}

private void SpawnPrefab()
{
    // 랜덤한 프리팹 인덱스 선택
    int randomIndex = Random.Range(0, prefabs.Length);


    // 선택한 프리팹을 스폰 위치에 생성하고 특정한 회전값 적용
        GameObject spawnedPrefab = Instantiate(prefabs[randomIndex], spawnPoint.position, desiredRotation);


    // 생성된 프리팹을 일정 시간 후에 파괴
        Destroy(spawnedPrefab, 10f);
}

private float GetRandomSpawnDelay()
{
    // 랜덤한 스폰 딜레이 계산
    return Random.Range(minSpawnDelay, maxSpawnDelay);
}
}