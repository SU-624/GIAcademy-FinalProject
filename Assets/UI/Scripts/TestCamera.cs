using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Vector3 _forwardDir;

    public float altitude = 80f;
    public float translationSpeed = 100f;
    private RaycastHit _hit;
    private Ray _ray;
    public float scrollspeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }

    // Update is called once per frame
    void Update()
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

    public void _TranslateCamera(int dir)
    {
        if (dir == 0)       // top
            transform.Translate(_forwardDir * Time.deltaTime * translationSpeed, Space.World);
        else if (dir == 1)  // right
            transform.Translate(transform.right * Time.deltaTime * translationSpeed);
        else if (dir == 2)  // bottom
            transform.Translate(-_forwardDir * Time.deltaTime * translationSpeed, Space.World);
        else if (dir == 3)  // left
            transform.Translate(-transform.right * Time.deltaTime * translationSpeed);

        // translate camera at proper altitude: cast a ray to the ground
        // and move up the hit point
        _ray = new Ray(transform.position, Vector3.up * -1000f);
        if (Physics.Raycast(
                _ray,
                out _hit,
                1000f
            //Globals.TERRAIN_LAYER_MASK
            ))
            transform.position = _hit.point + Vector3.up * altitude;
    }

    public void TestZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollspeed;

        // ����
        if (GetComponent<Camera>().orthographicSize <= 2f && scroll > 0)
        {
            float temp_Value = GetComponent<Camera>().orthographicSize;
            GetComponent<Camera>().orthographicSize = temp_Value;
        }
        else if (GetComponent<Camera>().orthographicSize >= 12f && scroll < 0)
        {
            float temp_Value = GetComponent<Camera>().orthographicSize;
            GetComponent<Camera>().orthographicSize = temp_Value;
        }
        else
        {
            // ��ũ�� �� �����̴� �ܰ�?
            GetComponent<Camera>().orthographicSize -= scroll * scrollspeed;
        }
    }
}
