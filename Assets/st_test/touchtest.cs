using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchtest : MonoBehaviour
{
    public AudioClip[] sounds; // ����� ������� ���� �迭

    private AudioSource audioSource;
    private Camera mainCamera;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        TouchTest();
    }

    private void TouchTest()
    {
        // ������� ��ġ �Է��� Ȯ���մϴ�.
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            // ��ġ�� ��ġ�� �������� ���带 ����մϴ�.
            Vector3 touchPosition = Input.GetTouch(0).position;

            // ���̼Ҹ�Ʈ�� ī�޶��� ���� ��ǥ�� ��ȯ�մϴ�.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.transform.position.z));

            // ������ �ε����� �����Ͽ� �迭���� ���带 �����մϴ�.
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip randomSound = sounds[randomIndex];

            // ���带 ����մϴ�.
            AudioSource.PlayClipAtPoint(randomSound, worldPosition);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // ��ġ�� ��ġ�� �������� ���带 ����մϴ�.
            Vector3 touchPosition = Input.GetTouch(0).position;

            // ���̼Ҹ�Ʈ�� ī�޶��� ���� ��ǥ�� ��ȯ�մϴ�.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.transform.position.z));

            // ������ �ε����� �����Ͽ� �迭���� ���带 �����մϴ�.
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip randomSound = sounds[randomIndex];

            // ���带 ����մϴ�.
            AudioSource.PlayClipAtPoint(randomSound, worldPosition);
        }
#endif

    }
}