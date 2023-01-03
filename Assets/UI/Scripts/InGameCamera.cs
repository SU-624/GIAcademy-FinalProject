using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 22. 12. 28 Mang
/// 
/// 아트 리소스 업데이트 - 씬 변경 시 인게임씬 카메라에 스크립트 붙어있는지 항상 확인하기
/// </summary>
public class InGameCamera : MonoBehaviour
{
    public float m_OrthoZoomSpeed = 0.5f;    // OrthoGraphic Mode
    public Camera camera;

    public float MoveSpeed;
    // Vector2 PrevPos = Vector2.zero;

    Vector2 prePos, nowPos;
    Vector3 movePos;

    float PrevDistance = 0.0f;

    Vector2 ClickPoint;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 줌인 줌아웃
        if (GameTime.Instance != null)
        {
            if (GameTime.Instance.IsGameMode == true && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.touchCount == 2)      // 줌인.아웃 가능한 손가락 2개만큼의 터치만 허용
                {
                    Debug.Log("손가락 두개");

                    PinchZoom();
                }
            }
        }

        // 카메라 이동
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("손가락 한개");

            ClickPoint = Input.GetTouch(0).position;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
                Debug.Log("손가락 처음찍은자리");
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * MoveSpeed;
                camera.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
                Debug.Log("손가락 움직이는 자리");
                Debug.Log(camera.transform.position.x + "     " + camera.transform.position.y + "     " + camera.transform.position.z);
            }
        }
    }

    public void PinchZoom()
    {
        Touch touchZero = Input.GetTouch(0);        // 첫번째 손가락 좌표
        Touch touchOne = Input.GetTouch(1);         // 두번째 손가락 좌표

        // deltaPosition 은 deltatime과 동일하게 delta 만큼 시간동안 움직인 거리이다
        // 현재 position에서 이전 delta 값을 빼주면 움직이기 전의 손가락 위치가 된다
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // 현재와 과거값의 움직임 크기 구하기
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // 두 값의 차이 = 확대 / 축소 할 때 얼만큼 많이 확대 / 축소 될지 결정
        float deltaMagnitudeDiff = prevTouchDeltaMag - TouchDeltaMag;

        if (camera.orthographic)
        {
            Debug.Log("손가락 두개로 줌 하는중");

            camera.orthographicSize += deltaMagnitudeDiff * m_OrthoZoomSpeed;

            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 3f);
            camera.orthographicSize = Mathf.Min(camera.orthographicSize, 12f);
        }

    }
}
