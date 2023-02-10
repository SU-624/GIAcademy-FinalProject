using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Vector3 m_Offset = new Vector3(1.5f, 1f, 0);

    void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(m_Target.position + m_Offset);
    }
}
