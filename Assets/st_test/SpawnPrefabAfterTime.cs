using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabAfterTime : MonoBehaviour
{
    public GameObject prefabToSpawn; // ������ ������
    public Transform spawnPosition; // ��ġ�� ��ġ
    public float targetTime = 5f; // Ư�� �ð� (��)

    private float timer = 0f; // Ÿ�̸� ����

    private void Update()
    {
        timer += Time.deltaTime; // ����� �ð��� Ÿ�̸ӿ� ���մϴ�.

        if (timer >= targetTime) // Ư�� �ð��� �ʰ��ϸ�
        {
            SpawnPrefab(); // �������� �����ϴ� �Լ��� ȣ���մϴ�.
            enabled = false; // Ÿ�̸� ������Ʈ�� �����մϴ�.
        }
    }

    private void SpawnPrefab()
    {
        GameObject prefabInstance = Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        // �������� �����ϰ�, spawnPosition�� ��ġ�� ��ġ�մϴ�.

        // �߰��� �ʿ��� �۾��� �ִٸ� �̰��� �ۼ��մϴ�.
    }
}