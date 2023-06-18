using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchtest : MonoBehaviour
{
    public AudioClip[] sounds; // 재생할 사운드들을 담을 배열

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
        // 사용자의 터치 입력을 확인합니다.
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            // 터치한 위치를 기준으로 사운드를 재생합니다.
            Vector3 touchPosition = Input.GetTouch(0).position;

            // 아이소메트릭 카메라의 월드 좌표로 변환합니다.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.transform.position.z));

            // 랜덤한 인덱스를 생성하여 배열에서 사운드를 선택합니다.
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip randomSound = sounds[randomIndex];

            // 사운드를 재생합니다.
            AudioSource.PlayClipAtPoint(randomSound, worldPosition);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // 터치한 위치를 기준으로 사운드를 재생합니다.
            Vector3 touchPosition = Input.GetTouch(0).position;

            // 아이소메트릭 카메라의 월드 좌표로 변환합니다.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.transform.position.z));

            // 랜덤한 인덱스를 생성하여 배열에서 사운드를 선택합니다.
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip randomSound = sounds[randomIndex];

            // 사운드를 재생합니다.
            AudioSource.PlayClipAtPoint(randomSound, worldPosition);
        }
#endif

    }
}