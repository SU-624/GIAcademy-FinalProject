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
    public float m_OrthoZoomSpeed = 0.04f;    // OrthoGraphic Mode
    public Camera camera;

    public float MoveSpeed;
    // Vector2 PrevPos = Vector2.zero;

    Vector2 prePos, nowPos;
    Vector3 movePos;

    float PrevDistance = 0.0f;

    [Space(5f)]
    [Header("줌인 줌아웃을 위한 Max, Min 값")]
    public float MaxValue = 8.0f;           // 줌인
    public float MinValue = 18.0f;          // 줌아웃
    public float ZoomSpeed = 50.0f;


    private Vector3 _forwardDir;
    public bool IsFixed;
    public GameObject FixedObject;

    private Vector3 m_velocity = Vector3.zero;
    private Vector3 m_cameraOffset = new Vector3(-20, 45, -23);

    private Vector3 m_block1OriginPos = new Vector3(0, 0, 52);
    private Vector3 m_block2OriginPos = new Vector3(34.8f, 0, 8);
    private Vector3 m_block3OriginPos = new Vector3(0, 0, -42.7f);
    private Vector3 m_block4OriginPos = new Vector3(-44.7f, 0, 8);

    private float m_cameraOrthoSize = 28f;
    public GameObject[] BlockAreas;

    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Rect rt = cam.rect;

        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)2340 / 1080);
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
        {
            rt.height = scaleHeight;
            rt.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rt.width = scaleWidth;
            rt.x = (1f - scaleWidth) / 2f;
        }

        cam.rect = rt;
    }

    // Start is called before the first frame update
    void Start()
    {
        _forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }

    void FixedUpdate()
    {
#if UNITY_ANDROID
        if (IsFixed)
        {
            if (FixedObject != null)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (GetComponent<Camera>().orthographicSize >= MaxValue + 4f)
                {
                    GetComponent<Camera>().orthographicSize -= Time.deltaTime * ZoomSpeed;
                }
                transform.position = Vector3.SmoothDamp(transform.position, FixedObject.transform.position + m_cameraOffset, ref m_velocity, 0.1f);
                BlockAreas[0].SetActive(false);
                BlockAreas[1].SetActive(false);
                BlockAreas[2].SetActive(false);
                BlockAreas[3].SetActive(false);
            }
        } 
        else
        {
            // 줌인 줌아웃
            if (GameTime.Instance != null)
            {
                if (GameTime.Instance.IsGameMode == true && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (Input.touchCount == 2)      // 줌인.아웃 가능한 손가락 2개만큼의 터치만 허용
                    {
                        //Debug.Log("손가락 두개");

                        PinchZoom();
                    }
                }
            }

            // 카메라 이동
            if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    prePos = touch.position - touch.deltaPosition;
                    //Debug.Log("손가락 처음찍은자리");
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    nowPos = touch.position - touch.deltaPosition;
                    //movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * (MoveSpeed + camera.orthographicSize * 0.01f);
                    //movePos = (Vector3)(prePos - nowPos).normalized;
                    //camera.transform.Translate(movePos);
                    movePos = (Vector3)(prePos - nowPos).normalized;
                    GetComponent<Rigidbody>().velocity += (_forwardDir * movePos.y + transform.right * movePos.x) * Time.deltaTime *
                                                          (MoveSpeed + camera.orthographicSize * 40);
                    prePos = touch.position - touch.deltaPosition;

                    //Debug.Log("손가락 움직이는 자리");
                    //Debug.Log(camera.transform.position.x + "     " + camera.transform.position.y + "     " + camera.transform.position.z);
                }
                else
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
#endif
    }

    public void ResetCameraPosition()
    {
        if (IsFixed)
        {
            FixedObject = null;
            IsFixed = false;

            BlockAreas[0].SetActive(true);
            BlockAreas[1].SetActive(true);
            BlockAreas[2].SetActive(true);
            BlockAreas[3].SetActive(true);
            //transform.position =
            //    Vector3.SmoothDamp(transform.position, m_cameraPosition, ref m_velocity, 0.1f);
            //transform.rotation = m_cameraRotation;
            //transform.rotation = Quaternion.Slerp(transform.rotation, m_cameraRotation, 0.3f);

            //GetComponent<Camera>().orthographicSize = MinValue;
            //StartCoroutine(ResetCameraZoom());
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
        float deltaMagnitudeDiff = (prevTouchDeltaMag - TouchDeltaMag) * m_OrthoZoomSpeed;

        if (camera.orthographic)
        {
            // Min(줌인) 
            if (GetComponent<Camera>().orthographicSize <= MaxValue && deltaMagnitudeDiff < 0)
            {
                GetComponent<Camera>().orthographicSize = MaxValue;
            }
            // Max(줌아웃)
            else if (GetComponent<Camera>().orthographicSize >= MinValue && deltaMagnitudeDiff > 0)
            {
                GetComponent<Camera>().orthographicSize = MinValue;
            }
            else
            {
                GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff;
            }
        }
    }
}
