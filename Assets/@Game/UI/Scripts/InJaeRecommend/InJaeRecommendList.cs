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
    [SerializeField] private ScrollRect m_Recommendcontent;
    [SerializeField] private Transform m_CompanyContentTransform;
    [SerializeField] private Transform m_RecommendContentTransform;
    [SerializeField] private RectTransform m_CompanyListRect;
    [SerializeField] private RectTransform m_RecommendListRect;

    public RectTransform CompanyListRect { get { return m_CompanyListRect; } set { m_CompanyListRect = value; } }
    public RectTransform RecommendListRect { get { return m_RecommendListRect; } set { m_RecommendListRect = value; } }
    public Transform CompanyContentTransform { get { return m_CompanyContentTransform; } set { m_CompanyContentTransform = value; } }
    public Transform RecommendContentTransform { get { return m_RecommendContentTransform; } set { m_RecommendContentTransform = value; } }
    public ScrollRect Companycontent { get { return m_Companycontent; } set { m_Companycontent = value; } }
    public ScrollRect Recommendcontent { get { return m_Recommendcontent; } set { m_Recommendcontent = value; } }

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
