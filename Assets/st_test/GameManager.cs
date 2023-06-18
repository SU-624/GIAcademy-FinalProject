using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
public GameObject[] prefabs; // ������ �����յ��� �迭
public Transform spawnPoint; // ���� ��ġ
    public float minSpawnDelay; // �ּ� ���� ������
    public float maxSpawnDelay; // �ִ� ���� ������
public Quaternion desiredRotation;

    private float nextSpawnTime; // ���� ���� �ð�

private void Start()
{
    // ù ��° ���� �ð� ����
    nextSpawnTime = Time.time + GetRandomSpawnDelay();
}

private void Update()
{
    // ���� �ð��� ���� ���� �ð��� �Ѿ ��� ���� ����
    if (Time.time >= nextSpawnTime)
    {
        SpawnPrefab();

        // ���� ���� �ð� ����
        nextSpawnTime = Time.time + GetRandomSpawnDelay();
    }
}

private void SpawnPrefab()
{
    // ������ ������ �ε��� ����
    int randomIndex = Random.Range(0, prefabs.Length);


    // ������ �������� ���� ��ġ�� �����ϰ� Ư���� ȸ���� ����
        GameObject spawnedPrefab = Instantiate(prefabs[randomIndex], spawnPoint.position, desiredRotation);


    // ������ �������� ���� �ð� �Ŀ� �ı�
        Destroy(spawnedPrefab, 10f);
}

private float GetRandomSpawnDelay()
{
    // ������ ���� ������ ���
    return Random.Range(minSpawnDelay, maxSpawnDelay);
}
}