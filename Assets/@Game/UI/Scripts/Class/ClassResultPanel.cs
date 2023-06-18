using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_ResultPopUpPanel;         // 화면 터치시 꺼줄 팝업 패널
    [SerializeField] private GameObject m_ClassResultObj;           // 팝업 패널을 꺼준 후 학생들의 스탯이 얼마나 올랐는지 보여주는 패널을 켜야한다.
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private TextMeshProUGUI m_PartName;
    [SerializeField] private Transform m_Content;
    [SerializeField] private ScrollRect m_ContentRect;
    [SerializeField] private GameObject m_CnotentObj;
    [SerializeField] private PopOffUI m_PopOffClassResultPanel;

    public GameObject m_NextButton;
    public GameObject m_PrevButton;
    public GameObject ResultPopUpPanel { get { return m_ResultPopUpPanel; } set { m_ResultPopUpPanel = value; } }
    public Transform ResultPanelContent { get { return m_Content; } set { m_Content = value; } }
    public ScrollRect ContentRect { get { return m_ContentRect; } set { m_ContentRect = value; } }

    private void Start()
    {
        m_CloseButton.onClick.AddListener(ClickCloseButton);
    }

    public void InitPanel()
    {
        DestroyObject();
        SetPanel(false);
        m_NextButton.SetActive(true);
        m_PrevButton.SetActive(false);
        m_PartName.text = "기획";
        m_ContentRect.verticalNormalizedPosition = 1f;
    }

    private void ClickCloseButton()
    {
        InitPanel();
        m_PopOffClassResultPanel.TurnOffUI();
    }

    public void SetPanel(bool _flag)
    {
        if (_flag)
        {
            m_ResultPopUpPanel.SetActive(false);
            m_ClassResultObj.SetActive(true);
        }
        // 초기화 해주기 위한 부분
        else
        {
            m_ResultPopUpPanel.SetActive(true);
            m_ClassResultObj.SetActive(false);
        }
    }

    public void SetPartName(string _partName)
    {
        m_PartName.text = _partName;
    }

    public void DestroyObject()
    {
        Transform[] _childCount = m_CnotentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_CnotentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }
}
