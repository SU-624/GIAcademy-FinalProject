using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameShowPanel : MonoBehaviour
{
    public delegate void GameShowListPanelApplyButtonClicked();
    public static event GameShowListPanelApplyButtonClicked OnGameShowListPanelApplyButtonClicked;
    
    public delegate void GameShowListPanelSelectButtonClicked();
    public static event GameShowListPanelSelectButtonClicked OnGameShowListPanelSelectButtonClicked;

    [Header("게임쇼 패널에 보여줄 정보들")]
    [SerializeField] private TextMeshProUGUI m_GameShowName;
    [SerializeField] private TextMeshProUGUI m_GameShowLevel;
    [SerializeField] private TextMeshProUGUI m_GameShowHostCompany;
    [SerializeField] private TextMeshProUGUI m_GameShowProgressDate;
    [SerializeField] private TextMeshProUGUI m_GameShowJude;
    [SerializeField] private TextMeshProUGUI m_GameShowReward;
    [SerializeField] private TextMeshProUGUI m_GameShowNeedHealth;
    [SerializeField] private TextMeshProUGUI m_GameShowNeedPassion;
    [SerializeField] private Transform m_GameShowListContent;
    [SerializeField] private GameObject m_GameShowListContentObj;
    [SerializeField] private GameObject m_IsNotPrevGameList;
    [SerializeField] private GameObject m_IsNotGameShowList;

    [Space(5f)]
    [Header("게임쇼 정보창에 띄워줄 재미,그래픽,완성도,장르")]
    [SerializeField] private GameObject[] m_Funny;
    [SerializeField] private GameObject[] m_Graphic;
    [SerializeField] private GameObject[] m_Genre;
    [SerializeField] private GameObject[] m_Perfection;

    [Space(5f)]
    [Header("게임쇼 참여를 위해 누르는 버튼들")]
    [SerializeField] private Button m_ApplyButton;
    [SerializeField] private Button m_SelectButton;
    [SerializeField] private Button m_CloseButton;

    [Space(5f)]
    [Header("게임쇼에 내보낼 내가 만들었던 게임들을 등급별로 보여주는 버튼들")]
    [SerializeField] private Button m_All;
    [SerializeField] private Button m_AAA;
    [SerializeField] private Button m_AA;
    [SerializeField] private Button m_A;
    [SerializeField] private Button m_B;
    [SerializeField] private Transform m_PrevGameListContent;
    [SerializeField] private GameObject m_PrevGameListContentObj;

    [Space(5f)]
    [SerializeField] private GameObject m_PrevGameListPanel;
    [SerializeField] private GameObject m_ConditionFailPanel;
    [SerializeField] private TextMeshProUGUI m_ConditionFailText;

    [Space(5f)]
    [SerializeField] private PopUpUI m_PopUpGameShowPanel;
    [SerializeField] private PopOffUI m_PopOffGameShowPanel;
    [Space(5f)]
    [SerializeField] private PopUpUI m_PopUpConditionFailedPanel;
    [SerializeField] private PopOffUI m_PopOffConditionFailedPanel;

    public Button ApplyButton { get { return m_ApplyButton; } set { m_ApplyButton = value; } }
    public GameObject GameShowListContentObj { get { return m_GameShowListContentObj; } set { m_GameShowListContentObj = value; } }
    public GameObject PrevGameListContentObj { get { return m_PrevGameListContentObj; } set { m_PrevGameListContentObj = value; } }
    public Transform GameShowListContent { get { return m_GameShowListContent; } set { m_GameShowListContent = value; } }
    public Transform PrevGameJamListContent {  get { return m_PrevGameListContent; } set { m_PrevGameListContent = value; } }


    public void Start()
    {
        m_ApplyButton.onClick.AddListener(OnGameShowListApplyButtonClicked);
        
        m_SelectButton.onClick.AddListener(OnGameShowListSelectButtonClicked);
        
        m_CloseButton.onClick.AddListener(ClickCloseButton);
    }

    private void OnGameShowListApplyButtonClicked()
    {
        if (OnGameShowListPanelApplyButtonClicked != null)
        {
            OnGameShowListPanelApplyButtonClicked();
        }
    }

    private void OnGameShowListSelectButtonClicked()
    {
        if(OnGameShowListPanelSelectButtonClicked != null)
        {
            OnGameShowListPanelSelectButtonClicked();
        }
    }


    public void InitGameShowPanel()
    {
        m_GameShowHostCompany.text = "";
        m_GameShowName.text = "";
        m_GameShowLevel.text = "";
        m_GameShowProgressDate.text = "";
        m_GameShowReward.text = "";
        m_GameShowJude.text = "";
        m_GameShowNeedHealth.text = "";
        m_GameShowNeedPassion.text = "";
        m_PrevGameListPanel.SetActive(false);
        m_IsNotPrevGameList.SetActive(false);
        m_ApplyButton.interactable = false;

        for (int i = 0; i < 3; i++)
        {
            m_Funny[i].SetActive(false);
            m_Graphic[i].SetActive(false);
            m_Perfection[i].SetActive(false);
            m_Genre[i].SetActive(false);
        }
    }

    public void SetActiveIsNotPrevGameList(bool _flag)
    {
        m_IsNotPrevGameList.SetActive(_flag);
    }

    public void SetGameShowListText(string _hostCompany, string _gameShowName, string _level, string _progressDate,
        string _judge, string _reward, string _needHealth, string _needPassion)
    {
        m_GameShowHostCompany.text = _hostCompany;
        m_GameShowName.text = _gameShowName;
        m_GameShowLevel.text = _level;
        m_GameShowProgressDate.text = _progressDate;
        m_GameShowReward.text = _reward;
        m_GameShowJude.text = _judge;
        m_GameShowNeedHealth.text = _needHealth;
        m_GameShowNeedPassion.text = _needPassion;
    }

    public void SetNoGameShow(bool _flag)
    {
        m_IsNotPrevGameList.SetActive(_flag);
        m_IsNotGameShowList.SetActive(_flag);
    }

    public void SetConditionFailPanel(string _text)
    {
        m_ConditionFailText.text = _text;
        m_PopUpConditionFailedPanel.TurnOnUI();
        m_PopOffConditionFailedPanel.DelayTurnOffUI();
    }

    public void SetFunny(int _count, bool _flag)
    {
        for (int i = 0; i < _count; i++)
        {
            m_Funny[i].SetActive(_flag);
        }
    }

    public void SetGraphic(int _count, bool _flag)
    {
        for (int i = 0; i < _count; i++)
        {
            m_Graphic[i].SetActive(_flag);
        }
    }

    public void SetPerfection(int _count, bool _flag)
    {
        for (int i = 0; i < _count; i++)
        {
            m_Perfection[i].SetActive(_flag);
        }
    }

    public void SetGenre(int _count, bool _flag)
    {
        for (int i = 0; i < _count; i++)
        {
            m_Genre[i].SetActive(_flag);
        }
    }

    public void ClickSelectButton()
    {
        if(m_PrevGameListPanel.activeSelf)
        {
            m_PrevGameListPanel.SetActive(false);
        }
        else
        {
            m_ApplyButton.interactable = false;
            m_PrevGameListPanel.SetActive(true);
        }
    }

    public void ClickCloseButton()
    {
        InitGameShowPanel();
        DestroyObj(m_GameShowListContentObj);
        m_PopOffGameShowPanel.TurnOffUI();
        m_IsNotPrevGameList.SetActive(false);
    }

    public void SetActiveGameShowPanel(bool _flag = true)
    {
        if (_flag)
        {
            m_PopUpGameShowPanel.TurnOnUI();
        }
        else
        {
            m_PopOffGameShowPanel.TurnOffUI();
        }
    }

    // 해당 오브젝트의 자식 오브젝트들을 다 없애준다.
    public void DestroyObj(GameObject _obj)
    {
        Transform[] _childCount = _obj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != _obj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }
}
