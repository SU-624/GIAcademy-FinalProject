using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class InGameCamera : MonoBehaviour
{
    public float m_OrthoZoomSpeed = 0.5f;    // OrthoGraphic Mode
    public Camera camera;
    public Transform cameraTransform;

    public float MoveSpeed;
    Vector2 PrevPos = Vector2.zero;
    float PrevDistance = 0.0f;

    Vector2 ClickPoint;

    // Start is called before the first frame update
    void Start()
    {
        // camera = new Camera();

        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount == 2)      // 줌인.아웃 가능한 손가락 2개만큼의 터치만 허용
        {
            PinchZoom();
        }


        if(Input.GetMouseButtonDown(0))
        {
            ClickPoint = Input.mousePosition;
        
            Vector3 position = camera.ScreenToViewportPoint((Vector2)Input.mousePosition - ClickPoint);
        
            Vector3 move = position * (Time.deltaTime * MoveSpeed);
        
            cameraTransform.Translate(move);
        }
    }

    public void OnMouseDrag()
    {
        int touchcount = Input.touchCount;

        ////뭐지
        if (touchcount == 1)
        {
            if (PrevPos == Vector2.zero)
            {
                PrevPos = Input.GetTouch(0).position;

                return;
            }

            Vector2 dir = (Input.GetTouch(0).position - PrevPos).normalized;
            Vector3 vec = new Vector3(dir.x, dir.y);

            cameraTransform.position -= vec * MoveSpeed * Time.deltaTime;
            PrevPos = Input.GetTouch(0).position;
        }
    }
    public void ExitDrag()
    {
        PrevPos = Vector2.zero;
        PrevDistance = 0.0f;
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
            camera.orthographicSize += deltaMagnitudeDiff * m_OrthoZoomSpeed;

            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 1.5f);
            camera.orthographicSize = Mathf.Min(camera.orthographicSize, 7.28f);
        }

    }

}
