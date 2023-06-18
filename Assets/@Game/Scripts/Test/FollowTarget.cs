using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 타겟의 포지션을 계속 받아와서 카메라가 움직여도 
/// UI가 화면에 남는 현상을 고쳐줌
/// 
/// 
/// </summary>
public class FollowTarget : MonoBehaviour
{
    public Transform m_Target;

    int CameraMaxOrthoSize = 18;

    float ChatBoxOriginalSize = 0.4f;
    float ChatBoxscaleAmount = 0.03f;
    float ChatBoxOffsetX = 0.5f;
    float ChatBoxLeftOffsetX = 1.1f;
    float ChatBoxOffsetY = 0.1f;

    public bool IsLeft = false;

    [SerializeField] private Vector3 m_Offset;

    void LateUpdate()
    {
        if (gameObject.name == "ChatBox(Clone)")
        {
            //m_Offset.x = 2.0f + transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length * (-Camera.main.orthographicSize + 21) * 0.2f;
            //m_Offset.y = 1.5f - (-Camera.main.orthographicSize + 20) * 0.1f;
            if (IsLeft)
            {
                transform.localScale = new Vector3(-ChatBoxOriginalSize, ChatBoxOriginalSize, ChatBoxOriginalSize) + (new Vector3(-1, 1, 1) * (-Camera.main.orthographicSize + CameraMaxOrthoSize) * ChatBoxscaleAmount);
                transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
                transform.position = Camera.main.WorldToScreenPoint(m_Target.position + m_Offset);
                transform.position =
                    new Vector3(
                        transform.position.x - transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length *
                        (-Camera.main.orthographicSize + CameraMaxOrthoSize + 1) * ChatBoxLeftOffsetX - 100.0f, transform.position.y + (-Camera.main.orthographicSize + CameraMaxOrthoSize) * ChatBoxOffsetY, transform.position.z);
            }
            else
            {
                transform.localScale = new Vector3(ChatBoxOriginalSize, ChatBoxOriginalSize, ChatBoxOriginalSize) + (Vector3.one * (-Camera.main.orthographicSize + CameraMaxOrthoSize) * ChatBoxscaleAmount);
                transform.GetChild(0).transform.localScale = Vector3.one;
                transform.position = Camera.main.WorldToScreenPoint(m_Target.position + m_Offset);
                transform.position =
                    new Vector3(
                        transform.position.x + transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length *
                        (-Camera.main.orthographicSize + CameraMaxOrthoSize + 1) * ChatBoxOffsetX, transform.position.y + (-Camera.main.orthographicSize + CameraMaxOrthoSize) * ChatBoxOffsetY, transform.position.z);
            }
        }
        else
        {
            m_Offset.y = 3.0f + (-Camera.main.orthographicSize + CameraMaxOrthoSize) * 0.08f;
            transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + CameraMaxOrthoSize) * 0.1f);
            transform.position = Camera.main.WorldToScreenPoint(m_Target.position + m_Offset);
        }
    }
}
