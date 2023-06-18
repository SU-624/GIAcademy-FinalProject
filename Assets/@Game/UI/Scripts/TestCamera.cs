using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Vector3 _forwardDir;

    public float altitude = 80f;
    public float translationSpeed = 300f;
    private RaycastHit _hit;
    private Ray _ray;
    public float scrollspeed = 2.0f;
    [Space(5f)]
    [Header("줌인 줌아웃을 위한 Max, Min 값")]
    public float MaxValue = 8.0f;           // 줌인
    public float MinValue = 18.0f;          // 줌아웃
    public float ZoomSpeed = 50.0f;

    [Header("카메라 초기 값")]
    [SerializeField] private Vector3 m_cameraPosition;
    [SerializeField] private Quaternion m_cameraRotation;

    //public Transform[] LimitPoints;
    public GameObject[] BlockAreas;
    //public LayerMask LimitLayerMask;

    public bool IsFixed;
    public GameObject FixedObject;

    private Vector3 m_velocity = Vector3.zero;
    private Vector3 m_cameraOffset = new Vector3(-20, 45, -23);

    private Vector3 m_block1OriginPos = new Vector3(0, 0, 52);
    private Vector3 m_block2OriginPos = new Vector3(34.8f, 0, 8);
    private Vector3 m_block3OriginPos = new Vector3(0, 0, -42.7f);
    private Vector3 m_block4OriginPos = new Vector3(-44.7f, 0, 8);

    private float m_cameraOrthoSize = 28f;

    // Start is called before the first frame update
    void Start()
    {
        _forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        FixedObject = null;
    }

    // Update is called once per frame
    void Update()
    {

        SetLimitArea();

        //if (!Input.anyKeyDown)
        //{
        //    if (GetComponent<Rigidbody>().velocity.magnitude > 0)
        //    {

        //    }
        //    GetComponent<Rigidbody>().velocity -= Time.deltaTime * Vector3.one;
        //}
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        if (IsFixed)
        {
            if (FixedObject != null)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (GetComponent<Camera>().orthographicSize >= MaxValue + 4f)
                {
                    GetComponent<Camera>().orthographicSize -= Time.deltaTime * ZoomSpeed;
                }
                //transform.position = FixedObject.transform.position + m_cameraPosition;
                transform.position = Vector3.SmoothDamp(transform.position, FixedObject.transform.position + m_cameraOffset, ref m_velocity, 0.1f);
                //transform.LookAt(FixedObject.transform);
                BlockAreas[0].SetActive(false);
                BlockAreas[1].SetActive(false);
                BlockAreas[2].SetActive(false);
                BlockAreas[3].SetActive(false);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
                _TranslateCamera(0);
            if (Input.GetKey(KeyCode.D))
                _TranslateCamera(1);
            if (Input.GetKey(KeyCode.S))
                _TranslateCamera(2);
            if (Input.GetKey(KeyCode.A))
                _TranslateCamera(3);

            TestZoomCamera();
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

    IEnumerator ResetCameraZoom()
    {
        while (GetComponent<Camera>().orthographicSize <= MinValue)
        {
            GetComponent<Camera>().orthographicSize += Time.deltaTime * ZoomSpeed;

            yield return null;
        }

        BlockAreas[0].SetActive(true);
        BlockAreas[1].SetActive(true);
        BlockAreas[2].SetActive(true);
        BlockAreas[3].SetActive(true);
    }

    public void _TranslateCamera(int dir)
    {
        if (dir == 0)
        {
            //transform.Translate(_forwardDir * Time.deltaTime * (translationSpeed + GetComponent<Camera>().orthographicSize * 4), Space.World);
            GetComponent<Rigidbody>().velocity += _forwardDir * Time.deltaTime * (translationSpeed + GetComponent<Camera>().orthographicSize * 4);
        }
        else if (dir == 1)
        {
            //transform.Translate(transform.right * Time.deltaTime * (translationSpeed + GetComponent<Camera>().orthographicSize * 4), Space.World);
            GetComponent<Rigidbody>().velocity += transform.right * Time.deltaTime *
                                                 (translationSpeed + GetComponent<Camera>().orthographicSize * 4);
        }
        else if (dir == 2)
        {
            //transform.Translate(-_forwardDir * Time.deltaTime * (translationSpeed + GetComponent<Camera>().orthographicSize * 4), Space.World);
            GetComponent<Rigidbody>().velocity += -_forwardDir * Time.deltaTime *
                                                 (translationSpeed + GetComponent<Camera>().orthographicSize * 4);
        }
        else if (dir == 3)
        {
            //transform.Translate(-transform.right * Time.deltaTime * (translationSpeed + GetComponent<Camera>().orthographicSize * 4), Space.World);
            GetComponent<Rigidbody>().velocity += -transform.right * Time.deltaTime *
                                                 (translationSpeed + GetComponent<Camera>().orthographicSize * 4);
        }
    }

    public void TestZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollspeed;

        // Min(줌인) 
        if (GetComponent<Camera>().orthographicSize <= MaxValue && scroll > 0)
        {
            //float temp_Value = GetComponent<Camera>().orthographicSize;
            //GetComponent<Camera>().orthographicSize = temp_Value;
            GetComponent<Camera>().orthographicSize = MaxValue;
        }
        // Max(줌아웃)
        else if (GetComponent<Camera>().orthographicSize >= MinValue && scroll < 0)
        {
            //float temp_Value = GetComponent<Camera>().orthographicSize;
            //GetComponent<Camera>().orthographicSize = temp_Value;
            GetComponent<Camera>().orthographicSize = MinValue;
        }
        else
        {
            GetComponent<Camera>().orthographicSize -= scroll;
        }
    }

    private void SetLimitArea()
    {
        BlockAreas[0].transform.position = m_block1OriginPos +
                                           new Vector3(0, 0, 1) *
                                           (-GetComponent<Camera>().orthographicSize + m_cameraOrthoSize);

        BlockAreas[1].transform.position = m_block2OriginPos +
                                           new Vector3(1, 0, 0) *
                                           (-GetComponent<Camera>().orthographicSize + m_cameraOrthoSize);

        BlockAreas[2].transform.position = m_block3OriginPos +
                                           new Vector3(0, 0, -1) *
                                           (-GetComponent<Camera>().orthographicSize + m_cameraOrthoSize);

        BlockAreas[3].transform.position = m_block4OriginPos +
                                           new Vector3(-1, 0, 0) *
                                           (-GetComponent<Camera>().orthographicSize + m_cameraOrthoSize);
    }
}
