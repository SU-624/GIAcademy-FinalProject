using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPanel : MonoBehaviour
{
    public RectTransform m_ContentsAnchor;
    public Transform m_ContentsTransform;

    // 1������ 3�������� ��Ŀ�� ����� ���ְ� �� �̻����� Ŀ���� �������� ��Ŀ�� �Ű��ش�.
    public void SetContentAnchor(bool _flag)
    {
        if (_flag)
        {
            m_ContentsAnchor.anchorMin = new Vector2(0.5f, 1f);
            m_ContentsAnchor.anchorMax = new Vector2(0.5f, 1f);
            m_ContentsAnchor.pivot = new Vector2(0.5f, 1f);
        }
        else
        {
            m_ContentsAnchor.anchorMin = new Vector2(0f, 1f);
            m_ContentsAnchor.anchorMax = new Vector2(0f, 1f);
            m_ContentsAnchor.pivot = new Vector2(0f, 1f);
        }
    }
}
