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

        if (Input.touchCount == 2)      // ����.�ƿ� ������ �հ��� 2����ŭ�� ��ġ�� ���
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

        ////����
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
        Touch touchZero = Input.GetTouch(0);        // ù��° �հ��� ��ǥ
        Touch touchOne = Input.GetTouch(1);         // �ι�° �հ��� ��ǥ

        // deltaPosition �� deltatime�� �����ϰ� delta ��ŭ �ð����� ������ �Ÿ��̴�
        // ���� position���� ���� delta ���� ���ָ� �����̱� ���� �հ��� ��ġ�� �ȴ�
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // ����� ���Ű��� ������ ũ�� ���ϱ�
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // �� ���� ���� = Ȯ�� / ��� �� �� ��ŭ ���� Ȯ�� / ��� ���� ����
        float deltaMagnitudeDiff = prevTouchDeltaMag - TouchDeltaMag;

        if (camera.orthographic)
        {
            camera.orthographicSize += deltaMagnitudeDiff * m_OrthoZoomSpeed;

            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 1.5f);
            camera.orthographicSize = Mathf.Min(camera.orthographicSize, 7.28f);
        }

    }

}
