using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InJaeRecommendList : MonoBehaviour
{
    [SerializeField] private PopOffUI m_PopOffInJaeRecommendListPanel;
    [SerializeField] private PopUpUI m_PopUpInJaeRecommendListPanel;
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private ScrollRect m_Companycontent;
    [SerializeField] private Transform m_CompanyContentTransform;
    [SerializeField] private Transform m_RecommendContentTransform;

    private void Start()
    {
        m_CloseButton.onClick.AddListener(ClickCloseButton);
    }

    // 닫기 버튼을 눌렀을 때는 초기화도 해줘야한다.
    public void ClickCloseButton()
    {
        PopOffRecommendListPanel();
        DestroyRecommendList();
    }

    public void LockCloseButton(bool _isTrue = true)
    {
        m_CloseButton.enabled = _isTrue;
    }

    public void PopOffRecommendListPanel()
    {
        m_Companycontent.verticalNormalizedPosition = 1f;
        m_PopOffInJaeRecommendListPanel.TurnOffUI();
    }

    public void PopUpRecommendListPanel()
    {
        m_PopUpInJaeRecommendListPanel.TurnOnUI();
    }

    public Transform CompanyContent()
    {
        return m_CompanyContentTransform;
    }

    public Transform RecommendeContent()
    {
        return m_RecommendContentTransform;
    }

    public void DestroyCompanyContent()
    {
        Transform[] _childCount = m_CompanyContentTransform.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_CompanyContentTransform.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    public void DestroyRecommendList()
    {
        Transform[] _childCount = m_RecommendContentTransform.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_RecommendContentTransform.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }
}
