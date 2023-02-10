using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ÿ���� �������� ��� �޾ƿͼ� ī�޶� �������� 
/// UI�� ȭ�鿡 ���� ������ ������
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
