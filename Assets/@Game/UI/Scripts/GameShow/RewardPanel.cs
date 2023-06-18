using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPanel : MonoBehaviour
{
    public RectTransform m_ContentsAnchor;
    public Transform m_ContentsTransform;

    // 1개부터 3개까지는 엥커를 가운데로 해주고 그 이상으로 커지면 왼쪽으로 엥커를 옮겨준다.
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
